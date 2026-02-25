using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Resurces
/// </summary>

public delegate void AfterPNCAFiltersLoaded();
public class PNCA_DirectorySearch
{
    private FullTextSearch.FullTextSearch myFTS;
    private string[] LCIDs = { "1033", "1036", "1034" };  //1033 - English USA; 3084 - French France; 1034: Spanish Spain

    private SqlDataAdapter dapt;

    public string ServiceId { set; get; }
    public string RegionId { set; get; }
    public string CityId { set; get; }
    public string Keywords { set; get; }
    public int Language { set; get; }

    public PNCA_DirectorySearch() { }
    public PNCA_DirectorySearch(string CommandText, CommandType commType, int lang) 
    {
        Language = lang;
        dapt = new SqlDataAdapter(new SqlCommand(CommandText));
        dapt.SelectCommand.CommandType = commType;
        dapt.SelectCommand.Parameters.AddWithValue("@LCID", LCIDs[lang - 1]);
    }

    public SqlDataAdapter Build()
    {
        if (!String.IsNullOrEmpty(ServiceId))
            dapt.SelectCommand.Parameters.AddWithValue("@ServiceId", ServiceId);

        if (!String.IsNullOrEmpty(RegionId))
            dapt.SelectCommand.Parameters.AddWithValue("@RegionId", RegionId);

        if (!String.IsNullOrEmpty(CityId))
            dapt.SelectCommand.Parameters.AddWithValue("@CityId", CityId);


        if (Keywords != "")
        {
            string searchTerm = Keywords.Trim();

            searchTerm = FTSAux.RemoveNoiseWords(searchTerm, LCIDs[Language - 1]);
            myFTS = new FullTextSearch.FullTextSearch(searchTerm);

            dapt.SelectCommand.Parameters.Add(new SqlParameter("@keywords", myFTS.NormalForm));

        }

        return dapt;
    }

    public string GetHeaderResult(int records, string searc_term, string library, string seo, string category = "", string subcategory = "")
    {
        string temp = "";
#if HEADER
        temp = "<p>There {0} <strong>{1} {2}</strong> found based on your search term <strong>'{3}'</strong>";
        temp = String.Format(temp, (records > 1 ? "are" : "is"), records.ToString(), (records > 1 ? "resources" : "resource"), searc_term);

        //if (subcategory  != "")
        //    temp += " from within the <strong>" + subcategory + "</strong> sub category";
        //else if (category != "")
        //    temp += " from within the <strong>" + category + "</strong> category";
        //else if (library != "")

        if (library != "")
        {
            temp += " from within the <strong>" + library + "</strong> library.</p>";
            temp += String.Format("<p>To clear your filters and view all available resources matching your search term <a href='{0}'>click here</a>.</p>", seo);
        }
        else
            temp += ".</p>";
#endif

        return temp;
    }
}
