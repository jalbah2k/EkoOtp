using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EKO_Directory : System.Web.UI.UserControl
{
    public string Parameters;
    protected string _seo = "";
    protected string imgPath { set; get; }
    public EKO_Directory() { }
    public EKO_Directory(string parm) { Parameters = parm; }
    protected void Page_Load(object sender, EventArgs e)
    {
        imgPath = ConfigurationManager.AppSettings["Organizations.Logo.Path"];

        if (
            //!IsPostBack 
            //&& 
            this.Page.RouteData.Values["id"] != null)
        {
            if ((_seo = this.Page.RouteData.Values["id"].ToString()) != "")
            {
                LoadItem();
                return;
            }

            LoadDirectory();
        }
    }
    private void LoadDirectory()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            //string sqlstr = @"select * from eko.Organizations where active=1 and deleted=0 and [Type]=1 order by featured desc, name";
            string sqlstr = @"select * from eko.Organizations where active=1 and deleted=0 and [Type]=1 
                            and OrganizationId != 53 /*EKO Guest*/ 
                            and OrganizationId in (select id from dbo.Organizations where AffiliateMember = 0)
                            order by name; 
                            select * from eko.Organizations where active=1 and deleted=0 and [Type]=1 
                            and OrganizationId != 53 /*EKO Guest*/ 
                            and OrganizationId in (select id from dbo.Organizations where AffiliateMember = 1)
                            order by name";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);

            dapt.Fill(ds);
        }

        repDirectory.DataSource = ds.Tables[0];
        repDirectory.DataBind();

        repAffiliate.DataSource = ds.Tables[1];
        repAffiliate.DataBind();

        pnlAffiliate.Visible = ds.Tables[1].Rows.Count > 0;

        SetView(pnlList);
    }
    protected void repDirectory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;

    
            Literal litLogo = (Literal)e.Item.FindControl("litLogo");
            Literal litContent = (Literal)e.Item.FindControl("litContent");

            HyperLink hlkOrg = (HyperLink)e.Item.FindControl("hlkOrg");
            hlkOrg.NavigateUrl = String.Format("{0}/{1}", ConfigurationManager.AppSettings["Organizations.Page"], dr["seo"].ToString());

            string stemp = "";
         
            if (dr["City"].ToString() != "")
                stemp = "<span class='dirCity'>" + dr["City"].ToString() + "</span>";

            stemp += "<h3>" + dr["name"].ToString() + "</h3>";
            litContent.Text = stemp;

            if (dr["Logo"].ToString() != "")
                litLogo.Text = String.Format("<img src=\"{0}/{1}\" alt=\"{2}\" />", imgPath + dr["id"].ToString(), dr["Logo"].ToString(), dr["AltTextLogo"].ToString());


        }
    }
    private void LoadItem()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = @"select * from eko.Organizations where active=1 and deleted=0 and seo=@seo and [Type]=1
                                select * from eko.OrganizationLocations where OrganizationId in (select id from eko.Organizations where active=1 and deleted=0 and seo=@seo and [Type]=1) ";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@seo", _seo));

            dapt.Fill(ds);
        }

        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            litTitle.Text = "";
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
            //{
            //    string sqlstr = @"select * from dbo.Organizations where id=@id";
            //    SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            //    dapt.SelectCommand.Parameters.Add(new SqlParameter("@id", dr["OrganizationId"].ToString()));

            //    DataTable dto = new DataTable();
            //    dapt.Fill(dto);
            //    if(dto.Rows.Count > 0)
            //    {
            //        if(Convert.ToBoolean( dto.Rows[0]["AffiliateMember"]))
            //            litTitle.Text = "<h2>Empowered Kids Ontario Affiliate Members</h2><br />";
            //        else
            //            litTitle.Text = "<h2>Empowered Kids Ontario General Members</h2><br />";

            //    }
            //}

            if (dr["Logo"].ToString() != "")
                litLogo.Text = String.Format("<div class='dirTopWrap'><img src=\"{0}/{1}\" alt=\"{2}\" />", imgPath + dr["id"].ToString(), dr["Logo"].ToString(), dr["AltTextLogo"].ToString());
            else
                litLogo.Text = "<div class='dirTopWrap'>";

            string stemp = "";
            if (dr["Address"].ToString() != "")
                stemp = dr["Address"].ToString() + ", ";
            if (dr["City"].ToString() != "")
                stemp += dr["City"].ToString() + ", ON, ";
            if (dr["PostalCode"].ToString() != "")
                stemp += dr["PostalCode"].ToString() + "<br>";

            if (ds.Tables[1].Rows.Count > 0)
            {
                stemp += "<a href='#Locations' class='viewOtherLoc'>View Other Locations</a><br>";
            }

            if (dr["PhoneNumber"].ToString() != "")
                stemp += "<span>Phone: </span>" + dr["PhoneNumber"].ToString();
            if (dr["Fax"].ToString() != "")
                stemp += "<div class='dirSep'>•</div><span>Fax: </span>" + dr["Fax"].ToString();
            if (dr["Toll_free"].ToString() != "")
                stemp += "<div class='dirSep'>•</div><span>Toll Free: </span>" + dr["Toll_free"].ToString();
                stemp += "<div class='emailUrlWrap'>";
            if (dr["Email"].ToString() != "")
                stemp += String.Format("<a href='mailto:{0}' target='_blank'>{0}</a>", dr["Email"].ToString()) + "<div class='dirSep'>•</div>";
            if (dr["URL"].ToString() != "")
                stemp += String.Format("<a href='{0}' target='_blank'>{0}</a>", dr["URL"].ToString());
                stemp += "</div>";
            stemp += "<div class='dirSocialWrap'>";
            if (dr["Facebook"].ToString() != "")
                stemp += String.Format("<a href='{0}' class='dirSocialLink' id='dirFacebook' title='{1} Facebook Page' target ='_blank'></a>",
                    dr["Facebook"].ToString(), dr["Name"].ToString());

            if (dr["Instagram"].ToString() != "")
                stemp += String.Format("<a href='{0}' class='dirSocialLink' id='dirInstagram' title='{1} Instagram Page' target ='_blank'></a>",
                     dr["Instagram"].ToString(), dr["Name"].ToString());

            if (dr["Twitter"].ToString() != "")
                stemp += String.Format("<a href='{0}' class='dirSocialLink' id='dirTwitter' title='{1} Twitter Page' target ='_blank'></a>",
                    dr["Twitter"].ToString(), dr["Name"].ToString());

            if (dr["LinkedIn"].ToString() != "")
                stemp += String.Format("<a href='{0}' class='dirSocialLink' id='dirLinkedIn' title='{1} LinkedIn Page' target ='_blank'></a>",
                    dr["LinkedIn"].ToString(), dr["Name"].ToString());

            if (dr["YouTube"].ToString() != "")
                stemp += String.Format("<a href='{0}' class='dirSocialLink' id='dirYouTube' title='{1} YouTube Page' target ='_blank'></a>",
                    dr["YouTube"].ToString(), dr["Name"].ToString());

            stemp += "</div>";



            if (stemp != "")
                litContent.Text = "<div>" + stemp + "</div></div>";

            stemp = "";

            if (dr["AboutUs"].ToString() != "")
                stemp += String.Format("<p>{0}</p></div>", dr["AboutUs"].ToString());

            stemp += "<div id='dirBtns'>";

            if (dr["MissionStatement"].ToString() != "")
                stemp += "<a href='#mission' id='dirMis'>Mission Statement</a>";


            if (dr["OurStory"].ToString() != "")
                stemp += "<a href='#story' id='dirSto'>Our Story</a>";

            if (dr["Services_ClientDemographics"].ToString() != "")
                stemp += "<a href='#services' id='dirSer'>Our Services</a>";

            if (dr["OurPeople_OurTeam"].ToString() != "")
                stemp += "<a href='#team' id='dirPeo'>Our People</a>";

            if (dr["Achievements_AwardsAndRecognition"].ToString() != "")
                stemp += "<a href='#awards' id='dirAwa'>Achievements & Awards</a>";

            if (dr["OurPartners"].ToString() != "")
                stemp += "<a href='#partners' id='dirPar'>Our Partners</a>";

            if (dr["LearnMoreAboutUs"].ToString() != "")
                stemp += "<a href='#learnmore' id='learn-more'>Learn More About Us</a>";

            if (ds.Tables[1].Rows.Count > 0)
            {
                stemp += "<a href='#Locations' id='dirLoc'>Other Locations</a>";
            }

            stemp += "</div>";

           



            if (dr["MissionStatement"].ToString() != "")
                stemp += String.Format("<div id='dirMainWrap'><div class='dirMasonary' ><a name='mission'></a><h2>{0}</h2>", "Mission Statement") + String.Format("<p>{0}</p></div>", dr["MissionStatement"].ToString());


            if (dr["OurStory"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='story'></a><h2>{0}</h2>", "Our Story") + String.Format("<p>{0}</p></div>", dr["OurStory"].ToString());

            if (dr["Services_ClientDemographics"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='services'></a><h2>{0}</h2>", "Services Client Demographics") + String.Format("<p>{0}</p></div>", dr["Services_ClientDemographics"].ToString());

            if (dr["OurPeople_OurTeam"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='team'></a><h2>{0}</h2>", "Our People/Our Team") + String.Format("<p>{0}</p></div>", dr["OurPeople_OurTeam"].ToString());

            if (dr["Achievements_AwardsAndRecognition"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='awards'></a><h2>{0}</h2>", "Achievement/Awards And Recognition") + String.Format("<p>{0}</p></div>", dr["Achievements_AwardsAndRecognition"].ToString());

            if (dr["OurPartners"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='partners'></a><h2>{0}</h2>", "Our Partners") + String.Format("<p>{0}</p></div>", dr["OurPartners"].ToString());

            if (dr["LearnMoreAboutUs"].ToString() != "")
                stemp += String.Format("<div class='dirMasonary'><a name='learnmore'></a><h2>{0}</h2>", "Learn More About Us") + String.Format(" <p>{0}</p></div>", dr["LearnMoreAboutUs"].ToString());

            if (ds.Tables[1].Rows.Count > 0)
            {
                stemp += "<div id='dirLocation'><a name='Locations'></a><h2>Other Locations</h2>";

                stemp += "<div class='dirLoc dirLocSub'>";

                foreach (DataRow dr1 in ds.Tables[1].Rows)
                {
                    stemp += "<div>" +
                        (dr1["Title"].ToString() != "" ? "<h3>" + dr1["Title"].ToString().Trim() + "</h3>" : "") + 
                        dr1["Description"].ToString().Replace(Environment.NewLine, "<br/>") + 
                        "</div>";
                }
                stemp += "</div></div>";

            }

            if (stemp != "")
                litContent.Text += stemp;


            SetView(pnlDetails);
            hlkViewAll.NavigateUrl = ConfigurationManager.AppSettings["Organizations.Page"];

        }
        else
            SetView(pnlList);


    }

    private void SetView(Panel mypanel)
    {
        pnlList.Visible = false;
        pnlDetails.Visible = false;
        mypanel.Visible = true;
    }

}