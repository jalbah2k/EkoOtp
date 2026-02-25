using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MarketPlace
/// </summary>
public class MarketPlace
{
    public MarketPlace()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void SaveThumbnail(string location, string filename, int MaxSideSize = 200, int MaxHeight = 150)
    {

        // create an image object, using the filename we just retrieved
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(location + filename);


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
            MaxSideSize = MaxHeight;
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

        //////try { intNewHeight = MaxHeight; }
        //////catch { }

        //string thumbfolder = HttpContext.Current.Server.MapPath(location + "/thumbnails/");
        string thumbfolder = location + "thumbnails\\";
        if (!Directory.Exists(thumbfolder))
            Directory.CreateDirectory(thumbfolder);

        // create the actual thumbnail image
        System.Drawing.Image thumbnailImage = imgInput.GetThumbnailImage(intNewWidth, intNewHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
        //Determine image format 
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        thumbnailImage.Save(thumbfolder + filename, fmtImageFormat);

    }

    public bool ThumbnailCallback()
    {
        return true;
    }
}