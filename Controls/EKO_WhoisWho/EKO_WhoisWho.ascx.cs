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
    public EKO_WhoisWho() { }
    public EKO_WhoisWho(string p) { }

    protected void Page_Load(object sender, EventArgs e)
    {
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

        const string sql = @"
            SELECT *
            FROM [EKO_OTP].[eko].[Members]
            WHERE OrganizationType_New IN (1, 4)
              AND IsVisible = 1";

        var connectionString = ConfigurationManager.AppSettings["CMServer"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("CMServer connection string was not found in AppSettings.");
        }

        var memberIds = new List<string>();

        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand(sql, conn))
        {
            conn.Open();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var first = ReadStr(reader, "FirtsName", "FirstName");
                    var last = ReadStr(reader, "LastName");
                    var name = (first + " " + last).Trim();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        continue;
                    }

                    var id = ReadStr(reader, "id");
                    var userId = ReadStr(reader, "userid");

                    var orgId = ReadStr(reader, "OrganizationId_New");
                    var organization = ReadStr(reader, "Organization_New");

                    int parsedOrgId;
                    if (int.TryParse(organization, out parsedOrgId) && orgById.ContainsKey(organization))
                    {
                        orgId = organization;
                        organization = orgById[organization];
                    }
                    else if (string.IsNullOrWhiteSpace(organization) &&
                             !string.IsNullOrWhiteSpace(orgId) &&
                             orgById.ContainsKey(orgId))
                    {
                        organization = orgById[orgId];
                    }

                    var pronouns = FormatPronouns(ReadStr(reader, "Pronouns"), ReadStr(reader, "PronounsOther"));
                    var about = CleanAbout(ReadStr(reader, "TellUs", "About", "Bio"));
                    var email = ReadStr(reader, "SecondaryEmail", "Email");
                    var photoBytes = ReadBytes(reader, "ProfilePhoto");
                    var photoUrl = (photoBytes != null && photoBytes.Length > 0)
                        ? "data:" + DetectImageContentType(photoBytes) + ";base64," + Convert.ToBase64String(photoBytes)
                        : "";

                    members.Add(new WhoisWhoMember
                    {
                        id = id,
                        name = name,
                        firstName = first,
                        lastName = last,
                        initials = BuildInitials(first, last, name),
                        organization = organization,
                        orgId = orgId,
                        title = ReadStr(reader, "Position_Title"),
                        pronouns = pronouns,
                        institution = ReadStr(reader, "Institution"),
                        certification = ReadStr(reader, "CertificationDegree"),
                        yearOfGraduation = ReadStr(reader, "YearOfGraduation"),
                        email = email,
                        phone = ReadStr(reader, "PhoneNumber"),
                        extension = ReadStr(reader, "PhoneExtension"),
                        mobile = ReadStr(reader, "MobilePhone"),
                        linkedIn = ReadStr(reader, "LinkedInProfile"),
                        about = about,
                        photoUrl = photoUrl,
                        committees = new List<string>()
                    });

                    memberIds.Add(id);
                }
            }
        }
        for (int i = 0; i < members.Count; i++)
        {
            members[i].committees = LoadCommitteesForMember(memberIds[i]);
        }

        return members
            .OrderBy(m => m.name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string ReadStr(SqlDataReader reader, params string[] names)
    {
        foreach (var name in names)
        {
            int ordinal;
            try
            {
                ordinal = reader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }

            if (!reader.IsDBNull(ordinal))
            {
                return Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ?? "";
            }
        }
        return "";
    }

    private static byte[] ReadBytes(SqlDataReader reader, string name)
    {
        int ordinal;
        try
        {
            ordinal = reader.GetOrdinal(name);
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }

        if (reader.IsDBNull(ordinal))
        {
            return null;
        }

        return (byte[])reader.GetValue(ordinal);
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
    private List<string> LoadCommitteesForMember(string memberId)
    {
        var names = new List<string>();

        int memberIdInt;
        if (!int.TryParse(memberId, out memberIdInt))
        {
            return names;
        }

        try
        {
            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            using (var cmd = new SqlCommand("[eko].[GetGroupsForWhoiswho]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = memberIdInt;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var groupName = ReadStr(reader, "yaf_GroupName", "GroupName", "name", "Name");
                        if (string.IsNullOrWhiteSpace(groupName))
                        {
                            continue;
                        }

                        if (!names.Exists(n => string.Equals(n, groupName, StringComparison.OrdinalIgnoreCase)))
                        {
                            names.Add(groupName);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                "Error loading committees for member " + memberId + ": " + ex.Message
            );
            return new List<string>();
        }

        names.Sort(StringComparer.OrdinalIgnoreCase);
        return names;
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

    private static string NormalizeKey(string value)
    {
        return Regex.Replace((value ?? "").Trim().ToLowerInvariant(), @"[\s_\-]+", "");
    }
}