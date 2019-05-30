using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
namespace ExcelAddIn_Graphics
{
    public partial class BubbleSquare : Form
    {
        public double ratio;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        public double Max_size;
        public double Min_size;
        EasyCharts Graphic = new EasyCharts();

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;

        public BubbleSquare()
        {
            InitializeComponent();
            ratio = double.Parse(textBox_Bandwidth.Text);
            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
           start_col = activecells.Column;
           start_row = activecells.Row;

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row,start_col];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + 1];

            string ChartOrder = "SquareBubble" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);

            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count == 2)
            {
                Excel.Series Sseries2 = series.Item(2);
                Sseries2.Delete();
            }

            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1,start_col];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1,start_col];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);

            Excel.Point point;
            //double ratio = 42;
            int SquareSize;

            //((Excel.Range)worksheet.Cells[Graphic.start_row, Graphic.start_col + 3]).Value2 = "Square Size";

            Max_size = Double.MinValue;
            Min_size = Double.MaxValue;
            int i;
            for (i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 2]) > Max_size) Max_size = double.Parse(str[i, 2]);
                if (double.Parse(str[i, 2]) < Min_size) Min_size = double.Parse(str[i, 2]);
            }

            Max_size = Math.Sqrt(Max_size);
            Min_size = Math.Sqrt(Min_size);
            //int[,] PointSize = new int[Graphic.rows - 1, 1];
            for (i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                //SquareSize = (int)((Math.Sqrt(double.Parse(str[i, 2])) - Min_size));// / (Max_size-Min_size) * ratio) + 2;
                SquareSize = (int)((Math.Sqrt(double.Parse(str[i, 2])) / Max_size * ratio) + 2);
                //PointSize[i - 1, 0] = SquareSize;
                //((Excel.Range)worksheet.Cells[Graphic.start_row + i, Graphic.start_col + 3]).Value2 = SquareSize;
                point.MarkerSize = SquareSize;
            }


            Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();

            Sseries.Format.Fill.Solid();
            Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.Format.Fill.Transparency = 0.3F;

            Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare;

            //Rggplot2();
            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();
            worksheet.Activate();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            ratio = double.Parse(textBox_Bandwidth.Text); ;
            if (ratio == 0) return;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries = series.Item(1);

            Excel.Point point;
            int SquareSize;
            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                //SquareSize = (int)((Math.Sqrt(double.Parse(str[i, 2]))));// - Min_size) / (Max_size - Min_size) * ratio) + 2;
                SquareSize = (int)((Math.Sqrt(double.Parse(str[i, 2])) / Max_size * ratio) + 2);
                //point = (Excel.Point)Sseries.Points(i);
                //SquareSize = (int)((double.Parse(str[i, 2]) - Min_size) / (Max_size - Min_size) * ratio) + 2;
                //PointSize[i - 1, 0] = SquareSize;
                //((Excel.Range)worksheet.Cells[Graphic.start_row + i, Graphic.start_col + 3]).Value2 = SquareSize;
                point.MarkerSize = SquareSize;
            }

            Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();

            Sseries.Format.Fill.Solid();
            Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.Format.Fill.Transparency = 0.3F;

            chart.Refresh();
            worksheet.Activate();
        }
    }
}
