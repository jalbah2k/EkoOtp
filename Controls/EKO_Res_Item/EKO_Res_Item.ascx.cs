#define NEW_PDF_API
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Res_Item : System.Web.UI.UserControl
{
    public string Parameter;
   //string PDF_clientId = "e9fb2b79971847c58e3e47e95eb20530";           //OLD
    string PDF_clientId = "453c3fd9338642cb90580aeff195a519";

    
    public EKO_Res_Item() 
    {
        IsPdf = IsVideo = false;
    }
    public EKO_Res_Item(string p) 
    { 
        Parameter = p; 
        IsPdf = IsVideo = false;
        IsImage = false;
    }

    #region Properties
    protected string _seo = "";
    private string ResourceID
    {
        set { ViewState["ResourceID"] = value; }
        get
        {
            if (ViewState["ResourceID"] != null)
                return ViewState["ResourceID"].ToString();
            else
                return "0";
        }
    }

    public bool IsVideo
    {
        set { ViewState["IsVideo"] = value; }
        get { return Convert.ToBoolean(ViewState["IsVideo"]); }
    }
    public bool IsPdf
    {
        set { ViewState["IsPdf"] = value; }
        get { return Convert.ToBoolean(ViewState["IsPdf"]); }
    }
    public bool IsImage
    {
        set { ViewState["IsImage"] = value; }
        get { return Convert.ToBoolean(ViewState["IsImage"]); }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();

        _linktopage = ConfigurationManager.AppSettings.Get("Resources.Page.Details");
        _item = new Res_ItemTemplate();

        if ( this.Page.RouteData.Values["id"] != null 
            //&& hfFavourite.Value == ""
            )
        {
            Populate();
        }

        //btnFavourite.Attributes.Add("onclick", String.Format("javascript:$('#{0}').val('1')", hfFavourite.ClientID));

        if (IsPostBack)
        {
            string resourceId = hfDownloadId.Value.Replace("btnDownload_", "");
            hfDownloadId.Value = "";
            DownloadFile(resourceId);
        }
    }

    private void DownloadFile(string resourceId)
    {
        ResourceSearch res = new ResourceSearch();
        res.DownloadFile(resourceId, Session["LoggedInId"].ToString());
    }

    private void Populate()
    {
        string seo = "";
        if ((seo = this.Page.RouteData.Values["id"].ToString()) != "")
        {
            Populate(seo);
        }
    }

    private void Populate(string seo)
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Resource_Get", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", seo);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
            dapt.SelectCommand.Parameters.AddWithValue("@Status", DBNull.Value);
            dapt.SelectCommand.Parameters.AddWithValue("@Show", DBNull.Value);

            dapt.Fill(ds);

            #region Resource Details
            DataTable dt = ds.Tables[0];


            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                ResourceID = dr["id"].ToString();

                string s = "<a href='/reslibrary'>Resources</a>> <h1>{0}</h1> <strong>Added on: {1}</strong>";
                EKO_Breadcrumbs1.Content = String.Format(s, dr["Title"].ToString(), Convert.ToDateTime(dr["CreatedDate"]).ToString("MMMM dd, yyyy"));

                this.Page.Title += " - " + dr["Title"].ToString();

                if (dr["Description"].ToString() != "")
                {
                    Literal litDescription = new Literal();
                    litDescription.Text = "<p>" + dr["Description"].ToString() + "</p>";
                    plBody.Controls.Add(litDescription);
                }

                btnFavourite.Attributes.Add("class", Convert.ToBoolean(dr["Favourite"]) ? "favBtn button favourite" : "favBtn button");
                btnFavourite.Attributes.Add("values", ResourceID + Session["LoggedInId"].ToString() + "&" + ResourceID.Length);

                StringBuilder script = new StringBuilder();

                if (Convert.ToBoolean(dr["IsDocument"]))
                {
                    script.Append(ShowDocument(dr));
                }
                else if (dr["docType"].ToString() == "link")
                {
                    btnDownload.Visible = false;
                    hlkView.Visible = true;
                    hlkView.Text = "view website";
                    hlkView.NavigateUrl = dr["URL"].ToString().Trim();
                    hlkView.Target = "_blank";
                }
                //else
                //{
                //    btnDownload.Visible = false;
                //    hlkView.Visible = true;
                //    hlkView.Text = "view";
                //    hlkView.NavigateUrl = String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString());
                //}

                script.Append(Environment.NewLine + "<script src=\"/controls/EKO_Res_Item/LikeIt.js\"></script>" + Environment.NewLine);

                ((_Default)this.Page).InjectContent("Scripts", script.ToString(), false);


            }
            else
            {
                btnDownload.Visible = hlkView.Visible = btnFavourite.Visible = false;
            }
            #endregion

            #region Last View
            if (ds.Tables[3].Rows.Count == 1)
            {
                Literal litDate = new Literal();
                litDate.Text = String.Format("<strong>You last viewed this on: {0}</strong>", Convert.ToDateTime(ds.Tables[3].Rows[0]["ActionDate"]).ToString("MMMM dd, yyyy"));
                plBody.Controls.Add(litDate);
            }
            #endregion

            #region All libraries where this Resource is available
            if (ds.Tables[1].Rows.Count > 0)

            {
                Literal litLibs = new Literal();
                string s = "<br><br><strong>This resource is available in:</strong> <span class='availLibs'>{0}</span>";
                string stemp = "";

                int i = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (i > 0)
                        stemp += ", ";

                    if (Convert.ToBoolean(dr["islocked"]))
                        stemp += String.Format("<span class='lock'>{0}</span>", dr["name"].ToString());
                    else
                        stemp += String.Format("<a href='{0}'>{1}</a>", "/" + "reslibrary" + "/" + dr["seo"].ToString().ToLower(), dr["name"].ToString());

                    i++;
                }

                litLibs.Text = String.Format(s + "<br><br>", stemp);
                plBody.Controls.Add(litLibs);
            }
            #endregion

            #region Associated resources
            if(pnlAssociated.Visible = ds.Tables[2].Rows.Count > 0)
            {
                litAssociated.Text = "<br /><h2>Related Resources</h2>";

                repeaterResources.DataSource = ds.Tables[2];
                repeaterResources.DataBind();

            }
            else
                litAssociated.Text = "";


            #endregion

        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        ResourceSearch res = new ResourceSearch();
        res.DownloadFile(ResourceID, Session["LoggedInId"].ToString());
    }

    protected void btnFavourite_Click(object sender, EventArgs e)
    {
        //using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@ResourceId", ResourceID));
        //    parms.Add(new SqlParameter("@userid", Session["LoggedInId"].ToString()));

        //    MyDAL.ExecuteNonQuery("update ResourcesUsers_Link set Favourite = case when Favourite = 1 then 0 else 1 end where ResourceId=@ResourceId and UserId=@userid ",
        //            parms.ToArray(),
        //            CommandType.Text,
        //            conn);

        //    Populate();
        //    hfFavourite.Value = "";
        //}
    }

    private string ShowDocument( DataRow dr)
    {
        StringBuilder scrp = new StringBuilder();

        //lnkView.Visible = false;
        if ( (dr["mime"].ToString().ToLower().Contains("video") || dr["mime"].ToString().ToLower().Contains("audio"))  && Convert.ToBoolean(dr["Viewable"].ToString()))
        {
            IsVideo = true;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ResourceId", ResourceID));
            parms.Add(new SqlParameter("@Status", 1));
            parms.Add(new SqlParameter("@Show", 1));
            parms.Add(new SqlParameter("@UserId", Session["LoggedInId"].ToString()));
            DataTable dtvideo = MyDAL_Resources.getSTable("Pie.Document_select", parms);

            if (dtvideo.Rows.Count > 0)
            {
                DataRow rw = dtvideo.Rows[0];
#if CODE64
                        Byte[] bytes = (Byte[])rw["content"];
                        string source = Convert.ToBase64String(bytes);
                        source = String.Format("data:{1};base64,{0}", source, rw["MIMEType"].ToString()) ;
#else
                string source = "";
                string width = "100%";
                string height = "auto";
                if (dr["mime"].ToString().ToLower().Contains("audio"))
                {
                    Byte[] bytes = (Byte[])rw["content"];
                    source = Convert.ToBase64String(bytes);
                    source = String.Format("data:{1};base64,{0}", source, rw["MIMEType"].ToString());
                    width = "575";
                    height = "45";
                }
                else
                {
                    ////string source = "/uploads/Data Management and Privacy Considerations for Private Pay Services (Kathryn Frelick March 10, 2020).mp4";
                    //source = ConfigurationManager.AppSettings["Resources.Video.Path"] + rw["path"].ToString().Replace("/Resources", "") + rw["FileName"].ToString();
                    source = rw["path"].ToString() + rw["FileName"].ToString();
                    source = source.Replace("//", "/");

                   // / Data / Resources / Data / 7652 / GMT20221025 - 140125_Recording_1760x900.mp4
                }
#endif
                litVideo.Text = String.Format("<div class='row row-video'><video poster=\"{0}\" id=\"video_{1}\" width=\"{4}\" height=\"{5}\" controls><source src=\"{2}\" type=\"{3}\"><track default kind=\"subtitles\" srclang=\"en\" src=\"{6}\" /></video></div><br>",
                                        "", "video_" + rw["id"].ToString(), source, rw["MIMEType"].ToString(), width, height, source);
            }
        }
        else if ( dr["docType"].ToString() == ".pdf")
        {
            IsPdf = true;

#if NEW_PDF_API           
            //string scr = @"<script type='text/javascript'>
            //                document.addEventListener('adobe_dc_view_sdk.ready', function()
            //                {
            //                    var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view'});
            //                    adobeDCView.previewFile(
            //                    {
            //                        content: { location: { url: '{0}' } },
            //                        metaData: { fileName: '{1}'}
            //                    });
            //                });
            //                </script>";

            //https://acrobatservices.adobe.com/view-sdk-demo/index.html#/customize/FULL_WINDOW/Bodea%20Brochure.pdf

            string scr = @"<script type='text/javascript'>
                            document.addEventListener('adobe_dc_view_sdk.ready', function () {
                                var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view' });
                                adobeDCView.previewFile(
                                    {
                                        content: { location: { url: '{0}' } },
                                        metaData: { fileName: '{1}' }
                                    },
                                    { defaultViewMode: 'FIT_WIDTH', showAnnotationTools: false, showDownloadPDF: false,
                                      showPrintPDF: false }
                                );
                            });
                        </script>";


            scr = scr.Replace("'", "\"");
            scr = scr.Replace("{0}", String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString()));
            scr = scr.Replace("{1}", dr["FileName"].ToString());
            scr = scr.Replace("{2}", PDF_clientId);

            scrp.Append(Environment.NewLine + "<script src=\"https://documentservices.adobe.com/view-sdk/viewer.js\"></script>" + Environment.NewLine);
#else
            string scr = @"<script>
                            document.addEventListener('adobe_dc_view_sdk.ready', function(){
                                var adobeDCView = new AdobeDC.View({ clientId: '{2}', divId: 'adobe-dc-view'});
                                adobeDCView.previewFile({
                                    content: { location: { url: '{0}' } },
                                    metaData: { fileName: '{1}'}
                                    }, { });
                            });
                            </script>";   

            scr = scr.Replace("{0}", String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString()));
            scr = scr.Replace("{1}", dr["FileName"].ToString());
            scr = scr.Replace("{2}", PDF_clientId);

            scrp.Append(Environment.NewLine + "<script src=\"https://documentcloud.adobe.com/view-sdk/main.js\"></script>" + Environment.NewLine);

#endif
            scrp.Append(Environment.NewLine + scr + Environment.NewLine);

            btn_newtab.HRef = String.Format("{0}://{1}/OpenFile.ashx?id={2}", Request.Url.Scheme, Request.Url.DnsSafeHost, dr["id"].ToString());
            btn_newtab.Target = "_blank";
            btn_newtab.Visible = true;
        }
        else if (dr["mime"].ToString().ToLower().Contains("image"))
        {
            IsImage = true;

            imgPhoto.ImageUrl = "/Controls/EKO_Res_Item/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=200";
            imgPhoto.AlternateText = dr["Title"].ToString() + " image";

        }
        return scrp.ToString();
    }

#region Associated Resources
    protected string _linktopage = "";
    Res_ItemTemplate _item;

    protected void repeaterResources_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();

            litContent.Text = _item.GetContent(rw, _linktopage);

            ph.Controls.Add(litContent);

        }
    } 
#endregion

}

