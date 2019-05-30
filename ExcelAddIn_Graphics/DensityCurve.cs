using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;


namespace ExcelAddIn_Graphics
{
    public partial class DensityCurve : Form
    {
        public double Bandwidth;
        //public int flag = 0;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        EasyCharts Graphic = new EasyCharts();
        public double Max_Value;
        public double Min_Value;
        public int Nstep=10;

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;
        public DensityCurve()
        {
            InitializeComponent();
            Bandwidth = double.Parse(textBox_Bandwidth.Text);
            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            Max_Value = Double.MinValue;
            Min_Value = Double.MaxValue;
            for (int i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 0]) > Max_Value) Max_Value = double.Parse(str[i, 0]);
                if (double.Parse(str[i, 0]) < Min_Value) Min_Value = double.Parse(str[i, 0]);
            }

            double Step = Bandwidth / Nstep;
            int Nrows = Convert.ToInt32((Max_Value - Min_Value) / Step + 2);
            double[,] data = new double[Nrows, 1];
            double[,] x = new double[Nrows, 1];

            double tempx;
            for (int i = 0; i < Nrows; i++)
            {
                x[i, 0] = i * Step + Min_Value;
                for (int j = 1; j < rows; j++)
                {
                    tempx = (x[i, 0] - double.Parse(str[j, 0])) / Bandwidth;
                    data[i, 0] = data[i, 0] + 1 / ((rows - 1) * Bandwidth) * Math.Exp(-tempx * tempx / 2) / Math.Sqrt(2 * 3.124259);
                }
            }

            ((Excel.Range)worksheet.Cells[start_row,start_col + cols + 1]).Value2 = "X axis";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "Y axis";

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col +cols + 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value =x;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows , start_col + cols + 2];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "DensityCurve" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(300, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (Excel.Range)worksheet.Cells[start_row,start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 2];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlArea;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Delete();

            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 1];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows , start_col + cols + 2];
            Sseries.Values = worksheet.get_Range(c1, c2);

            chart.HasLegend = false;
            chart.HasTitle = false;
            worksheet.Activate();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            Bandwidth = double.Parse(textBox_Bandwidth.Text); ;
            if (Bandwidth == 0) return;

            double Step = Bandwidth / Nstep;
            int Nrows = Convert.ToInt32((Max_Value - Min_Value) / Step + 2);
            double[,] data = new double[Nrows, 1];
            double[,] x = new double[Nrows, 1];

            double tempx;
            for (int i = 0; i < Nrows; i++)
            {
                x[i, 0] = i * Step + Min_Value;
                for (int j = 1; j < rows; j++)
                {
                    tempx = (x[i, 0] - double.Parse(str[j, 0])) / Bandwidth;
                    data[i, 0] = data[i, 0] + 1 / ((rows - 1) * Bandwidth) * Math.Exp(-tempx * tempx / 2) / Math.Sqrt(2 * 3.124259);
                }
            }

            ((Excel.Range)worksheet.Cells[start_row, start_col +cols + 1]).Value2 = "X axis";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "Y axis";

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1,start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = x;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1,start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 2];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (Excel.Range)worksheet.Cells[start_row,start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col +cols + 2];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            //chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlArea;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Delete();

            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1,start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 1];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col +cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 2];
            Sseries.Values = worksheet.get_Range(c1, c2);
            chart.Refresh();
            worksheet.Activate();
        }
    }
}
