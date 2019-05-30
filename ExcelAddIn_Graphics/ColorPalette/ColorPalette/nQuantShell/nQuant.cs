using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace nQuant
{
    public class nQuant
    {
        //public static Bitmap ConvertTo32bpp(Image img)
        //{
        //    var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //    using (var gr = Graphics.FromImage(bmp))
        //        gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
        //    return bmp;
        //}

        public static void Main()
        {
            Form_ColorPalette ColorPalette = new Form_ColorPalette();
            ColorPalette.ShowDialog();
            //var sourcePath = Environment.CurrentDirectory + @"\photo_pinksherbet.png";

            ////if (args.Length < 1)
            ////{
            ////    Console.WriteLine("You must provide a file name to quantize");
            ////    Environment.Exit(1);
            ////}
            ////var sourcePath = args[0];
            //if(!File.Exists(sourcePath))
            //{
            //    Console.WriteLine("The source file you specified does not exist.");
            //    Environment.Exit(1);
            //}

            //var lastDot = sourcePath.LastIndexOf('.');
            //var targetPath = sourcePath.Insert(lastDot, "-quant");
            ////if(args.Length > 1)
            ////    targetPath = args[1];

            //var quantizer = new WuQuantizer();
            //// QuantizedPalette ColorPalette = new QuantizedPalette();
            //var bitmap0 = new Bitmap(sourcePath);
            //Bitmap bitmap1 = ConvertTo32bpp(bitmap0);

            //using (var bitmap = bitmap1)
            ////using (var bitmap = new Bitmap(sourcePath))
            //{
            //    using(var quantized = quantizer.QuantizeImage(bitmap))
            //    {
            //        QuantizedPalette palette = quantizer.palette;
            //        quantized.Save(targetPath, ImageFormat.Png);
            //    }
            //}

        }
    }
}
