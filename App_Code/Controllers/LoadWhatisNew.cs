using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Http;

public class LoadWhatisNewController : ApiController
{
    // GET api/LoadWhatisNew/{pageNumber}/{pageSize}/{publish}/{category}
    public string Get(int offset, int records, int publish = 0, int category = 0)
    {
        DataTable dt = WhatisNewHelper.LoadPage(offset, records);

        if (dt.Rows.Count == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            string type = Convert.ToString(dr["Type"]);
            string id = Convert.ToString(dr["Id"]);
            string title = Convert.ToString(dr["Title"]);
            string dateText = string.Empty;

            if (dr["ActivityDate"] != DBNull.Value)
            {
                DateTime activityDate = Convert.ToDateTime(dr["ActivityDate"], CultureInfo.InvariantCulture);
                dateText = activityDate.ToString("MMM dd", CultureInfo.InvariantCulture);
            }

            string actionText;
            string url;
            WhatisNewHelper.GetAction(type, id, out actionText, out url);

            sb.Append("<div class=\"whats-new-row\">");
            sb.AppendFormat("<span class=\"whats-new-type\">{0}</span>", HttpUtility.HtmlEncode(type));
            sb.AppendFormat("<span class=\"whats-new-title\">{0}</span>", HttpUtility.HtmlEncode(title));
            sb.AppendFormat("<span class=\"whats-new-date\">{0}</span>", HttpUtility.HtmlEncode(dateText));
            sb.AppendFormat(
                "<a href=\"{0}\" class=\"whats-new-action\" title=\"{1}\">{2}</a>",
                HttpUtility.HtmlAttributeEncode(url),
                HttpUtility.HtmlAttributeEncode(title),
                HttpUtility.HtmlEncode(actionText));
            sb.Append("</div>");
        }

        return sb.ToString();
    }
}
