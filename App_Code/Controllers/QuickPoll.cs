using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Http;

public class QuickPollController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int id, int lang)
    {
        string lbl_Options = "";
        string lbl_Stats = "";
        string lbl_TotalVotes = "";
        string litRel = "";

        QuickPollHelper qh = new QuickPollHelper();
        qh.LoadResult(id.ToString(), lang.ToString(), out lbl_Options, out lbl_Stats, out lbl_TotalVotes, out litRel);

        StringBuilder sb = new StringBuilder("");
        sb.Append("<div><p>" + lbl_Options + "</p></div>");    
        sb.Append("<div><p>" + lbl_Stats + "</p></div>");
        sb.Append("<div><p>" + lbl_TotalVotes + "</p></div>");
        if (litRel != "")
            sb.Append("<div class=\"divRelatedArticle\"><h3>" + (lang == 1 ? "RELATED ARTICLE" : "Article connexe").ToUpper() + "</h3>" + litRel + " </div>");

        return sb.ToString();
    }

    // POST api/<controller>
    public void Post([FromBody]dynamic value)
    {
        string id = value["param"];
        try { int.Parse(id); }
        catch { return; }

        string mValue = value["val"];
        try { int.Parse(mValue); }
        catch { return; }

        string commandString = " insert into  ControlQuickPollSubmissions (Question_id, Option_id, Date_Created) values   (@Question_id, @Option_id, getdate())";
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@Question_id", id);
            cmd.Parameters.AddWithValue("@Option_id", mValue);
            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();
        }
    }


    // PUT api/<controller>/5
    public void Put(string name, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }

}
