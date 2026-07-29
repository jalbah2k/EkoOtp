-- =====================================================================
-- STEP 1 -- Semantic search for RESOURCES
-- Runs in the RESOURCES database (the "dbResources" connection).
-- Stores one embedding vector per resource. Permission trimming is
-- NOT done here -- it is inherited at query time from
-- Resources_Search_New (its @mytable candidate set).
-- Compatible with SQL Server 2012+ (no special features).
-- =====================================================================

-- ---------------------------------------------------------------------
-- Embedding store. One row per resource.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.ResourceEmbedding', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ResourceEmbedding
    (
        ResourceId      INT             NOT NULL PRIMARY KEY,   -- FK to dbo.Resources.id
        EmbeddingModel  VARCHAR(100)    NOT NULL,               -- e.g. 'text-embedding-3-small'
        Dimensions      INT             NOT NULL,               -- e.g. 1536
        Vector          VARBINARY(MAX)  NOT NULL,               -- packed float32[] (Dimensions*4 bytes)
        SourceHash      CHAR(64)        NOT NULL,               -- SHA-256 of the embedded text
        UpdatedUtc      DATETIME        NOT NULL CONSTRAINT DF_ResourceEmbedding_UpdatedUtc DEFAULT (GETUTCDATE())
    );

    -- Optional FK (only if you want cascade cleanup; comment out if it
    -- causes issues with how resources are deleted in your app).
    -- ALTER TABLE dbo.ResourceEmbedding
    --   ADD CONSTRAINT FK_ResourceEmbedding_Resources
    --   FOREIGN KEY (ResourceId) REFERENCES dbo.Resources(id) ON DELETE CASCADE;
END
GO

-- ---------------------------------------------------------------------
-- The text we embed for each resource, defined in ONE place so the
-- indexer and the change-hash always agree.
-- Title is repeated so it weighs more than description/keywords.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_ResourceEmbedText', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_ResourceEmbedText;
GO
CREATE FUNCTION dbo.fn_ResourceEmbedText (@ResourceId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @t NVARCHAR(MAX);
    SELECT @t =
        ISNULL(r.Title, N'') + N'. ' + ISNULL(r.Title, N'') + N'. '
      + ISNULL(r.[Description], N'') + N' '
      + ISNULL(r.Keywords, N'')
    FROM dbo.Resources r
    WHERE r.id = @ResourceId;
    RETURN LTRIM(RTRIM(@t));
END
GO

-- ---------------------------------------------------------------------
-- Resources that are new or whose embed-text changed since last run.
-- @PublishedStatus / @LanguageId come from config (see web.config keys
-- in the integration guide) so "published" is not hard-coded.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.Resources_GetNeedingEmbedding', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_GetNeedingEmbedding;
GO
CREATE PROCEDURE dbo.Resources_GetNeedingEmbedding
    @PublishedStatus INT,
    @LanguageId      INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  r.id AS ResourceId,
            dbo.fn_ResourceEmbedText(r.id) AS EmbedText
    FROM    dbo.Resources r
    LEFT JOIN dbo.ResourceEmbedding e ON e.ResourceId = r.id
    WHERE   r.[Show] = 1
      AND   r.[Status] = @PublishedStatus
      AND   r.LanguageId = @LanguageId
      AND (
              e.ResourceId IS NULL
              OR e.SourceHash <> CONVERT(CHAR(64),
                   HASHBYTES('SHA2_256',
                     CONVERT(VARBINARY(MAX), dbo.fn_ResourceEmbedText(r.id))), 2)
          );
END
GO

-- ---------------------------------------------------------------------
-- Resources that should NO LONGER be in the index (unpublished/hidden
-- but still have an embedding row). The indexer deletes these.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.Resources_GetStaleEmbeddings', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_GetStaleEmbeddings;
GO
CREATE PROCEDURE dbo.Resources_GetStaleEmbeddings
    @PublishedStatus INT,
    @LanguageId      INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.ResourceId
    FROM dbo.ResourceEmbedding e
    LEFT JOIN dbo.Resources r
        ON r.id = e.ResourceId
       AND r.[Show] = 1
       AND r.[Status] = @PublishedStatus
       AND r.LanguageId = @LanguageId
    WHERE r.id IS NULL;   -- exists in index but no longer qualifies
END
GO

-- ---------------------------------------------------------------------
-- Upsert one embedding.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.Resources_SaveEmbedding', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_SaveEmbedding;
GO
CREATE PROCEDURE dbo.Resources_SaveEmbedding
    @ResourceId  INT,
    @Model       VARCHAR(100),
    @Dimensions  INT,
    @Vector      VARBINARY(MAX),
    @SourceHash  CHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    MERGE dbo.ResourceEmbedding AS t
    USING (SELECT @ResourceId AS ResourceId) AS s
        ON t.ResourceId = s.ResourceId
    WHEN MATCHED THEN
        UPDATE SET EmbeddingModel = @Model, Dimensions = @Dimensions,
                   Vector = @Vector, SourceHash = @SourceHash,
                   UpdatedUtc = GETUTCDATE()
    WHEN NOT MATCHED THEN
        INSERT (ResourceId, EmbeddingModel, Dimensions, Vector, SourceHash)
        VALUES (@ResourceId, @Model, @Dimensions, @Vector, @SourceHash);
END
GO

IF OBJECT_ID('dbo.Resources_DeleteEmbedding', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_DeleteEmbedding;
GO
CREATE PROCEDURE dbo.Resources_DeleteEmbedding
    @ResourceId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.ResourceEmbedding WHERE ResourceId = @ResourceId;
END
GO

-- ---------------------------------------------------------------------
-- Load all vectors into the app cache at startup / after reindex.
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.Resources_GetAllEmbeddings', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_GetAllEmbeddings;
GO
CREATE PROCEDURE dbo.Resources_GetAllEmbeddings
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ResourceId, Dimensions, Vector FROM dbo.ResourceEmbedding;
END
GO

-- ---------------------------------------------------------------------
-- Load display rows for a set of resource IDs (used to fetch rows for
-- semantic-only hits that the keyword proc didn't return).
-- Pass a comma-separated id list; returns the same shape the search
-- UI already consumes (id, title, seo, ...).
-- ---------------------------------------------------------------------
IF OBJECT_ID('dbo.Resources_GetByIds', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Resources_GetByIds;
GO
CREATE PROCEDURE dbo.Resources_GetByIds
    @Ids NVARCHAR(MAX)   -- e.g. '12,45,991'
AS
BEGIN
    SET NOCOUNT ON;

    -- Simple, safe split (ids are integers we generated, not user text).
    ;WITH Ids AS (
        SELECT TRY_CAST(LTRIM(RTRIM(value)) AS INT) AS id
        FROM STRING_SPLIT(@Ids, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(value)) AS INT) IS NOT NULL
    )
    SELECT v.*, 'resource/' + CAST(v.id AS VARCHAR(20)) AS seo
    FROM vwResourcesOptimized v
    INNER JOIN Ids ON Ids.id = v.id;
END
GO
-- NOTE: STRING_SPLIT requires SQL Server 2016+. If on 2012/2014, replace
-- the Ids CTE with your existing dbo.fSplitString function (present in
-- this DB) -- it returns the same id column.
