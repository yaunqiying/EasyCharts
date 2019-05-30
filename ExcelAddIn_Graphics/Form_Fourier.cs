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
    public partial class Form_Fourier : Form
    {
        public double Pi = 3.1415926;

        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        //public System.Drawing.Color RGB0;
        static int Nchart = 0;
        //public double Max_Value;
        //public double Min_Value;

        EasyCharts Graphic = new EasyCharts();

        public double Span;
        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;
        public int Nrows;
        public Form_Fourier()
        {
            InitializeComponent();
        }

        /*'***************************************************************
           'FFT0 数组下标以0开始  FFT1 数组下标以1开始
           'AR() 数据实部     AI() 数据虚部
           'N 数据点数，为2的整数次幂
           'NI 变换方向 1为正变换，-1为反变换
    '*************************************************************** */

        private void Form_Fourier_Load(object sender, EventArgs e)
        {
            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            //double[] FFTProjectionArray = new double[rows - 1];
            double[,] NewFFTProjectionArray = new double[rows - 1, 1];  //store the projection curve of FFT

           double  TwoTime = 0;
            double TwoResult = rows-1;
            while (TwoResult >1)
            {
                TwoResult = TwoResult / 2;
                TwoTime = TwoTime + 1;
            }
            Nrows = Convert.ToInt32(Math.Pow(2.0, TwoTime));
            double[] AR = new double[Nrows];
            //double[] NewAR = new double[Nrows];

            for (int j = 1; j < rows; j++)
            {
                //FFTProjectionArray[j - 1] = double.Parse(str[j, 1]);
                AR[j - 1] = double.Parse(str[j, 1]);
            }

            double[] AI = new double[Nrows];
            FFT0(ref AR, ref AI, Nrows, 1);

            Span = double.Parse(textBox_Bandwidth.Text);

            comboBox_FourierMethod.Text = "Low Pass";

            for (int j = Convert.ToInt32(rows * Span) + 1; j <Nrows; j++)
            {
                AR[j] = 0;
                AI[j] = 0;
            }

            FFT0(ref AR, ref AI, Nrows, -1);
            for (int j = 1; j < rows; j++)
            {
                NewFFTProjectionArray[j - 1, 0] = AR[j - 1];
            }

             ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = NewFFTProjectionArray;

            //*******************************************************************************************8
            c1 = (Excel.Range)worksheet.Cells[start_row, start_col];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols];

            string ChartOrder = "CurveFourier" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Format.Line.Visible = Office.MsoTriState.msoFalse;
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


        private void FFT0(ref double[] AR,ref double [] AI, int num, int NI)
        {
            int i, j, K, L, M;
            int IP, LE;
            int L1, N1, N2;
            double SN, TR, TI, WR, WI;
            double UR, UI, US;

            M = NTOM(num);
            N2 = num / 2;
            N1 = num - 1;
            SN = NI;
            j = 1;
            for (i=1;i<= N1;i++)
            {
                if (i<j)
                {
                    TR = AR[j - 1];
                    AR[j - 1] = AR[i - 1];
                    AR[i - 1] = TR;
                    TI = AI[j - 1];
                    AI[j - 1] = AI[i - 1];
                    AI[i - 1]= TI;
                }
                K = N2;
                while(K < j)
                {
                    j = j - K;
                    K = K / 2;
                }
                j = j + K;
            }
            for (L = 1; L <= M; L++)
            {
                LE = Convert.ToInt32(Math.Pow(2.0, L)); ;
                L1 = LE / 2;
                UR = 1.0;
                UI = 0.0;
                WR = Math.Cos(Pi / L1);
                WI = SN * Math.Sin(Pi / L1);
                for (j=1;j<= L1;j++)
                {
                    for (i = j; i <= num; i = i + LE)
                    {
                        IP = i + L1;
                        TR = AR[IP - 1] * UR - AI[IP - 1] * UI;
                        TI = AI[IP - 1] * UR + AR[IP - 1] * UI;
                        AR[IP - 1] = AR[i - 1] - TR;
                        AI[IP - 1] = AI[i - 1] - TI;
                        AR[i - 1] = AR[i - 1] + TR;
                        AI[i - 1] = AI[i - 1] + TI;
                 }
                    US = UR;
                    UR = US * WR - UI * WI;
                    UI = UI * WR + US * WI;
                }
            }

           if(SN!=-1)
            { for(i=1;i<= num;i++)
                {
                    AR[i - 1] = AR[i - 1] / num;
                    AI[i - 1] = AI[i - 1] / num;
                }
            }
        }

        public int NTOM(int n)
        {
            double ND = n;
            int NTOM = 0;
            while (ND>1)
            {
                ND = ND / 2;
                NTOM = NTOM + 1;
            }
            return NTOM;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            Span = double.Parse(textBox_Bandwidth.Text);
            if (Span<0 | Span>1) return;


            double[,] NewFFTProjectionArray = new double[rows - 1, 1];  //store the projection curve of FFT
            double[] AR = new double[Nrows];
            //double[] NewAR = new double[Nrows];

            for (int j = 1; j < rows; j++)
            {
                //FFTProjectionArray[j - 1] = double.Parse(str[j, 1]);
                AR[j - 1] = double.Parse(str[j, 1]);
            }

            double[] AI = new double[Nrows];
            FFT0(ref AR, ref AI, Nrows, 1);

            if (comboBox_FourierMethod.SelectedItem.Equals("Low Pass"))
            {
                for (int j = Convert.ToInt32(rows * Span); j < Nrows; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
            else if (comboBox_FourierMethod.SelectedItem.Equals("High Pass"))
            {
                for (int j = 0; j < Convert.ToInt32(rows * Span); j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
            else if (comboBox_FourierMethod.SelectedItem.Equals("Band Pass"))
            {
                for (int j = 0; j < Convert.ToInt32(rows * Span) + 1; j++)
                    {
                        AR[j] = 0;
                        AI[j] = 0;
                    }
                for (int j = Convert.ToInt32(rows * (1 - Span)); j < Nrows; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
                 }
            else if (comboBox_FourierMethod.SelectedItem.Equals("Band Block"))
            {
                for (int j = Convert.ToInt32(rows * Span); j < Convert.ToInt32(rows * (1 - Span)) + 1; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
               
            FFT0(ref AR, ref AI, Nrows, -1);
            for (int j = 1; j < rows; j++)
            {
                NewFFTProjectionArray[j - 1, 0] = AR[j - 1];
            }

             ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = NewFFTProjectionArray;
        }

        private void comboBox_FourierMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            Span = double.Parse(textBox_Bandwidth.Text);
            if (Span < 0 | Span > 1) return;

            double[,] NewFFTProjectionArray = new double[rows - 1, 1];  //store the projection curve of FFT
            double[] AR = new double[Nrows];
            //double[] NewAR = new double[Nrows];

            for (int j = 1; j < rows; j++)
            {
                //FFTProjectionArray[j - 1] = double.Parse(str[j, 1]);
                AR[j - 1] = double.Parse(str[j, 1]);
            }

            double[] AI = new double[Nrows];
            FFT0(ref AR, ref AI, Nrows, 1);

            if (comboBox_FourierMethod.SelectedItem.Equals("Low Pass"))
            {
                for (int j = Convert.ToInt32(rows * Span); j < Nrows; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
            else if (comboBox_FourierMethod.SelectedItem.Equals("High Pass"))
            {
                for (int j = 0; j < Convert.ToInt32(rows * Span); j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
            else if (comboBox_FourierMethod.SelectedItem.Equals("Band Pass"))
            {

                for (int j = 0; j < Convert.ToInt32(rows * Span) + 1; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
                for (int j = Convert.ToInt32(rows * (1 - Span)); j < Nrows; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }
            else if (comboBox_FourierMethod.SelectedItem.Equals("Band Block"))
            {
                for (int j = Convert.ToInt32(rows * Span); j < Convert.ToInt32(rows * (1 - Span)) + 1; j++)
                {
                    AR[j] = 0;
                    AI[j] = 0;
                }
            }

            FFT0(ref AR, ref AI, Nrows, -1);
            for (int j = 1; j < rows; j++)
            {
                NewFFTProjectionArray[j - 1, 0] = AR[j - 1];
            }

             ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Smooth Y";
            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + 1 + rows - 2, start_col + cols];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = NewFFTProjectionArray;
        }
    }
}
