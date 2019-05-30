using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;
using System.Drawing.Imaging;

//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Tools.Excel;

namespace nQuant
{
    public partial class Form_ColorPalette : Form
    {
        private Bitmap bmDraw;
        public Form_ColorPalette()
        {
            InitializeComponent();
        }


        public static Bitmap ConvertTo32bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        private void button_ReadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();//创建事例            
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Templates);//指定初始目录            
            dlg.InitialDirectory = dir;//指定初始目录            
            dlg.Title = "图表对话框";
            dlg.ShowReadOnly = true;//以只读方式打开            
            dlg.ShowHelp = true;//显示帮助按钮            ///////            
            dlg.Filter = "图表.jpg|*.jpg|图表.tif|*.tif|图表.bmp|*.bmp|所有文件|*.*";//文件过滤器，指定打开文件类型            
            //dlg.ShowDialog();//打开对话框  
            //BMP 文件(*.bmp) | *.bmp | JPEG 文件(*.jpg, *.jpeg) | *.jpg, *.jpeg | PNG 文件(*.png) | *.png | GIF 文件(*.gif) | *.gif | TIFF 文件(*.tiff, *.tif) | *.tiff
            //MessageBox.Show(dlg.Title);//打开消息            
            //dlg.Multiselect = true;//是否允许一次打开多个文件  
            dlg.Multiselect = false;//是否允许一次打开多个文件          
                                    // if (dlg.ShowDialog() == DialogResult.OK)           
                                    //{           
            if (dlg.CheckPathExists)//检查路径是否存在           
            {
                if (dlg.CheckFileExists)//检查文件是否存在               
                {
                    // if (dlg.ValidateNames)//检查是否有效Win32文件名                    
                    //  {
                   if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string s = dlg.FileNames[0];
                        // foreach (string s in dlg.FileNames)
                        //{                                //string fileName = dlg.FileName;                 
                        //MessageBox.Show("打开文件:" + s);//打开消息对话框  

                        bmDraw = new Bitmap(this.pictureBox_Image.Width, this.pictureBox_Image.Height);
                        Bitmap inputimage = (Bitmap)Image.FromFile(s);

                        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmDraw);

                        double scale_width = Convert.ToDouble(bmDraw.Width) / inputimage.Width;
                        double scale_height = Convert.ToDouble(bmDraw.Height) / inputimage.Height;
                        double scale = scale_width;
                        if (scale_width > scale_height) scale = scale_height;

                        int centerx = bmDraw.Width / 2;
                        int centery = bmDraw.Height / 2;
                        int newWidth = Convert.ToInt32(inputimage.Width * scale / 2);
                        int newHeight = Convert.ToInt32(inputimage.Height * scale / 2);
                        Rectangle rg = new Rectangle(centerx - newWidth, centery - newHeight, newWidth * 2, newHeight * 2);
                        //将bm内rg所指定的区域绘制到bm1
                        g.DrawImage(inputimage, rg);
                        this.pictureBox_Image.Image = bmDraw;



                        var sourcePath = s;
                        if (!File.Exists(sourcePath))
                        {
                            Console.WriteLine("The source file you specified does not exist.");
                            Environment.Exit(1);
                        }

                        var lastDot = sourcePath.LastIndexOf('.');
                        var targetPath = sourcePath.Insert(lastDot, "-quant");
                        //if(args.Length > 1)
                        //    targetPath = args[1];

                        var quantizer = new WuQuantizer();
                        // QuantizedPalette ColorPalette = new QuantizedPalette();
                        var bitmap0 = new Bitmap(sourcePath);
                        Bitmap bitmap1 = ConvertTo32bpp(bitmap0);

                        using (var bitmap = bitmap1)
                        //using (var bitmap = new Bitmap(sourcePath))
                        {
                            using (var quantized = quantizer.QuantizeImage(bitmap))
                            {
                                QuantizedPalette palette = quantizer.palette;
                                //quantized.Save(targetPath, ImageFormat.Png);


                                inputimage = new Bitmap(quantized);
                                 scale_width = Convert.ToDouble(bmDraw.Width) / inputimage.Width;
                                scale_height = Convert.ToDouble(bmDraw.Height) / inputimage.Height;
                                scale = scale_width;
                                if (scale_width > scale_height) scale = scale_height;

                                centerx = bmDraw.Width / 2;
                                centery = bmDraw.Height / 2;
                                newWidth = Convert.ToInt32(inputimage.Width * scale / 2);
                                newHeight = Convert.ToInt32(inputimage.Height * scale / 2);
                                rg = new Rectangle(centerx - newWidth, centery - newHeight, newWidth * 2, newHeight * 2);
                                //将bm内rg所指定的区域绘制到bm1
                                g.DrawImage(inputimage, rg);
                                this.pictureBox_Image.Image = bmDraw;
                            }
                        }
                    }
                    // }
                }
            }
        }
    }
}
