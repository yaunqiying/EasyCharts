using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;


namespace ExcelAddIn_Graphics
{
    public partial class Form_GetData : Form
    {
        public Point currentPoint, formerpoint;//定义两个点（启点，终点）
        public static bool drawing = false;//设置一个启动标志
        public static bool Drawing_Line = true;//设置一个启动标志
        public static bool Falg_Inputimage = false;
        public static int Series_Num = 0;
        public static int Point_Num = 0;

        public bool Flag_XAxisMin = false;
        public bool Flag_XAxisMax = false;
        public bool Flag_YAxisMin = false;
        public bool Flag_YAxisMax = false;

        public Point XAxisMin, XAxisMax, YAxisMin, YAxisMax;
        //public int[,] GetData;
        public System.Collections.ArrayList DataX = new System.Collections.ArrayList();
        public System.Collections.ArrayList DataY = new System.Collections.ArrayList();

        public Point PointStart, PointEnd;
        public double[] RGBStart = new double[3];
        public double[] RGBEnd = new double[3];
        public double[] RGBLine = new double[3];

        public bool Flag_PointStart = false;
        public bool Flag_PointEnd = false;
        private Color srcColor;


        private Bitmap bmDraw, bmRegion;

        public Form_GetData()
        {
            InitializeComponent();

            //string item = "ScatterXolor2.jpg";
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + "/" + item;
            //this.pictureBox_Data.Image = Image.FromFile(path);
        }

