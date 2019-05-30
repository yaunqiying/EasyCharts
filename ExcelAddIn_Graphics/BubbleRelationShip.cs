using System;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class BubbleRelationShip : Form
    {
        public double ratio;
        public double Min_MarkerSize=10;
        public Chart chart;
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public string ChartType;
        static int Nchart = 0;
        public double Max_size;
        public double Min_size;

        public int rows = 1;
        public int cols = 1;
        public string[,] str = new string[1, 1];
        public int start_col;
        public int start_row;


        public System.Drawing.Color RGB0;

        EasyCharts Graphic = new EasyCharts();

        public BubbleRelationShip()
        {
            InitializeComponent();

            ratio = double.Parse(textBox_Bandwidth.Text); ;

            Graphic.RangeData(ref str, ref rows, ref cols);

            worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            Excel.Range activecells = Globals.ThisAddIn.Application.ActiveCell;
            start_col = activecells.Column;
            start_row = activecells.Row;

            int i, j;
            for (j = 1; j < cols; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1, start_col + j]).Value2 = str[0, j];
            }

            for (j = 1; j < rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1+j, start_col]).Value2 = str[j, 0];
            }


            for (i = 1; i < rows; i++)
            {
                for (j = 1; j < cols; j++)
                {
                    ((Excel.Range)worksheet.Cells[start_row + rows + 1 + i, start_col + j]).Value2 = i;// str[i, 0];
                }
            }

            ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col]).Value2 ="Assistant1";
            for (j = 1; j <rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows, start_col + j]).Value2 = 0;
            }

            ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows+1, start_col]).Value2 = "Assistant2";
            for (j = 1; j < rows; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col+j]).Value2 = j;
            }


            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1, start_col];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + rows - 1];

            string ChartOrder = "BubbleRelationShip" + Convert.ToString(Nchart);
            chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlRows);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            //double ratio = 21;
            //double Max_size = 0;
            Max_size = Double.MinValue;
            Min_size = Double.MaxValue;
            for (i = 1; i < rows; i++)
            {
                for (j = 1; j < cols; j++)
                {
                    if (double.Parse(str[i, j]) > Max_size) Max_size = double.Parse(str[i, j]);
                    if (double.Parse(str[i, j])< Min_size) Min_size = double.Parse(str[i, j]);
                }
            }

            Max_size = Math.Sqrt(Max_size);
            Min_size = Math.Sqrt(Min_size);

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Point point;
            Excel.Series Sseries;
            int SquareSize;
            RGB0 = System.Drawing.Color.FromArgb(255, 248, 118, 109);
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
                
                Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();

                Sseries.Format.Fill.Solid();
                Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                Sseries.Format.Fill.Transparency = 0.2F;

                //Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;

                for (j = 1; j < cols; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);
                    //SquareSize = (int)((double.Parse(str[i, j])-Min_size) / (Max_size-Min_size) * ratio) + 2;
                    SquareSize = (int)((Math.Sqrt(double.Parse(str[i, j] )  * ratio) / Max_size ) * Min_MarkerSize + 2);// - Min_size) / (Max_size - Min_size) * ratio) + 2;
                    //SquareSize = (int)((double.Parse(str[i, j]) / (Max_size * Max_size) * ratio) + 2);// - Min_size) / (Max_size - Min_size) * ratio) + 2;

                    if (SquareSize > 72)
                    { SquareSize = 72; }

                    point.MarkerSize = SquareSize;
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
            }
            SseriesX.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

            Excel.Series SseriesY = series.Item(rows+1);
            c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows , start_col + +1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows , start_col + rows - 1];
            SseriesY.XValues = worksheet.get_Range(c1, c2);

            c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + 1];
            c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + rows + 1, start_col + rows - 1];
            SseriesY.Values = worksheet.get_Range(c1, c2);
            SseriesY.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

            for (j = 1; j <rows; j++)
            {
                point = (Excel.Point)SseriesY.Points(j);
                point.HasDataLabel = true;
                point.DataLabel.Text = str[j,0];
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
            Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue,Excel.XlAxisGroup.xlPrimary);

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
            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory,Excel.XlAxisGroup.xlPrimary);

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
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255,255).ToArgb();
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
            chart.Refresh();
            //worksheet.Activate();
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

            Excel.Point point;
            Excel.Series Sseries;
            int SquareSize;
            //RGB0 = System.Drawing.Color.FromArgb(255, 109, 118, 248);
            for (int i = 1; i < rows; i++)
            {
                Sseries = series.Item(i);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;
                //Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                if (comboBox_FourierMethod.SelectedItem.Equals("Circle"))
                {
                    Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                }
                else if (comboBox_FourierMethod.SelectedItem.Equals("Square"))
                {
                    Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare;
                }


                Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb();
                Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb(); 

                Sseries.Format.Fill.Solid();
                Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb(); 
                Sseries.Format.Fill.Transparency = 0.2F;

                for (int j = 1; j < cols; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);
                    SquareSize = (int)((Math.Sqrt(double.Parse(str[i, j])  * ratio) / Max_size) * Min_MarkerSize + 2);
                    //SquareSize = (int)((double.Parse(str[i, j]) - Min_size) / (Max_size - Min_size) * ratio) + 2;
                    //SquareSize = (int)((double.Parse(str[i, j]) / (Max_size * Max_size) * ratio) + 2);// - Min_size) / (Max_size - Min_size) * ratio) + 2;

                    if (SquareSize > 72)
                    { SquareSize = 72; }

                    point.MarkerSize = SquareSize;
                }
            }
            chart.Refresh();
            //worksheet.Activate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            RGB0 = colorDialog1.Color;

            button_ColorSelection.BackColor = RGB0;
            button_ColorSelection.ForeColor = RGB0;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Point point;
            Excel.Series Sseries;
            int SquareSize;
            //RGB0 = System.Drawing.Color.FromArgb(255, 109, 118, 248);
            for (int i = 1; i < rows; i++)
            {
                Sseries = series.Item(i);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;
                Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb(); 
                Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb(); 

                Sseries.Format.Fill.Solid();
                Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB0.B, RGB0.G, RGB0.R).ToArgb(); 
                Sseries.Format.Fill.Transparency = 0.2F;

                for (int j = 1; j < cols; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);
                    //SquareSize = (int)(double.Parse(str[i, j]) / Max_size * ratio) + 2;
                    SquareSize = (int)((Math.Sqrt(double.Parse(str[i, j])  * ratio) / Max_size ) * Min_MarkerSize + 2);
                    //SquareSize = (int)((double.Parse(str[i, j]) / Max_size  * ratio) + 2);// - Min_size) / (Max_size - Min_size) * ratio) + 2;

                    if (SquareSize > 72)
                    { SquareSize = 72; }

                    point.MarkerSize = SquareSize;
                }
            }
            chart.Refresh();
            worksheet.Activate();
        }

        private void comboBox_FourierMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Bandwidth_TextChanged(sender, e);
        }

        private void BubbleRelationShip_Load(object sender, EventArgs e)
        {
            this.comboBox_FourierMethod.Text = "Circle";
        }
    }
}
