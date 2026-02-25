using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FolderContent : System.Web.UI.Page
{
    string path = "~/data/documents";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadFolders();
            LoadFoldersNotInDb();
        }
    }

    private void LoadFoldersNotInDb()
    {
        string folderPath = Server.MapPath(path); // change as needed
        if (!Directory.Exists(folderPath)) return;

        // 1) Get folder names from disk
        string[] dirs = Directory.GetDirectories(folderPath);

        // 2) Get folder names that already exist in DB
        HashSet<string> existing = GetExistingFolderNamesFromDb();

        // 3) Show only folders NOT in DB
        blFolders.Items.Clear();
        foreach (string dir in dirs)
        {
            string name = Path.GetFileName(dir);

            // case-insensitive compare (matches most SQL collations)
            if (existing.Contains(name))
                blFolders.Items.Add(name);
        }
    }

    private HashSet<string> GetExistingFolderNamesFromDb()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        string cs = ConfigurationManager.ConnectionStrings["CMServer"].ConnectionString;
        using (var con = new SqlConnection(cs))
       // using (var cmd = new SqlCommand("SELECT id FROM dbo.DocumentGroups", con))
        using (var cmd = new SqlCommand("SELECT cast(id as varchar(200)) as name FROM dbo.DocumentGroups " +
            "where groupid not in (select id from groups)" +
            "", con))
        {
            con.Open();
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                        set.Add(rdr.GetString(0));
                }
            }
        }
        return set;
    }

    private void LoadFolders()
    {
        // Change this path as needed
        string folderPath = Server.MapPath(path);

        if (Directory.Exists(folderPath))
        {
            string[] directories = Directory.GetDirectories(folderPath);

            foreach (string dir in directories)
            {
                blFolders.Items.Add(Path.GetFileName(dir));
            }
        }
    }
}