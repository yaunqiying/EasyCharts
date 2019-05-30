using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class CurveLOESS : Form
    {
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        //public System.Drawing.Color RGB0;
        static int Nchart = 0;
        //public double Max_Value;
        //public double Min_Value;

        EasyCharts Graphic = new EasyCharts();

        public long Span;
        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;
        public double[] X, Y;
        public CurveLOESS()
        {
            InitializeComponent();

            //int rows = 1;
            //int cols = 1;
            //string[,] str = new string[1, 1];
            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

             Y = new double[rows - 1];
             X = new double[rows - 1];
            int  j;
            for (j = 1; j < rows; j++)
            {
                X[j - 1] = double.Parse(str[j, 0]);
                Y[j - 1] = double.Parse(str[j, 1]);
            }

            Span = long.Parse(textBox_Bandwidth.Text); 
            double[] yLoess = new double[rows - 1];
            QLOESS(ref Y, ref X, ref yLoess, Span);

            double[,] data = new double[rows - 1, 1];
            for (j = 1; j < rows; j++)
            {
                data[j - 1, 0] = yLoess[j - 1];
            }

          ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (Excel.Range)worksheet.Cells[start_row, start_col];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols];

            string ChartOrder = "CurveLOESS" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
          
            Excel.Series Sseries2= series.Item(2);
            Sseries2.Format.Line.Visible= Office.MsoTriState.msoFalse;
            Sseries2.Format.Fill.Solid();
            Sseries2.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            //Sseries2.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            //Sseries2.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255,229, 184,0).ToArgb();
            //Sseries2.Format.Fill.Transparency = 0.6F;

            Sseries2.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
            Sseries2.MarkerSize = 6;

            Excel.Series Sseries3 = series.Item(3);
            Sseries3.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            Sseries3.Format.Line.Visible = Office.MsoTriState.msoTrue;
            Sseries3.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;
            //Sseries3.Format.Line.ForeColor.RGB= System.Drawing.Color.FromArgb(255, 248, 118, 109).ToArgb();
            Sseries3.Format.Line.Weight = 1.25F;

            Excel.Series Sseries1 = series.Item(1);
            Sseries1.Delete();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            Span = long.Parse(textBox_Bandwidth.Text);
            if (Span == 0) return;

            double[] yLoess = new double[rows - 1];
            QLOESS(ref Y, ref X, ref yLoess, Span);

            double[,] data = new double[rows - 1, 1];
            int j;
            for (j = 1; j < rows; j++)
            {
                data[j - 1, 0] = yLoess[j - 1];
            }

          ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            chart.Refresh();
        }

        private void QLOESS(ref double[] Y, ref double[] X, ref double[] yLoess, long Span)
        {
            long i, Ubd;
            long iMin, iMax, iPoint;
            double maxDist;
            decimal SumWts, SumWtX, SumWtX2, SumWtX3, SumWtX4, SumWtY, SumWtYX, SumWtYXX;
            decimal Denom, A, B, C;
            double xNow;
            double[] distance, weight, X_out;// X,

            Ubd = Y.Length;
            //X = new double[Ubd];
            X_out = new double[Ubd];

            for (i = 0; i < Ubd; i++)
            {
                //X[i] = i + 1;
                X_out[i] = X[i];// i + 1;
            }

            for (iPoint = 0; iPoint < Ubd; iPoint++)
            {
                iMin = 0;
                iMax = Ubd - 1;
                xNow = X_out[iPoint];

                distance = new double[iMax + 1];
                weight = new double[iMax + 1];

                for (i = iMin; i <= iMax; i++)
                {
                    //populate x,y distance
                    distance[i] = Math.Abs(X[i] - xNow);
                }

                while (iMax + 1 - iMin > Span)
                {
                    //if (iMax + 1 - iMin <= Span) break;
                    if (distance[iMin] > distance[iMax])
                    {
                        //remove first point
                        iMin = iMin + 1;
                    }
                    else if (distance[iMin] < distance[iMax])
                    {
                        // remove last point
                        iMax = iMax - 1;
                    }
                    else
                    {
                        // remove both point
                        iMin = iMin + 1;
                        iMax = iMax - 1;
                    }
                }

                //Find max distance
                maxDist = Double.MinValue;
                for (i = iMin; i <= iMax; i++)
                {
                    if (distance[i] > maxDist) maxDist = distance[i]+(0.000000000000000000001);
                }

                //calculate weights using scaled distances
                double tempweight;
                for (i = iMin; i <= iMax; i++)
                {
                    tempweight = 1.0 - (distance[i] * distance[i] * distance[i]) / (maxDist * maxDist * maxDist);
                    weight[i] = tempweight * tempweight * tempweight;
                }

                //do the sums of squares
                SumWts = 0;
                SumWtX = 0;
                SumWtX2 = 0;
                SumWtX3 = 0;
                SumWtX4 = 0;
                SumWtY = 0;
                SumWtYX = 0;
                SumWtYXX = 0;

                for (i = iMin; i <= iMax; i++)
                {
                    SumWts = SumWts + Convert.ToDecimal(weight[i]);
                    SumWtX = SumWtX + Convert.ToDecimal(X[i] * weight[i]);
                    SumWtX2 = SumWtX2 + Convert.ToDecimal((X[i] * X[i]) * weight[i]);
                    SumWtX3 = SumWtX3 + Convert.ToDecimal((X[i] * X[i] * X[i]) * weight[i]);
                    SumWtX4 = SumWtX4 + Convert.ToDecimal((X[i] * X[i] * X[i] * X[i]) * weight[i]);
                    SumWtY = SumWtY + Convert.ToDecimal(Y[i] * weight[i]);
                    SumWtYX = SumWtYX + Convert.ToDecimal(X[i] * Y[i] * weight[i]);
                    SumWtYXX = SumWtYXX + Convert.ToDecimal(Y[i] * X[i] * X[i] * weight[i]);
                }

                Denom = (SumWts * SumWtX2 * SumWtX4) - (SumWts * SumWtX3 * SumWtX3) - (SumWtX * SumWtX * SumWtX4) + (2 * SumWtX3 * SumWtX * SumWtX2) - (SumWtX2 * SumWtX2 * SumWtX2);

                if (Denom > 0 || Denom < 0)
                {
                    // calculate the regression coefficients , and finally the loess value : Y = A * X^2 + B * X + C
                    A = ((SumWtX3 * SumWtX * SumWtY) - (SumWtX * SumWtX * SumWtYXX) - (SumWts * SumWtX3 * SumWtYX) + (SumWts * SumWtX2 * SumWtYXX) + (SumWtX * SumWtX2 * SumWtYX) - (SumWtX2 * SumWtX2 * SumWtY)) / Denom;
                    B = (-(SumWts * SumWtX3 * SumWtYXX) + (SumWts * SumWtX4 * SumWtYX) - (SumWtX2 * SumWtX2 * SumWtYX) + (SumWtX * SumWtX2 * SumWtYXX) + (SumWtX2 * SumWtX3 * SumWtY) - (SumWtX * SumWtX4 * SumWtY)) / Denom;
                    C = ((SumWtX * SumWtX3 * SumWtYXX) - (SumWtX * SumWtX4 * SumWtYX) + (SumWtX2 * SumWtX3 * SumWtYX) - (SumWtX2 * SumWtX2 * SumWtYXX) + (SumWtX2 * SumWtX4 * SumWtY) - (SumWtX3 * SumWtX3 * SumWtY)) / Denom;
                    yLoess[iPoint] = Convert.ToDouble((A * Convert.ToDecimal(xNow * xNow)) + (B * Convert.ToDecimal(xNow)) + C);
                }
                else
                {
                    yLoess[iPoint] = Y[iPoint];
                }

                //if (iPoint > 591)
                //    {
                //        Single ps = 0;
                //    }

            }
        }
    }


}


//private void CurveLOESSsmooth()
//{
//    int rows = 1;
//    int cols = 1;
//    string[,] str = new string[1, 1];
//    RangeData(ref str, ref rows, ref cols);

//    worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
//    range activecells = (range)Globals.ThisAddIn.Application.ActiveCell;
//    int start_col = activecells.Column;
//    int start_row = activecells.Row;

//    double[] Y = new double[rows - 1];
//    double[] X = new double[rows - 1];
//    int i, j;
//    for (j = 1; j < rows; j++)
//    {
//        X[j - 1] = double.Parse(str[j, 0]);
//        Y[j - 1] = double.Parse(str[j, 1]);
//    }

//    long Span = 10;
//    double[] yLoess = new double[rows - 1];
//    QLOESS(ref Y, ref X, ref yLoess, Span);

//    double[,] data = new double[rows - 1, 1];
//    for (j = 1; j < rows; j++)
//    {
//        data[j - 1, 0] = yLoess[j - 1];
//    }

//        ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
//    Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
//    Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
//    Excel.Range range = worksheet.get_Range(c1, c2);
//    range.Value = data;
//}
