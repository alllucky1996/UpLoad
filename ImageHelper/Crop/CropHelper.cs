using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageHelper.Crop
{
    public class CropHelper
    {
        public Image<Bgr, byte> MainImage { get; set; }
        public CropHelper() { }
        public CropHelper(Bitmap bitmapImage) {
            MainImage = new Image<Bgr, byte>(bitmapImage);
        }
        public CropHelper(string uriImage)
        {
            MainImage = new Image<Bgr, byte>(uriImage);
        }
        public CropHelper(Mat matImg)
        {
            MainImage = matImg.ToImage<Bgr, byte>();
        }
        public static Image<Bgr, byte>  Crop(Image<Bgr, byte>  img,int x, int y, int width, int height)
        {
            return Crop(img, new Rectangle(x, y, width, height));
        }
        public static Image<Bgr, byte> Crop(Image<Bgr, byte> img, Rectangle rectangle)
        {
            //Mat mat = new Mat();
            // mat = img.Mat;
            return new Mat(img.Mat, rectangle).ToImage<Bgr, byte>();
        }
        public static Bitmap CropToBitmap(Image<Bgr, byte> img, Rectangle rectangle)
        {
            return new Mat(img.Mat, rectangle).Bitmap;
        }
        public  Bitmap ToBitMap()
        {
           return MainImage.ToBitmap();
        }
        public Mat ToMat()
        {
            return MainImage.Mat;
        }
        // save return ok? true: false
        public bool Save(string fileName)
        {
            try
            {
                MainImage.Save(fileName);
                return true;
            }
            catch
            {
                return false;

            }
        }
        // error = save ok? null: ex
        public void Save(string fileName, out string error)
        {
            try
            {
                MainImage.Save(fileName);
                error = null;
            }
            catch(Exception ex )
            {
                error = ex.Message ;
            }
        }
    }
}
