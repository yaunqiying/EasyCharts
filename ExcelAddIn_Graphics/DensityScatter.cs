using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;


namespace ExcelAddIn_Graphics
{
    public partial class DensityScatter: Form
    {
        public double Bandwidth;
        //public int flag = 0;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        EasyCharts Graphic =new EasyCharts();

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;

        public DensityScatter()
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

            int i;
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "X-axis Value";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "Y-axis Value";

            double Pdensity = Bandwidth * 100;
            double Maxtemp = 1000;
            double[,] data = new double[rows, 2];
            for (i = 1; i < rows; i++)
            {
                data[i - 1, 0] = Math.Floor(double.Parse(str[i, 0]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
                data[i - 1, 1] = Math.Floor(double.Parse(str[i, 1]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
                //((Excel.Range)worksheet.Cells[start_row + i, start_col + cols]).Value2 = Math.Floor(double.Parse(str[i, 0]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
                //((Excel.Range)worksheet.Cells[start_row + i, start_col + cols + 1]).Value2 = Math.Floor(double.Parse(str[i, 1]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
            }

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + cols - 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "DensityScatter" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (Excel.Range)worksheet.Cells[start_row, start_col + cols];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + cols - 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count==2)
            {
                Excel.Series Sseries2 = series.Item(2);
                Sseries2.Delete();
            }
           
            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);

            Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
            //Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Sseries.MarkerSize = 5;
            Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 109, 118, 248).ToArgb();
            Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 109, 118, 248).ToArgb();
            //Sseries.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;

            Sseries.Format.Fill.Solid();
            Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 109, 118, 248).ToArgb();
            Sseries.Format.Fill.Transparency = 0.9F;

            worksheet.Activate();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
           // textBox_Bandwidth.Text = Convert.ToString(hScrollBar_Parameter1.Value);
            if (textBox_Bandwidth.Text=="") return;
            Bandwidth = double.Parse(textBox_Bandwidth.Text); ;
            if (Bandwidth == 0) return;

            double Pdensity = Bandwidth * 100;
            double Maxtemp = 1000;
            int i;
            double[,] data = new double[rows, 2];
            for (i = 1; i < rows; i++)
            {
                data[i - 1, 0] = Math.Floor(double.Parse(str[i, 0]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
                data[i - 1, 1] = Math.Floor(double.Parse(str[i, 1]) * Maxtemp / Pdensity) * Pdensity / Maxtemp;
            }

            //worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + cols - 1];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            //Microsoft.Office.Interop.Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;
            c1 = (Excel.Range)worksheet.Cells[start_row, start_col + cols];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + cols - 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.Refresh();
            worksheet.Activate();
        }

     
    }
}
