using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class ColumnColor : Form
    {
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        public System.Drawing.Color RGB0;
        static int Nchart = 0;
        public double Max_Value;
        public double Min_Value;

        EasyCharts Graphic = new EasyCharts();

        public double Hrange;
        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;

        public ColumnColor()
        {
            InitializeComponent();

            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row, start_col];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols - 2];

            string ChartOrder = "ColorColumn" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnClustered;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count==2)
            {
                Excel.Series Sseries2 = series.Item(2);
                Sseries2.Delete();
            }
            
            Excel.Series Sseries = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);

            Excel.Point point;
            int[] HSV0 = new int[3];

            RGB0 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            System.Drawing.Color RGB1 = System.Drawing.Color.FromArgb(255, 96, 157, 202);

            Graphic.RGB2HSV(RGB0, ref HSV0);

            Max_Value = Double.MinValue;
            Min_Value = Double.MaxValue;
            for (int i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 2]) > Max_Value) Max_Value = double.Parse(str[i, 1]);
                if (double.Parse(str[i, 2]) < Min_Value) Min_Value = double.Parse(str[i, 1]);
            }

            int[] HSV = new int[3];
            HSV0.CopyTo(HSV, 0);
            double ratio;
            Hrange = double.Parse(textBox_Bandwidth.Text);
            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                ratio = (double.Parse(str[i, 2]) - Min_Value) / (Max_Value - Min_Value);
                HSV[2] = HSV0[2] + Convert.ToInt32(Hrange * (ratio - 0.5));
                Graphic.HSV2RGB(ref RGB1, HSV);
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb(); 
                point.Format.Fill.Transparency = 0.0F;
            }
            //Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            //Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.Format.Line.Visible= Office.MsoTriState.msoCTrue;
            Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(0,0,0).ToArgb();
            Sseries.Format.Line.Weight = 0.75F;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();
            worksheet.Activate();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            RGB0 = colorDialog1.Color;

            button_ColorSelection.BackColor = RGB0;
            button_ColorSelection.ForeColor = RGB0;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
 
            Excel.Series Sseries = series.Item(1);

            Excel.Point point;
            System.Drawing.Color RGB1=System.Drawing.Color.FromArgb(255, 96, 157, 202);
            
            int[] HSV0 = new int[3];

            Graphic.RGB2HSV(RGB0, ref HSV0);

            int[] HSV = new int[3];
            HSV0.CopyTo(HSV, 0);
            double ratio;
            //double Hrange = double.Parse(textBox_Bandwidth.Text);
            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                ratio = (double.Parse(str[i, 2]) - Min_Value) / (Max_Value - Min_Value);
                HSV[2] = HSV0[2] + Convert.ToInt32(Hrange * (ratio - 0.5));
                Graphic.HSV2RGB(ref RGB1, HSV);
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb(); 
                point.Format.Fill.Transparency = 0.0F;
            }
            chart.Refresh();
            worksheet.Activate();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "") return;
            Hrange = double.Parse(textBox_Bandwidth.Text);
            if (Hrange == 0) return;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries = series.Item(1);

            Excel.Point point;
            System.Drawing.Color RGB1 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            int[] HSV0 = new int[3];

            Graphic.RGB2HSV(RGB0, ref HSV0);

            int[] HSV = new int[3];
            HSV0.CopyTo(HSV, 0);
            double ratio;
            
            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                ratio = (double.Parse(str[i, 2]) - Min_Value) / (Max_Value - Min_Value);
                HSV[2] = HSV0[2] + Convert.ToInt32(Hrange * (ratio - 0.5));
                Graphic.HSV2RGB(ref RGB1, HSV);
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB1.B, RGB1.G, RGB1.R).ToArgb();
                point.Format.Fill.Transparency = 0.0F;
            }
            chart.Refresh();
            //worksheet.Activate();
        }

    }
}
