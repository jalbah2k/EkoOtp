using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Resurces
/// </summary>

public class PNCA_ItemTemplate
{
    IDictionary<string, string> icons = new Dictionary<string, string>();
    string _descripDefault;
    string _footer;

    string _id;
    string _name;
    string _region;
    string _logo;
    string _alt;
    string _website;
    string _admincontact;
    string _emails;
    string _services;


    public PNCA_ItemTemplate()
    {    

    }
    private string GetContent()
    {
        StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("Organizations.PNCA.ItemTemplate")));
        string template = sr.ReadToEnd();
        sr.Close();

        string emails = "";
        foreach (string s in _emails.Split(new char[] { ';' }))
        {
            if (emails != "")
                emails += ", ";

            emails += String.Format("<a href='mailto:{0}'>{0}</a>", s);
        }

        string contact = _admincontact + ((_admincontact != "" && emails != "") ? ", " + emails : "");
        contact = contact != "" ? contact : "";

        _website = _website != "" ? String.Format("<a href='{0}' alt='_blank'>{0}</a>", _website) : "";

        string img = "";
        if (_logo != "")
            img = String.Format("<img src='{0}/{1}' alt='{2}' title='{2}' />", ConfigurationManager.AppSettings["Organizations.PNCA.Logo.Path"] + _id, _logo, _alt);

        string myservices = "";
        string[] stemps = _services.Replace(", ", ",").Split(new char[] { ',' });
        foreach(string s in stemps)
        {
            myservices += "<div class='pill'>" + s + "</div>";
        }

        string stemp = String.Format(template,
              _region,
              _name,
              contact,
              _website,
              myservices,
              img
              );

        return stemp;
    }

    public string GetContent(DataRowView rw)
    {
        _id = rw["id"].ToString();
        _name = rw["name"].ToString();
        _region = rw["region"].ToString();
        _emails = rw["email"].ToString();
        _admincontact = rw["AdminContact"].ToString();
        _website = rw["website"].ToString();
        _logo = rw["Logo"].ToString();
        _alt = rw["AltTextLogo"].ToString().Replace("'", "’");
        _services = rw["services"].ToString();
        
        return GetContent();
    }

    public string GetContent(DataRow rw)
    {
        _id = rw["id"].ToString();
        _name = rw["name"].ToString();
        _region = rw["region"].ToString();
        _emails = rw["email"].ToString();
        _admincontact = rw["AdminContact"].ToString();
        _website = rw["website"].ToString();
        _logo = rw["Logo"].ToString();
        _alt = rw["AltTextLogo"].ToString().Replace("'", "’");
        _services = rw["services"].ToString();

        return GetContent();
    }

}
