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

public class Res_ItemTemplate
{
    IDictionary<string, string> icons = new Dictionary<string, string>();
    string _descripDefault;
    string _footer;

    string _id;
    string _title;
    string _icon;
    string _description;
    string _docType;
    bool _favourite;
    bool _isnew;
    bool _isdoc;

    //          •	Video Icon: avi / mp4 / m4v / mov(*player might not support, download only)
    //          •	Image Icon: bmp / gif / jpg / jpeg / png / tiff
    //          •	Document Icon(generic, not word specific): doc / docx / pages / txt / htm / html / pdf / zip / tar /
    //          •	Music Icon: mp3 / m4v / wav
    //          •	Presentation Icon: ppt / pptx / key
    //          •	Spreadsheet Icon: xls / xlsx / csv /
    //          •	Link Icon: web link

    public Res_ItemTemplate()
    {    
        //icons.Add("extension-file", "class-name");

        //Videos
        icons.Add(".avi", "Video");         //adding a key/value using the Add() method
        icons.Add(".mp4", "Video");
        icons.Add(".m4v", "Video");
        icons.Add(".mov", "Video");         //(*player might not support, download only)

        //Images
        icons.Add(".bmp", "Image");
        icons.Add(".gif", "Image");
        icons.Add(".jpg", "Image");
        icons.Add(".jpeg", "Image");
        icons.Add(".png", "Image");
        icons.Add(".tiff", "Image");

        //Document
        icons.Add(".doc", "Document");
        icons.Add(".docx", "Document");
        icons.Add(".page", "Document");
        icons.Add(".txt", "Document");
        icons.Add(".htm", "Document");
        icons.Add(".html", "Document");
        icons.Add(".pdf", "Document");
        icons.Add(".zip", "Document");
        icons.Add(".tar", "Document");

        //Music
        icons.Add(".mp3", "Music");
        icons.Add(".wav", "Music");
        icons.Add(".m4a", "Music");
        
        //Presentation
        icons.Add(".ppt", "Presentation");
        icons.Add(".pptx", "Presentation");
        icons.Add(".key", "Presentation");

        //Spreadsheet
        icons.Add(".xls", "Spreadsheet");
        icons.Add(".xlsx", "Spreadsheet");
        icons.Add(".csv", "Spreadsheet");

        //Link
        icons.Add("link", "Link");

        //Unknown
        icons.Add("unknown", "Unknown");



        _descripDefault = ConfigurationManager.AppSettings.Get("Resources.ItemTemplate.DefaultDescription");
        _footer = ConfigurationManager.AppSettings.Get("Resources.ItemTemplate.Footer");

    }
    private string GetContent(string seo)
    {
        StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("Resources.ItemTemplate")));
        string template = sr.ReadToEnd();
        sr.Close();

        string classes = "";
        string btnDownloadClass = "";

        if (_icon != "")
        {
            classes = _icon;
        }
        else
        {
            try { classes = icons.FirstOrDefault(x => x.Key == _docType.ToString()).Value; }
            catch { }
        }

        if (_favourite)
            classes += " favourite";

        if (_isnew)
            classes += " new";

        if (!_isdoc)
            btnDownloadClass += " btn_hidden";

        string s = String.Format(template,
            _title,
            _footer,
            classes,
            "/" + seo + "/" + _id.ToLower(),
            _description,
            _id,
            btnDownloadClass
            );

        return s;
    }

    public string GetContent(DataRowView rw, string seo)
    {
        _id = rw["id"].ToString();
        _title = rw["Title"].ToString();
        _docType = rw["docType"].ToString();
        _description = rw["Description"].ToString();
        _icon = rw["IconType"].ToString();

        if (String.IsNullOrEmpty(_description))
            _description = _descripDefault;

        _favourite = Convert.ToBoolean(rw["favourite"]);
        _isnew = Convert.ToBoolean(rw["new"]);
        _isdoc = Convert.ToBoolean(rw["IsDocument"]);
        
        return GetContent(seo);
    }

    public string GetFullContent(DataRowView rw, string seo)
    {
        _id = rw["id"].ToString();
        _title = rw["Title"].ToString();
        _docType = rw["docType"].ToString();
        _description = rw["FullDescription"].ToString();
        _icon = rw["IconType"].ToString();

        if (String.IsNullOrEmpty(_description))
            _description = _descripDefault;

        _favourite = Convert.ToBoolean(rw["favourite"]);
        _isnew = Convert.ToBoolean(rw["new"]);
        _isdoc = Convert.ToBoolean(rw["IsDocument"]);

        return GetContent(seo);
    }

    public string GetContent(DataRow rw, string seo)
    {
        _id = rw["id"].ToString();
        _title = rw["Title"].ToString();
        _docType = rw["docType"].ToString();
        _description = rw["Description"].ToString();
        _icon = rw["IconType"].ToString();

        if (String.IsNullOrEmpty(_description))
            _description = _descripDefault;

        _favourite = Convert.ToBoolean(rw["favourite"]);
        _isnew = Convert.ToBoolean(rw["new"]);
        _isdoc = Convert.ToBoolean(rw["IsDocument"]);

        return GetContent(seo);

    }
}
