using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PNCA_Directory : System.Web.UI.UserControl
{

    public string Parameter;
    public PNCA_Directory() { }
    public PNCA_Directory(string p) { Parameter = p; }
    enum Languages { English = 1, French };

    PNCA_ItemTemplate _item;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInId"] != null)
        {
            PNCA_Filters.doSearch += new AfterPNCAFiltersLoaded(LoadDirectory);

            _item = new PNCA_ItemTemplate();

            LoadDirectory();
        }
    }
    private void LoadDirectory()
    {

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {

            PNCA_DirectorySearch dir = new PNCA_DirectorySearch("pnca.Organizations_PublicSearch", CommandType.StoredProcedure, (int)Languages.English);
            dir.ServiceId = PNCA_Filters.ServiceId;
            dir.RegionId = PNCA_Filters.RegionId;
            dir.CityId = PNCA_Filters.CityId;
            dir.Keywords = PNCA_Filters.SearchTerm;

            SqlDataAdapter dapt = dir.Build();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            repDirectory.DataSource = dt;
            repDirectory.DataBind();

            //Literal litHeader = new Literal();
            //if (PNCA_Filters.SearchTerm != "")
            //{
            //    int records = dt.Rows.Count;

            //    litHeader.Text = dir.GetHeaderResult(records, PNCA_Filters.SearchTerm,
            //        (PNCA_Filters.ServiceId != "" ? PNCA_Filters.ServiceName : ""),
            //        ConfigurationManager.AppSettings["Organizations.PNCA.Page"] + "?search_term=" + PNCA_Filters.SearchTerm

            //        );

            //}
            //else if (ds.Tables[1].Rows.Count > 0)
            //{
            //    DataRow dr = ds.Tables[1].Rows[0];
            //    litHeader.Text = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());

            //}

            //plHeader.Controls.Add(litHeader);

        }
    }

    protected void repDirectory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();

            litContent.Text = _item.GetContent(rw);

            ph.Controls.Add(litContent);

        }
    }

}