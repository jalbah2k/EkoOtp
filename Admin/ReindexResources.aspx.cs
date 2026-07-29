using System;
using System.Configuration;
using System.Web.UI;
using EkoSearch;

public partial class Admin_ReindexResources : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!(Session["LoggedInID"] != null && Session["LoggedInID"].ToString() == "1"))
        //{
        //    Response.Redirect(String.Format("/Membership/Account/Login?ap=cms&g=admin&t={0}{1}", Request.QueryString["c"],
        //        !String.IsNullOrEmpty(Request.QueryString.ToString()) ? "&q=" + Request.QueryString.ToString() : ""));

        //}

        // Unattended trigger for Task Scheduler:
        //   /Admin/ReindexResources?run=1&token=THE_SECRET
        if (!IsPostBack && Request.QueryString["run"] == "1")
        {
            string expected = ConfigurationManager.AppSettings["Search.Reindex.Token"];
            string given    = Request.QueryString["token"];

            // constant-ish comparison; require a configured token
            if (string.IsNullOrEmpty(expected) || !SlowEquals(expected, given))
            {
                Response.StatusCode = 403;
                Response.Write("Forbidden");
                Response.End();
                return;
            }

            RunIndexer();      // writes a plain-text result the scheduler can log
            Response.End();
        }
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        RunIndexer();
    }

    private void RunIndexer()
    {
        Server.ScriptTimeout = 600;   // first full run can take minutes
        var r = ResourceEmbeddingIndexer.RunAll();

        string msg = (r.Error != null)
            ? "ERROR: " + r.Error
            : "Embedded " + r.Embedded + ", removed " + r.Removed + ".";

        // For the scheduled call, emit plain text (shows up in curl/PS output/logs)
        if (Request.QueryString["run"] == "1")
        {
            Response.ClearContent();
            Response.ContentType = "text/plain";
            Response.Write(msg);
        }
        else
        {
            litResult.Text = "<p>" + Server.HtmlEncode(msg) + "</p>";
        }
    }

    // Length-constant comparison so the token can't be guessed via timing.
    private static bool SlowEquals(string a, string b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
        return diff == 0;
    }
}