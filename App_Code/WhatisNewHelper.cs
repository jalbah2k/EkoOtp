using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public static class WhatisNewHelper
{
    public static DataTable LoadPage(int pageNumber, int pageSize, int userid, bool IgnoreLastVisit = false)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter da = new SqlDataAdapter("BreakingNews_Members", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@PageNumber", pageNumber);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);
            da.SelectCommand.Parameters.AddWithValue("@UserId", userid);
            if(IgnoreLastVisit)
                da.SelectCommand.Parameters.AddWithValue("@IgnoreLastVisit", 1);

            da.Fill(dt);
        }

        // SP unions multiple sources; keep only the requested page size
        if (dt.Rows.Count > pageSize)
        {
            DataTable limited = dt.Clone();
            for (int i = 0; i < pageSize; i++)
            {
                limited.ImportRow(dt.Rows[i]);
            }
            return limited;
        }

        return dt;
    }

    public static void GetAction(string type, string id, string title, out string actionText, out string url)
    {
        switch ((type ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "resource":
                actionText = "View resource";
                url = "/resource/" + id;
                break;
            case "watercooler":
                actionText = "Join discussion";
                url = String.Format("/membership/posts/t{0}-{1}", id, title.Trim().Replace(" ", "-"));        //e.g. /membership/posts/t6978-This-is-new-topic-to-use-as-a-test
                break;
            case "news":
            default:
                actionText = "Read news";
                url = "/membernews?newsid=" + id;
                break;
            case "watercoolerreplies":
                actionText = "View thread";
                url = String.Format("/membership/posts/t{0}-{1}", id, title.Trim().Replace(" ", "-"));        //Kishor: you should remove this prefix e.g.'3 replies in: '
                break;


        }
    }
}
