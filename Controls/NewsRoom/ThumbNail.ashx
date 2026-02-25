<%@ WebHandler Language="C#" Class="ThumbNail" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Net;
using System.Configuration;

public class ThumbNail : IHttpHandler {

    private SqlBinary GetBinary(int PictureID)
    {

        SqlConnection conn = null;

        //try
        {

            //Connect to the database and bring back the image contents & MIME type for the specified picture
            conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);

            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "News_GetPicture";
            comm.CommandType = CommandType.StoredProcedure;
            comm.Connection = conn;
            comm.Parameters.AddWithValue("@id", PictureID);

            SqlDataReader myReader = comm.ExecuteReader();
            if (myReader.Read())
                return myReader.GetSqlBinary(0);
            else
                return null;

        }
    }

    public void ProcessRequest (HttpContext context) {

        UrlParameterWhitelistValidator validator = new UrlParameterWhitelistValidator();
        if (!validator.ValidateAndFilter("/ThumbNail.ashx", context.Request.QueryString))
        {
            throw new Exception("Invalid parameter.");
        }



        int PictureID = 0;
        if (context.Request.QueryString["PictureID"] != null)
        {
            //PictureID = Convert.ToInt32(Request.QueryString["PictureID"]);
            if (Int32.TryParse(context.Request.QueryString["PictureID"], out PictureID) == false)
            {
                throw new Exception("QueryString parameter must be an Integer value.");
            }
        }
        else
        {
            return;
        }


        //////// get the file name
        ////////if (context.Request.QueryString["file"] == null)
        ////////    return;

        ////////string file = context.Request.QueryString["file"];

        ////////// create an image object, using the filename we just retrieved
        ////////System.Drawing.Image imgInput = System.Drawing.Image.FromFile(context.Server.MapPath(file));


        int MaxSideSize;
        try { MaxSideSize = int.Parse(context.Request.QueryString["maxsz"]); }
        catch { MaxSideSize = 100; }

        // get image from object. 
        byte[] _ImageData = new byte[0];
        try { _ImageData = (byte[])GetBinary(PictureID); }
        catch { return; }

        System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ImageData);
        System.Drawing.Image imgInput = System.Drawing.Image.FromStream(_MemoryStream);



        #region Calculate new size
        int intNewWidth;
        int intNewHeight;

        //get image original width and height 
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait 
        int intMaxSide;

        if (intOldWidth >= intOldHeight)
        {
            intMaxSide = intOldWidth;
        }
        else
        {
            intMaxSide = intOldHeight;
        }


        if (intMaxSide > MaxSideSize)
        {
            //set new width and height 
            double dblCoef = MaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
        }
        else
        {
            intNewWidth = intOldWidth;
            intNewHeight = intOldHeight;
        }
        #endregion


        // create the actual thumbnail image
        System.Drawing.Image thumbnailImage = imgInput.GetThumbnailImage(intNewWidth, intNewHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

        // make a memory stream to work with the image bytes
        MemoryStream imageStream = new MemoryStream();

        //Determine image format 
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        // put the image into the memory stream
        thumbnailImage.Save(imageStream, fmtImageFormat);

        // make byte array the same size as the image
        byte[] imageContent = new Byte[imageStream.Length];

        // rewind the memory stream
        imageStream.Position = 0;

        // load the byte array with the image
        imageStream.Read(imageContent, 0, (int)imageStream.Length);

        // return byte array to caller with image type
        context.Response.ContentType = fmtImageFormat.ToString();
        context.Response.BinaryWrite(imageContent);

    }

    /// <summary>
    /// Required, but not used
    /// </summary>
    /// <returns>true</returns>
    public bool ThumbnailCallback()
    {
        return true;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}