using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class TableWidget : System.Web.UI.UserControl
{
    public string Parameter;

    public TableWidget()
    {

    }

    public TableWidget(string p)
    {
        Parameter = p;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ((_Default)this.Page).InjectContent("head", "<link rel=\"stylesheet\" href=\"/Controls/TableWidget/table-widget.css\" />" + Environment.NewLine, (int)CMSHelper.counters.table_header);
        ((_Default)this.Page).InjectContent("Scripts", "<script src=\"/Controls/TableWidget/table.heading.js\"></script>", (int)CMSHelper.counters.table_footer);

        if (!IsPostBack)
        {
            string sqlstr = @"SELECT        r.RowNo, r.id as rowid, t.Title, t.SingleHeader
                                FROM            TableWidget.Tables AS t INNER JOIN
                                                         TableWidget.Rows AS r ON t.Id = r.TableId
						                                 where r.TableId=@tableid 
						                                 order by r.Priority

                                SELECT        Id, Caption
                                FROM            TableWidget.Columns AS c
                                where TableId = @tableid
                                ORDER BY Priority

                                SELECT        v.RowId, v.ColumnId, v.Value, c.Caption
                                FROM            TableWidget.Cells AS v INNER JOIN
                                                         TableWidget.Columns AS c ON v.ColumnId = c.Id INNER JOIN
                                                         TableWidget.Rows AS r ON v.RowId = r.Id
                                WHERE        (r.TableId = @tableid)
                                ORDER BY r.Priority, c.Priority";

            DataSet ds = new DataSet();
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@TableId", Parameter);
            ds = getDataset(sq);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow drow = ds.Tables[0].Rows[0];
                bool bSingleHeader = Convert.ToBoolean(drow["SingleHeader"]);
                string stemp = "<table id=\"TableW_{0}\" class=\"tableStack singleheader\" cellspacing=\"0\" cellpadding=\"0\" border='1'><caption>{1}</caption><tbody><tr>{2}</tr>";

                if(!bSingleHeader)
                    stemp = "<table id=\"TableW_{0}\" class=\"tableStack doubleheader\" cellspacing=\"0\" cellpadding=\"0\" border='1'><caption>{1}</caption><tbody><tr>{2}</tr>";

                int i = 0;
                string headers = "";
                foreach (DataRow dcol in ds.Tables[1].Rows)
                {
                    if (!bSingleHeader && i == 0)
                        headers = "<td></td>";
                    else
                        headers += "<th>" + dcol["Caption"].ToString() + "</th>";

                    i++;
                }

                stemp = String.Format(stemp, Parameter, drow["Title"].ToString(), headers);


                foreach (DataRow drw in ds.Tables[0].Rows)
                {
                    string row = "<tr>";
                    int j = 0;
                    foreach (DataRow dr in ds.Tables[2].Select("RowId=" + drw["rowid"].ToString()))
                    {
                        if (!bSingleHeader && j == 0)
                            row += "<th>" + dr["value"].ToString() + "</th>";
                        else
                            row += "<td>" + (dr["value"].ToString() != "" ? dr["value"].ToString() : "&nbsp;") + "</td>";

                        j++;
                    }
                    stemp += row + "</tr>";

                }

                stemp += "</tbody></table>";
                litOutputTable.Text = stemp;
            }
        }
    }

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    private DataSet getDataset(SqlCommand cmd)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }
}