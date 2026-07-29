-- =====================================================================
-- LEVER 1 -- Enrich the resource embed text with metadata.
-- Run in the RESOURCES database. Replaces fn_ResourceEmbedText.
--
-- Adds, on top of Title/Description/Keywords:
--   * distinct ResourceTypes.name  (the categories/types the resource
--     is filed under -- e.g. "Feeding & Nutrition", "Clinical Tools")
--   * the library name + its description (ResourcesGroups)
--
-- Why this helps: most resources have empty Description/Keywords, so the
-- old embed text was little more than the title. Folding in the type and
-- library names gives the embedding real topical context to match
-- against, which spreads genuine matches above the noise band.
--
-- After running this, RE-INDEX (admin page) so every resource is
-- re-embedded with the richer text. The SourceHash will differ for all
-- rows, so the indexer will re-embed the whole library once.
-- =====================================================================

IF OBJECT_ID('dbo.fn_ResourceEmbedText', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_ResourceEmbedText;
GO
CREATE FUNCTION dbo.fn_ResourceEmbedText (@ResourceId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @title NVARCHAR(MAX), @desc NVARCHAR(MAX), @kw NVARCHAR(MAX);
    SELECT @title = ISNULL(r.Title, N''),
           @desc  = ISNULL(r.[Description], N''),
           @kw    = ISNULL(r.Keywords, N'')
    FROM dbo.Resources r
    WHERE r.id = @ResourceId;

    -- Distinct type/category names for this resource (a resource can be
    -- filed under several). FOR XML PATH concatenation works at compat 100.
    DECLARE @types NVARCHAR(MAX);
    SELECT @types = STUFF((
        SELECT N', ' + t.name
        FROM dbo.Resource_Types_Link l
        JOIN dbo.ResourceTypes t ON t.id = l.TypeId
        WHERE l.ResourceId = @ResourceId
          AND t.name IS NOT NULL
        GROUP BY t.name
        FOR XML PATH(N''), TYPE).value(N'.', N'NVARCHAR(MAX)'), 1, 2, N'');

    -- Library name(s) + description(s) the resource belongs to, via the
    -- group link. Distinct, concatenated.
    DECLARE @libs NVARCHAR(MAX);
    SELECT @libs = STUFF((
        SELECT N'. ' + g.name + N' ' + ISNULL(g.[description], N'')
        FROM (SELECT DISTINCT l.GroupId
              FROM dbo.Resource_Types_Link l
              WHERE l.ResourceId = @ResourceId) gl
        JOIN dbo.ResourcesGroups g ON g.id = gl.GroupId
        FOR XML PATH(N''), TYPE).value(N'.', N'NVARCHAR(MAX)'), 1, 2, N'');

    -- Assemble. Title twice (weight), then description, keywords, types,
    -- library context.
    DECLARE @out NVARCHAR(MAX);
    SET @out = @title + N'. ' + @title + N'. '
             + @desc + N' '
             + @kw + N' '
             + ISNULL(@types, N'') + N' '
             + ISNULL(@libs, N'');

    RETURN LTRIM(RTRIM(@out));
END
GO

-- Quick check: eyeball the enriched text for the resources we tested.
-- Confirm the type/library context is now present.
--   SELECT id, dbo.fn_ResourceEmbedText(id) AS EmbedText
--   FROM dbo.Resources
--   WHERE id IN (6507, 7850, 8315, 8643, 8736, 13203);
