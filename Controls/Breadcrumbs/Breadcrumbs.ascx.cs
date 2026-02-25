using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class Breadcrumbs : System.Web.UI.UserControl
{
    public Breadcrumbs()
    {

    }

    public Breadcrumbs(string parameters)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		/*try
		{
			SqlDataAdapter dapt = new SqlDataAdapter("select text from menuitems where menuid=1 and navigateurl in ( select navigateurl from menuitems where menuid =(select param from content where id = (select content_id from pages_content_zone where Page_ID=@pageid and zone_id=10)))", ConfigurationManager.AppSettings["CMServer"]);
			dapt.SelectCommand.Parameters.AddWithValue("@pageid", Session["PageId"].ToString());
			DataTable dt = new DataTable();
			dapt.Fill(dt);
			//litBreadCrumb.Text = dt.Rows[0][0].ToString();
		}
		catch{
		}*/


		try
		{

            SqlDataAdapter dapt = new SqlDataAdapter("breadcrumbs", ConfigurationManager.AppSettings["CMServer"]);

            //string IsMobileDevice = Request.Browser["IsMobileDevice"];
            //if(IsMobileDevice != "")
            //{
            //    if (Convert.ToBoolean(IsMobileDevice))
            //    {
            //        dapt.SelectCommand.CommandText = "Breadcrumbs_mobile";
            //    }
            //}

            //if(fBrowserIsMobile())
            //    dapt.SelectCommand.CommandText = "Breadcrumbs_mobile";

            DataSet ds = new DataSet();
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@pageid", Session["PageID"].ToString());

          
            dapt.SelectCommand.Parameters.AddWithValue("@css", "breadcrumbs");
            dapt.SelectCommand.Parameters.AddWithValue("@delim", ""); 
            dapt.Fill(ds);

            string s = Convert.ToString(ds.Tables[0].Rows[0][0]);
            int npos = s.LastIndexOf("<div><a>");
            string sub = "";
            string newsub = "";
            if (npos < 0)
            {
                //npos = s.LastIndexOf("<div>");
                //sub = s.Substring(npos);
                //newsub = sub.Replace("<div>", "<div class='div-page-title'><h1>");
                //newsub = newsub.Replace("</div>", "</h1><div>");
            }
            else
            {
                sub = s.Substring(npos);
                newsub = sub.Replace("<div><a>", "<div class='div-page-title'><h1>");
                newsub = newsub.Replace("</a></div>", "</h1><div>");
            }

            string seo = Page.RouteData.Values["seo"].ToString().ToLower();
            if(seo == "memberdirectory" && Page.RouteData.Values["id"].ToString() != "")
            {

                string newvalue = GetMember(Page.RouteData.Values["id"].ToString());

                var regex = new Regex(@"<h1>(.*?)</h1>");
                var match = regex.Match(newsub);
                var result = match.Groups[1].Value;

                //Response.Write(newvalue + "<br>");
                //Response.Write(result + "<br>");
                //Response.Write(newsub + "<br>");
                newsub = newsub.Replace("<h1>" + result.ToString() + "</h1>", "<h1>" + newvalue + "</h1>");
            }

            s = s.Replace(sub, newsub);
            litBreadCrumb.Text = s;
           
        }
		catch (Exception ex) { }

    }

    private string GetMember(string seo)
    {
        string ret = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = @"select name from eko.Organizations where active=1 and deleted=0 and seo=@seo";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@seo", seo));

            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if(dt.Rows.Count > 0)
            {
                ret = dt.Rows[0]["name"].ToString();
            }
        }

        return ret;
    }

    public bool fBrowserIsMobile()
    {
        Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
        {
            var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

            if (u.Length < 4)
                return false;

            if (MobileCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0, 4)))
                return true;
        }

        return false;
    }
}