        private void tabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                button_Automatic.Enabled = false;
                button_GetData2.Enabled = false;
                button_LableData.Enabled = false;
            }
            else
            {
                button_Automatic.Enabled = false;
                button_GetData2.Enabled = false;
                button_LableData.Enabled = true;
            }

        }

        private void button_Xaxismin_Click(object sender, EventArgs e)
        {
            Flag_XAxisMin = true;
            Flag_XAxisMax = false;
            Flag_YAxisMin = false;
            Flag_YAxisMax = false;

        }


        private void button_Xaxismax_Click(object sender, EventArgs e)
        {
            Flag_XAxisMin = false;
            Flag_XAxisMax = true;
            Flag_YAxisMin = false;
            Flag_YAxisMax = false;

        }

        private void button_Yaxismin_Click(object sender, EventArgs e)
        {
            Flag_XAxisMin = false;
            Flag_XAxisMax = false;
            Flag_YAxisMin = true;
            Flag_YAxisMax = false;

            //button__Yaxismax.Enabled = true;
        }

        private void button__Yaxismax_Click(object sender, EventArgs e)
        {
            Flag_XAxisMin = false;
            Flag_XAxisMax = false;
            Flag_YAxisMin = false;
            Flag_YAxisMax = true;

        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            Flag_PointStart = true;
        }

        private void button_End_Click(object sender, EventArgs e)
        {
            Flag_PointStart = false;
            Flag_PointEnd = true;
        }

        private void button_Automatic_Click(object sender, EventArgs e)
        {
            DataX = new System.Collections.ArrayList();
            DataY = new System.Collections.ArrayList();

            for (int i = 0; i < 3; i++)
            { RGBLine[i] = (RGBEnd[i] + RGBStart[i]) / 2; }

            int Mat_cols = PointEnd.X - PointStart.X + 1;
            int Mat_rows = YAxisMin.Y-YAxisMax.Y + 1;

           
            int pointx, pointy;
            
            double Min_Distance = double.MaxValue;
            int Min_indexX = PointStart.X;
            int Min_indexY= PointStart.Y;

            int Step = Convert.ToInt32(textBox_Step.Text);
            double Threshold = double.Parse(textBox_Threshold.Text);
            int Search_range = 50;

            double[,] Distance = new double[Search_range*2+1, Mat_cols];
            for (int j = 0; j < Mat_cols; j++)
            {
                for (int i = 0; i < Search_range * 2 + 1; i++)
                {
                    Distance[i, j] = double.MaxValue;
                }
            }

            DataX.Add(PointStart.X);
            DataY.Add(PointStart.Y);

            for (int j = 1; j < Mat_cols; j=j+Step)
            {
                pointx = PointStart.X + j;
                Min_Distance = double.MaxValue;
                for (int i = -Search_range; i <= Search_range; i++)
                {
                    pointy = Convert.ToInt32(DataY[DataY.Count - 1]) + i;
                    if (pointy <= YAxisMin.Y & pointy>= YAxisMax.Y)
                    {
                        srcColor = bmDraw.GetPixel(pointx, pointy);
                        Distance[i+ Search_range, j] = Math.Sqrt(Math.Pow((RGBLine[0]- srcColor.R),2)+ Math.Pow((RGBLine[1] - srcColor.G), 2)+ Math.Pow((RGBLine[2] - srcColor.B), 2));

                        if (Distance[i + Search_range, j] < Min_Distance)
                        {
                            Min_Distance = Distance[i + Search_range, j];
                            Min_indexX = pointx;
                            Min_indexY = pointy;
                        }

                    }
                }

                if (Min_Distance< Threshold)
                {
                    DataX.Add(Min_indexX);
                    DataY.Add(Min_indexY);
                }
            }


            System.Drawing.Graphics g = this.pictureBox_Data.CreateGraphics();
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmDraw);
            for (int i = 0; i < DataX.Count; i++)
            {
                g.DrawEllipse(new Pen(Color.Red, 4), Convert.ToInt32(DataX[i]) - 2, Convert.ToInt32(DataY[i]) - 2, 4, 4);
            }
            //g.Save();
            //Dispose();
            //this.pictureBox_Data.Image = bmDraw;
            button_GetData2.Enabled = true;

            Series_Num = Series_Num + 1;
        }


           
        private void PictureBox_Data_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Falg_Inputimage)
            {
                //System.Drawing.Graphics g = pictureBox_LocalRegion.CreateGraphics();
                //Cursor.Current = myCursor;
                Rectangle sourceRectangle = new Rectangle(e.X - 10, e.Y - 10, 20, 20);
                //Rectangle destRectangle1 = new Rectangle(0, 0, 160, 150);
               // g.DrawImage(this.pictureBox_Data.Image, destRectangle1, sourceRectangle, GraphicsUnit.Pixel);
                Bitmap bitmap = (Bitmap)this.pictureBox_Data.Image;
                Bitmap Region = bitmap.Clone(sourceRectangle, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                int centerx = pictureBox_LocalRegion.Width / 2;
                int centery = pictureBox_LocalRegion.Height / 2;
               


                bmRegion = new Bitmap(this.pictureBox_LocalRegion.Width, this.pictureBox_LocalRegion.Height);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmRegion);

                double scale_width = Convert.ToDouble(bmRegion.Width) / Region.Width;
                double scale_height = Convert.ToDouble(bmRegion.Height) / Region.Height;
                double scale = scale_width;
                if (scale_width > scale_height) scale = scale_height;

               // centerx = bmRegion.Width / 2;
               // centery = bmRegion.Height / 2;
                int newWidth = Convert.ToInt32(Region.Width * scale / 2);
                int newHeight = Convert.ToInt32(Region.Height * scale / 2);
                Rectangle rg = new Rectangle(centerx - newWidth, centery - newHeight, newWidth * 2, newHeight * 2);
                //将bm内rg所指定的区域绘制到bm1
                g.DrawImage(Region, rg);

                g.DrawEllipse(new Pen(Color.Red, 2), centerx - 2, centery - 2, 4, 4);

                Point point1 =new Point(0, centery);
                Point point2 = new Point(bmRegion.Width, centery);
                g.DrawLine(new Pen(Color.Blue, 1), point1, point2);
                point1 = new Point(centerx,0);
                point2 = new Point(centerx, bmRegion.Height);
                g.DrawLine(new Pen(Color.Blue, 1), point1, point2);

                this.pictureBox_LocalRegion.Image = bmRegion;

                System.Drawing.Graphics g2 = this.pictureBox_Data.CreateGraphics();
                if (Flag_XAxisMin)
                { this.pictureBox_Data.Refresh();
                    g2.DrawString("XAxis-min", new System.Drawing.Font("Arial", 8.0F), new SolidBrush(Color.Black), e.X, e.Y); }
                if (Flag_XAxisMax)
                {
                    this.pictureBox_Data.Refresh();
                    g2.DrawString("XAxis-max", new System.Drawing.Font("Arial", 8.0F), new SolidBrush(Color.Black), e.X, e.Y);
                }
                if (Flag_YAxisMin)
                {
                    this.pictureBox_Data.Refresh();
                    g2.DrawString("YAxis-min", new System.Drawing.Font("Arial", 8.0F), new SolidBrush(Color.Black), e.X, e.Y);
                }
                if (Flag_YAxisMax)
                {
                    this.pictureBox_Data.Refresh();
                    g2.DrawString("YAxis-max", new System.Drawing.Font("Arial", 8.0F), new SolidBrush(Color.Black), e.X, e.Y);
                }
            }


        }

        private void Form_GetData_Load(object sender, EventArgs e)
        {
            button_GetData.Enabled =false;
            button_LableData.Enabled = false;
            button_Xaxismin.Enabled = false;
            button_Xaxismax.Enabled = false;
            button_Yaxismin.Enabled = false;
            button__Yaxismax.Enabled = false;

            button_End.Enabled = false;


            drawing = false;//设置一个启动标志
            Drawing_Line = true;//设置一个启动标志
           Falg_Inputimage = false;
           Series_Num = 0;
           Point_Num = 0;

          Flag_XAxisMin = false;
         Flag_XAxisMax = false;
         Flag_YAxisMin = false;
         Flag_YAxisMax = false;

         DataX = new System.Collections.ArrayList();
         DataY = new System.Collections.ArrayList();
    }

        private void textBox_Step_TextChanged(object sender, EventArgs e)
        {
            if  (button_Automatic.Enabled)
            {
                Series_Num = Series_Num - 1;
                button_Automatic_Click(sender, e);
            }
        }

        private void textBox_Threshold_TextChanged(object sender, EventArgs e)
        {
            if (button_Automatic.Enabled)
            {
                Series_Num = Series_Num - 1;
                button_Automatic_Click(sender, e);
            }
            
        }

        private void button_GetData2_Click(object sender, EventArgs e)
        {
            button_GetData_Click(sender, e);
        }

        

        //private void PictureBox_Data_MouseEnter(object sender, EventArgs e)
        //{
        //    if (Falg_Inputimage)
        //    {
        //        System.Drawing.Graphics g = pictureBox_LocalRegion.CreateGraphics();
        //        //Cursor.Current = myCursor;
        //        // Rectangle sourceRectangle = new Rectangle(e.X - 10, e.Y - 10, 20, 20);
        //        //Rectangle destRectangle1 = new Rectangle(0, 0, 160, 150);
        //        // g.DrawImage(this.pictureBox_Data.Image, destRectangle1, sourceRectangle, GraphicsUnit.Pixel);
        //        //Bitmap bitmap = (Bitmap)this.pictureBox_Data.Image;
        //        //pictureBox_LocalRegion.Image = bitmap.Clone(sourceRectangle, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //        int centerx = pictureBox_LocalRegion.Width / 2;
        //        int centery = pictureBox_LocalRegion.Height / 2;
        //        g.DrawEllipse(new Pen(Color.Red, 4), centerx - 2, centery - 2, 4, 4);
        //    }
        //    //throw new NotImplementedException();
        //}

        private void button_LableData_Click(object sender, EventArgs e)
        {
            drawing = true;
            Point_Num = 0;
            Series_Num = Series_Num+1;

            DataX = new System.Collections.ArrayList();
            DataY = new System.Collections.ArrayList();

            button_GetData.Enabled = true;
        }

        //throw new System.NotImplementedException();
        // }

        //private void PictureBox_Data_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        //{

        //    //drawing = false;
        //    //throw new System.NotImplementedException();
        //}

        private void PictureBox_Data_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                drawing = false;
                return;
            }

            currentPoint = new Point(e.X, e.Y);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmDraw);

            //System.Drawing.Graphics g = pictureBox_Data.CreateGraphics();
            if (drawing)
            {
               // System.Drawing.Graphics g = pictureBox_Data.CreateGraphics();
                g.DrawEllipse(new Pen(Color.Red, 4), e.X-2, e.Y-2, 4, 4);

                if (Point_Num >= 1 & Drawing_Line)
                {
                    //Point currentPoint = new Point(e.X, e.Y);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿
                    g.DrawLine(new Pen(Color.Blue, 1), formerpoint, currentPoint);
                }
            }

            formerpoint = new Point(e.X, e.Y);
            Point_Num = Point_Num + 1;

            DataX.Add(e.X);
            DataY.Add(e.Y);
            // throw new System.NotImplementedException();   

            int RectR = 4;

            if (Flag_XAxisMin)
            {
                XAxisMin.X = e.X;
                XAxisMin.Y = e.Y;
                Flag_XAxisMin = false;

                g.DrawRectangle(new Pen(Color.Red, 2), e.X- RectR, e.Y- RectR, RectR * 2, RectR * 2);

                button_Xaxismax.Enabled = true;
            }

            if (Flag_XAxisMax)
            {
                XAxisMax.X = e.X;

                if (XAxisMin.Y==0)
                { XAxisMax.Y = e.Y; }
                else
                { XAxisMax.Y = XAxisMin.Y; }
                
                Flag_XAxisMax = false;

                //System.Drawing.Graphics g = pictureBox_Data.CreateGraphics();
                g.DrawRectangle(new Pen(Color.Red, 2), e.X- RectR, e.Y- RectR, RectR*2, RectR * 2);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿
                g.DrawLine(new Pen(Color.Red, 1), XAxisMin, XAxisMax);


                button_Yaxismin.Enabled = true;
            }

            if (Flag_YAxisMin)
            {
                YAxisMin.X = e.X;
                YAxisMin.Y = e.Y;
                Flag_YAxisMin = false;

                g.DrawRectangle(new Pen(Color.Blue, 2), e.X- RectR, e.Y- RectR, RectR * 2, RectR * 2);
                button__Yaxismax.Enabled = true;
            }

            if (Flag_YAxisMax)
            {
                if (YAxisMin.X==0)
                { YAxisMax.X = e.X; }
                else
                { YAxisMax.X = YAxisMin.X; }
                
                YAxisMax.Y = e.Y;
                Flag_YAxisMax = false;

               // System.Drawing.Graphics g = pictureBox_Data.CreateGraphics();
                g.DrawRectangle(new Pen(Color.Blue, 2), e.X- RectR, e.Y- RectR, RectR * 2, RectR * 2);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿
                g.DrawLine(new Pen(Color.Blue, 1), YAxisMin, YAxisMax);

                button_LableData.Enabled = true;
            }

            if (Flag_PointStart)
            {
                button_End.Enabled = true;

                srcColor = bmDraw.GetPixel(e.X, e.Y);
                button_Start.BackColor = srcColor;
                PointStart.X = e.X;
                PointStart.Y = e.Y;

                RGBStart[0] = srcColor.R;
                RGBStart[1] = srcColor.G;
                RGBStart[2] = srcColor.B;

                g.DrawEllipse(new Pen(srcColor, 2), e.X - RectR, e.Y - RectR, RectR * 2, RectR * 2);
            }


            if (Flag_PointEnd)
            {
                button_Automatic.Enabled = true;
                srcColor = bmDraw.GetPixel(e.X, e.Y);
                PointEnd.X = e.X;
                PointEnd.Y = e.Y;

                button_End.BackColor = srcColor;
                RGBEnd[0] = srcColor.R;
                RGBEnd[1] = srcColor.G;
                RGBEnd[2] = srcColor.B;

                g.DrawEllipse(new Pen(srcColor, 2), e.X - RectR, e.Y - RectR, RectR * 2, RectR * 2);
            }

            g.Save();
            //Dispose();
            this.pictureBox_Data.Image =bmDraw;

        }

        private void button_GetData_Click(object sender, EventArgs e)
        {
            double XAxisMin_Real = double.Parse(textBox__Xaxismin.Text);
            double XAxisMax_Real = double.Parse(textBox__Xaxismax.Text);
            double YAxisMax_Real = double.Parse(textBox__Yaxismax.Text);
            double YAxisMin_Real = double.Parse(textBox__Yaxismin.Text);

            Point_Num = DataX.Count;

            double[,] Data = new double[Point_Num, 2];
            for (int i=0;i < Point_Num;i++)
            {
                Data[i, 0] = (Convert.ToDouble(DataX[i]) - XAxisMin.X) / (XAxisMax.X-XAxisMin.X)*(XAxisMax_Real - XAxisMin_Real) + XAxisMin_Real;
                Data[i, 1] = (Convert.ToDouble(DataY[i]) - YAxisMin.Y) / (YAxisMax.Y - YAxisMin.Y) * (YAxisMax_Real - YAxisMin_Real) + YAxisMin_Real;
            }

            Microsoft.Office.Tools.Excel.Worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            string s= "Serise" + Convert.ToString(Series_Num)+"_X";
            ((Excel.Range)worksheet.Cells[1, (Series_Num-1)*2+1]).Value2=s;
            s = "Serise" + Convert.ToString(Series_Num)+"_Y";
            ((Excel.Range)worksheet.Cells[1, (Series_Num - 1) * 2 + 2]).Value2 =s;

            Excel.Range c1 = (Excel.Range)worksheet.Cells[1+1, (Series_Num - 1) * 2 + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[1+Point_Num, (Series_Num - 1) * 2 + 2];

            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = Data;

            drawing = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Drawing_Line = false;
            checkBox_Line.Checked = false;
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

                        bmDraw = new Bitmap(this.pictureBox_Data.Width, this.pictureBox_Data.Height);
                        Bitmap inputimage=(Bitmap)Image.FromFile(s);

                        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmDraw);

                        double scale_width = Convert.ToDouble(bmDraw.Width) / inputimage.Width;
                        double scale_height = Convert.ToDouble(bmDraw.Height) / inputimage.Height;
                        double scale= scale_width;
                        if (scale_width > scale_height) scale = scale_height;

                        int centerx = bmDraw.Width / 2;
                        int centery = bmDraw.Height / 2;
                        int newWidth = Convert.ToInt32(inputimage.Width * scale / 2);
                        int newHeight = Convert.ToInt32(inputimage.Height * scale / 2);
                        Rectangle rg = new Rectangle(centerx- newWidth, centery- newHeight, newWidth *2, newHeight*2);
                        //将bm内rg所指定的区域绘制到bm1
                        g.DrawImage(inputimage, rg);
                        this.pictureBox_Data.Image = bmDraw;



                        //this.pictureBox_Data.Image = Image.FromFile(s);
                        Falg_Inputimage = true;

                        button_Xaxismin.Enabled = true;
                        // }
                    }
                    // }
                }
            }

        }

        

     

    }
}
