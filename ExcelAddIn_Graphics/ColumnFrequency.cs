using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class ColumnFrequency : Form
    {
        public double Bandwidth=0.1;
        //public int flag = 0;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        public double Max_Value;
        public double Min_Value;
        EasyCharts Graphic = new EasyCharts();

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;


        public ColumnFrequency()
        {
            InitializeComponent();
            //hScrollBar_Parameter1.Value = 2;
            //Graphics Graphic = new Graphics();
            Bandwidth = double.Parse(textBox_Bandwidth.Text);
            //if (Form_Parater.flag == 0) return;
            //int rows = 1;
            //int cols = 1;
            //string[,] str = new string[1, 1];
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

            int Nrows = Convert.ToInt32((Max_Value - Min_Value) / Bandwidth + 2);
            double[,] data = new double[Nrows,1];
            int idx = 0;
            for (int i = 1; i < rows; i++)
            {
                idx = Convert.ToInt32((double.Parse(str[i, 0]) - Min_Value) / Bandwidth);
                data[idx,0] = data[idx,0] + 1;
            }
            data[Nrows - 2,0] = data[Nrows - 2,0] + data[Nrows - 1,0];


            double[,] x = new double[Nrows-1,1];
            string[,] label = new string[Nrows-1,1];
            for (int i = 0; i < Nrows - 1; i++)
            {
                label[i,0] = "[" + Convert.ToString(Math.Floor((i * Bandwidth + Min_Value) * 100) / 100) + ","
                            + Convert.ToString(Math.Floor(((i + 1) * Bandwidth + Min_Value) * 100) / 100) + ")";
                //((Excel.Range)worksheet.Cells[start_row + 1 + i, start_col + cols + 1]).Value2 = label[i];
                //((Excel.Range)worksheet.Cells[start_row + 1 + i, start_col + cols + 3]).Value2 = data[i];
                x[i, 0] = i * Bandwidth + Min_Value;
            }
         
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "X bandwidth";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "X axis";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 3]).Value2 = "Y axis";

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = label;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows-1, start_col + cols + 2];
            range = worksheet.get_Range(c1, c2);
            range.Value = x;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col +cols + 3];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;
            ((Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 3]).Value2=""; ;

            string ChartOrder = "FrequencyColumn" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(300, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (Excel.Range)worksheet.Cells[start_row, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1,start_col + cols + 3];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnClustered;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count == 2)
            {
                Excel.Series Sseries2 = series.Item(2);
                Sseries2.Delete();
            }

            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col +cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 2];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1,start_col + cols + 3];
            Sseries.Values = worksheet.get_Range(c1, c2);


            Sseries.Format.Fill.Visible = Office.MsoTriState.msoTrue;
            Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 88, 180, 13).ToArgb();
            Sseries.Format.Line.Visible = Office.MsoTriState.msoTrue;
            Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255,0, 0, 0).ToArgb();

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

           
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

            int Nrows = Convert.ToInt32((Max_Value - Min_Value) / Bandwidth + 2);
            double[,] data = new double[Nrows, 1];
            int idx = 0;
            for (int i = 1; i < rows; i++)
            {
                idx = Convert.ToInt32((double.Parse(str[i, 0]) - Min_Value) / Bandwidth);
                data[idx, 0] = data[idx, 0] + 1;
            }
            data[Nrows - 2, 0] = data[Nrows - 2, 0] + data[Nrows - 1, 0];


            double[,] x = new double[Nrows - 1, 1];
            string[,] label = new string[Nrows - 1, 1];
            for (int i = 0; i < Nrows - 1; i++)
            {
                label[i, 0] = "[" + Convert.ToString(Math.Floor((i * Bandwidth + Min_Value) * 100) / 100) + ","
                            + Convert.ToString(Math.Floor(((i + 1) * Bandwidth + Min_Value) * 100) / 100) + ")";
                //((Excel.Range)worksheet.Cells[start_row + 1 + i, start_col + cols + 1]).Value2 = label[i];
                //((Excel.Range)worksheet.Cells[start_row + 1 + i, start_col + cols + 3]).Value2 = data[i];
                x[i, 0] = i * Bandwidth + Min_Value;
            }

            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "X bandwidth";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "X axis";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 3]).Value2 = "Y axis";

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = label;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 2];
            range = worksheet.get_Range(c1, c2);
            range.Value = x;

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 3];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;
            ((Excel.Range)worksheet.Cells[start_row + Nrows, start_col + cols + 3]).Delete(); ;

            c1 = (Excel.Range)worksheet.Cells[start_row, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 3];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Delete();

            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 2];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (Excel.Range)worksheet.Cells[start_row + Nrows - 1, start_col + cols + 3];
            Sseries.Values = worksheet.get_Range(c1, c2);

            chart.Refresh();
            worksheet.Activate();
        }

    }
}
