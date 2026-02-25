using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegexTester : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////var pattern = "<a href=(\"|')https?:\\/\\/(www.)?empoweredkidsontario.ca\b([-a-zA-Z0-9()!@:%_\\+.~#?&\\/\\/=]*)";
        //var pattern = "href=\"https?:\\/\\/(www.)?empoweredkidsontario.ca\\b([-a-zA-Z0-9()!@:%_\\+.~#?&\\/\\/=]*)";

        string input = ReadTXT(Server.MapPath("~/regex/test2.html"));

        string[] week_days = GetWeekDays(5).Split(new char[] { ';' });

        AddUtmToAllLinks(week_days[0], input);

        Response.Write("<br>" + input);

    }


    private  string AddUtmToAllLinks(string week_day, string mybody)
    {
        //https://ihateregex.io/expr/url/
        //regex for url = https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()!@:%_\+.~#?&\/\/=]*)

        var pattern_reg = "href=\"https?:\\/\\/(www.)?empoweredkidsontario.ca\\b([-a-zA-Z0-9()!@:%_\\+.~#?&\\/\\/=]*)";

        MatchCollection matches = Regex.Matches(mybody, pattern_reg);
        Match[] matchArray = matches.Cast<Match>().ToArray();

        string newurl = "";

        string utm_source = String.Format("utm_source=Email+Newsletter&utm_medium=EKO+Mail&utm_id=Week+of+{0}",
                                        week_day.Replace(" ", "+").Replace(",", ""));

        foreach (Match match in matchArray)
        {
            string s = match.Value;
            Response.Write(s + "<br>");

            string closer = match.Value.Contains("\"") ? "\"" : "'";
            string original_url = match.Value;


            if (original_url.Contains("?"))
            {
                string[] substrings = original_url.Split(new char[] { '?' });
                newurl = substrings[0];
                if (substrings.Length > 1)
                {
                    newurl += "?" + substrings[1] + String.Format("&{0}", utm_source);
                }
                else
                    newurl += String.Format("?{0}", utm_source);
            }
            else
            {
                newurl = original_url + String.Format("?{0}", utm_source);
            }

            Response.Write(newurl + "<hr><br>");

        }
        return mybody;
    }
    private static string ReadTXT(string file)
    {
        // System.IO.StreamReader re = new System.IO.StreamReader(file, Encoding.GetEncoding("iso-8859-1"));
        System.IO.StreamReader re = new System.IO.StreamReader(file);
        string _txt = re.ReadToEnd();
        re.Close();
        return _txt;
    }


    private static string GetWeekDays(int offset = 0)
    {
        DateTime firstDay = DateTime.Today.AddDays(-7);
        DateTime startOfWeek = firstDay.AddDays(
          ((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + offset) -
          (int)DateTime.Today.DayOfWeek);

        string result = string.Join(";",        //        "," + Environment.NewLine, 
            Enumerable
            .Range(0, 7)
            .Select(i => startOfWeek
                .AddDays(i)
                .ToString("MMM dd, yyyy")));

        return result;
    }

}