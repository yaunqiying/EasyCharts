using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class ColumnThreshold : Form
    {
        public double ThreholdArea;
        public int flag = 0;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        EasyCharts Graphic = new EasyCharts();
        public double Max_Value;
        public double Min_Value;
        //public int Nstep=10;

        public int rows;
        public int cols;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;

        public double Axis_Scale = 1.2;

        public ColumnThreshold()
        {
            InitializeComponent();

            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            double Max_Value = Double.MinValue;
            double Min_Value = Double.MaxValue;
            for (int i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 0]) > Max_Value) Max_Value = double.Parse(str[i, 1]);
                if (double.Parse(str[i, 0]) < Min_Value) Min_Value = double.Parse(str[i, 1]);
            }
            ThreholdArea = (Max_Value + Min_Value) / 2;
            textBox_Bandwidth.Text = Convert.ToString(ThreholdArea);
            textBox_Bandwidth.Refresh();


            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "Assiatant";

            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = ">Threshold";
            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 3]).Value2 = "<=Threshold";

            double[,] data = new double[rows - 1, 3];
            for (int i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 1]) > ThreholdArea)
                {
                    data[i - 1, 2] = double.Parse(str[i, 1]) - ThreholdArea;
                    data[i - 1, 1] = 0;
                    data[i - 1, 0] = ThreholdArea - data[i - 1, 1];
                }
                else
                {
                    data[i - 1, 2] = 0;
                    data[i - 1, 1] = ThreholdArea - double.Parse(str[i, 1]);
                    data[i - 1, 0] = ThreholdArea - data[i - 1, 1];
                }
            }

            //double Max_data = Double.MinValue;
            //double Min_data = Double.MaxValue;
            //for (int i = 1; i < rows; i++)
            //{
            //    if (data[i - 1, 0] > Max_data) Max_data = data[i - 1, 0];
            //    if (data[i - 1, 1] < Min_data) Min_data = data[i - 1, 1];
            //}

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "ColumnThreshold" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(300, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (Excel.Range)worksheet.Cells[start_row, start_col + cols + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnStacked;

            //*****************************************Primary Axis********************************************
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries1 = series.Item(1);
            c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            //c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            //c2 = (Excel.Range)worksheet.Cells[start_row + rows-1, start_col + cols + 1];
            //Sseries1.Values = worksheet.get_Range(c1, c2);

            //c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col];
            //c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col];
            //Sseries2.XValues = worksheet.get_Range(c1, c2);

            //c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            //c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            //Sseries2.Values = worksheet.get_Range(c1, c2);


            ((Excel.Range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Threshold";
            ((Excel.Range)worksheet.Cells[start_row + 1, start_col + cols]).Value2 = ThreholdArea;


            //*****************************************Chart Style********************************************
            //Excel.Series Sseries1 = series.Item(1);
            Sseries1.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            Sseries1.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Excel.Series Sseries2 = series.Item(2);

            Sseries2.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 196, 191, 0).ToArgb();
            Sseries2.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb();
            Sseries2.Format.Line.Weight = 0.25F;


            Excel.Series Sseries3 = series.Item(3);
            Sseries3.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 109, 118, 248).ToArgb();
            Sseries3.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb();
            Sseries3.Format.Line.Weight = 0.25F;


            //************************************************************************************************
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            // Legend
            chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
            chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Size = 10;

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            chart.ChartArea.Height = 340.157480315;
            chart.ChartArea.Width = 380.5039370079;

            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

            //y axis
            Excel.Axis axis = (Excel.Axis)chart.Axes(
                Excel.XlAxisType.xlValue,
                Excel.XlAxisGroup.xlPrimary);

            axis.MinorUnit = axis.MajorUnit / 2;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)1.25;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            axis.MinorGridlines.Format.Line.Weight = (float)0.25;
            axis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.HasTitle = true;
            axis.AxisTitle.Text = "y axis";

            axis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axis.TickLabels.Font.Name = "Times New Roman";
            axis.TickLabels.Font.Size = 10;
            axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            axis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;


            //x axis
            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            axis.TickMarkSpacing = 3;

            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)1.25;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            axis.MinorGridlines.Format.Line.Weight = (float)0.25;
            axis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;

            axis.HasTitle = true;
            axis.AxisTitle.Text = "x axis";

            axis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axis.TickLabels.Font.Name = "Times New Roman";
            axis.TickLabels.Font.Size = 10;
            axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            axis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;
            // Change plot area ForeColor

            chart.HasLegend = false;
            chart.HasTitle = false;


            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.Overlap= 100;
            group.GapWidth = 0;

            chart.Refresh();
            //worksheet.Activate();
            flag = 1;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Bandwidth_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Bandwidth.Text == "" | flag == 0) return;
            ThreholdArea = double.Parse(textBox_Bandwidth.Text); ;
            if (ThreholdArea == 0) return;

            ((Excel.Range)worksheet.Cells[start_row + 1, start_col + cols]).Value2 = ThreholdArea;

            double[,] data = new double[rows - 1, 3];
            for (int i = 1; i < rows; i++)
            {
                if (double.Parse(str[i, 1]) > ThreholdArea)
                {
                    data[i - 1, 2] = double.Parse(str[i, 1]) - ThreholdArea;
                    data[i - 1, 1] = 0;
                    data[i - 1, 0] = ThreholdArea - data[i - 1, 1];
                }
                else
                {
                    data[i - 1, 2] = 0;
                    data[i - 1, 1] = ThreholdArea - double.Parse(str[i, 1]);
                    data[i - 1, 0] = ThreholdArea - data[i - 1, 1];
                }
            }

            //double Max_data = Double.MinValue;
            //double Min_data = Double.MaxValue;
            //for (int i = 1; i < rows; i++)
            //{
            //    if (data[i - 1, 0] > Max_data) Max_data = data[i - 1, 0];
            //    if (data[i - 1, 1] < Min_data) Min_data = data[i - 1, 1];
            //}

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            Excel.Range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            chart.Refresh();
            //worksheet.Activate();
        }
    }
}
