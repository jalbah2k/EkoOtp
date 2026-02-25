<%@ WebHandler Language="C#" Class="ThumbNail" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

public class ThumbNail : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        // get the file name
        if (context.Request.QueryString["file"] == null)
            return;
        
        string file = context.Request.QueryString["file"];

        int MaxSideSize;        
        try { MaxSideSize = int.Parse(context.Request.QueryString["maxsz"]); }
        catch { MaxSideSize = 100; }

        // create an image object, using the filename we just retrieved
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(context.Server.MapPath(file));


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