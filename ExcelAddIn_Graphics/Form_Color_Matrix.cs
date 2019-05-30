using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using range = Microsoft.Office.Interop.Excel.Range;
namespace ExcelAddIn_Graphics
{
    public partial class Form_Color_Matrix : Form
    {
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        EasyCharts Graphic = new EasyCharts();
        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;
        public int height;
        public Form_Color_Matrix()
        {
            InitializeComponent();
            height = int.Parse(textBox_height.Text);
            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            int width = (int)Math.Ceiling(Convert.ToDouble(rows) / Convert.ToDouble(height));
            int[,] data = new int[height, width];
            int[] temp = new int[width];
           // for (int i = 0; i < height; i++)
           // {
                //temp = new int[width];
              //  for (int j = 0; j < width; j++)
             //   {
             //      // temp[j] = 1;
             //       data[i, j] = 1;
             //       ((range)worksheet.Cells[start_row + rows + 1 + i , start_col + j ]).Value2 = 1;
             //   }
                //Sseries = series.NewSeries();
                //Sseries.Values = temp;

           // }

            range c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows + 1 + height - 1, start_col + width - 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            //chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnStacked100;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries;
         
            Excel.Point point;
            int RGB_B, RGB_G, RGB_R;
            for (int i = 0; i < height; i++)
            {
                temp = new int[width];
                for (int j = 0; j <width; j++)
                {
                    temp[j] = 1;
                    data[i, j] = 1;
                }
                Sseries = series.NewSeries();
                Sseries.Values = temp;

            }

            for (int i = 1; i <= height; i++)
            {
                Sseries = series.Item(i);
                for (int j = 1; j <= width; j++)
                {
                    {
                        point = (Excel.Point)Sseries.Points(j);
                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                        int index = (i-1) * width + j ;
                        if (index < rows)
                        {
                            RGB_B = Convert.ToInt32(double.Parse(str[index, cols - 1])); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                            RGB_G = Convert.ToInt32(double.Parse(str[index, cols - 2])); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                            RGB_R = Convert.ToInt32(double.Parse(str[index, cols - 3])); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                            point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb();

                        }
                        else
                        {
                            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                            point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255,255, 255).ToArgb();

                        }
                    }


                }
            }
           

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            height = int.Parse(textBox_height.Text);
            
            int width = (int)Math.Ceiling(Convert.ToDouble(rows) / Convert.ToDouble(height));
            int[,] data = new int[height, width];
            int[] temp = new int[width];
            
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            int N_Ser = series.Count;
            Excel.Series Sseries;
            if (N_Ser > height) {
                for (int i = N_Ser; i > height; i--)
                {
                    series.Item(i).Delete();
                }
            }
            else{
                for (int i = N_Ser+1; i <= height; i++)
                {
                    Sseries = series.NewSeries();
                }
            }

           

            Excel.Point point;
            int RGB_B, RGB_G, RGB_R;
            for (int i = 0; i < height; i++)
            {
                temp = new int[width];
                for (int j = 0; j < width; j++)
                {
                    temp[j] = 1;
                    data[i, j] = 1;
                }
               //Sseries = series.NewSeries();
                series.Item(i+1).Values = temp;

            }

            for (int i = 1; i <= height; i++)
            {
                Sseries = series.Item(i);
                for (int j = 1; j <= width; j++)
                {
                    {
                        point = (Excel.Point)Sseries.Points(j);
                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                        int index = (i - 1) * width + j;
                        if (index < rows)
                        {
                            RGB_B = Convert.ToInt32(double.Parse(str[index, cols - 1])); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                            RGB_G = Convert.ToInt32(double.Parse(str[index, cols - 2])); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                            RGB_R = Convert.ToInt32(double.Parse(str[index, cols - 3])); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                            point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb();

                        }
                        else
                        {
                            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                            point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();

                        }
                    }


                }
            }

        }
    }
}
