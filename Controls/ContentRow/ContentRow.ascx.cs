using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContentRow : System.Web.UI.UserControl
{
    //https://github.com/kenwheeler/slick/issues/1730
    //https://github.com/kenwheeler/slick/issues/1933


    public string parameter;

    public string ClassName;

    public string SubrowClass;
    public ContentRow() { }
    public ContentRow(string p) { parameter = p; }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadZones();
    }

    private void LoadZones()
    {
        SqlDataAdapter cmserver;
        DataTable dt = new DataTable();

        cmserver = new SqlDataAdapter(@"SELECT pcr.ContentRow_ID, pcr.Content_ID, cr.name, cr.[columns], cr.color, cr.class, pcr.Priority, c.control, c.param, cr.subrow_class 
                                        FROM     Pages_ContentRow AS pcr INNER JOIN
                                                          ContentRow AS cr ON cr.id = pcr.ContentRow_ID INNER JOIN
						                                  Content c on c.id = pcr.Content_ID
                                        where ContentRow_ID = @id and base = 0 order by priority", 

        ConfigurationManager.AppSettings["CMServer"]);
        cmserver.SelectCommand.CommandType = CommandType.Text;
        cmserver.SelectCommand.Parameters.AddWithValue("@id", parameter);

        cmserver.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            ClassName = dt.Rows[0]["class"].ToString();
            SubrowClass = dt.Rows[0]["subrow_class"].ToString();

            foreach (DataRow dr in dt.Rows)
            {
                UserControl userControl = LoadControlExtension.LoadControl(this, String.Format("~/Controls/{0}/{0}.ascx", dr["control"].ToString()), dr["param"].ToString());
                MainRow.Controls.Add(userControl);
            }
        }
    }
}