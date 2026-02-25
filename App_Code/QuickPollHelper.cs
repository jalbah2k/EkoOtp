using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for QuickPollHelper
/// </summary>
public class QuickPollHelper
{
    public QuickPollHelper()
    {

    }

    public void LoadResult(string mQuestion_id, string lang, out string lbl_Options, out string lbl_Stats, out string lbl_TotalVotes, out string litRel)
    {
        lbl_Options = "";
        lbl_Stats = "";
        lbl_TotalVotes = "";
        litRel = "";

        string mOptions = "", mStats = "";

        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();


        dt3 = mGet_One_Question(Convert.ToInt32(mQuestion_id));

        if (lang == "2")
        {
            if (dt3.Rows[0]["Related_Article_Fr"].ToString().Trim() != "")
            {
                litRel = "<a href=\"" + dt3.Rows[0]["Related_Article_Fr"].ToString() +
                                    "\" target=\"_blank\">" + dt3.Rows[0]["Related_Article_Title_Fr"].ToString() + "</a>";
            }
        }
        else
        {
            if (dt3.Rows[0]["Related_Article"].ToString().Trim() != "")
            {
                litRel = "<a href=\"" + dt3.Rows[0]["Related_Article"].ToString() +
                                    "\" target=\"_blank\">" + dt3.Rows[0]["Related_Article_Title"].ToString() + "</a>";
            }
        }


        int ShowMode = Convert.ToInt32(dt3.Rows[0]["ShowMode"]);
        if (ShowMode == 0)
        {
            lbl_Options = "Thank you for your answer";

            if (lang == "2")
                lbl_Options = "Merci pour avez reponder";

            lbl_Stats = "";
            lbl_TotalVotes = "";
            return;
        }


        bool onlyPercent = ShowMode == 2;

        //for percentage calculation - get total record count
        dt2 = mGet_Submissioin_ByQuestionid(Convert.ToInt32(mQuestion_id));
        decimal mDivider = dt2.Rows.Count;
        dt1 = LoadOptionValues(mQuestion_id, lang, dt1, dt3, mDivider, onlyPercent, out mOptions, out mStats);

        if (!onlyPercent)
        {
            lbl_TotalVotes = "Total: " + mDivider.ToString();
            if (lang == "2")
                lbl_TotalVotes = "Totale: " + mDivider.ToString();
        }
        else
            lbl_TotalVotes = "";

        lbl_Options = mOptions;
        lbl_Stats = mStats;
    }

    private DataTable LoadOptionValues(string mQuestion_id, string lang, DataTable dt1, DataTable dt3, decimal mDivider, bool onlyPercent, out string mOptions, out string mStats)
    {
        DataTable dt = new DataTable();
        dt = mGet_One_Options_ByQuestionid(Convert.ToInt32(mQuestion_id));

        mOptions = "";
        mStats = "";
        decimal mPercentage;
        string s;


        foreach (DataRow dr in dt.Rows)
        {
            s = "0";
            dt1 = mGet_Submissioin_ByOptionid(Convert.ToInt32(mQuestion_id), Convert.ToInt32(dr["id"].ToString()));
            decimal mrowCount = Convert.ToDecimal(dt1.Rows.Count);
            //for % calculation
            if (mDivider != 0)
            {
                mPercentage = mrowCount / mDivider;
                mPercentage = mPercentage * 100;

                s = string.Format("{0:00.00}", mPercentage);
            }

            if (lang == "2")
            {
                mOptions = mOptions + dr["Option_Text_fr"].ToString();
            }
            else
            {
                mOptions = mOptions + dr["Option_Text"].ToString();
            }

            mOptions = mOptions + "<br>";


            if (!onlyPercent)       //dt3.Rows[0]["Show_Percentage"].ToString() == "no")
            {

                mStats = mStats + dt1.Rows.Count;

                if (s != "0")
                {
                    mStats = mStats + " - " + s + " %";
                }
            }
            else
            {
                if (s != "0")
                {
                    mStats = mStats + s + " %";
                }
            }



            mStats = mStats + "<br>";

        }

        return dt1;
    }

    #region DAL
    private DataTable mGet_One_Question(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from ControlQuickPollQuestions where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }

    private DataTable mGet_Submissioin_ByQuestionid(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from ControlQuickPollSubmissions where Question_id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    private DataTable mGet_One_Options_ByQuestionid(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from ControlQuickPollOptions where Question_id = @mID order by Priority asc";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }

    private DataTable mGet_Submissioin_ByOptionid(int mQuestionid, int mOptionid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from ControlQuickPollSubmissions where Question_id = @mQuestionid and Option_id = @mOptionid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mQuestionid", mQuestionid);
            cmd.Parameters.AddWithValue("@mOptionid", mOptionid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }
    #endregion

}