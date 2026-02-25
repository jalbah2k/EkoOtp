using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using obout_ASPTreeView_2_NET;

public partial class oTreeMenu : System.Web.UI.UserControl
{
    private Tree oTree = new Tree();
    private DataTable dt = new DataTable();
    
    public string selectedNode
    { set; get;}

    #region DAL
    public DataTable mGet_MenuItems_ByMenuid(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select distinct mi.id,menuid,parentid,priority,case when LEN(text)>20 then SUBSTRING(text,0,20)+'...' else text end as text,tooltip,navigateurl,pageid,target,visible,enabled,mi.linkid,name,mi.linkedmenuid from Menuitems mi inner join Menus m on mi.menuid = m.id INNER JOIN Menu_Group AS mg ON mi.id = mg.MenuItem_id where menuid=@mID order by priority";

        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
           // cmd.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }
    public DataTable mGet_RootMenuItems(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select id,name from Menus where language ='1' and id=@id";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }
    #endregion

    public oTreeMenu()
    {
        selectedNode = "";
    }

    public void PopulateFields( string mMenuid, bool Expanded)
    {
        dt = mGet_MenuItems_ByMenuid(int.Parse(mMenuid));
        litTreeView.Text = PopulateFields(Expanded, mMenuid);
    }

    private string PopulateFields( bool Expanded, string id)
    {
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            oTree.id = "tree_" + dr["menuid"].ToString();
            string root_id = "menuid_" + dr["menuid"].ToString();

            oTree.Add("root", root_id, dr["name"].ToString(), Expanded, "orangesquare.png", null);
            if (selectedNode.Contains("menuid_"))
            {
                if (selectedNode.Substring(selectedNode.IndexOf("_") + 1) == dr["menuid"].ToString())
                {
                    oTree.SelectedId = root_id;
                }
            }

            oTree.DragDisableId = root_id;

            ExportToTreeview(null, Expanded, root_id);

            oTree.FolderIcons = "libraries/tree2/icons";
            oTree.FolderScript = "libraries/tree2/script";
            oTree.FolderStyle = "libraries/tree2/style/Classic";

            oTree.Width = "200px";
            oTree.SelectedEnable = true;
            oTree.DragAndDropEnable = true;

            // Write treeview to your page.
        }
        else
        { 
             DataTable dt1 = mGet_RootMenuItems(id);
             if (dt1.Rows.Count > 0)
             {
                DataRow dr1 = dt1.Rows[0];
                oTree.id = "tree_" + dr1["id"].ToString();
                string root_id2 = "menuid_" + dr1["id"].ToString();

                oTree.Add("root", root_id2, dr1["name"].ToString(), Expanded, "orangesquare.png", null);
                if (selectedNode.Contains("menuid_"))
                {
                     if (selectedNode.Substring(selectedNode.IndexOf("_") + 1) == dr1["id"].ToString())
                     {
                         oTree.SelectedId = root_id2;
                     }
                 }

                oTree.DragDisableId = root_id2;

                oTree.FolderIcons = "libraries/tree2/icons";
                oTree.FolderScript = "libraries/tree2/script";
                oTree.FolderStyle = "libraries/tree2/style/Classic";

                oTree.Width = "200px";
                oTree.SelectedEnable = true;
                oTree.DragAndDropEnable = true;
             }            
        }
        return oTree.HTML();

    }
    
    private void ExportToTreeview(int? Id, bool Expanded, string parent_node)
    {
        string expression = "";

        if (Id != null)
            expression = "[parentid] = " + Id.ToString();

        DataRow[] foundRows = dt.Select(expression, "parentid");
        if (foundRows.Length == 0)
            return;

        foreach (DataRow dr in foundRows)
        {
            if (Id == null && foundRows[0]["parentid"].ToString() != dr["parentid"].ToString())
            {
                //Only root elements are serialized
                break;
            }
            else if(Id == null && !string.IsNullOrEmpty(parent_node))
            {
                oTree.Add(parent_node, dr["id"].ToString(), dr["text"].ToString(), Expanded, "orangeball.png", null);
            }
            else
            {
                oTree.Add(dr["parentid"].ToString(), dr["id"].ToString(), dr["text"].ToString(), Expanded, "orangeball.png", null);
            }

            if ( selectedNode == dr["id"].ToString())
            {
                oTree.SelectedId = selectedNode;
            }

            if (dr["linkedmenuid"] != DBNull.Value)
                continue;
            
            ExportToTreeview(int.Parse(dr["id"].ToString()), Expanded, "");
        }

        return;
    }

     
}