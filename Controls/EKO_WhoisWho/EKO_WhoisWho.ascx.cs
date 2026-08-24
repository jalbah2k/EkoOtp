using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class EKO_WhoisWho : System.Web.UI.UserControl
{
    private static readonly string[] ExcludedRoleNames =
    {
        "Administrators",
        "EKO Administrators",
        "Guests",
        "Juan Test Role",
        "Open Pages",
        "PNCA Administrator",
        "PNCA Executive Team",
        "PNCA OAP URS",
        "PNCA PreRegistration",
        "PNCA Registered User",
        "Provincial Network of Coordinating Agencies",
        "Registered Users",
        "Vivian Test Roles"
    };

    public EKO_WhoisWho() { }
    public EKO_WhoisWho(string p) { }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (TryServeProfilePhoto())
        {
            return;
        }

        RegisterWhoisWhoAssets();

        if (!IsMemberSignedIn())
        {
            pnlDirectory.Visible = false;
            pnlSignedOut.Visible = true;
            return;
        }

        pnlDirectory.Visible = true;
        pnlSignedOut.Visible = false;
        EnsureOrganizationDropdown();

        if (!IsPostBack)
        {
            try
            {
                BindDirectory();
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(litMembersJson.Text))
                {
                    litMembersJson.Text = "[]";
                }
            }
        }
    }

    private void RegisterWhoisWhoAssets()
    {
        string version = ConfigurationManager.AppSettings["CSSVersion"] ?? "1";
        string cssUrl = ResolveClientUrl("~/Controls/EKO_WhoisWho/EKO_WhoisWho.css?v=" + version);

        if (Page.Header != null && Page.Header.FindControl("lnkEkoWhoisWhoCss") == null)
        {
            HtmlLink link = new HtmlLink();
            link.ID = "lnkEkoWhoisWhoCss";
            link.Attributes["rel"] = "stylesheet";
            link.Attributes["type"] = "text/css";
            link.Href = cssUrl;
            Page.Header.Controls.Add(link);
        }
    }

    private bool IsMemberSignedIn()
    {
        return Session["LoggedInID"] != null
            || Session["MemberId"] != null
            || Session["MemberID"] != null;
    }

    private bool TryServeProfilePhoto()
    {
        var raw = Request.QueryString["whoPhoto"];
        int memberId;
        if (string.IsNullOrWhiteSpace(raw) || !int.TryParse(raw, out memberId) || memberId <= 0)
        {
            return false;
        }

        if (!IsMemberSignedIn())
        {
            Response.StatusCode = 401;
            Response.End();
            return true;
        }

        byte[] photo = null;
        using (var conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        using (var cmd = new SqlCommand(
            @"SELECT ProfilePhoto
              FROM eko.Members
              WHERE id = @id AND ISNULL(IsVisible, 0) = 1", conn))
        {
            cmd.Parameters.AddWithValue("@id", memberId);
            conn.Open();
            var value = cmd.ExecuteScalar();
            if (value != null && value != DBNull.Value)
            {
                photo = (byte[])value;
            }
        }

        if (photo == null || photo.Length == 0)
        {
            Response.StatusCode = 404;
            Response.End();
            return true;
        }

        Response.Clear();
        Response.Buffer = true;
        Response.Cache.SetCacheability(HttpCacheability.Private);
        Response.Cache.SetMaxAge(TimeSpan.FromMinutes(5));
        Response.ContentType = DetectImageContentType(photo);
        Response.BinaryWrite(photo);
        Response.End();
        return true;
    }

    private static string DetectImageContentType(byte[] photo)
    {
        if (photo.Length >= 3 && photo[0] == 0xFF && photo[1] == 0xD8 && photo[2] == 0xFF)
        {
            return "image/jpeg";
        }
        if (photo.Length >= 8 && photo[0] == 0x89 && photo[1] == 0x50 && photo[2] == 0x4E && photo[3] == 0x47)
        {
            return "image/png";
        }
        if (photo.Length >= 3 && photo[0] == 0x47 && photo[1] == 0x49 && photo[2] == 0x46)
        {
            return "image/gif";
        }
        return "image/jpeg";
    }

    private void EnsureOrganizationDropdown()
    {
        if (ddlOrganization.Items.Count > 0)
        {
            return;
        }

        ddlOrganization.Items.Clear();
        ddlOrganization.Items.Add(new ListItem("All Organizations", ""));

        try
        {
            var organizations = LoadOrganizations();

            foreach (var org in organizations)
            {
                ddlOrganization.Items.Add(
                    new ListItem(org.Name, org.Id)
                );
            }
        }
        catch (Exception ex)
        {
            // Log the error instead of silently ignoring it
            System.Diagnostics.Debug.WriteLine(
                "Error loading organizations: " + ex.Message
            );
        }
    }

    private void BindDirectory()
    {
        var orgById = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase
        );

        ddlOrganization.Items.Clear();
        ddlOrganization.Items.Add(
            new ListItem("All Organizations", "")
        );

        try
        {
            var organizations = LoadOrganizations();

            foreach (var org in organizations)
            {
                orgById[org.Id] = org.Name;

                ddlOrganization.Items.Add(
                    new ListItem(org.Name, org.Id)
                );
            }
        }
        catch (Exception ex)
        {
            // Log the error instead of silently ignoring it
            System.Diagnostics.Debug.WriteLine(
                "Error binding organizations: " + ex.Message
            );
        }

        var members = LoadMembers(orgById);

        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = int.MaxValue
        };

        litMembersJson.Text = serializer.Serialize(members);
    }

    private List<WhoisWhoOrganization> LoadOrganizations()
    {
        var list = new List<WhoisWhoOrganization>();

        const string sql = @"
        SELECT 
            id,
            name
        FROM [EKO_OTP].[eko].[Organizations]
        WHERE type = 1
          AND active = 1
          AND deleted = 0
        ORDER BY name;
    ";

        var connectionString =
            ConfigurationManager.AppSettings["CMServer"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception(
                "CMServer connection string was not found in AppSettings."
            );
        }

        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(sql, conn))
        {
            conn.Open();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader["id"] == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(reader["id"]);

                    var name = reader["name"] == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(reader["name"]);

                    if (string.IsNullOrWhiteSpace(id) ||
                        string.IsNullOrWhiteSpace(name))
                    {
                        continue;
                    }

                    list.Add(new WhoisWhoOrganization
                    {
                        Id = id.Trim(),
                        Name = name.Trim()
                    });
                }
            }
        }

        return list;
    }

    private List<WhoisWhoMember> LoadMembers(Dictionary<string, string> orgById)
    {
        var members = new List<WhoisWhoMember>();
        DataTable memberTable = FillTable(
    @"SELECT
        m.id,
        m.userid,
        m.yaf_userid,
        LTRIM(RTRIM(ISNULL(m.FirtsName, ''))) AS firstname,
        LTRIM(RTRIM(ISNULL(m.LastName, ''))) AS lastname,
        LTRIM(RTRIM(ISNULL(CONVERT(varchar(500), m.Organization_New), ''))) AS Organization_New,
        m.OrganizationId_New,
        m.OrganizationType_New,
        m.Position_Title,
        m.Pronouns,
        m.PronounsOther,
        m.SecondaryEmail,
        m.PhoneNumber,
        m.PhoneExtension,
        m.MobilePhone,
        m.Institution,
        m.CertificationDegree,
        m.YearOfGraduation,
        m.LinkedInProfile,
        m.TellUs,
        u.email AS UserEmail,
        CASE
            WHEN m.ProfilePhoto IS NULL THEN 0
            ELSE 1
        END AS HasPhoto
      FROM [EKO_OTP].[eko].[Members] m
      LEFT JOIN [EKO_OTP].[dbo].[Users] u
        ON u.id = m.userid
      WHERE m.OrganizationType_New IN (1,4)
        AND ISNULL(m.IsVisible, 0) = 1",
            @"SELECT
    m.id,
    m.userid,
    m.yaf_userid,
    LTRIM(RTRIM(ISNULL(m.FirtsName, ''))) AS firstname,
    LTRIM(RTRIM(ISNULL(m.LastName, ''))) AS lastname,
    LTRIM(RTRIM(ISNULL(CONVERT(varchar(500), m.Organization_New), ''))) AS Organization_New,
    m.OrganizationId_New,
    m.OrganizationType_New,
    m.Position_Title,
    m.Pronouns,
    m.PronounsOther,
    m.SecondaryEmail,
    m.PhoneNumber,
    m.PhoneExtension,
    m.MobilePhone,
    m.Institution,
    m.CertificationDegree,
    m.YearOfGraduation,
    m.LinkedInProfile,
    m.TellUs,
    CASE
        WHEN m.ProfilePhoto IS NULL THEN 0
        ELSE 1
    END AS HasPhoto
  FROM [EKO_OTP].[eko].[Members] m
  WHERE m.OrganizationType_New IN (1,4)
    AND ISNULL(m.IsVisible, 0) = 1");

        Dictionary<string, List<string>> committeesByUser;
        try
        {
            committeesByUser = LoadCommitteesByUserId();
        }
        catch
        {
            committeesByUser = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }
        var photoPath = Request.RawUrl.Split('#')[0].Split('?')[0];

        foreach (DataRow row in memberTable.Rows)
        {
            var first = Col(row, "firstname", "FirtsName", "FirstName");
            var last = Col(row, "lastname", "LastName");
            var name = (first + " " + last).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            var orgId = Col(row, "OrganizationId_New", "Organization_New");
            var organization = Col(row, "Organization_New");
            int parsedOrgId;
            if (int.TryParse(organization, out parsedOrgId) && orgById.ContainsKey(organization))
            {
                orgId = organization;
                organization = orgById[organization];
            }
            else if (string.IsNullOrWhiteSpace(organization) && !string.IsNullOrWhiteSpace(orgId) && orgById.ContainsKey(orgId))
            {
                organization = orgById[orgId];
            }

            var pronouns = FormatPronouns(Col(row, "Pronouns"), Col(row, "PronounsOther"));
            var userId = Col(row, "userid");
            var id = Col(row, "id");
            var committees = LookupCommittees(committeesByUser, userId, id);
            var about = CleanAbout(Col(row, "TellUs", "About", "Bio"));
            var email = Col(row, "SecondaryEmail", "UserEmail", "Email");
            var hasPhoto = IsTruthy(row["HasPhoto"]);
            members.Add(new WhoisWhoMember
            {
                id = id,
                name = name,
                firstName = first,
                lastName = last,
                initials = BuildInitials(first, last, name),
                organization = organization,
                orgId = orgId,
                title = Col(row, "Position_Title"),
                pronouns = pronouns,
                institution = Col(row, "Institution"),
                certification = Col(row, "CertificationDegree"),
                yearOfGraduation = Col(row, "YearOfGraduation"),
                email = email,
                phone = Col(row, "PhoneNumber"),
                extension = Col(row, "PhoneExtension"),
                mobile = Col(row, "MobilePhone"),
                linkedIn = Col(row, "LinkedInProfile"),
                about = about,
                photoUrl = hasPhoto ? photoPath + "?whoPhoto=" + HttpUtility.UrlEncode(id) : "",
                committees = committees
            });
        }

        return members
            .OrderBy(m => m.name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static List<string> LookupCommittees(Dictionary<string, List<string>> committeesByUser, params string[] keys)
    {
        var names = new List<string>();
        foreach (var key in keys)
        {
            List<string> found;
            if (string.IsNullOrWhiteSpace(key) || !committeesByUser.TryGetValue(key, out found))
            {
                continue;
            }

            foreach (var name in found)
            {
                if (!names.Exists(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
                {
                    names.Add(name);
                }
            }
        }

        names.Sort(StringComparer.OrdinalIgnoreCase);
        return names;
    }

    private static string CleanAbout(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        var text = HttpUtility.HtmlDecode(value);
        text = Regex.Replace(text, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"</p\s*>", "\n", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, @"<[^>]+>", " ");
        text = Regex.Replace(text, @"[ \t]+", " ");
        text = Regex.Replace(text, @"\n\s*\n+", "\n\n").Trim();
        if (text.StartsWith("Lorem ipsum", StringComparison.OrdinalIgnoreCase))
        {
            return "";
        }

        return text;
    }
    private Dictionary<string, List<string>> LoadCommitteesByUserId()
    {
        var result = new Dictionary<string, List<string>>(
            StringComparer.OrdinalIgnoreCase);

        using (var conn = new SqlConnection(
            ConfigurationManager.AppSettings["CMServer"]))
        using (var cmd = new SqlCommand(
            "[eko].[GetGroupsForWhoiswho]", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string userId = "";

                    if (reader["User_id"] != DBNull.Value)
                    {
                        userId = Convert.ToString(reader["User_id"]);
                    }
                    else if (reader["userid"] != DBNull.Value)
                    {
                        userId = Convert.ToString(reader["userid"]);
                    }

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        continue;
                    }

                    string groupName = "";

                    if (reader["name"] != DBNull.Value)
                    {
                        groupName = Convert.ToString(reader["name"]);
                    }
                    else if (reader["GroupName"] != DBNull.Value)
                    {
                        groupName = Convert.ToString(reader["GroupName"]);
                    }
                    else if (reader["yaf_GroupName"] != DBNull.Value)
                    {
                        groupName = Convert.ToString(reader["yaf_GroupName"]);
                    }

                    if (string.IsNullOrWhiteSpace(groupName))
                    {
                        continue;
                    }

                    List<string> committees;

                    if (!result.TryGetValue(userId, out committees))
                    {
                        committees = new List<string>();
                        result[userId] = committees;
                    }

                    if (!committees.Exists(
                        x => string.Equals(
                            x,
                            groupName,
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        committees.Add(groupName);
                    }
                }
            }
        }

        foreach (var item in result)
        {
            item.Value.Sort(StringComparer.OrdinalIgnoreCase);
        }

        return result;
    }

    private static DataTable FillTable(params string[] sqlOptions)
    {
        Exception last = null;
        foreach (var sql in sqlOptions)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                using (var dapt = new SqlDataAdapter(sql, conn))
                {
                    var table = new DataTable();
                    dapt.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                last = ex;
            }
        }

        if (last != null)
        {
            throw last;
        }

        return new DataTable();
    }

    private static bool IsExcludedRoleName(string name)
    {
        var key = NormalizeKey(name);
        return ExcludedRoleNames.Any(item => NormalizeKey(item) == key);
    }

    private static string FormatPronouns(string pronouns, string other)
    {
        if (string.IsNullOrWhiteSpace(pronouns) || NormalizeKey(pronouns) == "prefernot")
        {
            return "";
        }

        if (NormalizeKey(pronouns) == "other")
        {
            return (other ?? "").Trim();
        }

        return pronouns.Trim();
    }

    private static string BuildInitials(string first, string last, string name)
    {
        var a = string.IsNullOrWhiteSpace(first) ? "" : first.Trim().Substring(0, 1);
        var b = string.IsNullOrWhiteSpace(last) ? "" : last.Trim().Substring(0, 1);
        if (a.Length + b.Length > 0)
        {
            return (a + b).ToUpperInvariant();
        }

        var parts = (name ?? "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return "";
        }
        if (parts.Length == 1)
        {
            return parts[0].Substring(0, 1).ToUpperInvariant();
        }
        return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
    }

    private static bool HasColumn(DataTable table, string name)
    {
        return table.Columns.Contains(name);
    }

    private static string Col(DataRow row, params string[] names)
    {
        foreach (var name in names)
        {
            if (row.Table.Columns.Contains(name) && row[name] != DBNull.Value)
            {
                return Convert.ToString(row[name], CultureInfo.InvariantCulture) ?? "";
            }
        }
        return "";
    }

    private static bool IsTruthy(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return false;
        }
        if (value is bool)
        {
            return (bool)value;
        }
        if (value is byte || value is short || value is int || value is long)
        {
            return Convert.ToInt64(value) != 0;
        }

        var text = Convert.ToString(value, CultureInfo.InvariantCulture);
        return text == "1" || string.Equals(text, "true", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeKey(string value)
    {
        return Regex.Replace((value ?? "").Trim().ToLowerInvariant(), @"[\s_\-]+", "");
    }
}
