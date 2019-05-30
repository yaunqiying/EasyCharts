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
using nQuant;

using range = Microsoft.Office.Interop.Excel.Range;
using worksheet = Microsoft.Office.Tools.Excel.Worksheet;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Microsoft.Office.Tools.Ribbon;

namespace ExcelAddIn_Graphics
{
    public partial class Form_ColorPalette : Form
    {
        private Bitmap bmDraw;
        private Bitmap Orignialimage;
        private QuantizedPalette palette;
        public List<Color> ColorOutput;
        public List<int> ColorLight_idx;
        EasyCharts Graphic = new EasyCharts();
        public static Bitmap ConvertTo32bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (var gr = System.Drawing.Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }
        public Form_ColorPalette()
        {
            InitializeComponent();
            button_ColorOutput.Enabled = false;
            button_ColorPalette.Enabled = false;
        }

        private void button_ReadImage_Click(object sender, EventArgs e)
        {
            dataGridView_Color.Rows.Clear();
            button_ColorOutput.Enabled = false;
            button_ColorPalette.Enabled = false;

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

                        //Step 1: Convert the Image to Use an 8-bit Palette

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
                                Orignialimage = new Bitmap(quantized);
                            palette = quantizer.palette;
                            //quantized.Save(targetPath, ImageFormat.Png);
                           
                            button_ColorPalette.Enabled =true;

                        }
                    }
                }
                // }
            }
        }

        private void button_ColorPalette_Click(object sender, EventArgs e)
        {
            dataGridView_Color.Rows.Clear();

            Bitmap inputimage = Orignialimage;
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

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmDraw);
            g.DrawImage(inputimage, rg);
            this.pictureBox_Image.Image = bmDraw;

            //Step 2: Count Up How Many Pixels of Each Color You Have*************************************************************
            int MaxColor = palette.Colors.Count;// 256;
            int MaxPiexls = palette.PixelIndex.Length;// 256;
            List<long> ColorFrequent = new List<long>(MaxColor);

            for (int i = 0; i < MaxColor; i++)
            {
                ColorFrequent.Add(0);
            }

            //long[] ColorFrequent = new long[MaxColor];
            int idx;
            for (int i = 0; i < MaxPiexls; i++)
            {
                idx = palette.PixelIndex[i];
                ColorFrequent[idx] = ColorFrequent[idx] + 1;
            }

            //Step 3: Sort the Colors in the Palette by Frequency*************************************************************
            var sorted = ColorFrequent.Select((x, i) => new KeyValuePair<long, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<long> ColorFrequent_Sort = sorted.Select(x => x.Key).ToList();
            List<int> ColorFrequent_idx = sorted.Select(x => x.Value).ToList();


            Color[] colors = new Color[MaxColor];
            for (int i = 0; i < MaxColor; i++)
            {
                var Color_RGB = palette.Colors[i];
                colors[i] = System.Drawing.Color.FromArgb(Color_RGB.R, Color_RGB.G, Color_RGB.B);
            }

            Color[] SortColors = new Color[MaxColor];
            for (int i = 0; i < MaxColor; i++)
            {
                SortColors[i] = colors[ColorFrequent_idx[i]];
            }

            //Step 5: Loop through All of Our Colors and Pick Out the Top Ones*************************************************************
            int maxColors = 16;
            ColorOutput = GetTopUniqueColors(SortColors, maxColors);
            // RGB2HSV(System.Drawing.Color RGB, ref int[] HSV)
            int[] HSV = new int[3] { 0, 0, 0 };
            int ColorOutputN = ColorOutput.Count;
            List<int> ColorLight = new List<int>(ColorOutputN);

            for (int i = 0; i < ColorOutputN; i++)
            {
                ColorLight.Add(0);
            }
            for (int i = 0; i < ColorOutputN; i++)
            {
                var Color_RGB = ColorOutput[i];
                Graphic.RGB2HSV(Color_RGB, ref HSV);
               
                ColorLight[i] = HSV[2];
            }

            var ColorLight_sorted = ColorLight.Select((x, i) => new KeyValuePair<int, int>(x, i))
               .OrderBy(x => x.Key)
               .ToList();

            List<int> ColorLight_Sort = ColorLight_sorted.Select(x => x.Key).ToList();
            ColorLight_idx = ColorLight_sorted.Select(x => x.Value).ToList();

            for (int i = 0; i < ColorOutputN; i++)
            {
                var Color_RGB = ColorOutput[ColorLight_idx[i]];
                // var Frequent = ColorFrequent_Sort[MaxColor - 1 - i];
                String strRGB = (Color_RGB.R).ToString() + "," + (Color_RGB.G).ToString() + "," + (Color_RGB.B).ToString();
                int index = this.dataGridView_Color.Rows.Add();
                this.dataGridView_Color.Rows[index].Cells[0].Value = (i + 1).ToString();
                this.dataGridView_Color.Rows[index].Cells[1].Style.BackColor = System.Drawing.Color.FromArgb(Color_RGB.R, Color_RGB.G, Color_RGB.B);
                this.dataGridView_Color.Rows[index].Cells[2].Value = strRGB;
                // this.dataGridView_Color.Rows[index].Cells[3].Value = (Frequent).ToString();
            }

            //for (int i = 0; i < MaxColor; i++)
            //{
            //    var Color_RGB = SortColors[i ];
            //    var Frequent = ColorFrequent_Sort[MaxColor -1-i];
            //    String strRGB = (Color_RGB.R).ToString() + "," + (Color_RGB.G).ToString() + "," + (Color_RGB.B).ToString();
            //    int index = this.dataGridView_Color.Rows.Add();
            //    this.dataGridView_Color.Rows[index].Cells[0].Value = (i + 1).ToString();
            //    this.dataGridView_Color.Rows[index].Cells[1].Style.BackColor = System.Drawing.Color.FromArgb(Color_RGB.R, Color_RGB.G, Color_RGB.B);
            //    this.dataGridView_Color.Rows[index].Cells[2].Value = strRGB;
            //    this.dataGridView_Color.Rows[index].Cells[3].Value = (Frequent).ToString();
            //}
            button_ColorOutput.Enabled = true;
        }

        //Step 4: Define a Distance Functions for Colors
        private bool WithinTolerance(Color c1, Color c2, double tolerance)
        {
            double maxDistance = 255 * 255 * 3;
            int toleranceDistance = (int)(tolerance * maxDistance);

            int distance = (int)Math.Pow((double)(c1.R - c2.R), 2);
            distance += (int)Math.Pow((double)(c1.G - c2.G), 2);
            distance += (int)Math.Pow((double)(c1.B - c2.B), 2);

            return (bool)(distance <= toleranceDistance);
        }

        private bool ColorIsDifferent(Color color, List<Color> colorList)
        {
            double tolerance = 0.015;
            foreach (Color c in colorList)
            {
                if (WithinTolerance(c, color, tolerance))
                {
                    return false;
                }
            }
            return true;
        }

        //Step 5: Loop through All of Our Colors and Pick Out the Top Ones
        private List<Color> GetTopUniqueColors(Color[] colors, int maxColors)
        {
            List<Color> uniqueColors = new List<Color>();

            for (int i = 0; i < colors.Length && uniqueColors.Count < maxColors; ++i)
            {
                // read the colors from the end of the array
                // since they are sorted in increasing order of frequency
                Color color = colors[colors.Length - 1 - i];
                if (ColorIsDifferent(color, uniqueColors))
                {
                    uniqueColors.Add(color);
                }
            }

            return uniqueColors;
        }


        //public void RGB2HSV(System.Drawing.Color RGB, ref int[] HSV)
        //{
        //    // H:0-360, S；0-100, V;：0-100
        //    //System.Drawing.Color MyColor = System.Drawing.Color.FromArgb(R, G, B);
        //    int B = RGB.B;
        //    int G = RGB.G;
        //    int R = RGB.R;
        //    HSV[0] = Convert.ToInt32(RGB.GetHue());

        //    //奇怪——用微软自己的方法获得的S值和V值居然不对
        //    //S=Convert.ToInt32(MyColor.GetSaturation()/255*100);
        //    //V=Convert.ToInt32(MyColor.GetBrightness()/255*100);

        //    decimal min;
        //    decimal max;
        //    decimal delta;

        //    decimal R1 = Convert.ToDecimal(R) / 255;
        //    decimal G1 = Convert.ToDecimal(G) / 255;
        //    decimal B1 = Convert.ToDecimal(B) / 255;

        //    min = Math.Min(Math.Min(R1, G1), B1);
        //    max = Math.Max(Math.Max(R1, G1), B1);
        //    HSV[2] = Convert.ToInt32(max * 100);
        //    delta = (max - min) * 100;

        //    if (max == 0 || delta == 0)
        //        HSV[1] = 0;
        //    else
        //        HSV[1] = Convert.ToInt32(delta / max);
        //}

        private void button_ColorOutput_Click(object sender, EventArgs e)
        {
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;
            ((range)worksheet.Cells[start_row, start_col]).Value2 = "ID";
            ((range)worksheet.Cells[start_row, start_col + 1]).Value2 = "Color";
            ((range)worksheet.Cells[start_row, start_col + 2]).Value2 = "R";
            ((range)worksheet.Cells[start_row, start_col + 3]).Value2 = "G";
            ((range)worksheet.Cells[start_row, start_col + 4]).Value2 = "B";

            int ColorOutputN = ColorOutput.Count;
            for (int i = 0; i < ColorOutputN; i++)
            {
                var Color_RGB = ColorOutput[ColorLight_idx[i]];
                //String strRGB = (Color_RGB.R).ToString() + "," + (Color_RGB.G).ToString() + "," + (Color_RGB.B).ToString();

                ((range)worksheet.Cells[start_row + i + 1, start_col]).Value2 = i + 1;
                ((range)worksheet.Cells[start_row + i + 1, start_col + 1]).Interior.Color = System.Drawing.Color.FromArgb(Color_RGB.R, Color_RGB.G, Color_RGB.B);
                ((range)worksheet.Cells[start_row + i + 1, start_col + 2]).Value2 = Color_RGB.R;
                ((range)worksheet.Cells[start_row + i + 1, start_col + 3]).Value2 = Color_RGB.G;
                ((range)worksheet.Cells[start_row + i + 1, start_col + 4]).Value2 = Color_RGB.B;
            }


        }
    }
}
