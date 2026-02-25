using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class EKO_Directory_Logos : System.Web.UI.UserControl
{
    #region Properties

    public int LogosQty = 0;
    protected string imgPath { set; get; }

    private bool _autoplay = true;
    public bool Autoplay
    {
        get
        {
            return _autoplay;
        }
        set
        {
            _autoplay = value;
        }
    }

    private bool _shuffle = false;
    public bool Shuffle
    {
        get
        {
            return _shuffle;
        }
        set
        {
            _shuffle = value;
        }
    }

    public string Parameters;

    #endregion Properties
    public EKO_Directory_Logos() { }
    public EKO_Directory_Logos(string par) { Parameters = par; }
    protected void Page_Load(object sender, EventArgs e)
    {
        imgPath = ConfigurationManager.AppSettings["Organizations.Logo.Path"];

        LoadGallery();

    }

    private void LoadGallery()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlstr = "select id, Name, URL, Logo, AltTextLogo, seo from eko.Organizations where isnull(Logo, '') != '' and active=1 and deleted=0 and [Type]=1 ORDER BY NEWID()";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.Fill(dt);
        }

        LogosQty = dt.Rows.Count;

        if (LogosQty > 0)
        {
            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "myslick responsive");
            divRow.ID = "DirectoryLogos_" + Parameters;

            Literal litContent = new Literal();

            foreach (DataRow dr in dt.Rows)
            {
                string url = dr["URL"].ToString();

                //string stemp = "<div{0}>&nbsp;</div>";
                //stemp = String.Format(stemp, " class='innerHomeBanner'");

                //litContent.Text += String.Format("<div{1}>{0}</div>",
                //    stemp,
                //    String.Format(" style=\"background:url('{0}{1}/{2}') no-repeat; \"",
                //    imgPath, dr["id"].ToString(), dr["Logo"].ToString()));

                string alttext = dr["AltTextLogo"].ToString();
                if (alttext == "")
                {
                    alttext = "Logo of '" + dr["name"].ToString() + "'";
                }
                string stemp = String.Format("<div><img src=\"{0}/{1}\" alt=\"{2}\" /></div>", imgPath + dr["id"].ToString(), dr["Logo"].ToString(),
                    alttext);

                
                //if (url != "")
                //{
                //    stemp = String.Format("<a href=\"{1}\" target=\"_blank\">{0}</a>", stemp, url);
                //}
                //else
                {
                    stemp = String.Format("<a href=\"{0}/{1}\" >{2}</a>", ConfigurationManager.AppSettings["Organizations.Page"], dr["seo"].ToString(), stemp);
                }


                litContent.Text += stemp;
            }

            divRow.Controls.Add(litContent);
            plhGallery.Controls.Add(divRow);

            //https://kenwheeler.github.io/slick/

            string script = @"$(document).ready(function () {
                            $('{0}.myslick.responsive').slick({
                                dots: false,
                                infinite: true,
                                autoplay: {1},
                                speed: {4},
                                autoplaySpeed: {2},
                                arrows: {3},
                                slidesToShow: 5,
                                slidesToScroll: 4,
                                responsive: [
                                    {
                                        breakpoint: 1024,
                                        settings: {
                                            slidesToShow: 3,
                                            slidesToScroll: 3,
                                            infinite: true,
                                            dots: false
                                        }
                                    },
                                    {
                                        breakpoint: 600,
                                        settings: {
                                            slidesToShow: 2,
                                            slidesToScroll: 2
                                        }
                                    },
                                    {
                                        breakpoint: 480,
                                        settings: {
                                            slidesToShow: 1,
                                            slidesToScroll: 1
                                        }
                                    }
                                ]
                             });
                        });" + Environment.NewLine;

            script = script.Replace("{0}", "#" + divRow.ClientID);
            script = script.Replace("{1}", Autoplay ? "true" : "false");
            script = script.Replace("{2}", "3000");
            script = script.Replace("{4}", "600");
            script = script.Replace("{3}", LogosQty > 1 ? "true" : "false");


            ((_Default)this.Page).InjectContent("Scripts", script, true);

        }

    }

}