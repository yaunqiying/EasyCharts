using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class Form_Cofficient : Form
    {

        public double ratio;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        //public double Max_size;
        //public double Min_size;

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;

        public System.Drawing.Color RGB0;
        public System.Drawing.Color RGB1;

        EasyCharts Graphic = new EasyCharts();

        public Form_Cofficient()
        {
            InitializeComponent();

            ratio = double.Parse(textBox_Bandwidth.Text); ;

            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            double[,] Mat_Cofficent = new double[cols, cols ];
            double[] temp1 = new double [rows - 1];
            double[] temp2 = new double [rows - 1];

            int i, j;
            for ( i=0;i< cols;i++)
            {
                Mat_Cofficent[i, i] = 1;

                for ( j = 1; j < rows; j++)
                {
                    temp1[j - 1] = double.Parse(str[j, i]);
                }

                for (int k = i + 1; k < cols; k++)
                {
                    for ( j = 1; j < rows; j++)
                    {
                        temp2[j - 1] = double.Parse(str[j, k]);
                    }

                    Mat_Cofficent[i, k] = GetCorrelation(temp1, temp2);
                    Mat_Cofficent[k, i] = Mat_Cofficent[i, k];
                }
            }

            for ( i = 0; i < cols; i++)
            {
                ((Excel.Range)worksheet.Cells[start_row+rows+1, start_col + i+1]).Value2 =str[0,i];

                ((Excel.Range)worksheet.Cells[start_row + rows + 1+i+1, start_col]).Value2 = str[0, i];
            }
             
              
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows + 2+cols-1, start_col + 1+cols-1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = Mat_Cofficent;

            //****************************************************************************************************
            ratio = double.Parse(textBox_Bandwidth.Text); ;
            str = new string[cols+1, cols+1];
            for (i = 0; i <= cols; i++)
            {
                for (j =0; j <= cols; j++)
                {
                    str[i, j] =Convert.ToString( ((Excel.Range)worksheet.Cells[start_row + rows + 1+i, start_col + j]).Value2);
                }
            }
            // Graphic.RangeData(ref str, ref rows, ref cols);
            start_row = start_row + rows + 1;

            rows = cols+1;
            cols = cols + 1;
            //worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            //activecells = Globals.ThisAddIn.Application.ActiveCell;
            //start_col = activecells.Column;
           

            for (j = 1; j < cols; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1, start_col + j]).Value2 = str[0, j];
            }

            for (j = 1; j < rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1 + j, start_col]).Value2 = str[j, 0];
            }


            for (i = 1; i < rows; i++)
            {
                for (j = 1; j < cols; j++)
                {
                    ((Excel.Range)worksheet.Cells[start_row + rows + 1 + i, start_col + j]).Value2 = i;// str[i, 0];
                }
            }

            ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col]).Value2 = "Assistant1";
            for (j = 1; j < rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col + j]).Value2 = 0;
            }

            ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col]).Value2 = "Assistant2";
            for (j = 1; j < rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + j]).Value2 = j;
            }


             c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1, start_col];
             c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + rows - 1];

            string ChartOrder = "BubbleRelationShip" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 300, 300, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlRows);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            //double ratio = 21;
            //double Max_size = 0;
            //Max_size = Double.MinValue;
            //Min_size = Double.MaxValue;
            //for (i = 1; i < rows; i++)
            //{
            //    for (j = 1; j < cols; j++)
            //    {
            //        if (double.Parse(str[i, j]) > Max_size) Max_size = double.Parse(str[i, j]);
            //        if (double.Parse(str[i, j]) < Min_size) Min_size = double.Parse(str[i, j]);
            //    }
            //}
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Point point;
            Excel.Series Sseries;
            int SquareSize;

            RGB0 = System.Drawing.Color.FromArgb(255, 248, 118, 109);
            RGB1 = System.Drawing.Color.FromArgb(255, 0, 184, 229);
            for (i = 1; i < rows; i++)
            {
                Sseries = series.Item(i);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;

                //if (comboBox_FourierMethod.SelectedItem.Equals("Circle"))
                //{
                Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                //}
                //else if (comboBox_FourierMethod.SelectedItem.Equals("Square"))
                //{
                //    Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare;
                //}
                //Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;

                for (j = 1; j < cols; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);

                    if (double.Parse(str[i, j])>=0)
                    {
                        point.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                        point.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();

                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                        point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                        point.Format.Fill.Transparency = 0.2F;
                    }
                    
                    else
                    {
                        point.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();
                        point.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();

                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                        point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();
                        point.Format.Fill.Transparency = 0.2F;
                    }

                    // SquareSize = (int)((double.Parse(str[i, j]) - Min_size) / (Max_size - Min_size) * ratio) + 2;
                    if (i == j)
                    {
                        SquareSize = 2;
                        point.MarkerSize = SquareSize;
                    }
                    else
                    {
                        SquareSize = (int)(Math.Sqrt(Math.Abs((double.Parse(str[i, j])))) * ratio + 2);
                        if (SquareSize > 72)
                        { SquareSize = 72; }
                        point.MarkerSize = SquareSize;
                    }
                }
            }


            Excel.Series SseriesX = series.Item(rows);
            for (j = 1; j < cols; j++)
            {
                point = (Excel.Point)SseriesX.Points(j);
                point.HasDataLabel = true;
                point.DataLabel.Text = str[0, j];
                point.DataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionBelow;

                point.DataLabel.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                point.DataLabel.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.Size = 10;
                point.DataLabel.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

                point.DataLabel.Format.TextFrame2.Orientation = Office.MsoTextOrientation.msoTextOrientationUpward;
            }
            SseriesX.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

            Excel.Series SseriesY = series.Item(rows + 1);
            c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col + +1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col + rows - 1];
            SseriesY.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + rows - 1];
            SseriesY.Values = worksheet.get_Range(c1, c2);
            SseriesY.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

            for (j = 1; j < rows; j++)
            {
                point = (Excel.Point)SseriesY.Points(j);
                point.HasDataLabel = true;
                point.DataLabel.Text = str[j, 0];
                point.DataLabel.Position = Excel.XlDataLabelPosition.xlLabelPositionLeft;

                point.DataLabel.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                point.DataLabel.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
                point.DataLabel.Format.TextFrame2.TextRange.Font.Size = 10;
                point.DataLabel.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;
            }
            //***************************************************** Style**********************************************
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            //chart.ChartArea.Height = 340.157480315;
            //chart.ChartArea.Width = 380.5039370079;

            // Chart Type: XYScatter
            // Add GridLinesMinorMajor
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

            //y axis
            Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);

            axis.MinorUnit = 1;
            axis.MajorUnit = 1;

            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)0.25;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            //axis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            //axis.MinorGridlines.Format.Line.Weight = (float)0.25;
            //axis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.HasTitle = true;
            axis.AxisTitle.Text = "y axis";

            axis.HasDisplayUnitLabel = false;
            //axis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //axis.TickLabels.Font.Name = "Times New Roman";
            //axis.TickLabels.Font.Size = 10;
            axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionNone;

            //axis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;


            //x axis
            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);

            axis.MaximumScale = cols;
            axis.MinimumScale = 0;
            //axis.AxisBetweenCategories = false;
            axis.MinorUnit = 1;
            axis.MajorUnit = 1;

            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            //axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            //axis.MajorGridlines.Format.Line.Weight = (float)1.25;
            //axis.MajorGridlines.Format.Line.DashStyle = Microsoft.Office.Core.MsoLineDashStyle.msoLineSolid;

            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)0.25;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;

            axis.HasTitle = true;
            axis.AxisTitle.Text = "x axis";


            //axis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //axis.TickLabels.Font.Name = "Times New Roman";
            //axis.TickLabels.Font.Size = 10;
            axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionNone;

            //axis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            //axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();
            worksheet.Activate();

        }

        private void Form_Cofficient_Load(object sender, EventArgs e)
        {

        }

        private static double GetAverage(double[] data)
        {
            int len = data.Length;

            if (len == 0)
                throw new Exception("No data");

            double sum = 0;

            for (int i = 0; i < data.Length; i++)
                sum += data[i];

            return sum / len;
        }

        /// <summary>
        /// 获取标准差，求传入数组的标准差
        /// </summary>
        private static double GetStdev(double[] data)
        {
            // return Math.Sqrt(GetVariance(data));
            double x2 = 0;
            double sumx = 0;
            int n = data.Length;
            foreach (double d in data)
            {
                x2 += d * d;
                sumx += d;
            }
            return Math.Sqrt((x2 * n - sumx * sumx) / (n * (n - 1)));
        }

        /// <summary>
        /// 求两列数据的相关系数
        /// </summary>
        private static double GetCorrelation(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                throw new Exception("Length of sources is different");
            double avgX = 0;
            double avgY = 0;
            double sumx = 0;
            double sumy = 0;
            double xy = 0;
            double x2 = 0;
            double y2 = 0;
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                sumx += x[i];
                sumy += y[i];
                xy += x[i] * y[i];
                x2 += x[i] * x[i];
                y2 += y[i] * y[i];
            }
            avgX = sumx / len;
            avgY = sumy / len;
            double exy = xy / len;
            double exey = avgX * avgY;
            double ex2 = x2 / len;
            double e2x = avgX * avgX;
            double ey2 = y2 / len;
            double e2y = avgY * avgY;
            double fm1 = Math.Sqrt(ex2 - e2x);
            double fm2 = Math.Sqrt(ey2 - e2y);
            return (exy - exey) / (fm1 * fm2);
        }

        /// <summary>
        /// 获取离散系数，求传入数组的标准差/均值即离散系数。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double GetCoefficientofVariation(double[] x)
        {
            double x2 = 0;
            double sumx = 0;
            int n = x.Length;
            foreach (double d in x)
            {
                x2 += d * d;
                sumx += d;
            }
            return Math.Sqrt((x2 * n - sumx * sumx) / (n * (n - 1))) / (sumx / n);
        }

        /// <summary>
        /// 获取预警区间，（均值-标准差，均值+标准差）
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double[] GetForewarnInterval(double[] x)
        {
            double[] yjqj = { 0, 0 };
            double x2 = 0;
            double sumx = 0;
            int n = x.Length;
            foreach (double d in x)
            {
                x2 += d * d;
                sumx += d;
            }
            double a = sumx / n;
            double b = Math.Sqrt((x2 * n - sumx * sumx) / (n * (n - 1)));
            yjqj[0] = a - b;
            yjqj[1] = a + b;
            return yjqj;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            ratio = double.Parse(textBox_Bandwidth.Text); ;
            if (ratio == 0) return;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Point point;
            Excel.Series Sseries;
            int SquareSize;

            int i, j;
            for (i = 1; i < rows; i++)
            {
                Sseries = series.Item(i);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;

                //if (comboBox_FourierMethod.SelectedItem.Equals("Circle"))
                //{
                Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                //}
                //else if (comboBox_FourierMethod.SelectedItem.Equals("Square"))
                //{
                //    Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare;
                //}
                //Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;

                for (j = 1; j < cols; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);

                    if (double.Parse(str[i, j]) >= 0)
                    {
                        point.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                        point.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();

                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                        point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                        point.Format.Fill.Transparency = 0.2F;
                    }

                    else
                    {
                        point.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();
                        point.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();

                        point.Format.Fill.Solid();
                        point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                        point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                        point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();
                        point.Format.Fill.Transparency = 0.2F;
                    }

                    // SquareSize = (int)((double.Parse(str[i, j]) - Min_size) / (Max_size - Min_size) * ratio) + 2;
                    if (i==j)
                    {
                        SquareSize =2;
                        point.MarkerSize = SquareSize;
                    }
                    else
                    {
                        SquareSize = (int)(Math.Sqrt(Math.Abs((double.Parse(str[i, j])))) * ratio + 2);
                        if (SquareSize > 72)
                        { SquareSize = 72; }
                        point.MarkerSize = SquareSize;
                    }
                   
                }
            }
        }
    }
}

