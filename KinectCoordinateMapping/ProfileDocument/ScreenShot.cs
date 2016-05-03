using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KinectCoordinateMapping.ProfileDocument
{
    public class ScreenShot
    {
        public void Screenshot()
        {
            //if (this.replayFrame.bitmapSource != null)
            //{
            //    // create a png bitmap encoder which knows how to save a .png file
            //    BitmapEncoder encoder = new PngBitmapEncoder();

            //    // create frame from the writable bitmap and add to encoder
            //    encoder.Frames.Add(BitmapFrame.Create(this.replayFrame.bitmapSource));

            //    string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            //    string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            //    string path = System.IO.Path.Combine(myPhotos, "KinectScreenshot--Posture" + time + ".png");

            //    // write the new file to disk
            //    try
            //    {
            //        // FileStream is IDisposable
            //        using (FileStream fs = new FileStream(path, FileMode.Create))
            //        {
            //            encoder.Save(fs);
            //        }

            //        //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.SavedScreenshotStatusTextFormat, path);
            //    }
            //    catch (IOException)
            //    {
            //        //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedScreenshotStatusTextFormat, path);
            //    }
            //}
        }
    }
}
