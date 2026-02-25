using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Obout.Ajax.UI.TreeView;
using CuteWebUI;

public partial class Controls_Content_AddJwVideo : OboutInc.oboutAJAXPage
{
    string initialDirectory = "/Uploads";
    string DefaultFolder;
    int expandedLevel = 14;

    void Page_Load(object sender, EventArgs e)
    {
			if (Session["LoggedInID"] == null)
            Response.Redirect("/login.aspx");


        DefaultFolder = Server.MapPath(initialDirectory);

        ThrowExceptionsAtClient = true;
        ShowErrorsAtClient = true;


        // set default initial directory
        if (!IsPostBack)
        {
            hfPath.Value = initialDirectory;
            LoadTreeView();
            SelectDir(string.Empty);
        }
        lblCurrentDir.Text = hfPath.Value;
    }

    private void LoadTreeView()
    {
        Node rootNode = new Node() { Text = "Uploads", Expanded = true };
        ObTree.Nodes.Add(rootNode);

        DirectoryInfo rootFolder = new DirectoryInfo(DefaultFolder);
        LoadDirRecursive(rootNode, rootFolder);
    }

    private void LoadDirRecursive(Node ParentNode, DirectoryInfo rootFolder)
    {
        expandedLevel++;

        foreach (DirectoryInfo dir in rootFolder.GetDirectories())
        {
            string dirName = dir.Name;
            string dirID = dirName;

            bool expanded = true;
            if (expandedLevel >= 15)
                expanded = false;

            Node child = new Node() { ClientID = dirID, Text = dirName, Expanded = expanded, ImageUrl = "obout/TreeView/icons/folder.gif" };
            ParentNode.ChildNodes.Add(child);

            LoadDirRecursive(child, new DirectoryInfo(rootFolder + "/" + dirName));
        }
    }

    // populate grid with directory content
    public void SelectDir(string dirID)
    {
        string path = string.Empty;
        path = initialDirectory;

        if (!string.IsNullOrEmpty(dirID))
            path += "/" + dirID;

        ViewState["dirID"] = path;

        LoadGrid();

        UpdatePanel("cpDir");
    }

    public void LoadGrid()
    {
        string dirID = ViewState["dirID"].ToString();

        DataSet dsDir = new DataSet();
        dsDir.Tables.Add(new DataTable());
        dsDir.Tables[0].Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
        dsDir.Tables[0].Columns.Add(new DataColumn("size", System.Type.GetType("System.Int32")));
        dsDir.Tables[0].Columns.Add(new DataColumn("type", System.Type.GetType("System.String")));
        dsDir.Tables[0].Columns.Add(new DataColumn("datemodified", System.Type.GetType("System.String")));
        dsDir.Tables[0].Columns.Add(new DataColumn("imageType", System.Type.GetType("System.String")));

        if (dirID == "root_tree_0")
            dirID = initialDirectory;

        DirectoryInfo rootFolder = new DirectoryInfo(Server.MapPath(dirID.Replace("_", "/")));

        foreach (DirectoryInfo dir in rootFolder.GetDirectories())
        {
            string dirName = dir.Name;
            string dirDateTime = dir.LastAccessTime.ToString("d/M/yyyy h:m:s tt");
            string dirImageType = "Folder";
            dsDir.Tables[0].Rows.Add(new object[] { dirName, 0, "File Folder", dirDateTime, dirImageType });
        }

        foreach (FileInfo file in rootFolder.GetFiles())
        {
            string fileName = file.Name;
            string fileSize = file.Length.ToString();
            string fileType = file.Extension.Replace(".", "");
            string fileImageType = "File";
            string fileDateTime = file.LastAccessTime.ToString("d/M/yyyy h:m:s tt");

            if (fileType == "flv" || fileType == "swf" || fileType == "jpg" || fileType == "mp4")
                dsDir.Tables[0].Rows.Add(new object[] { fileName, fileSize, fileType, fileDateTime, fileImageType });
        }

        gridDir.DataSource = dsDir;
        gridDir.DataBind();
    }
    public void fileup(object o, UploaderEventArgs e)
    {
        ViewState["dirID"] = hfPath.Value;
        e.CopyTo(Request.MapPath(hfPath.Value + "\\" + e.FileName));
        //e.CopyTo(Request.MapPath(lblCurrentDir.Text + "\\" + e.FileName));
        //Response.Write(lblCurrentDir.Text + " - " + hfPath.Value);

    }
    public void refresh(object o, UploaderEventArgs[] e)
    {
        LoadGrid();
    }

}