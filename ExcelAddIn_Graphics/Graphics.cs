using System;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

using range = Microsoft.Office.Interop.Excel.Range;
using worksheet = Microsoft.Office.Tools.Excel.Worksheet;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using CSharpWin_JD.CaptureImage;//CSharpWin_JD.CaptureImage
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//using PdfToImage;
//using PdfPageTools;
using ColorManagment;

using System.IO;
//using SpotLight2;

namespace ExcelAddIn_Graphics
{

    public partial class EasyCharts
    {
        public Point formLoad, formLeft, formRight;
        //Form_ColorPixel Form_Assiatant = new Form_ColorPixel();
        public int[] SelectRGB;
        public Int32 elementID, arg1, arg2;

        //***********************************HighLight*******************
        public int Nchart = 0;
        //***********************************HighLight*******************
        private XlWorkbookHelper exWin;
        private Excel.Application app;
        private HookScroll hs;

        //public string[,] str = new string[1, 1];
        //public int rows = 1;
        //public int cols = 1;
        //public int start_col =0;
        //public int start_row = 0; 
        //public string ChartType;

        private void Graphics_Load(object sender, RibbonUIEventArgs e)
        {

        }

        //private void button_gglot2_Click(object sender, RibbonControlEventArgs e)
        private void Rggplot2(Excel.Chart chart)
        {
            //worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            //range cells = (range)Globals.ThisAddIn.Application.Selection;
            //Microsoft.Office.Tools.Excel.Chart chart = worksheet.Controls.AddChart(0, 0, 450, 400, "chart");
            //chart.SetSourceData(cells,Type.Missing);
            //chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlXYScatter;

            //Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

            // Change plot area ForeColor
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


            // Chart Type: XYScatter
            if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlBubble)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLine)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlArea)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
            {

                // Add GridLinesMinorMajor
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
                axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);

                if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                | chart.ChartType.Equals(Excel.XlChartType.xlBubble))
                {
                    axis.MinorUnit = axis.MajorUnit / 2;
                }
                else if (chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
               | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
               | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
               | chart.ChartType.Equals(Excel.XlChartType.xlLine)
               | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
               | chart.ChartType.Equals(Excel.XlChartType.xlArea)
               | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
               | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
                {
                    axis.TickMarkSpacing = 3;
                }

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

            }


            else if (chart.ChartType.Equals(Excel.XlChartType.xlRadar)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarFilled)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarMarkers))
            {
                //chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlRadarFilled;

                chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
                chart.ChartArea.Format.Fill.Transparency = 0;


                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMajor);
                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.HasMajorGridlines = true;
                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)1.25;
                axis.MajorGridlines.Format.Line.Transparency = 0;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.Format.Line.Weight = (float)0.75;
                axis.Format.Line.Transparency = 0;
                //axis.Format.Line.ForeColor.TintAndShade = 0;
                //axis.Format.Line.ForeColor.Brightness = 0;
                axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

                //axis.HasDisplayUnitLabel = true;
                //axis.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //axis.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //axis.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //axis.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //axis.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
                //Excel.ChartGroup group =(Excel.ChartGroup)chart.ChartGroups(1);

                //group.HasRadarAxisLabels = true;
                ////group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            }
            // Chart title delete

            //chart.HasTitle = false;
            //chart.ChartTitle.Delete();
            chart.Refresh();
        }


        private void PythonSeaborn()
        {
            Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 242, 234, 234).ToArgb();
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


            // Chart Type: XYScatter
            if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlBubble)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLine)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlArea)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
            {

                // Add GridLinesMinorMajor
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = 1;


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
                axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = 1;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

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

            }


            else if (chart.ChartType.Equals(Excel.XlChartType.xlRadar)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarFilled)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarMarkers))
            {

                chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 242, 234, 234).ToArgb();
                chart.ChartArea.Format.Fill.Transparency = 0;

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.HasMajorGridlines = true;
                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)1.25;
                axis.MajorGridlines.Format.Line.Transparency = 0;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                axis.Format.Line.Weight = (float)0.75;
                axis.Format.Line.Transparency = 0;
                //axis.Format.Line.ForeColor.TintAndShade = 0;
                //axis.Format.Line.ForeColor.Brightness = 0;
                axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

                Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);

                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            }
            // Chart title delete


            //chart.HasTitle = false;
            chart.ChartTitle.Delete();
            chart.Refresh();
        }


        private void Matlab2013()
        {
            Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
            chart.PlotArea.Format.Line.Weight = 0.25F;

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


            // Chart Type: XYScatter
            if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlBubble)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLine)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlArea)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
            {

                // Add GridLinesMinorMajor
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)0.75;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineDash;

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

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
                axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)0.75;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineDash;

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

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

            }


            else if (chart.ChartType.Equals(Excel.XlChartType.xlRadar)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarFilled)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarMarkers))
            {
                chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

                chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                chart.ChartArea.Format.Fill.Transparency = 0;

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.HasMajorGridlines = true;
                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)1.25;
                axis.MajorGridlines.Format.Line.Transparency = 0;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.Format.Line.Weight = (float)0.75;
                axis.Format.Line.Transparency = 0;
                //axis.Format.Line.ForeColor.TintAndShade = 0;
                //axis.Format.Line.ForeColor.Brightness = 0;
                axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

                Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);

                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            }
            // Chart title delete


            //chart.HasTitle = false;
            chart.ChartTitle.Delete();
            chart.Refresh();
        }


        private void Matlab2014()
        {
            Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
            chart.PlotArea.Format.Line.Weight = 0.25F;

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


            // Chart Type: XYScatter
            if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlBubble)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLine)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlArea)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
            {

                // Add GridLinesMinorMajor
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMajor);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)0.75;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

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
                axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)0.75;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

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

            }


            else if (chart.ChartType.Equals(Excel.XlChartType.xlRadar)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarFilled)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarMarkers))
            {
                chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

                chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                chart.ChartArea.Format.Fill.Transparency = 0;

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.HasMajorGridlines = true;
                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)1.25;
                axis.MajorGridlines.Format.Line.Transparency = 0;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.Format.Line.Weight = (float)0.75;
                axis.Format.Line.Transparency = 0;
                //axis.Format.Line.ForeColor.TintAndShade = 0;
                //axis.Format.Line.ForeColor.Brightness = 0;
                axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

                Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);

                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            }
            // Chart title delete


            //chart.HasTitle = false;
            chart.ChartTitle.Delete();
            chart.Refresh();
        }

        private void ExcelSimaple()
        {
            Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
            chart.PlotArea.Format.Line.Weight = 0.25F;
            //chart.PlotArea.Width=180;
            //chart.PlotArea.Height = 180;

            // Legend
            chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
            chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Size = 9;

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            chart.ChartArea.Height = 230;
            chart.ChartArea.Width = 300;


            // Chart Type: XYScatter
            if (chart.ChartType.Equals(Excel.XlChartType.xlXYScatter)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLines)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterLinesNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmooth)
                 | chart.ChartType.Equals(Excel.XlChartType.xlXYScatterSmoothNoMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlBubble)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnClustered)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlColumnStacked100)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLine)
                 | chart.ChartType.Equals(Excel.XlChartType.xlLineMarkers)
                 | chart.ChartType.Equals(Excel.XlChartType.xlArea)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked)
                 | chart.ChartType.Equals(Excel.XlChartType.xlAreaStacked100))
            {

                // Add GridLinesMinorMajor
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesNone);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesNone);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
                chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
                axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;

                axis.HasTitle = true;
                axis.AxisTitle.Text = "y axis";

                axis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                axis.TickLabels.Font.Name = "Times New Roman";
                axis.TickLabels.Font.Size = 9;
                axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

                axis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;


                //x axis
                axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);

                axis.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 127, 127, 127).ToArgb();
                axis.Format.Line.Weight = 0.25F;
                axis.MajorTickMark = Excel.XlTickMark.xlTickMarkInside;

                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
                axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;

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
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 9;
                axis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            }


            else if (chart.ChartType.Equals(Excel.XlChartType.xlRadar)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarFilled)
                | chart.ChartType.Equals(Excel.XlChartType.xlRadarMarkers))
            {
                chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

                chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                chart.ChartArea.Format.Fill.Transparency = 0;

                //y axis
                Excel.Axis axis = (Excel.Axis)chart.Axes(
                    Excel.XlAxisType.xlValue,
                    Excel.XlAxisGroup.xlPrimary);

                axis.HasMajorGridlines = true;
                axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.MajorGridlines.Format.Line.Weight = (float)1.25;
                axis.MajorGridlines.Format.Line.Transparency = 0;
                axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

                axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
                axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 191, 191, 191).ToArgb();
                axis.Format.Line.Weight = (float)0.75;
                axis.Format.Line.Transparency = 0;
                //axis.Format.Line.ForeColor.TintAndShade = 0;
                //axis.Format.Line.ForeColor.Brightness = 0;
                axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

                Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);

                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
                //group.RadarAxisLabels.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            }
            // Chart title delete


            //chart.HasTitle = false;
            chart.ChartTitle.Delete();
            chart.Refresh();
        }


        private void comboBox_ColorTheme_TextChanged(object sender, RibbonControlEventArgs e)
        {

            string item = comboBox_ColorTheme.Text.ToString();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "/" + item + ".xml";
            Globals.ThisAddIn.Application.ActiveWorkbook.Theme.ThemeColorScheme.Load(path);
            //    string path = System.AppDomain.CurrentDomain.BaseDirectory+ "Theme Color/R ggplot2 Set1.xml";  
            //    //string path ="..\\Theme Color\\R ggplot2 Set1.xml";  
            //    Globals.ThisAddIn.Application.ActiveWorkbook.Theme.ThemeColorScheme.Load(path);
        }

        private void comboBox_GraphicStyle_TextChanged(object sender, RibbonControlEventArgs e)
        {
            //string item = comboBox_GraphicStyle.Text.ToString();
            if (comboBox_GraphicStyle.Text.Equals("R ggplot2"))
            {
                Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;
                Rggplot2(chart);
            }
            else if (comboBox_GraphicStyle.Text.Equals("Python Seaborn"))
            {
                PythonSeaborn();
            }
            else if (comboBox_GraphicStyle.Text.Equals("Matlab 2013"))
            {
                Matlab2013();
            }
            else if (comboBox_GraphicStyle.Text.Equals("Matlab 2014"))
            {
                Matlab2014();
            }
            else if (comboBox_GraphicStyle.Text.Equals("Excel Simaple"))
            {
                ExcelSimaple();
            }
        }

        public void RangeData(ref string[,] str, ref int rows, ref int cols)
        {
            range cells = (range)Globals.ThisAddIn.Application.Selection;
            object[,] iarr = cells.Value2;

            rows = iarr.GetLength(0);
            cols = iarr.GetLength(1);
            str = new string[rows, cols];
            //double[,] str = new double[rows, cols];

            int i = 0;
            int j = 0;
            double num = 0;
            foreach (object s in iarr)
            {
                i = (int)Math.Floor(num / cols);
                j = (int)num - cols * i;
                //str[i, j] = double.Parse(s.ToString());
                try
                {
                    if (s != null)
                    {
                        str[i, j] = s.ToString();
                    }
                    num = num + 1;
                }
                catch (Exception ee)
                {
                    throw new Exception(ee.ToString());
                }

            }
        }


        //private void PolarLine()
        //{
        //    int rows = 1;
        //    int cols = 1;
        //    string[,] str = new string[1, 1];
        //    RangeData(ref str, ref rows, ref cols);

        //    worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

        //    range activecells = Globals.ThisAddIn.Application.ActiveCell;
        //    int start_col = activecells.Column;
        //    int start_row = activecells.Row;

        //    ((range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "x axis";
        //    ((range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "y axis";
        //    double[,] data = new double[rows-1, 2];
        //    double Angle;
        //    for (int i = 1; i < rows; i++)
        //    {
        //        Angle = double.Parse(str[i, 0]);
        //        data[i - 1, 0] = double.Parse(str[i, 1]) * Math.Cos(Angle / 180 *3.14159);
        //        data[i - 1, 1] = double.Parse(str[i, 1]) * Math.Sin(Angle / 180 * 3.14159);
        //    }
        //    range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
        //    range c2 = (range)worksheet.Cells[start_row + rows-1, start_col + cols + 2];
        //    range range = worksheet.get_Range(c1, c2);
        //    range.Value = data;


        //    //***********************************************xlSecondary*************************************


        //    string ChartOrder = "chart" + Convert.ToString(Nchart);
        //    Microsoft.Office.Tools.Excel.Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
        //    Nchart = Nchart + 1;
        //    c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
        //    chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
        //    chart.ChartType = Excel.XlChartType.xlXYScatter;

        //    Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

        //    //***********************************************xlPrimary*************************************
        //    Excel.Series Sseries = series.Item(1);
        //    Sseries.ChartType = Excel.XlChartType.xlXYScatterSmoothNoMarkers;
        //    //Sseries.AxisGroup = Microsoft.Office.Interop.Excel.XlAxisGroup.xlSecondary;

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 1];
        //    Sseries.XValues = worksheet.get_Range(c1, c2);

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 2];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
        //    Sseries.Values = worksheet.get_Range(c1, c2);

        //    Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();

        //    Excel.Axis axisValue = (Excel.Axis)chart.Axes(
        //          Excel.XlAxisType.xlValue,
        //          Excel.XlAxisGroup.xlPrimary);
        //    axisValue.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    //axisValue.HasDisplayUnitLabel = false;
        //    axisValue.HasMajorGridlines = false;
        //    axisValue.Delete();

        //    Excel.Axis axisCategory = (Excel.Axis)chart.Axes(
        //          Excel.XlAxisType.xlCategory,
        //          Excel.XlAxisGroup.xlPrimary);
        //    axisCategory.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    //axisCategory.HasDisplayUnitLabel = false;
        //    axisCategory.HasMajorGridlines = false;
        //    axisCategory.Delete();

        //    double Max_value = Double.MinValue;
        //    for (int i = 1; i < rows; i++)
        //    {
        //        if (Math.Abs(data[i - 1, 0]) > Max_value) Max_value = Math.Abs(data[i - 1, 0]);
        //        if (Math.Abs(data[i - 1, 1]) > Max_value) Max_value = Math.Abs(data[i - 1, 1]);
        //    }

        //    //double MaximumScale1 = Math.Max(axisValue.MaximumScale, axisCategory.MaximumScale);
        //    //double MaximumScale2 = Math.Min(axisValue.MinimumScale, axisCategory.MinimumScale);
        //    //double MaximumScale = Math.Max(MaximumScale1, Math.Abs(MaximumScale2));
        //    double MaximumScale = Math.Ceiling(Max_value * 1.2 / axisValue.MinorUnit) * axisValue.MinorUnit;
        //    axisValue.MaximumScale = MaximumScale;
        //    axisCategory.MaximumScale = MaximumScale;

        //    axisValue.MinimumScale = -MaximumScale;
        //    axisCategory.MinimumScale = -MaximumScale;

        //    //***********************************************xlSecondary*************************************
        //    ((range)worksheet.Cells[start_row, start_col + cols + 4]).Value2 = "Label";
        //    int Step = 5;  // 整数，且被360整除 
        //    int Nrows = 360 / Step;
        //    double[,] PLRPLT = new double[Nrows, 1];
        //   // double MaximumScale = 100;
        //    int n = 0;
        //    for (int i = Nrows; i > 0; i--)
        //    {
        //        Angle = i * Step;
        //        if (i == Nrows) Angle = 0;

        //        if (Angle % 30 == 0)
        //        {
        //            ((range)worksheet.Cells[start_row + 1 + Nrows - i, start_col + cols + 4]).Value2 = Angle;
        //            PLRPLT[Nrows - i, 0] = MaximumScale;
        //            //PLRPLT[Nrows - i, 1] = MaximumScale * Math.Cos(Angle / 180 * 3.14159);
        //            //PLRPLT[Nrows - i, 2] = MaximumScale * Math.Sin(Angle / 180 * 3.14159);
        //        }
        //        n = n + 1;
        //    }

        //    //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 6]).Value2 = "Cos Value";
        //    //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 7]).Value2 = "Sin Value";

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
        //    range = worksheet.get_Range(c1, c2);
        //    range.Value = PLRPLT;

        //    Excel.Series Sseries2 = series.Item(2);
        //    //Sseries2.AxisGroup = Microsoft.Office.Interop.Excel.XlAxisGroup.xlSecondary;
        //    Sseries2.ChartType = Excel.XlChartType.xlRadar;

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 4];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 4];
        //    Sseries2.XValues = worksheet.get_Range(c1, c2);

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
        //    Sseries2.Values = worksheet.get_Range(c1, c2);

        //    Sseries2.Format.Line.Visible = Office.MsoTriState.msoTrue;
        //    Sseries2.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
        //    Sseries2.Format.Line.Weight = (float)0.75;
        //    Sseries2.Format.Line.Transparency = 0;
        //    Sseries2.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDashDot;

        //    Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
        //    axis.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    axis.HasDisplayUnitLabel = true;
        //    axis.MaximumScale = MaximumScale;

        //    axis.HasMajorGridlines = true;
        //    axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
        //    axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
        //    axis.MajorGridlines.Format.Line.Weight = (float)1.25;
        //    axis.MajorGridlines.Format.Line.Transparency = 0;
        //    axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

        //    //****************************************Style******************************************************
        //    // Change plot area ForeColor
        //    chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
        //    chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
        //    chart.PlotArea.Format.Fill.Transparency = 0;

        //    chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

        //    // Legend
        //    chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
        //    chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Size = 10;

        //    chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
        //    chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
        //    chart.ChartArea.Format.Fill.Transparency = 0;

        //    // ChartArea Line
        //    chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    chart.HasLegend = false;
        // }
        //private void LabelAdd()
        //{
        //    Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;

        //    range Rangelabel = Globals.ThisAddIn.Application.InputBox("Range for data labels?",Type: 8);
        //    //if (Rangelabel == null) return;
        //    //System.Windows.Forms.MessageBox.Show(ConvertE(Rangelabel.Text));
        //    Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
        //    Excel.Series Sseries = series.Item(1);

        //    Sseries.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowValue, true, false);

        //    Excel.Points points = Sseries.Points();
        //    int Ndata = points.Count;
        //    Excel.Point point;
        //    for (int j = 1; j < Ndata; j++)
        //    {
        //        point = (Excel.Point)Sseries.Points(j);
        //        point.HasDataLabel = true;
        //        //point.DataLabel.Text = Rangelabel[j].Address;
        //        //string s= "=" + "'" & Rangelabel.Parent.Name & "!" & Rangelabel[j].Address(Excel.XlReferenceStyle.xlR1C1);
        //        point.DataLabel.Text = Convert.ToString(Rangelabel[j].Value2);
        //    }
        //}

        private void CurveConfidence()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            double[,] data = new double[rows - 1, 1];

            ((range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Range";

            for (int i = 1; i < rows; i++)
            {
                //double v1 = double.Parse(str[i, start_col + cols - 3]);
                //double v2 = double.Parse(str[i, start_col + cols - 2]);
                data[i - 1, 0] = Math.Abs(double.Parse(str[i, start_col + cols - 2]) - double.Parse(str[i, start_col + cols - 3]));
            }

            range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];
            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;
            c1 = (range)worksheet.Cells[start_row, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlLine;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries0 = series.Item(1);
            //Sseries0.Format.Line.ForeColor.ObjectThemeColor = Microsoft.Office.Core.MsoThemeColorIndex.msoThemeColorAccent2;
            Sseries0.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 38, 87, 37).ToArgb();
            Sseries0.Format.Line.Weight = 0.25F;


            //**********************************************New Series*************************************
            Excel.Series Sseries1 = series.NewSeries();
            c1 = (range)worksheet.Cells[start_row + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + 2];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 2];
            Sseries1.Values = worksheet.get_Range(c1, c2);
            Sseries1.ChartType = Excel.XlChartType.xlAreaStacked;
            Sseries1.Format.Fill.Visible = Office.MsoTriState.msoFalse;


            Excel.Series Sseries2 = series.NewSeries();
            c1 = (range)worksheet.Cells[start_row + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries2.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];
            Sseries2.Values = worksheet.get_Range(c1, c2);

            Sseries2.ChartType = Excel.XlChartType.xlAreaStacked;
            Sseries2.Format.Fill.Solid();
            Sseries2.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            Sseries2.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            Sseries2.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 74, 175, 77).ToArgb();
            //Sseries2.Format.Fill.ForeColor.ObjectThemeColor = Microsoft.Office.Core.MsoThemeColorIndex.msoThemeColorAccent2;
            Sseries2.Format.Fill.Transparency = 0.3F;


            //**********************************************Style*************************************

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            // Legend
            chart.HasLegend = false;

            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
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
            axis = (Excel.Axis)chart.Axes(
                  Excel.XlAxisType.xlCategory,
                  Excel.XlAxisGroup.xlPrimary);

            int Unit = Convert.ToInt32(axis.MajorUnit / 2);
            if (Unit == 0) Unit = 1;
            axis.MajorUnit = Unit * 2;
            axis.MinorUnit = Unit;

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
        }


        private void CurveStep()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "x error";
            ((range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "y error";
            double[,] data = new double[rows - 1, 2];

            for (int i = 1; i < rows; i++)
            {
                if (i == 1)
                {
                    data[i - 1, 0] = double.Parse(str[i, 0]);
                }
                else
                {
                    data[i - 1, 0] = double.Parse(str[i, 0]) - double.Parse(str[i - 1, 0]);
                }

                if (i < rows - 1)
                    data[i - 1, 1] = double.Parse(str[i, 1]) - double.Parse(str[i + 1, 1]);
            }
            range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;
            c1 = (range)worksheet.Cells[start_row, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols - 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Delete();
            //***********************************************xlPrimary*************************************
            Excel.Series Sseries = series.Item(1);

            c1 = (range)worksheet.Cells[start_row + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);


            // Error bar
            Sseries.HasErrorBars = true;

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 1];
            chart.SetElement(Office.MsoChartElementType.msoElementErrorBarNone);
            Excel.ErrorBars barx = Sseries.ErrorBar(Excel.XlErrorBarDirection.xlX, Excel.XlErrorBarInclude.xlErrorBarIncludeMinusValues,
                Excel.XlErrorBarType.xlErrorBarTypeCustom,
                Amount: worksheet.get_Range(c1, c2), MinusValues: worksheet.get_Range(c1, c2));

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            Excel.ErrorBars bary = Sseries.ErrorBar(Excel.XlErrorBarDirection.xlY, Excel.XlErrorBarInclude.xlErrorBarIncludeMinusValues,
                Excel.XlErrorBarType.xlErrorBarTypeCustom,
                Amount: worksheet.get_Range(c1, c2), MinusValues: worksheet.get_Range(c1, c2));

            Sseries.ErrorBars.EndStyle = Excel.XlEndStyleCap.xlNoCap;
            //bary.EndStyle = Microsoft.Office.Interop.Excel.XlEndStyleCap.xlNoCap;

            Sseries.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;
            Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

        }


        //private void PolarScatter()
        //{
        //    int rows = 1;
        //    int cols = 1;
        //    string[,] str = new string[1, 1];
        //    RangeData(ref str, ref rows, ref cols);

        //    worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

        //    range activecells = Globals.ThisAddIn.Application.ActiveCell;
        //    int start_col = activecells.Column;
        //    int start_row = activecells.Row;

        //    ((range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "x axis";
        //    ((range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "y axis";
        //    double[,] data = new double[rows - 1, 2];
        //    double Angle;
        //    for (int i = 1; i < rows; i++)
        //    {
        //        Angle = double.Parse(str[i, 0]);
        //        data[i - 1, 0] = double.Parse(str[i, 1]) * Math.Cos(Angle / 180 * 3.14159);
        //        data[i - 1, 1] = double.Parse(str[i, 1]) * Math.Sin(Angle / 180 * 3.14159);
        //    }
        //    range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
        //    range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
        //    range range = worksheet.get_Range(c1, c2);
        //    range.Value = data;


        //    //***********************************************xlSecondary*************************************
        //    string ChartOrder = "chart" + Convert.ToString(Nchart);
        //    Microsoft.Office.Tools.Excel.Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
        //    Nchart = Nchart + 1;
        //    c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
        //    chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
        //    chart.ChartType = Excel.XlChartType.xlXYScatter;

        //    Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

        //    //***********************************************xlPrimary*************************************
        //    Excel.Series Sseries = series.Item(1);
        //    Sseries.ChartType = Excel.XlChartType.xlXYScatterSmooth;
        //    //Sseries.AxisGroup = Microsoft.Office.Interop.Excel.XlAxisGroup.xlSecondary;

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 1];
        //    Sseries.XValues = worksheet.get_Range(c1, c2);

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 2];
        //    c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
        //    Sseries.Values = worksheet.get_Range(c1, c2);

        //    Sseries.MarkerSize = 8;
        //    Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
        //    Sseries.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 145,108 , 255).ToArgb();
        //    Sseries.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 145, 108, 255).ToArgb();

        //    Sseries.Format.Fill.Solid();
        //    Sseries.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
        //    Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
        //    Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 145, 108, 255).ToArgb();
        //    //Sseries.Format.Fill.Transparency = 0.3F;
        //    Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 145, 108, 255).ToArgb();
        //    Sseries.Format.Line.Weight = 0.25F;

        //    Excel.Axis axisValue = (Excel.Axis)chart.Axes(
        //          Excel.XlAxisType.xlValue,
        //          Excel.XlAxisGroup.xlPrimary);
        //    axisValue.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    //axisValue.HasDisplayUnitLabel = false;
        //    axisValue.HasMajorGridlines = false;
        //    axisValue.Delete();

        //    Excel.Axis axisCategory = (Excel.Axis)chart.Axes(
        //          Excel.XlAxisType.xlCategory,
        //          Excel.XlAxisGroup.xlPrimary);
        //    axisCategory.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    //axisCategory.HasDisplayUnitLabel = false;
        //    axisCategory.HasMajorGridlines = false;
        //    axisCategory.Delete();

        //    double Max_value = Double.MinValue;
        //    for (int i = 1; i < rows; i++)
        //    {
        //        if (Math.Abs(data[i - 1, 0]) > Max_value) Max_value = Math.Abs(data[i - 1, 0]);
        //        if (Math.Abs(data[i - 1, 1]) > Max_value) Max_value = Math.Abs(data[i - 1, 1]);
        //    }

        //    //double MaximumScale1 = Math.Max(axisValue.MaximumScale, axisCategory.MaximumScale);
        //    //double MaximumScale2 = Math.Min(axisValue.MinimumScale, axisCategory.MinimumScale);
        //    //double MaximumScale = Math.Max(MaximumScale1, Math.Abs(MaximumScale2));
        //    double MaximumScale = Math.Ceiling(Max_value * 1.2 / axisValue.MinorUnit) * axisValue.MinorUnit;
        //    axisValue.MaximumScale = MaximumScale;
        //    axisCategory.MaximumScale = MaximumScale;

        //    axisValue.MinimumScale = -MaximumScale;
        //    axisCategory.MinimumScale = -MaximumScale;

        //    //***********************************************xlSecondary*************************************
        //    ((range)worksheet.Cells[start_row, start_col + cols + 4]).Value2 = "Label";
        //    int Step = 5;  // 整数，且被360整除 
        //    int Nrows = 360 / Step;
        //    double[,] PLRPLT = new double[Nrows, 1];
        //    // double MaximumScale = 100;
        //    int n = 0;
        //    for (int i = Nrows; i > 0; i--)
        //    {
        //        Angle = i * Step;
        //        if (i == Nrows) Angle = 0;

        //        if (Angle % 30 == 0)
        //        {
        //            ((range)worksheet.Cells[start_row + 1 + Nrows - i, start_col + cols + 4]).Value2 = Angle;
        //            PLRPLT[Nrows - i, 0] = MaximumScale ;
        //            //PLRPLT[Nrows - i, 1] = MaximumScale * Math.Cos(Angle / 180 * 3.14159);
        //            //PLRPLT[Nrows - i, 2] = MaximumScale * Math.Sin(Angle / 180 * 3.14159);
        //        }
        //        n = n + 1;
        //    }

        //    //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 6]).Value2 = "Cos Value";
        //    //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 7]).Value2 = "Sin Value";

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
        //    range = worksheet.get_Range(c1, c2);
        //    range.Value = PLRPLT;

        //    Excel.Series Sseries2 = series.Item(2);
        //    //Sseries2.AxisGroup = Microsoft.Office.Interop.Excel.XlAxisGroup.xlSecondary;
        //    Sseries2.ChartType = Excel.XlChartType.xlRadar;

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 4];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 4];
        //    Sseries2.XValues = worksheet.get_Range(c1, c2);

        //    c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
        //    c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
        //    Sseries2.Values = worksheet.get_Range(c1, c2);

        //    Sseries2.Format.Line.Visible = Office.MsoTriState.msoTrue;
        //    Sseries2.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
        //    Sseries2.Format.Line.Weight = (float)0.75;
        //    Sseries2.Format.Line.Transparency = 0;
        //    Sseries2.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDashDot;

        //    Excel.Axis axis = (Excel.Axis)chart.Axes(
        //         Excel.XlAxisType.xlValue,
        //         Excel.XlAxisGroup.xlSecondary);
        //    axis.MaximumScale = MaximumScale;

        //    axis.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    axis.HasDisplayUnitLabel = true;

        //    axis.HasMajorGridlines = true;
        //    axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
        //    axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
        //    axis.MajorGridlines.Format.Line.Weight = (float)1.25;
        //    axis.MajorGridlines.Format.Line.Transparency = 0;
        //    axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

        //    //****************************************Style******************************************************
        //    // Change plot area ForeColor
        //    chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
        //    chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
        //    chart.PlotArea.Format.Fill.Transparency = 0;

        //    chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

        //    // Legend
        //    chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
        //    chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
        //    chart.Legend.Format.TextFrame2.TextRange.Font.Size = 10;

        //    chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
        //    chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
        //    chart.ChartArea.Format.Fill.Transparency = 0;

        //    // ChartArea Line
        //    chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
        //    chart.HasLegend = false;
        //}

        private void PolarArea()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            double[] x = new double[rows - 1];
            double[] y = new double[rows - 1];
            for (int i = 1; i < rows; i++)
            {
                x[i - 1] = double.Parse(str[i, 0]);
                y[i - 1] = double.Parse(str[i, 1]);
            }

            int Nrows = 360;
            double[] t0 = new double[Nrows];
            double[] z0 = new double[Nrows];
            if (rows != 361)
            {
                for (int i = 0; i < Nrows; i++)
                {
                    t0[i] = i;
                }
                Spline(x, y, 0.0, 0.0, t0, ref z0);
            }
            else
            {
                x.CopyTo(t0, 0);
                y.CopyTo(z0, 0);
            }

            double[,] t = new double[Nrows, 1];
            double[,] z = new double[Nrows, 1];
            for (int i = 0; i < Nrows; i++)
            {
                t[i, 0] = t0[i];
                z[i, 0] = z0[i];

                if (i % 30 == 0)
                    ((range)worksheet.Cells[start_row + 1 + i, start_col + cols + 2]).Value2 = i;
            }

           ((range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "x axis";
            ((range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "x label";
            ((range)worksheet.Cells[start_row, start_col + cols + 3]).Value2 = "y label";

            range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = t;

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 3];
            range = worksheet.get_Range(c1, c2);
            range.Value = z;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (range)worksheet.Cells[start_row, start_col + cols + 2];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 3];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlRadarFilled;

            //************************************xlPrimary axis***********************************************
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries = series.Item(1);
            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 2];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 3];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 3];
            Sseries.Values = worksheet.get_Range(c1, c2);

            //Sseries.Format.Fill.Solid();
            Sseries.Format.Fill.Visible = Office.MsoTriState.msoTrue;
            Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.Format.Fill.Transparency = 0.45F;

            Sseries.Format.Line.Visible = Office.MsoTriState.msoTrue;
            Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 77, 175, 74).ToArgb();
            Sseries.Format.Line.Weight = 1.5F;

            Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;

            axis.HasMajorGridlines = true;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)1.25;
            axis.MajorGridlines.Format.Line.Transparency = 0;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            //************************************* xlSecondary axis***************************************************
            ((range)worksheet.Cells[start_row, start_col + cols + 5]).Value2 = "Backgroud_x";
            ((range)worksheet.Cells[start_row, start_col + cols + 6]).Value2 = "Backgroud_y";
            int Nrows2 = 12;
            for (int i = 0; i < Nrows2; i++)
            {
                ((range)worksheet.Cells[start_row + 1 + i, start_col + cols + 5]).Value2 = i * 30;
                ((range)worksheet.Cells[start_row + 1 + i, start_col + cols + 6]).Value2 = axis.MaximumScale;
            }

            Excel.Series Sseries2 = series.Item(2);
            //Sseries2.Delete();
            Sseries2.AxisGroup = Excel.XlAxisGroup.xlSecondary;

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
            c2 = (range)worksheet.Cells[start_row + Nrows2, start_col + cols + 5];
            Sseries2.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 6];
            c2 = (range)worksheet.Cells[start_row + Nrows2, start_col + cols + 6];
            Sseries2.Values = worksheet.get_Range(c1, c2);

            Sseries2.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            //Sseries2.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            //Sseries2.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();
            //Sseries2.Format.Fill.Transparency = 1.0F;

            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
            axis.HasDisplayUnitLabel = false;
            //axis.HasTitle = false;
            //axis.AxisTitle.Delete();

            axis.HasMajorGridlines = false;

            axis.Format.Line.Visible = Office.MsoTriState.msoTrue;
            axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axis.Format.Line.Weight = (float)0.75;
            //axis.Format.Line.Transparency = 0;
            //axis.Format.Line.ForeColor.TintAndShade = 0;
            //axis.Format.Line.ForeColor.Brightness = 0;
            axis.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDash;

            //Style
            // Change plot area ForeColor
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

            chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.ChartArea.Format.Fill.Transparency = 0;

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
            chart.HasLegend = false;
        }


        private void DashBoard()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            double Pointer_Start = double.Parse(str[2, 0]);
            double Pointer_End = double.Parse(str[3, 0]);

            ((range)worksheet.Cells[start_row, start_col + 1]).Value2 = "pointer";
            double pointer = (double.Parse(str[1, 0]) - Pointer_Start) / (Pointer_End - Pointer_Start) * 270;
            ((range)worksheet.Cells[start_row + 1, start_col + 1]).Value2 = pointer;
            ((range)worksheet.Cells[start_row + 2, start_col + 1]).Value2 = 0;
            ((range)worksheet.Cells[start_row + 3, start_col + 1]).Value2 = 360 - pointer;

            ((range)worksheet.Cells[start_row + rows + 1, start_col]).Value2 = "label";
            ((range)worksheet.Cells[start_row + rows + 1, start_col + 1]).Value2 = "Dashboard";


            int Nrows = 23;
            double Pointer_Step = (Pointer_End - Pointer_Start) / 10;
            double[,] data = new double[Nrows, 1];
            for (int i = 0; i < Nrows; i++)
            {
                if (i % 2 == 0)
                {
                    data[i, 0] = 0;
                    ((range)worksheet.Cells[start_row + rows + 2 + i, start_col]).Value2 = (i / 2) * Pointer_Step + Pointer_Start;
                }
                else
                {
                    data[i, 0] = 27;
                }
            }


            range c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col + 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            ((range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col]).Value2 = str[1, 0] + "%";
            ((range)worksheet.Cells[start_row + rows + 2 + Nrows - 2, start_col + 1]).Value2 = 0;
            ((range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col + 1]).Value2 = 90;

            c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + Nrows + 1, start_col + 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 220, 200, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlDoughnut;

            chart.ClearToMatchStyle();
            chart.ChartStyle = 12;

            chart.HasLegend = false;
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            //*******************************************************series*************************************
            Excel.Series Sseries1 = series.Item(1);
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + Nrows + 1, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + +rows + Nrows + 1, start_col + 1];
            Sseries1.Values = worksheet.get_Range(c1, c2);

            Sseries1.ChartType = Excel.XlChartType.xlDoughnut;
            Sseries1.Format.Fill.Visible = Office.MsoTriState.msoTrue;
            Sseries1.Format.Line.Visible = Office.MsoTriState.msoFalse;
            //Sseries1.Format.Shadow.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
            Sseries1.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            Sseries1.Format.Shadow.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();

            Sseries1.HasDataLabels = true;
            Excel.DataLabels label = (Excel.DataLabels)Sseries1.DataLabels();

            label.ShowValue = false;
            label.ShowCategoryName = true;
            label.ShowPercentage = false;
            //label.Position = Microsoft.Office.Interop.Excel.XlDataLabelPosition.xlLabelPositionOutsideEnd;
            //label.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.TintAndShade = 0;
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.Brightness = 0;
            //label.Format.TextFrame2.TextRange.Font.Fill.Transparency = 0;
            label.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Size = 10;

            //*****************************************************************************
            Excel.Series Sseries3 = series.NewSeries();
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + Nrows + 1, start_col];
            Sseries3.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + +rows + Nrows + 1, start_col + 1];
            Sseries3.Values = worksheet.get_Range(c1, c2);

            Sseries3.ChartType = Excel.XlChartType.xlDoughnut;
            Sseries3.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            Excel.Point point;
            point = Sseries3.Points(Nrows);
            point.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            point.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Excel.Series Sseries4 = series.NewSeries();
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + Nrows + 1, start_col];
            Sseries4.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + +rows + Nrows + 1, start_col + 1];
            Sseries4.Values = worksheet.get_Range(c1, c2);

            Sseries4.ChartType = Excel.XlChartType.xlDoughnut;


            Sseries4.Format.Fill.Visible = Office.MsoTriState.msoTrue;
            Sseries4.Format.Line.Visible = Office.MsoTriState.msoFalse;
            Sseries4.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            //Sseries4.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 217, 217, 217).ToArgb();
            Sseries4.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 217, 217, 217).ToArgb();
            //Sseries4.Format.Fill. ForeColor.TintAndShade = 0;
            //Sseries4.Format.Fill.ForeColor.Brightness = 0;
            //Sseries4.Format.Fill.Solid();
            //Sseries4.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 217, 217, 217).ToArgb();
            //Sseries4.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 217, 217, 217).ToArgb();
            //**********************************************************************************
            Excel.Series Sseries2 = series.Item(2);
            Sseries2.ChartType = Excel.XlChartType.xlPie;
            c1 = (range)worksheet.Cells[start_row + 1, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            Sseries2.Values = worksheet.get_Range(c1, c2);
            Excel.ChartGroup group2 = (Excel.ChartGroup)chart.ChartGroups(1);
            //group2.DoughnutHoleSize = 80;
            group2.FirstSliceAngle = 225;

            Sseries2.Format.Line.Visible = Office.MsoTriState.msoFalse;
            //Sseries2.Format.Line.Weight = 1.25F;

            Excel.ChartGroup group1 = (Excel.ChartGroup)chart.ChartGroups(2);
            group1.FirstSliceAngle = 225;
            group1.DoughnutHoleSize = 50;

            point = Sseries2.Points(1);
            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            point = Sseries2.Points(3);
            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            point = Sseries2.Points(2);
            point.Format.Line.Visible = Office.MsoTriState.msoTrue;
            point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 255).ToArgb();
            point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 255).ToArgb();
            point.Format.Line.Weight = 4.0F;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.ChartArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            //float Centeri = Convert.ToSingle((chart.PlotArea.InsideLeft + chart.PlotArea.InsideWidth / 2));
            //float Centerj =  Convert.ToSingle((chart.PlotArea.InsideTop + chart.PlotArea.InsideHeight / 2));

            //Microsoft.Office.Interop.Excel.Shape shape = worksheet.Shapes.AddShape(Microsoft.Office.Core.MsoAutoShapeType.msoShapeOval,
            //     Centeri,Centerj,10,10);
        }

        private void RoseColor()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;


            int i, j, k;
            double[,] Sub_slice = new double[rows - 1, cols - 1];
            for (i = 0; i < rows - 1; i++)
            {
                ((range)worksheet.Cells[start_row + rows + i, start_col]).Value2 = "Sub Sum" + Convert.ToString(i + 1);

                for (j = 0; j < cols - 1; j++)
                {
                    if (i == 0)
                    {
                        Sub_slice[i, j] = 0;
                    }
                    else if (i == 1)
                    {
                        Sub_slice[i, j] = double.Parse(str[i + 1, j + 1]);
                    }
                    else
                    {
                        Sub_slice[i, j] = Sub_slice[i - 1, j] + double.Parse(str[i + 1, j + 1]);
                    }

                }
            }

            range c1 = (range)worksheet.Cells[start_row + rows, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + rows + 1 + rows - 3, start_col + cols - 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Sub_slice;


            int Nrows = 31;
            double[,] data = new double[Nrows, cols - 1];

            for (i = 1; i < cols; i++)
            {
                ((range)worksheet.Cells[start_row + rows * 2 - 1, start_col + i]).Value2 = str[0, i];
            }

            for (i = 1; i <= Nrows; i++)
            {
                ((range)worksheet.Cells[start_row + rows * 2 - 1 + i, start_col]).Value2 = i;
            }

            for (i = 0; i < Nrows; i++)
            {
                for (j = 0; j < cols - 1; j++)
                {
                    data[i, j] = double.Parse(str[1, j + 1]);
                }
            }

            c1 = (range)worksheet.Cells[start_row + rows * 2 - 1 + 1, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows * 2 - 1 + Nrows, start_col + cols - 1];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (range)worksheet.Cells[start_row + rows * 2 - 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows * 2 - 1 + Nrows, start_col + cols - 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlRows);
            chart.ChartType = Excel.XlChartType.xlDoughnut;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.DoughnutHoleSize = 10;

            Excel.Point point;
            Excel.Series Sseries;
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            int Nseries = rows - 2;
            for (i = 1; i <= Nrows; i++)
            {
                Sseries = series.Item(i);
                //Sseries.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
                for (j = 1; j <= cols - 1; j++)
                {
                    point = (Excel.Point)Sseries.Points(j);
                    //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                    for (k = 0; k < rows - 2; k++)
                    {
                        if (i > Convert.ToInt32(Sub_slice[k, j - 1] * (Nrows - 1)) & i <= Convert.ToInt32(Sub_slice[k + 1, j - 1] * (Nrows - 1))) break;
                    }

                    if (k == 1)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent1;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent1;
                    }
                    else if (k == 2)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent2;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent2;
                    }
                    else if (k == 0)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent3;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent3;
                    }
                    else if (k == 4)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent4;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent4;
                    }
                    else if (k == 3)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent5;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent5;
                    }
                    else if (k == 5)
                    {
                        point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent6;
                        point.Format.Line.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent6;
                    }

                    if (k >= rows - 2)
                    {
                        point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
                        point.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
                    }
                }
            }
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.ChartArea.Format.Fill.Transparency = 0;

            // Add New serives
            //Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Pie = series.NewSeries();

            c1 = (range)worksheet.Cells[activecells.Row + 1, activecells.Column + 1];
            c2 = (range)worksheet.Cells[activecells.Row + 1, activecells.Column + 1 + cols - 2];
            Pie.Values = worksheet.get_Range(c1, c2);

            //chart.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataLabelOutSideEnd);
            c1 = (range)worksheet.Cells[activecells.Row + 2, activecells.Column + 1];
            c2 = (range)worksheet.Cells[activecells.Row + 2, activecells.Column + 1 + cols - 2];
            Pie.XValues = worksheet.get_Range(c1, c2);

            //Pie.PlotOrder = 1;

            Pie.ChartType = Excel.XlChartType.xlPie;

            Pie.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();

            Pie.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Pie.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Pie.Format.Line.Weight = 0.25F;
            ////Pie.PlotOrder = Nrows-1;

            //Pie.HasDataLabels = true;
            //Excel.DataLabels label = (Excel.DataLabels)Pie.DataLabels();
            ////label.ShowCategoryName = true;

            //label.ShowValue = false;
            //label.ShowCategoryName = true;
            //label.ShowPercentage = false;
            //label.Position = Microsoft.Office.Interop.Excel.XlDataLabelPosition.xlLabelPositionOutsideEnd;
            //label.Format.TextFrame2.TextRange.Font.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.TintAndShade = 0;
            //label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.Brightness = 0;
            //label.Format.TextFrame2.TextRange.Font.Fill.Transparency = 0;
            //label.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            //label.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            //label.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            //label.Format.TextFrame2.TextRange.Font.Size = 10;

            // Series line
            //for (j = 1; j < cols; j++)
            //{
            //    Sseries = series.Item(j);
            //    Sseries.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
            //    Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            //    Sseries.Format.Line.Weight = (float)1.5;
            //}

            //Legend
            chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
            chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Size = 12;

            //Excel.LegendEntry LegendPie = chart.Legend.LegendEntries(1);
            //LegendPie.Delete();

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            //group.DoughnutHoleSize = 0;
            Sseries = series.Item(Nrows);
            Sseries.ChartType = Excel.XlChartType.xlPie;

            Sseries.Format.Fill.Visible = Office.MsoTriState.msoFalse;

            Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Sseries.Format.Line.Weight = 0.75F;
            Sseries.HasDataLabels = true;
            Excel.DataLabels label = (Excel.DataLabels)Sseries.DataLabels();
            //label.ShowCategoryName = true;

            label.ShowValue = false;
            label.ShowCategoryName = true;
            label.ShowPercentage = false;
            label.Position = Excel.XlDataLabelPosition.xlLabelPositionOutsideEnd;
            label.Format.TextFrame2.TextRange.Font.Fill.Visible = Office.MsoTriState.msoTrue;
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.TintAndShade = 0;
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.Brightness = 0;
            label.Format.TextFrame2.TextRange.Font.Fill.Transparency = 0;
            label.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Size = 10;

            // string s = Application.ProductVersion;
            if (Application.ProductVersion.Equals("16.0.6769.2015"))
            {
                group.DoughnutHoleSize = 0;
            }
        }


        private void Rose()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row + rows + 1;

            ((range)worksheet.Cells[start_row, start_col]).Value2 = "Percentage of 360";
            ((range)worksheet.Cells[start_row + 1, start_col]).Value2 = "Start Angle";
            ((range)worksheet.Cells[start_row + 2, start_col]).Value2 = "Finish Angle";

            double Sum_Slice = 0;
            double[] Percen = new double[cols - 1];
            int i, j;
            for (j = 1; j < cols; j++)
            {
                Sum_Slice = Sum_Slice + double.Parse(str[1, j]);
            }
            for (j = 1; j < cols; j++)
            {
                ((range)worksheet.Cells[start_row, start_col + j]).Value2 = double.Parse(str[1, j]) / Sum_Slice;
                Percen[j - 1] = double.Parse(str[1, j]) / Sum_Slice;
            }

            double Sum_Sangle = 0;
            double Sum_Eangle = 0;
            double[] Sangle = new double[cols - 1];
            double[] Eangle = new double[cols - 1];
            for (j = 1; j < cols; j++)
            {
                if (j > 1) Sum_Sangle = Sum_Sangle + Percen[j - 2];

                Sum_Eangle = Sum_Eangle + Percen[j - 1];

                ((range)worksheet.Cells[start_row + 1, start_col + j]).Value2 = Sum_Sangle * 360;
                ((range)worksheet.Cells[start_row + 2, start_col + j]).Value2 = Sum_Eangle * 360;

                Sangle[j - 1] = Sum_Sangle * 360;
                Eangle[j - 1] = Sum_Eangle * 360;
            }

            ((range)worksheet.Cells[start_row + 4, start_col]).Value2 = "Angles";
            for (j = 1; j < cols; j++)
            {
                ((range)worksheet.Cells[start_row + 4, start_col + j]).Value2 = str[0, j];
            }
            for (i = 0; i <= 360; i++)
            {
                ((range)worksheet.Cells[start_row + 5 + i, start_col]).Value2 = i;
            }

            double[,] Data = new double[361, cols - 1];
            for (j = 1; j < cols; j++)
            {
                for (i = 0; i <= 360; i++)
                {
                    if (i >= Sangle[j - 1] & i <= Eangle[j - 1])
                    {
                        Data[i, j - 1] = double.Parse(str[2, j]);
                    }
                    else
                    {
                        Data[i, j - 1] = 0;
                    }
                }
            }

            range c1 = (range)worksheet.Cells[start_row + 5, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + 5 + 360, start_col + 1 + cols - 2];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Data;

            c1 = (range)worksheet.Cells[start_row + 4, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + 4 + 360, start_col + 1 + cols - 2];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Type.Missing);
            chart.ChartType = Excel.XlChartType.xlRadarFilled;

            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.ChartArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.ChartArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.ChartArea.Format.Fill.Transparency = 0;

            //y axis
            Excel.Axis axis = (Excel.Axis)chart.Axes(
                Excel.XlAxisType.xlValue,
                Excel.XlAxisGroup.xlPrimary);
            axis.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.Delete();

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.RadarAxisLabels.Delete();

            //Add New series
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Pie = series.NewSeries();
            Pie.ChartType = Excel.XlChartType.xlPie;

            c1 = (range)worksheet.Cells[activecells.Row + 1, activecells.Column + 1];
            c2 = (range)worksheet.Cells[activecells.Row + 1, activecells.Column + 1 + cols - 2];
            Pie.Values = worksheet.get_Range(c1, c2);
            Pie.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();

            Pie.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Pie.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();

            //chart.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataLabelOutSideEnd);
            c1 = (range)worksheet.Cells[activecells.Row + 2, activecells.Column + 1];
            c2 = (range)worksheet.Cells[activecells.Row + 2, activecells.Column + 1 + cols - 2];
            Pie.XValues = worksheet.get_Range(c1, c2);
            Pie.HasDataLabels = true;
            Excel.DataLabels label = (Excel.DataLabels)Pie.DataLabels();
            //label.ShowCategoryName = true;

            label.ShowValue = false;
            label.ShowCategoryName = true;
            label.ShowPercentage = false;
            label.Position = Excel.XlDataLabelPosition.xlLabelPositionOutsideEnd;
            label.Format.TextFrame2.TextRange.Font.Fill.Visible = Office.MsoTriState.msoTrue;
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.TintAndShade = 0;
            label.Format.TextFrame2.TextRange.Font.Fill.ForeColor.Brightness = 0;
            label.Format.TextFrame2.TextRange.Font.Fill.Transparency = 0;
            label.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            label.Format.TextFrame2.TextRange.Font.Size = 10;

            // Series line
            for (j = 1; j < cols; j++)
            {
                Excel.Series Sseries = series.Item(j);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                Sseries.Format.Line.Weight = (float)1.5;
            }

            //Legend
            chart.SetElement(Office.MsoChartElementType.msoElementLegendRight);
            chart.Legend.Format.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            chart.Legend.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            chart.Legend.Format.TextFrame2.TextRange.Font.Size = 12;

            Excel.LegendEntry LegendPie = chart.Legend.LegendEntries(1);
            LegendPie.Delete();

            // ChartArea Line
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            //chart.ChartArea.Height = 340.157480315;
            //chart.ChartArea.Width = 380.5039370079;
        }


        private void UncommonBar()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);


            double Sum_Slice = 0;
            int i, j;
            for (j = 1; j < cols; j++)
            {
                Sum_Slice = Sum_Slice + double.Parse(str[1, j]);
            }

            int k;
            double[] Sub_Sum = new double[cols - 1];
            for (i = 1; i < cols; i++)
            {
                for (k = 1; k <= i; k++)
                {
                    Sub_Sum[i - 1] = Sub_Sum[i - 1] + double.Parse(str[1, k]) / Sum_Slice * 100;
                }
            }

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row + rows, start_col]).Value2 = "Sub Sum";
            range c1 = (range)worksheet.Cells[start_row + rows, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + rows, start_col + cols - 1];

            range range = worksheet.get_Range(c1, c2);
            range.Value = Sub_Sum;

            //((Excel.Range)worksheet.Cells[start_row + rows+2, start_col]).Value2 = "X-axis Value";
            for (j = 1; j < cols; j++)
            {
                ((range)worksheet.Cells[start_row + rows + 2, start_col + j]).Value2 = str[0, j];
            }

            int NewRows = 3 * (cols - 2) + 2;
            for (i = 0; i < NewRows; i++)
            {
                if (i == 0)
                {
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = 0;
                }
                else if (i == NewRows - 1)
                {
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = 100;
                }
                else
                {
                    double temp = i;
                    int idx = (int)Math.Ceiling(temp / 3) - 1;
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = Sub_Sum[idx];
                }
            }

            double[,] data = new double[NewRows, cols - 1];
            int IdexRow = 0;
            for (j = 1; j < cols; j++)
            {
                if (j == 1)
                {
                    data[0, j - 1] = double.Parse(str[2, j]);
                    data[1, j - 1] = double.Parse(str[2, j]);
                    IdexRow = 1;
                }
                else
                {
                    data[0 + IdexRow + 2, j - 1] = double.Parse(str[2, j]);
                    data[1 + IdexRow + 2, j - 1] = double.Parse(str[2, j]);
                    IdexRow = 1 + IdexRow + 2;
                }
            }

            c1 = (range)worksheet.Cells[start_row + rows + 3, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows + 3 + NewRows - 1, start_col + cols - 1];
            range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 2 + NewRows, start_col + cols - 1];
            range = worksheet.get_Range(c1, c2);

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlArea;

            Excel.Axis axis = (Excel.Axis)chart.Axes(
               Excel.XlAxisType.xlCategory,
               Excel.XlAxisGroup.xlPrimary);
            axis.CategoryType = Excel.XlCategoryType.xlTimeScale;
            axis.TickLabels.NumberFormatLocal = "0_);[Red](0)";
            axis.MajorUnit = 10;


            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            // Series line
            for (j = 1; j < cols; j++)
            {
                Excel.Series Sseries = series.Item(j);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                Sseries.Format.Line.Weight = (float)1.5;
            }
        }

        private void ColumnStructure()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);


            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            int lable_rows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cols) / 2));

            for (int i = 0; i < cols; i++)
            {
                ((range)worksheet.Cells[start_row + rows + 1, start_col + i]).Value2 = str[0, i];
            }

            ((range)worksheet.Cells[start_row + rows + 1, start_col + cols]).Value2 = "Total";

            for (int i = 0; i < rows - 1; i++)
            {
                ((range)worksheet.Cells[start_row + rows + 1 + i * cols + lable_rows, start_col]).Value2 = str[i + 1, 0];
            }

            double[] Sub_sum = new double[rows - 1];
            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 1; j < cols; j++)
                {
                    Sub_sum[i] = Sub_sum[i] + double.Parse(str[i + 1, j]);
                }

            }

            int Nrows = (rows - 1) * cols;
            int Ncols = cols;
            double[,] data = new double[Nrows, Ncols];
            for (int i = 0; i < Nrows; i++)
            {
                int index_rows1 = i / cols;//求整数
                int index_rows2 = i % cols;//求余数

                if (index_rows2 != 0)
                {
                    data[i, index_rows2 - 1] = double.Parse(str[index_rows1 + 1, index_rows2]);

                    data[i, Ncols - 1] = Sub_sum[index_rows1];
                }
            }

            range c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col + 1 + Ncols - 1];

            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (range)worksheet.Cells[start_row + rows + 1, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows + 1, start_col + 1 + Ncols - 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnStacked;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;
            group.Overlap = 100;

            //*************************************************************Style******************************
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

            // Add GridLinesMinorMajor
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

            //y axis
            Excel.Axis Yaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);

            Yaxis.Format.Line.Visible = Office.MsoTriState.msoFalse;
            Yaxis.HasTitle = true;
            Yaxis.AxisTitle.Text = "y axis";

            Yaxis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Yaxis.TickLabels.Font.Name = "Times New Roman";
            Yaxis.TickLabels.Font.Size = 10;
            Yaxis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;


            //x axis
            Excel.Axis Xaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);

            Xaxis.TickMarkSpacing = 1;

            Xaxis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Xaxis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Xaxis.MajorGridlines.Format.Line.Weight = (float)1.25;
            Xaxis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Xaxis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Xaxis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            Xaxis.MinorGridlines.Format.Line.Weight = (float)0.25;
            Xaxis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Xaxis.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Xaxis.HasTitle = true;
            Xaxis.AxisTitle.Text = "x axis";

            Xaxis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Xaxis.TickLabels.Font.Name = "Times New Roman";
            Xaxis.TickLabels.Font.Size = 10;
            Xaxis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            //**********************************************************************
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            int Nseries = series.Count;

            Excel.Series Sseries1 = series.Item(Nseries);
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 2 + Nrows, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);
            Sseries1.Format.Fill.ForeColor.ObjectThemeColor = Microsoft.Office.Core.MsoThemeColorIndex.msoThemeColorBackground1;
            Sseries1.Format.Fill.ForeColor.TintAndShade = 0;
            //float B = (float)(-0.5 + 0.25 * i);
            Sseries1.Format.Fill.ForeColor.Brightness = -0.349999994F;
            Sseries1.Format.Fill.Transparency = 0;

            for (int i = 0; i < Nseries - 1; i++)
            {
                Sseries1 = series.Item(i + 1);
                Sseries1.AxisGroup = Excel.XlAxisGroup.xlSecondary;
                Sseries1.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries1.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                Sseries1.Format.Line.Weight = 0.25F;
            }


            //***************************************Series*******************************

            Excel.Axis YSaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
            // Yaxis.MaximumScale = 1.2F;
            //Yaxis.MinimumScale = 0F;
            YSaxis.MaximumScale = Yaxis.MaximumScale;
            YSaxis.MinimumScale = Yaxis.MinimumScale;
            Yaxis.MinorUnit = Yaxis.MajorUnit / 2;
            Yaxis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Yaxis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Yaxis.MajorGridlines.Format.Line.Weight = (float)1.25;
            Yaxis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Yaxis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Yaxis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            Yaxis.MinorGridlines.Format.Line.Weight = (float)0.25;
            Yaxis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;
        }
        private void ColumnPyramid()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);


            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            double[,] data = new double[rows - 1, 1];
            ((range)worksheet.Cells[start_row, start_col + cols]).Value2 = str[0, 1];
            for (int i = 1; i < rows; i++)
            {
                data[i - 1, 0] = -double.Parse(str[i, 1]);
            }
            range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];

            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            c1 = (range)worksheet.Cells[start_row, start_col + cols - 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlBarStacked;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;
            group.Overlap = 100;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries1 = series.Item(1);

            c1 = (range)worksheet.Cells[start_row + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            for (int i = 1; i <= 2; i++)
            {
                Excel.Series Sseries = series.Item(i);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Line.Weight = 0.75F;
            }


            //*******************************************************Style**********************************************************
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

            Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            axis.TickLabels.NumberFormatLocal = "#,##0.00;[Red]#,##0.00";
            axis.HasMajorGridlines = true;
            axis.HasMinorGridlines = true;

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



            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            axis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;
            axis.HasMajorGridlines = false;
            axis.HasMinorGridlines = true;

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
        }

        private void Bullet()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);


            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            range c1 = (range)worksheet.Cells[start_row, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols - 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnStacked;
            //***************************************Type*********************************
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

            // Add GridLinesMinorMajor
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryGridLinesMinorMajor);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryValueAxisTitleAdjacentToAxis);
            chart.SetElement(Office.MsoChartElementType.msoElementPrimaryCategoryAxisTitleAdjacentToAxis);

            //y axis
            Excel.Axis Yaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);



            Yaxis.Format.Line.Visible = Office.MsoTriState.msoFalse;
            Yaxis.HasTitle = true;
            Yaxis.AxisTitle.Text = "y axis";

            Yaxis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Yaxis.TickLabels.Font.Name = "Times New Roman";
            Yaxis.TickLabels.Font.Size = 10;
            Yaxis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            Yaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;


            //x axis
            Excel.Axis Xaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);

            Xaxis.TickMarkSpacing = 1;

            Xaxis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Xaxis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Xaxis.MajorGridlines.Format.Line.Weight = (float)1.25;
            Xaxis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Xaxis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Xaxis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            Xaxis.MinorGridlines.Format.Line.Weight = (float)0.25;
            Xaxis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Xaxis.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Xaxis.HasTitle = true;
            Xaxis.AxisTitle.Text = "x axis";

            Xaxis.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Xaxis.TickLabels.Font.Name = "Times New Roman";
            Xaxis.TickLabels.Font.Size = 10;
            Xaxis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            Xaxis.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            //***************************************Series*******************************
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Real_Series = series.Item(1);
            Real_Series.AxisGroup = Excel.XlAxisGroup.xlSecondary;
            Real_Series.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Real_Series.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Real_Series.Format.Line.Weight = 0.25F;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(2);
            group.GapWidth = 400;
            Excel.Axis YSaxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
            Yaxis.MaximumScale = 1.2F;
            Yaxis.MinimumScale = 0F;
            YSaxis.MaximumScale = Yaxis.MaximumScale;
            YSaxis.MinimumScale = Yaxis.MinimumScale;

            Yaxis.MinorUnit = Yaxis.MajorUnit / 2;
            Yaxis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Yaxis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            Yaxis.MajorGridlines.Format.Line.Weight = (float)1.25;
            Yaxis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Yaxis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Yaxis.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(242, 242, 242).ToArgb();
            Yaxis.MinorGridlines.Format.Line.Weight = (float)0.25;
            Yaxis.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            Excel.Series Goal_Series = series.Item(2);
            Goal_Series.ChartType = Excel.XlChartType.xlLineMarkers;
            Goal_Series.Format.Line.Visible = Office.MsoTriState.msoFalse;
            Goal_Series.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleDash;
            Goal_Series.MarkerSize = 22;

            group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 150;
            group.Overlap = 100;

            int Nseries = series.Count - 2;
            for (int i = 0; i < Nseries; i++)
            {
                Excel.Series Series345 = series.Item(i + 3);
                Series345.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Series345.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
                Series345.Format.Line.Weight = 0.25F;
            }

            Excel.Series Series3 = series.Item(3);
            Series3.Format.Fill.ForeColor.ObjectThemeColor = Microsoft.Office.Core.MsoThemeColorIndex.msoThemeColorText1;
            Series3.Format.Fill.ForeColor.TintAndShade = 0;
            Series3.Format.Fill.ForeColor.Brightness = 0.349999994F;
            Series3.Format.Fill.Transparency = 0;

            for (int i = 0; i < Nseries - 1; i++)
            {
                Excel.Series Series4 = series.Item(4 + i);
                Series4.Format.Fill.ForeColor.ObjectThemeColor = Microsoft.Office.Core.MsoThemeColorIndex.msoThemeColorBackground1;
                Series4.Format.Fill.ForeColor.TintAndShade = 0;
                float B = (float)(-0.5 + 0.25 * i);
                Series4.Format.Fill.ForeColor.Brightness = B;
                Series4.Format.Fill.Transparency = 0;
            }


        }


        private void StackedBar()
        {

            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            double Sum_Slice = 0;
            int i, j;
            for (j = 1; j < rows; j++)
            {
                Sum_Slice = Sum_Slice + double.Parse(str[j, 1]);
            }

            ((range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Percentage %";
            int[] data = new int[rows];
            data[0] = 0;
            for (i = 1; i < rows; i++)
            {
                for (j = 1; j <= i; j++)
                {
                    data[i] = data[i] + (int)(double.Parse(str[j, 1]) / Sum_Slice * 100);
                }

                ((range)worksheet.Cells[start_row + i, start_col + cols]).Value2 = data[i];
            }
            data[rows - 1] = 100;

            string[] Seriesname = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            int Nrows = 11;
            for (i = 1; i < Nrows; i++)
            {
                if (i < rows)
                {
                    ((range)worksheet.Cells[start_row + rows + 1, start_col + i]).Value2 = str[i, 0];
                }
                else
                {
                    ((range)worksheet.Cells[start_row + rows + 1, start_col + i]).Value2 = Seriesname[i - 1];
                }
                ((range)worksheet.Cells[start_row + rows + 1 + i, start_col]).Value2 = Seriesname[i - 1];
                for (j = 1; j < Nrows; j++)
                {
                    ((range)worksheet.Cells[start_row + rows + 1 + i, start_col + j]).Value2 = 1;
                }
            }

            range c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col + Nrows - 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnStacked100;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Point point;
            Excel.Series Sseries;
            int Num = 1;
            int k;
            for (j = 1; j < Nrows; j++)
            {
                Sseries = series.Item(j);

                Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Line.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Line.Weight = 2;

                for (i = 1; i < Nrows; i++)
                {
                    point = (Excel.Point)Sseries.Points(i);

                    for (k = 1; k < rows; k++)
                    {
                        if (Num > data[k - 1] & Num <= data[k]) break;
                    }
                    if (k == 1) point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent1;
                    else if (k == 2) point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent2;
                    else if (k == 3) point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent3;
                    else if (k == 4) point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent4;
                    else if (k == 5) point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent5;
                    else point.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent6;

                    Num = Num + 1;
                }

            }
        }

        public void RGB2HSV(System.Drawing.Color RGB, ref int[] HSV)
        {
            // H:0-360, S；0-100, V;：0-100
            //System.Drawing.Color MyColor = System.Drawing.Color.FromArgb(R, G, B);
            int B = RGB.B;
            int G = RGB.G;
            int R = RGB.R;
            HSV[0] = Convert.ToInt32(RGB.GetHue());

            //奇怪——用微软自己的方法获得的S值和V值居然不对
            //S=Convert.ToInt32(MyColor.GetSaturation()/255*100);
            //V=Convert.ToInt32(MyColor.GetBrightness()/255*100);

            decimal min;
            decimal max;
            decimal delta;

            decimal R1 = Convert.ToDecimal(R) / 255;
            decimal G1 = Convert.ToDecimal(G) / 255;
            decimal B1 = Convert.ToDecimal(B) / 255;

            min = Math.Min(Math.Min(R1, G1), B1);
            max = Math.Max(Math.Max(R1, G1), B1);
            HSV[2] = Convert.ToInt32(max * 100);
            delta = (max - min) * 100;

            if (max == 0 || delta == 0)
                HSV[1] = 0;
            else
                HSV[1] = Convert.ToInt32(delta / max);
        }

        public void HSV2RGB(ref System.Drawing.Color RGB, int[] HSV)
        {
            int R, G, B;
            int H, S, V;
            H = HSV[0];
            S = HSV[1];
            V = HSV[2];

            if (V > 100) V = 99;
            if (V <= 0) V = 1;

            H = Convert.ToInt32(Convert.ToDecimal(H) / 360 * 255);
            S = Convert.ToInt32(Convert.ToDecimal(S) / 100 * 255);
            V = Convert.ToInt32(Convert.ToDecimal(V) / 100 * 255);

            if (S == 0)
            {
                R = 0;
                G = 0;
                B = 0;
            }

            decimal fractionalSector;
            decimal sectorNumber;
            decimal sectorPos;
            sectorPos = (Convert.ToDecimal(H) / 255 * 360) / 60;
            sectorNumber = Convert.ToInt32(Math.Floor(Convert.ToDouble(sectorPos)));
            fractionalSector = sectorPos - sectorNumber;

            decimal p;
            decimal q;
            decimal t;

            decimal r = 0;
            decimal g = 0;
            decimal b = 0;
            decimal ss = Convert.ToDecimal(S) / 255;
            decimal vv = Convert.ToDecimal(V) / 255;


            p = vv * (1 - ss);
            q = vv * (1 - (ss * fractionalSector));
            t = vv * (1 - (ss * (1 - fractionalSector)));

            switch (Convert.ToInt32(sectorNumber))
            {
                case 0:
                    r = vv;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = vv;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = vv;
                    b = t;
                    break;
                case 3:

                    r = p;
                    g = q;
                    b = vv;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = vv;
                    break;
                case 5:
                    r = vv;
                    g = p;
                    b = q;
                    break;
            }
            R = Convert.ToInt32(r * 255);
            G = Convert.ToInt32(g * 255);
            B = Convert.ToInt32(b * 255);

            RGB = System.Drawing.Color.FromArgb(R, G, B);
        }

        private void Spline(double[] x,              /*x坐标序列*/
                         double[] y,              /*y坐标序列*/
                         double ddy1, double ddyn,/*第一点和最末点二阶导数*/
                         double[] t,              /*插值点的x坐标序列*/
                         ref double[] z)              /*差值点的y坐标序列*/
        {
            int n = x.Length;    /*输入数据个数*/
            int m = t.Length;   /*插值点个数*/
            int i, j;
            double h0, h1, alpha, beta;
            double[] s = new double[n];
            double[] dy = new double[n];
            dy[0] = -0.5;
            h0 = x[1] - x[0];
            s[0] = 3.0 * (y[1] - y[0]) / (2.0 * h0) - ddy1 * h0 / 4.0;
            h1 = 0.0001;
            for (j = 1; j <= n - 2; j++)
            {
                h1 = x[j + 1] - x[j];
                alpha = h0 / (h0 + h1);
                beta = (1.0 - alpha) * (y[j] - y[j - 1]) / h0;
                beta = 3.0 * (beta + alpha * (y[j + 1] - y[j]) / h1);
                dy[j] = -alpha / (2.0 + (1.0 - alpha) * dy[j - 1]);
                s[j] = (beta - (1.0 - alpha) * s[j - 1]);
                s[j] = s[j] / (2.0 + (1.0 - alpha) * dy[j - 1]);
                h0 = h1;
            }
            dy[n - 1] = (3.0 * (y[n - 1] - y[n - 2]) / h1 + ddyn * h1 / 2.0 - s[n - 2]) / (2.0 + dy[n - 2]);
            for (j = n - 2; j >= 0; j--)
                dy[j] = dy[j] * dy[j + 1] + s[j];

            for (j = 0; j <= n - 2; j++)
                s[j] = x[j + 1] - x[j];

            for (j = 0; j <= m - 1; j++)
            {
                if (t[j] >= x[n - 1])
                {
                    i = n - 2;
                }
                else
                {
                    i = 0;
                    while (t[j] > x[i + 1]) i = i + 1;
                }

                h1 = (x[i + 1] - t[j]) / s[i];
                h0 = h1 * h1;
                z[j] = (3.0 * h0 - 2.0 * h0 * h1) * y[i];
                z[j] = z[j] + s[i] * (h0 - h0 * h1) * dy[i];
                h1 = (t[j] - x[i]) / s[i];
                h0 = h1 * h1;
                z[j] = z[j] + (3.0 * h0 - 2.0 * h0 * h1) * y[i + 1];
                z[j] = z[j] - s[i] * (h0 - h0 * h1) * dy[i + 1];
            }
            //delete[] s;
            //delete[] dy;
        }


        private void ScatterBead()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            //double[] x = new double[rows - 1];
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row, start_col + cols]).Value2 = "Assist Y";

            int i = 1;
            for (i = 1; i < rows; i++)
            {
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Value2 = i;
            }

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 300, 400, ChartOrder);
            Nchart = Nchart + 1;

            range c1 = (range)worksheet.Cells[start_row, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlBarClustered;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            //*******************************************************series*************************************
            Excel.Series Sseries2;
            if (series.Count == 2)
            {
                Sseries2 = series.Item(2);
                Sseries2.Delete();
            }

            Excel.Series Sseries1 = series.Item(1);

            i = 0;
            Sseries1.Name = str[0, i];

            c1 = (range)worksheet.Cells[start_row + 1, start_col + i];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + i];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            //c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
            //c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];
            //Sseries1.Values = worksheet.get_Range(c1, c2);

            Sseries1.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            Sseries1.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Excel.Series Sseries3 = series.NewSeries();
            Sseries3.AxisGroup = Excel.XlAxisGroup.xlSecondary;
            Sseries3.ChartType = Excel.XlChartType.xlXYScatter;

            i = 1;
            Sseries3.Name = str[0, i];

            c1 = (range)worksheet.Cells[start_row + 1, start_col + i];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + i];
            Sseries3.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];
            Sseries3.Values = worksheet.get_Range(c1, c2);

            Sseries3.Format.Fill.Visible = Office.MsoTriState.msoTrue;
            Sseries3.Format.Fill.Solid();
            Sseries3.MarkerSize = 8;
            Sseries3.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;


            if (cols == 3)
            {
                i = 2;
                Excel.Series Sseries4 = series.NewSeries();
                //Sseries4.AxisGroup = Excel.XlAxisGroup.xlSecondary;

                Sseries4.ChartType = Excel.XlChartType.xlXYScatter;

                Sseries4.Name = str[0, i];

                c1 = (range)worksheet.Cells[start_row + 1, start_col + i];
                c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + i];
                Sseries4.XValues = worksheet.get_Range(c1, c2);

                c1 = (range)worksheet.Cells[start_row + 1, start_col + cols];
                c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols];
                Sseries4.Values = worksheet.get_Range(c1, c2);

                Sseries4.Format.Fill.Visible = Office.MsoTriState.msoTrue;
                //Sseries4.Format.Fill.Solid();
                Sseries4.MarkerSize = 8;
                Sseries4.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;

            }

            Excel.Axis axisSecondary = (Excel.Axis)chart.Axes(
                 Excel.XlAxisType.xlValue,
                 Excel.XlAxisGroup.xlSecondary);
            axisSecondary.MinimumScale = 1;
            axisSecondary.MaximumScale = rows - 1;
            axisSecondary.MinorUnit = 1;
            axisSecondary.HasDisplayUnitLabel = false;
            axisSecondary.HasMinorGridlines = true;
            axisSecondary.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionNone;

            axisSecondary.Format.Line.Visible = Office.MsoTriState.msoFalse;

            axisSecondary.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axisSecondary.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axisSecondary.MinorGridlines.Format.Line.Weight = (float)0.75;
            axisSecondary.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;


            Excel.Axis axisPrimary = (Excel.Axis)chart.Axes(
                Excel.XlAxisType.xlCategory,
                Excel.XlAxisGroup.xlPrimary);
            axisPrimary.AxisBetweenCategories = false;

            axisPrimary.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axisPrimary.HasTitle = true;
            axisPrimary.AxisTitle.Text = "y axis";

            axisPrimary.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axisPrimary.TickLabels.Font.Name = "Times New Roman";
            axisPrimary.TickLabels.Font.Size = 10;
            axisPrimary.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            axisPrimary.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            Excel.Axis axisValue = (Excel.Axis)chart.Axes(
               Excel.XlAxisType.xlValue,
               Excel.XlAxisGroup.xlPrimary);
            axisValue.MinorUnit = axisValue.MajorUnit / 2;
            axisValue.HasMinorGridlines = true;
            axisValue.HasMajorGridlines = false;

            axisValue.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axisValue.HasTitle = true;
            axisValue.AxisTitle.Text = "x axis";

            axisValue.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axisValue.MinorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
            axisValue.MinorGridlines.Format.Line.Weight = (float)0.75;
            axisValue.MinorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

            axisValue.TickLabels.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axisValue.TickLabels.Font.Name = "Times New Roman";
            axisValue.TickLabels.Font.Size = 10;
            axisValue.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;

            axisValue.AxisTitle.Format.TextFrame2.TextRange.Characters.Font.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            axisValue.AxisTitle.Format.TextFrame2.TextRange.Font.NameComplexScript = "Times New Roman";
            axisValue.AxisTitle.Format.TextFrame2.TextRange.Font.NameFarEast = "Times New Roman";
            axisValue.AxisTitle.Format.TextFrame2.TextRange.Font.Name = "Times New Roman";
            axisValue.AxisTitle.Format.TextFrame2.TextRange.Font.Size = 10;
            axisValue.AxisTitle.Format.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoFalse;

            // Change plot area ForeColor
            chart.PlotArea.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
            chart.PlotArea.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(229, 229, 229).ToArgb();
            chart.PlotArea.Format.Fill.Transparency = 0;

            chart.PlotArea.Format.Line.Visible = Office.MsoTriState.msoFalse;
            chart.ChartArea.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Excel.LegendEntry LegendPie = chart.Legend.LegendEntries(1);
            LegendPie.Delete();

            chart.Refresh();

            chart.Activate();
        }

        private void AreaSeries()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            for (int i = 0; i < cols; i++)
            {
                ((range)worksheet.Cells[start_row + rows + 1, start_col + i]).Value2 = str[0, i];
            }

            int Nrows = (rows - 1) * (cols - 1);
            for (int i = 1; i <= Nrows; i++)
            {
                ((range)worksheet.Cells[start_row + rows + 1 + i, start_col]).Value2 = str[(i - 1) % (rows - 1) + 1, 0];
            }

            for (int j = 1; j < cols; j++)
            {
                for (int i = 1; i < rows; i++)
                {
                    ((range)worksheet.Cells[start_row + rows + 1 + (j - 1) * (rows - 1) + i, start_col + j]).Value2 = str[i, j];
                }
            }


            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            range c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows, start_col + cols - 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlLine;

            //*************************************Add new series*********************************************
            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            if (series.Count == cols)
            {
                Excel.Series Sseries0 = series.Item(1);
                Sseries0.Delete();
            }

            Office.MsoThemeColorIndex ObjectThemeColor;
            for (int j = 1; j < cols; j++)
            {

                if (j == 1)
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent1;
                }
                else if (j == 2)
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent2;
                }
                else if (j == 3)
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent3;
                }
                else if (j == 4)
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent4;
                }
                else if (j == 5)
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent5;
                }
                else
                {
                    ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent6;
                }

                Excel.Series Sseries = series.NewSeries();
                c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
                c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows, start_col];
                Sseries.XValues = worksheet.get_Range(c1, c2);

                c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + j];
                c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows, start_col + j];
                Sseries.Values = worksheet.get_Range(c1, c2);
                Sseries.ChartType = Excel.XlChartType.xlArea;

                Sseries.Format.Line.Visible = Office.MsoTriState.msoTrue;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb();
                Sseries.Format.Line.Weight = (float)0.75;

                Excel.Series Sseries0 = series.Item(j);
                Sseries0.Format.Fill.Solid();
                Sseries0.Format.Line.Visible = Office.MsoTriState.msoTrue;
                Sseries0.Format.Line.ForeColor.ObjectThemeColor = ObjectThemeColor;
                Sseries0.Format.Line.Weight = (float)1.75;

                Sseries.Format.Fill.Visible = Office.MsoTriState.msoTrue;
                Sseries.Format.Fill.ForeColor.ObjectThemeColor = ObjectThemeColor;
                Sseries.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Fill.Transparency = 0.1F;
            }

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

            // Add GridLinesMinorMajor
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
            axis.CategoryType = Excel.XlCategoryType.xlCategoryScale;

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

            //Excel.LegendEntry Legend1;
            //for (int i=1;i< cols;i++)
            //{
            //    Legend1 = chart.Legend.LegendEntries(i);
            //    Legend1.Delete();
            //}

        }

        private void SmoothArea()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            double[] x = new double[rows - 1];
            double[] y = new double[rows - 1];
            for (int i = 1; i < rows; i++)
            {
                x[i - 1] = double.Parse(str[i, 0]);
                y[i - 1] = double.Parse(str[i, 1]);
            }

            int Parameter_Spline = 10;
            double Step = (x[1] - x[0]) / Parameter_Spline;
            int Nrows = Convert.ToInt32((x[rows - 2] - x[0]) / Step) + 1;
            double[] t = new double[Nrows];
            double[] z = new double[Nrows];
            for (int i = 0; i < Nrows; i++)
            {
                t[i] = Step * i + x[0];
            }
            Spline(x, y, 0.0, 0.0, t, ref z);

            double[,] tz = new double[Nrows, 2];
            for (int i = 0; i < Nrows; i++)
            {
                tz[i, 0] = t[i];
                tz[i, 1] = z[i];
            }

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row + rows + 1, start_col]).Value2 = "Spline X";
            ((range)worksheet.Cells[start_row + rows + 1, start_col + 1]).Value2 = "Spline Y";

            range c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col + 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = tz;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col + 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlArea;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            Excel.Series Sseries2 = series.Item(2);
            Sseries2.Delete();

            Excel.Series Sseries = series.Item(1);
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);

        }


        //private void button_ParallelCoordinates_Click(object sender, RibbonControlEventArgs e)
        //{
        //    int rows = 1;
        //    int cols = 1;
        //    string[,] str = new string[1, 1];
        //    RangeData(ref str, ref rows, ref cols);

        //    worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
        //    range activecells = (range)Globals.ThisAddIn.Application.ActiveCell;
        //    int start_col = activecells.Column;
        //    int start_row = activecells.Row;
        //    for (int i = 0; i < cols; i++)
        //    {
        //        ((Excel.Range)worksheet.Cells[start_row+rows+1, start_col+i]).Value2 = str[0, i];
        //    }

        //    int Nrows = (rows - 1) * (cols - 1);
        //    double[,] data=new double[Nrows,cols];
        //    for (int j = 0; j < cols-1; j++)
        //    {
        //        for (int i = 0; i < rows-1; i++)
        //        {
        //            data[j * (rows-1) + i, j] = double.Parse(str[i+1, j]);
        //            data[j * (rows - 1) + i, j + 1] = double.Parse(str[i + 1, j + 1]);

        //            ((Excel.Range)worksheet.Cells[start_row + rows + 2 + j * (rows - 1) + i, start_col+j]).Value2 = str[i + 1, j];
        //            ((Excel.Range)worksheet.Cells[start_row + rows + 2 + j * (rows - 1) + i, start_col + j+1]).Value2 = str[i + 1, j + 1];
        //        }
        //    }

        //    string ChartOrder = "chart" + Convert.ToString(Nchart);
        //    Microsoft.Office.Tools.Excel.Chart chart = worksheet.Controls.AddChart(0, 0, 450, 400, ChartOrder);
        //    Nchart = Nchart + 1;

        //    Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row + rows + 1, start_col];
        //    Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows + 1 + Nrows , start_col + cols - 1];
        //    chart.SetSourceData(worksheet.get_Range(c1, c2), Microsoft.Office.Interop.Excel.XlRowCol.xlRows);
        //    chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

        //    Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
        //    for (int j = 1; j <= Nrows; j++)
        //    {
        //        Excel.Series Sseries = (Excel.Series)series.Item(j);

        //        Sseries.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
        //        Sseries.Format.Line.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
        //        Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 202, 157,  96). ToArgb();
        //        Sseries.Format.Line.Weight = 0.25F;
        //    }
        //}

        private void Mosaicplot()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            double Sum_Slice = 0;
            int i, j;
            for (j = 1; j < cols; j++)
            {
                Sum_Slice = Sum_Slice + double.Parse(str[1, j]);
            }

            int k;
            double[] Sub_Sum = new double[cols - 1];
            for (i = 1; i < cols; i++)
            {
                for (k = 1; k <= i; k++)
                {
                    Sub_Sum[i - 1] = Sub_Sum[i - 1] + double.Parse(str[1, k]) / Sum_Slice * 100;
                }
            }

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            int NewCols = (cols - 1) * (rows - 2);
            for (j = 1; j <= NewCols; j++)
            {
                if (j <= rows - 2)
                {
                    ((range)worksheet.Cells[start_row + rows + 2, start_col + j]).Value2 = str[j + 1, 0];
                }
                else
                {
                    ((range)worksheet.Cells[start_row + rows + 2, start_col + j]).Value2 = "Series" + Convert.ToString(j);
                }
            }

            int NewRows = 3 * (cols - 2) + 2;
            for (i = 0; i < NewRows; i++)
            {
                if (i == 0)
                {
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = 0;
                }
                else if (i == NewRows - 1)
                {
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = 100;
                }
                else
                {
                    double temp = i;
                    int idx = (int)Math.Ceiling(temp / 3) - 1;
                    ((range)worksheet.Cells[start_row + rows + 3 + i, start_col]).Value2 = Sub_Sum[idx];
                }
            }

            double[,] data = new double[NewRows, NewCols];
            int IdexRow = 0;
            for (j = 1; j < cols; j++)
            {
                if (j == 1)
                {
                    for (k = 1; k <= rows - 2; k++)
                    {
                        data[0, j - 1 + k - 1] = double.Parse(str[2 + k - 1, j]);
                        data[1, j - 1 + k - 1] = double.Parse(str[2 + k - 1, j]);
                    }
                    IdexRow = 1;
                }
                else
                {
                    for (k = 1; k <= rows - 2; k++)
                    {
                        data[0 + IdexRow + 2, k - 1 + (j - 1) * (rows - 2)] = double.Parse(str[2 + k - 1, j]);
                        data[1 + IdexRow + 2, k - 1 + (j - 1) * (rows - 2)] = double.Parse(str[2 + k - 1, j]);
                    }
                    IdexRow = 1 + IdexRow + 2;
                }
            }

            range c1 = (range)worksheet.Cells[start_row + rows + 3, start_col + 1];
            range c2 = (range)worksheet.Cells[start_row + rows + 3 + NewRows - 1, start_col + 1 + NewCols - 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = data;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 3 + NewRows - 1, start_col + 1 + NewCols - 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlAreaStacked100;

            Excel.Axis axis = (Excel.Axis)chart.Axes(
             Excel.XlAxisType.xlCategory,
             Excel.XlAxisGroup.xlPrimary);
            axis.CategoryType = Excel.XlCategoryType.xlTimeScale;
            //axis.TickLabels.NumberFormatLocal = "0_);[Red](0)";
            axis.MajorUnit = 10;


            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            // Series line
            for (j = 1; j <= NewCols; j++)
            {
                Excel.Series Sseries = series.Item(j);
                Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();
                Sseries.Format.Line.Weight = (float)1.5;

                k = j % (rows - 2);
                if (k == 0) Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent1;
                else if (k == 1) Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent2;
                else if (k == 2) Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent3;
                else if (k == 3) Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent4;
                else if (k == 4) Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent5;
                else Sseries.Format.Fill.ForeColor.ObjectThemeColor = Office.MsoThemeColorIndex.msoThemeColorAccent6;
            }
        }



        private void gallery_Bubble_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Bubble.SelectedItem.Tag.Equals("1"))
            {
                BubbleSquare Form_Parater = new BubbleSquare();
                Form_Parater.Show();

            }
            else if (gallery_Bubble.SelectedItem.Tag.Equals("2"))
            {
                BubbleRelationShip Form_Parater = new BubbleRelationShip();
                Form_Parater.Show();
            }

            else if (gallery_Bubble.SelectedItem.Tag.Equals("4"))
            {
                ScatterColor Form_Parater = new ScatterColor();
                //Form_Parater.ShowDialog();
                Form_Parater.Show();
            }
            else if (gallery_Bubble.SelectedItem.Tag.Equals("5"))
            {
                ScatterBead();
            }
        }

        private void gallery_Column_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Column.SelectedItem.Tag.Equals("5"))
            {
                SmoothArea();
            }
            else if (gallery_Column.SelectedItem.Tag.Equals("1"))
            {
                AreaThreshold Form_Parater = new AreaThreshold();
                Form_Parater.Show();
            }
            else if (gallery_Column.SelectedItem.Tag.Equals("4"))
            {
                AreaSeries();
            }
        }

        private void gallery_Bar_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Bar.SelectedItem.Tag.Equals("1"))
            {
                UncommonBar();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("2"))
            {
                Mosaicplot();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("3"))
            {
                StackedBar();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("4"))
            {
                ColumnColor Form_Parater = new ColumnColor();
                Form_Parater.Show();
            }
            //else if (gallery_Bar.SelectedItem.Tag.Equals("5"))
            //{
            //    ColumnFrequency Form_Parater = new ColumnFrequency();
            //    //Form_Parater.StartPosition = 0;
            //    //Form_Parater.Left = 3100;
            //    //Form_Parater.Top = 300;
            //    Form_Parater.SetDesktopLocation(250, 250);
            //    Form_Parater.Show();
            //}
            else if (gallery_Bar.SelectedItem.Tag.Equals("5"))
            {
                Bullet();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("6"))
            {
                ColumnThreshold Form_Parater = new ColumnThreshold();
                Form_Parater.Show();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("7"))
            {
                ColumnPyramid();
            }
            else if (gallery_Bar.SelectedItem.Tag.Equals("8"))
            {
                ColumnStructure();
            }

        }

        private void gallery_Circle_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Circle.SelectedItem.Tag.Equals("1"))
            {
                Rose();
            }
            else if (gallery_Circle.SelectedItem.Tag.Equals("2"))
            {
                RoseColor();
            }
            else if (gallery_Circle.SelectedItem.Tag.Equals("3"))
            {
                DashBoard();
            }

        }

        private void gallery_Polar_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Polar.SelectedItem.Tag.Equals("1"))
            {
                PolarArea();
            }
            //else if (gallery_Polar.SelectedItem.Tag.Equals("2"))
            //{
            //    PolarLine();
            //}
            //else if (gallery_Polar.SelectedItem.Tag.Equals("3"))
            //{
            //    PolarScatter();
            //}
        }

        private void gallery_Line_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Line.SelectedItem.Tag.Equals("1"))
            {
                CurveStep();
            }
            else if (gallery_Line.SelectedItem.Tag.Equals("2"))
            {
                CurveConfidence();
            }

        }


        private void button_QQGroup_Click(object sender, RibbonControlEventArgs e)
        {
            Form_QQGroup Form_Personal = new Form_QQGroup();
            Form_Personal.Show();
        }

        private void button_ColorPixel_Click(object sender, RibbonControlEventArgs e)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_ColorPixel());
            //Form_ColorPixel Color_Get = new Form_ColorPixel();

            Form_ColorPixel Form_Assiatant = new Form_ColorPixel();
            Form_Assiatant.Show();

        }

        private void button_Camera_Click(object sender, RibbonControlEventArgs e)
        {
            //Form_Camera Form_Assiatant = new Form_Camera();
            //Form_Assiatant.Show();
            CaptureImageTool capture = new CaptureImageTool();
            capture.ShowDialog();

        }

        private void gallery_Money_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Money.SelectedItem.Tag.Equals("1"))
            {
                Form_Money Form_Personal = new Form_Money();
                Form_Personal.Show();
            }
            else if (gallery_Money.SelectedItem.Tag.Equals("2"))
            {
                Form_WeChart Form_Personal = new Form_WeChart();
                Form_Personal.Show();
            }
        }

        private void button_ColorWheel_Click(object sender, RibbonControlEventArgs e)
        {
            Form_ColorWheel Form_Assiatant = new Form_ColorWheel();
            Form_Assiatant.Show();
        }

        private void button_GetData_Click(object sender, RibbonControlEventArgs e)
        {
            Form_GetData Form_Assiatant = new Form_GetData();
            Form_Assiatant.Show();
        }

        private void button_help_Click(object sender, RibbonControlEventArgs e)
        {
            string item = "EasyCharts Help.chm";
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "/" + item;
            System.Diagnostics.Process.Start(path);
        }

        private void gallery_DataStatistic_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_DataStatistic.SelectedItem.Tag.Equals("1"))
            {
                ColumnFrequency Form_Parater = new ColumnFrequency();
                //Form_Parater.StartPosition = 0;
                //Form_Parater.Left = 3100;
                //Form_Parater.Top = 300;
                Form_Parater.SetDesktopLocation(250, 250);
                Form_Parater.Show();
            }
            else if (gallery_DataStatistic.SelectedItem.Tag.Equals("2"))
            {
                DensityCurve Form_Parater = new DensityCurve();
                Form_Parater.Show();
            }
            else if (gallery_DataStatistic.SelectedItem.Tag.Equals("3"))
            {
                DensityScatter Form_Parater = new DensityScatter();
                Form_Parater.Show();
            }
        }

        private void gallery_DataSmooth_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_DataSmooth.SelectedItem.Tag.Equals("1"))
            {
                CurveLOESS Form_Parater = new CurveLOESS();
                //Form_Fourier Form_Parater = new Form_Fourier();
                Form_Parater.Show();
                //CurveLOESSsmooth();
            }
            else if (gallery_DataSmooth.SelectedItem.Tag.Equals("2"))
            {
                //CurveLOESS Form_Parater = new CurveLOESS();
                Form_Fourier Form_Parater = new Form_Fourier();
                Form_Parater.Show();
                //CurveLOESSsmooth();
            }
        }

        private void gallery_Cofficient_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_Cofficient.SelectedItem.Tag.Equals("1"))
            {
                Form_Cofficient Form_Parater = new Form_Cofficient();
                //Form_Fourier Form_Parater = new Form_Fourier();
                Form_Parater.Show();
                //CurveLOESSsmooth();
            }
        }


        private void button_ChartSave_Click(object sender, RibbonControlEventArgs e)
        {
            bool SelectedImage = true;
            string FileFormat = "bmp";

            Excel.Chart chart = Globals.ThisAddIn.Application.ActiveChart;
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            if (SelectedImage)
            {
                //默认文件名
                //saveFileDialog.FileName = "JD" +DateTime.Now.ToString("yyyyMMddHHmmss")+"_"+ new Random().Next(1000000, 9999999) + "." + saveFileDialog.DefaultExt;
                saveFileDialog.FileName = "JD" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + saveFileDialog.DefaultExt;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    int index = fileName.LastIndexOf('.');
                    string extion = fileName.Substring(index + 1, fileName.Length - index - 1);
                    extion = extion.ToLower();

                    //ImageFormat imageFormat = ImageFormat.Bmp;
                    switch (extion)
                    {
                        case "jpg":
                        case "jpeg":
                            FileFormat = "jpg";
                            break;
                        case "png":
                            FileFormat = "png";
                            break;
                        //case "tiff":
                        //case "tif":
                        //    FileFormat = "tiff";
                        //    break;
                        case "gif":
                            FileFormat = "gif";
                            break;
                    }

                    ////Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet;

                    ////Microsoft.Office.Interop.Excel.ChartObjects charts = sheet1.ChartObjects();
                    ////chart.CopyPicture();

                    //chart.CopyPicture(Excel.XlPictureAppearance.xlPrinter,Excel.XlCopyPictureFormat.xlBitmap);
                    ////worksheet.PasteSpecial();
                    //IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
                    //if (data.GetDataPresent(typeof(Bitmap)))
                    // {
                    // //Image Pciture;
                    // //Clipboard.SetImage(Pciture);
                    // Image Pciture = (Image)data.GetData(typeof(Bitmap));
                    // Pciture.Save(saveFileDialog.FileName);
                    // }
                    //


                    chart.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF,
                                              saveFileDialog.FileName,
                                              Excel.XlFixedFormatQuality.xlQualityStandard,
                                              false,
                                              true,
                                              1,
                                              1,
                                              false);


                    //pdf2image.Program convertor = new Program();
                    //convertor.ConvertPDF2Image("C:\\Users\\Peter_Zhang\\Desktop\\JD20180204220346.bmp.pdf", "C:\\Users\\Peter_Zhang\\Desktop\\", "test", 1, 3, ImageFormat.Jpeg, Definition.twentyfour);

                    //convertor.ConvertPDF2Image("C:\\Users\\Peter_Zhang\\Desktop\\JD20180204220346.bmp.pdf", "C:\\Users\\Peter_Zhang\\Desktop\\", "test", 1, 3, ImageFormat.Jpeg, Definition.Three);


                    //     System.IO.File.Delete(saveFileDialog.FileName);
                    // chart.Export(saveFileDialog.FileName, FileFormat, Excel.XlFixedFormatQuality.xlQualityStandard);
                }
            }
            else
            {
                MessageBox.Show("请先选择图像。", "图表", MessageBoxButtons.OK);
            }

           // chart.CopyPicture(Excel.XlPictureAppearance.xlScreen, Excel.XlCopyPictureFormat.xlPicture);
           // range location = (range)worksheet.Cells[10, 10];
           // worksheet.Paste(location);

            // Form_PictureExport convertor = new Form_PictureExport();
            // convertor.Show();
        }

        private void button_ChartSize_Click(object sender, RibbonControlEventArgs e)
        {
            Form_ChartSize Form_Parater = new Form_ChartSize();
            //Form_Fourier Form_Parater = new Form_Fourier();
            Form_Parater.Show();
        }


        private void Color_Scatter()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            //double[] x = new double[rows - 1];
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row, start_col];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 300, 400, ChartOrder);
            Nchart = Nchart + 1;

            //range c1 = (range)worksheet.Cells[start_row, start_col];
            //range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count == 2)
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

            //System.Drawing.Color RGB0 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            //System.Drawing.Color RGB1 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            int RGB_B, RGB_G, RGB_R;

            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                RGB_B = Convert.ToInt32(double.Parse(str[i, cols - 1])); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                RGB_G = Convert.ToInt32(double.Parse(str[i, cols - 2])); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                RGB_R = Convert.ToInt32(double.Parse(str[i, cols - 3])); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                //point.Format.Fill.Transparency = 0.0F;

                //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                //point.Format.Line.ForeColor.RGB = RGB0.ToArgb();
            }

            Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
            Sseries.MarkerSize = 8;

            //Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            //group.GapWidth = 0;

            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();

            chart.Refresh();
            //worksheet.Activate();
        }

        private void Color_Bar()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            //double[] x = new double[rows - 1];
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            Excel.Range c1 = (Excel.Range)worksheet.Cells[start_row, start_col];
            Excel.Range c2 = (Excel.Range)worksheet.Cells[start_row + rows - 1, start_col + 1];

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 300, 400, ChartOrder);
            Nchart = Nchart + 1;

            //range c1 = (range)worksheet.Cells[start_row, start_col];
            //range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnClustered;


            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count == 2)
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

            //System.Drawing.Color RGB0 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            //System.Drawing.Color RGB1 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            int RGB_B, RGB_G, RGB_R;

            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                RGB_B = Convert.ToInt32(double.Parse(str[i, cols - 1])); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                RGB_G = Convert.ToInt32(double.Parse(str[i, cols - 2])); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                RGB_R = Convert.ToInt32(double.Parse(str[i, cols - 3])); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                point.Format.Fill.Transparency = 0.0F;

                //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                //point.Format.Line.ForeColor.RGB = RGB0.ToArgb();
            }

            Sseries.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Sseries.Format.Line.Weight = 0.75F;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();

            chart.Refresh();
            //worksheet.Activate();
        }

        private void Color_Circle()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            //double[] x = new double[rows - 1];
            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            int Step_H = 10;
            double Step_S = 0.1;
            int Slice_H = Convert.ToInt32(360 / Step_H);
            int Slice_S = Convert.ToInt32(1.0 / Step_S) + 1;

            int[,] Data_Circle = new int[Slice_H, Slice_S + 1];

            ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "Label";

            for (int j = 1; j < Slice_S; j++)
            {
                ((Excel.Range)worksheet.Cells[start_row, start_col + cols + 1 + j]).Value2 = "Circel" + Convert.ToString(j); ;
            }

            for (int i = 0; i < Slice_H; i++)
            {
                Data_Circle[i, 0] = 360 - Step_H * (i + 1);
                for (int j = 1; j < Slice_S + 1; j++)
                {
                    Data_Circle[i, j] = 1;
                }
            }

            Excel.Range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            Excel.Range c2 = (range)worksheet.Cells[start_row + 1 + Slice_H - 1, start_col + cols + Slice_S + 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Data_Circle;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 300, 400, ChartOrder);
            Nchart = Nchart + 1;

            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlDoughnut;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.DoughnutHoleSize = 20;


            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            Excel.Series Sseries = series.Item(1);
            Sseries.Delete();

            int Nseries = series.Count;
            for (int i = 1; i <= Nseries; i++)
            {
                Sseries = series.Item(i);
                Sseries.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
                Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 166, 166, 166).ToArgb();
                Sseries.Format.Line.Weight = 0.25F;
            }

            Excel.Point point;
            Sseries = series.Item(Nseries);
            Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;

            //Sseries.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowLabel, true, false);

            for (int j = 1; j < Slice_H; j++)
            {
                point = (Excel.Point)Sseries.Points(j);
                point.HasDataLabel = true;
                //point.DataLabel.Text = Rangelabel[j].Address;
                //string s= "=" + "'" & Rangelabel.Parent.Name & "!" & Rangelabel[j].Address(Excel.XlReferenceStyle.xlR1C1);
                point.DataLabel.Text = Convert.ToString(Data_Circle[j, 0]);
            }

            point = (Excel.Point)Sseries.Points(Slice_H);
            point.HasDataLabel = true;
            point.DataLabel.Text = Convert.ToString(Data_Circle[0, 0]);

            int x, y;
            int R, G, B;

            for (int i = 1; i < rows; i++)
            {
                x = Convert.ToInt32(Math.Round((360 - double.Parse(str[i, 0])) / Step_H)) - 1;
                y = Convert.ToInt32(Math.Floor(double.Parse(str[i, 1]) / Step_S));

                if (y <= 0) y = 1;
                Sseries = series.Item(y);
                if (x <= 0) x = Slice_H;
                if (x >= Slice_H) x = 1;
                point = (Excel.Point)Sseries.Points(x);

                R = Convert.ToInt32(double.Parse(str[i, 2]));
                G = Convert.ToInt32(double.Parse(str[i, 3]));
                B = Convert.ToInt32(double.Parse(str[i, 4]));
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, B, G, R).ToArgb();
            }

        }

        private void Color_Spider()
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);

            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row, start_col + cols + 1]).Value2 = "x axis";
            ((range)worksheet.Cells[start_row, start_col + cols + 2]).Value2 = "y axis";
            double[,] data = new double[rows - 1, 2];
            double Angle;
            for (int i = 1; i < rows; i++)
            {
                Angle = double.Parse(str[i, 0]);
                data[i - 1, 0] = double.Parse(str[i, 1]) * Math.Cos(Angle / 180 * 3.14159);
                data[i - 1, 1] = double.Parse(str[i, 1]) * Math.Sin(Angle / 180 * 3.14159);
            }
            range c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            range range = worksheet.get_Range(c1, c2);
            range.Value = data;


            //***********************************************xlSecondary*************************************
            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Microsoft.Office.Tools.Excel.Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;
            c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlXYScatter;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();
            //*******************************************************series*************************************
            Excel.Series Sseries2;
            if (series.Count == 2)
            {
                Sseries2 = series.Item(2);
                Sseries2.Delete();
            }

            Excel.Series Sseries = series.Item(1);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 1];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 2];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            Sseries.Values = worksheet.get_Range(c1, c2);


            Excel.Axis axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            axis.MaximumScale = 1.0F;
            axis.MinimumScale = -1.0F;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            //x axis
            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            axis.MaximumScale = 1.0F;
            axis.MinimumScale = -1.0F;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;
            axis.MinorGridlines.Format.Line.Visible = Office.MsoTriState.msoFalse;

            Excel.Point point;
            int RGB_B, RGB_G, RGB_R;

            for (int i = 1; i < rows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                RGB_B = Convert.ToInt32(double.Parse(str[i, cols - 1])); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                RGB_G = Convert.ToInt32(double.Parse(str[i, cols - 2])); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                RGB_R = Convert.ToInt32(double.Parse(str[i, cols - 3])); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                //point.Format.Fill.Transparency = 0.0F;

                //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                //point.Format.Line.ForeColor.RGB = RGB0.ToArgb();
            }

            Sseries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
            Sseries.MarkerSize = 8;

            chart.HasLegend = false;
            chart.HasTitle = false;
            //***********************************************xlSecondary*************************************
            ((range)worksheet.Cells[start_row, start_col + cols + 4]).Value2 = "Label";
            int Step = 5;  // 整数，且被360整除 
            int Nrows = 360 / Step;
            double[,] PLRPLT = new double[Nrows, 1];
            // double MaximumScale = 100;
            int n = 0;
            for (int i = Nrows; i > 0; i--)
            {
                Angle = i * Step;
                if (i == Nrows) Angle = 0;

                if (Angle % 30 == 0)
                {

                    if (Angle + 90 >= 360)
                    {
                        ((range)worksheet.Cells[start_row + 1 + Nrows - i, start_col + cols + 4]).Value2 = Angle + 90 - 360;
                    }
                    else
                    {
                        ((range)worksheet.Cells[start_row + 1 + Nrows - i, start_col + cols + 4]).Value2 = Angle + 90;
                    }
                    PLRPLT[Nrows - i, 0] = 1.0;
                    //PLRPLT[Nrows - i, 1] = MaximumScale * Math.Cos(Angle / 180 * 3.14159);
                    //PLRPLT[Nrows - i, 2] = MaximumScale * Math.Sin(Angle / 180 * 3.14159);
                }
                n = n + 1;
            }

            //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 6]).Value2 = "Cos Value";
            //((Excel.Range)worksheet.Cells[start_row, start_col + cols + 7]).Value2 = "Sin Value";

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
            range = worksheet.get_Range(c1, c2);
            range.Value = PLRPLT;


            Sseries2 = series.NewSeries(); ;
            //Sseries2.AxisGroup = Microsoft.Office.Interop.Excel.XlAxisGroup.xlSecondary;
            Sseries2.ChartType = Excel.XlChartType.xlRadar;

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 4];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 4];
            Sseries2.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + cols + 5];
            c2 = (range)worksheet.Cells[start_row + Nrows, start_col + cols + 5];
            Sseries2.Values = worksheet.get_Range(c1, c2);

            Sseries2.Format.Line.Visible = Office.MsoTriState.msoTrue;
            Sseries2.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(216, 216, 216).ToArgb();
            Sseries2.Format.Line.Weight = (float)0.25;
            Sseries2.Format.Line.Transparency = 0;
            Sseries2.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineLongDashDot;

            axis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
            axis.MaximumScale = 1.0F;
            axis.MinimumScale = 0.0F;

            axis.Format.Line.Visible = Office.MsoTriState.msoCTrue; //Office.MsoTriState.msoFalse;
            axis.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(215, 216, 216).ToArgb();
            axis.Format.Line.Weight = (float)0.25;
            axis.HasDisplayUnitLabel = true;

            axis.HasMajorGridlines = true;
            axis.MajorGridlines.Format.Line.Visible = Office.MsoTriState.msoCTrue;
            axis.MajorGridlines.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(215, 216, 216).ToArgb();
            axis.MajorGridlines.Format.Line.Weight = (float)0.25;
            axis.MajorGridlines.Format.Line.Transparency = 0;
            axis.MajorGridlines.Format.Line.DashStyle = Office.MsoLineDashStyle.msoLineSolid;

        }

        private void ReadText(string path, ref double[,] Readdata)
        {
            StreamReader rd = File.OpenText(path);
            string s = rd.ReadLine();
            string[] ss = s.Split('	');

            int row = int.Parse(ss[0]); //行数
            int col = int.Parse(ss[1]);  //每行数据的个数

            Readdata = new double[row, col]; //数组

            for (int i = 0; i < row; i++)  //读入数据并赋予数组
            {
                string line = rd.ReadLine();
                string[] data = line.Split('	');
                for (int j = 0; j < col; j++)
                {
                    Readdata[i, j] = double.Parse(data[j]);
                }
            }

        }

        private void Spectral_Curve()
        {

            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            double[] x = new double[rows - 1];
            double[] y = new double[rows - 1];
            for (int i = 1; i < rows; i++)
            {
                x[i - 1] = double.Parse(str[i, 0]);
                y[i - 1] = double.Parse(str[i, 1]);
            }

            int Parameter_Spline = 10;
            double Step = (x[1] - x[0]) / Parameter_Spline;
            int Nrows = Convert.ToInt32((x[rows - 2] - x[0]) / Step) + 1;
            double[] t = new double[Nrows];
            double[] z = new double[Nrows];
            for (int i = 0; i < Nrows; i++)
            {
                t[i] = Step * i + x[0];
            }
            Spline(x, y, 0.0, 0.0, t, ref z);

            double[,] tz = new double[Nrows, 2];
            for (int i = 0; i < Nrows; i++)
            {
                tz[i, 0] = t[i];
                tz[i, 1] = z[i];
            }

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row + rows + 1, start_col]).Value2 = "Spline X";
            ((range)worksheet.Cells[start_row + rows + 1, start_col + 1]).Value2 = "Spline Y";

            range c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            range c2 = (range)worksheet.Cells[start_row + rows + 2 + Nrows - 1, start_col + 1];
            range range = worksheet.get_Range(c1, c2);
            range.Value = tz;

            string ChartOrder = "chart" + Convert.ToString(Nchart);
            Chart chart = worksheet.Controls.AddChart(250, 50, 450, 400, ChartOrder);
            Nchart = Nchart + 1;

            c1 = (range)worksheet.Cells[start_row + rows + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col + 1];
            chart.SetSourceData(worksheet.get_Range(c1, c2), Excel.XlRowCol.xlColumns);
            chart.ChartType = Excel.XlChartType.xlColumnClustered;

            Excel.SeriesCollection series = (Excel.SeriesCollection)chart.SeriesCollection();

            if (series.Count == 2)
            {
                Excel.Series Sseries2 = series.Item(2);
                Sseries2.Delete();
            }

            Excel.Series Sseries = series.Item(1);
            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col];
            Sseries.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + rows + 2, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows + 1 + Nrows - 1, start_col + 1];
            Sseries.Values = worksheet.get_Range(c1, c2);


            Excel.Point point;

            //System.Drawing.Color RGB0 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            //System.Drawing.Color RGB1 = System.Drawing.Color.FromArgb(255, 96, 157, 202);
            string item = "Spectral Reflection Curve.txt";
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "/" + item;

            double[,] ReadData = new double[1, 1];
            ReadText(path, ref ReadData);

            int RGB_B, RGB_G, RGB_R;

            for (int i = 1; i < Nrows; i++)
            {
                point = (Excel.Point)Sseries.Points(i);
                point.Format.Fill.Solid();
                point.Format.Fill.Visible = Office.MsoTriState.msoCTrue;
                point.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 229, 229, 229).ToArgb();

                RGB_B = Convert.ToInt32(ReadData[i - 1, 2]); if (RGB_B < 0) RGB_B = 0; if (RGB_B > 255) RGB_B = 255;
                RGB_G = Convert.ToInt32(ReadData[i - 1, 1]); if (RGB_G < 0) RGB_G = 0; if (RGB_G > 255) RGB_G = 255;
                RGB_R = Convert.ToInt32(ReadData[i - 1, 0]); if (RGB_R < 0) RGB_R = 0; if (RGB_R > 255) RGB_R = 255;
                point.Format.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(255, RGB_B, RGB_G, RGB_R).ToArgb();
                point.Format.Fill.Transparency = 0.0F;
                //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                //point.Format.Line.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                //point.Format.Line.ForeColor.RGB = RGB0.ToArgb();
            }

            Sseries.Format.Line.Visible = Office.MsoTriState.msoFalse;
            // Sseries.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            Sseries.Format.Line.Weight = 0.75F;

            Excel.ChartGroup group = (Excel.ChartGroup)chart.ChartGroups(1);
            group.GapWidth = 0;

            Excel.Axis axis = (Excel.Axis)chart.Axes(
                      Excel.XlAxisType.xlCategory,
                      Excel.XlAxisGroup.xlPrimary);
            axis.CategoryType = Microsoft.Office.Interop.Excel.XlCategoryType.xlTimeScale;


            //--------------------------------Line--------------------------------
            Excel.Series Sseries1 = series.NewSeries();
            c1 = (range)worksheet.Cells[start_row + 1, start_col];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col];
            Sseries1.XValues = worksheet.get_Range(c1, c2);

            c1 = (range)worksheet.Cells[start_row + 1, start_col + 1];
            c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + 1];
            Sseries1.Values = worksheet.get_Range(c1, c2);
            Sseries1.ChartType = Excel.XlChartType.xlXYScatterLines;
            // Sseries1.Format.Fill.Visible = Office.MsoTriState.msoFalse;
            Sseries1.Format.Line.Visible = Office.MsoTriState.msoTrue;
            Sseries1.Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
            // Sseries1.Format.Fill.BackColor.RGB = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();

            Sseries1.MarkerSize = 8;
            Sseries1.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
            Sseries1.MarkerBackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();
            Sseries1.MarkerForegroundColor = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb();

            chart.HasLegend = false;
            chart.HasTitle = false;
            //chart.ChartTitle.Delete();

            chart.Refresh();
            //worksheet.Activate();
        }

        private void gallery_ColorSpace_Click(object sender, RibbonControlEventArgs e)
        {
            if (gallery_ColorSpace.SelectedItem.Tag.Equals("1"))
            {
                Color_Scatter(); //2016-09-06
            }
            else if (gallery_ColorSpace.SelectedItem.Tag.Equals("2"))
            {
                Color_Bar(); //2016-09-06
            }
            else if (gallery_ColorSpace.SelectedItem.Tag.Equals("3"))
            {
                //Color_Circle();//2016-09-06
                Color_Spider();//2016-09-21
            }
            else if (gallery_ColorSpace.SelectedItem.Tag.Equals("4"))
            {
                Form_Color_Matrix form_Color_Matrix = new Form_Color_Matrix();
                form_Color_Matrix.Show();
            }
            else if (gallery_ColorSpace.SelectedItem.Tag.Equals("5"))
            {
                Spectral_Curve();//2016-09-21
            }
        }

        //private void button_ChartSize_Click(object sender, RibbonControlEventArgs e)
        //{
        //    Form_ChartSize_Save Form_Parater = new Form_ChartSize_Save();
        //    //Form_Fourier Form_Parater = new Form_Fourier();
        //    Form_Parater.Show();
        //}

        //private void gallery_Label_Click(object sender, RibbonControlEventArgs e)
        //{
        //    if (gallery_Label.SelectedItem.Tag.Equals("1"))
        //    {
        //        LabelAdd();
        //    }
        //}

        //************************************************RGB to Lab*********************************************************
        // RGB2Lab : sRGB system, D50,reference http://www.brucelindbloom.com/
        private void RGB2XYZ(int R, int G, int B, ref double X, ref double Y, ref double Z)
        {
            double r = degamma(R);
            double g = degamma(G);
            double b = degamma(B);
            X = 0.436075 * r + 0.385065 * g + 0.14308 * b;
            Y = 0.222505 * r + 0.716879 * g + 0.060617 * b;
            Z = 0.013932 * r + 0.097105 * g + 0.714173 * b;

        }
        private void XYZ2Lab(double X, double Y, double Z, ref double L, ref double a, ref double b)
        {
            double x0 = 0.96422, y0 = 1.00, z0 = 0.82521;  //chromatic adaptation : D50
            double Xx0, Yy0, Zz0;
            double x, y, z;
            Xx0 = X / x0; Yy0 = Y / y0; Zz0 = Z / z0;
            if (Xx0 > 0.008856)
                x = Math.Pow(Xx0, 0.333333);
            else
                x = 7.787 * Xx0 + 0.137931;

            if (Yy0 > 0.008856)
                y = Math.Pow(Yy0, 0.333333);
            else
                y = 7.787 * Yy0 + 0.137931;

            if (Zz0 > 0.008856)
                z = Math.Pow(Zz0, 0.333333);
            else
                z = 7.787 * Zz0 + 0.137931;

            L = (116.0 * y) - 16.0;
            a = 504.3 * (x - y);
            b = 201.7 * (y - z);
        }

        private double degamma(int Rcolor)
        {
            double R = (double)Rcolor / 255.0;
            double r;
            if (R <= 0.04045)
                r = R / 12.92;
            else
                r = Math.Pow((R + 0.055) / 1.055, 2.4);
            return r;
        }
        public void RGB2Lab(int R, int G, int B, ref double L, ref double a, ref double b)
        {
            double X, Y, Z;
            X = 0.0; Y = 0.0; Z = 0.0;
            RGB2XYZ(R, G, B, ref X, ref Y, ref Z);
            XYZ2Lab(X, Y, Z, ref L, ref a, ref b);

        }

        //*****************************************************Lab to RGB***********************************************
        //RGB2Lab : sRGB system, D50 ,reference http://www.brucelindbloom.com/
        private Boolean Lab2RGB(double L, double a, double b, ref double R, ref double G, ref double B)
        {
            double X, Y, Z;

            X = 0.0; Y = 0.0; Z = 0.0;

            Lab2XYZ(L, a, b, ref X, ref Y, ref Z);

            XYZ2RGB(X, Y, Z, ref R, ref G, ref B);

            if (R <= 255.0 && R >= 0.0 && G <= 255.0 && G >= 0.0 && B <= 255.0 && B >= 0.0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Lab2XYZ(double L, double a, double b, ref double X, ref double Y, ref double Z)
        {
            double k = 903.3; double e = 0.008856;
            double fx, fy, fz;
            double xr, yr, zr;
            double Xr = 0.96422; double Yr = 1.00000; double Zr = 0.82521;
            fy = (L + 16.0) / 116.0;
            fx = a / 500.0 + fy;
            fz = fy - b / 200.0;

            if (Math.Pow(fx, 3) > e)
                xr = Math.Pow(fx, 3);
            else
                xr = (fx * 116.0 - 16.0) / k;
            if (L > k * e)
                yr = Math.Pow((L + 16.0) / 116.0, 3);
            else
                yr = L / k;
            if (Math.Pow(fz, 3) > e)
                zr = Math.Pow(fz, 3);
            else
                zr = (fz * 116.0 - 16.0) / k;

            X = xr * Xr;
            Y = yr * Yr;
            Z = zr * Zr;
        }

        private void XYZ2RGB(double X, double Y, double Z, ref double R, ref double G, ref double B)
        {
            double r, g, b;
            r = 3.133856 * X - 1.61687 * Y - 0.49061 * Z;
            g = (-0.97877) * X + 1.916142 * Y + 0.033454 * Z;
            b = 0.071945 * X - 0.22899 * Y + 1.405243 * Z;
            R = (gamma(r) * 255.0);
            G = (gamma(g) * 255.0);
            B = (gamma(b) * 255.0);
        }

        private double gamma(double r)    //sRGB
        {
            double R;
            if (r <= 0.0031308)
                R = 12.92 * r;
            else
                R = Math.Pow(r, 1.0 / 2.4) * 1.055 - 0.055;
            return R;
        }


        //**************************************RGB-Lab*******************************************************************
        private void button_RGB_Lab_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "L";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "a";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "b";

            double[,] Lab = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            // L = 0.0; a = 0.0; b = 0.0;

            ColorRGB nRGB1;
            ColorLab nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nRGB1 = new ColorRGB(double.Parse(str[i, 0]) / 255, double.Parse(str[i, 1]) / 255, double.Parse(str[i, 2]) / 255);
                nLab2 = Converter.ToLab(nRGB1, WhitepointName.D65);
                Lab[i, 0] = nLab2.L;
                Lab[i, 1] = nLab2.a;
                Lab[i, 2] = nLab2.b;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

                //RGB2Lab(R, G, B, ref L, ref a, ref b);
                //Lab[i, 0] = L;
                //Lab[i, 1] = a;
                //Lab[i, 2] = b;
            }

            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Lab;


        }

        //**************************************Lab-RGB*******************************************************************
        private void button_Lab_RGB_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "R";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "G";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "B";

            double[,] RGB = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            //R= 0.0; G = 0.0; B = 0.0;

            ColorRGB nRGB1;
            ColorLab nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nLab2 = new ColorLab(new Whitepoint(WhitepointName.D65), double.Parse(str[i, 0]), double.Parse(str[i, 1]), double.Parse(str[i, 2]));
                //nLab2 = Converter.(nLab2, WhitepointName.D50);
               
                nRGB1 = Converter.ToRGB(nLab2, RGBSpaceName.sRGB);
                RGB[i, 0] = nRGB1.R * 255;
                RGB[i, 1] = nRGB1.G * 255;
                RGB[i, 2] = nRGB1.B * 255;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

            }
            //    for (i = 0; i < rows; i++)
            //{
            //    L = (double.Parse(str[i, 0]));
            //    a = (double.Parse(str[i, 1]));
            //    b = (double.Parse(str[i, 2]));
            //    Lab2RGB(L,a,b, ref R, ref G, ref B);
            //    RGB[i, 0] = R;
            //    RGB[i, 1] = G;
            //    RGB[i, 2] = B;
            //}
            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = RGB;
        }

        private void button_ColorPalette_Click(object sender, RibbonControlEventArgs e)
        {
            Form_ColorPalette ColorPalette = new Form_ColorPalette();
            ColorPalette.Show();
        }

        //****************************************************Spotlight************************************************
        void app_WorkbookBeforeClose(Excel.Workbook Wb, ref bool Cancel)
        {

        }

        void app_WorkbookOpen(Excel.Workbook Wb)
        {

        }

        void app_WorkbookActivate(Excel.Workbook Wb)
        {
            hs = new HookScroll(exWin.HWND_WinWorkbook, this.Draw);
            hs.Hook();

            //hs1 = new HookScroll(exWin.HWND_Desktop, this.Draw);
            //hs1.Hook();

            Excel.Range Target = app.Selection;
            this.Draw();
        }

        void app_WorkbookDeactivate(Excel.Workbook Wb)
        {
            hs.UnHook();
            //hs1.UnHook();
        }

        void app_WindowResize(Excel.Workbook Wb, Excel.Window Wn)
        {
            Excel.Range Target = app.Selection;
            this.Draw();
        }

        void app_SheetActivate(object Sh)
        {
            Excel.Range Target = app.Selection;
            this.Draw();
        }

        void app_SheetChange(object Sh, Excel.Range Target)
        {
            System.Windows.Forms.MessageBox.Show("app_SheetChange");
            this.Draw();
        }

        void app_SheetSelectionChange(object Sh, Excel.Range Target)
        {
            this.Draw();
        }

        private void Draw()
        {

            //////////////////////////////////////
            /// 条件判断
            //////////////////////////////////////
            // Excel.Range Target = app.Selection;
            range Target = Globals.ThisAddIn.Application.ActiveCell;
            //double w = Target.Width;
            //double top = Target.Top;

            if (Target.Areas.Count > 1) return;

            //////////////////////////////////////
            /// 初始值
            //////////////////////////////////////
            Excel.Window win = app.ActiveWindow;
            exWin.Win = win;
            Excel.Pane pan = win.ActivePane;

            //刷屏
            //if (app.Version != "15.0") { app.ScreenUpdating = true; }
            //app.ScreenUpdating = true;

            //////////////////////////////////////
            /// 采用 .net 绘制
            //////////////////////////////////////
            //初始
            System.Drawing.Graphics g = exWin.GS_Excel;

            Rectangle rectWrokbook = exWin.GetClientRect(exWin.HWND_WinWorkbook);     //主区域
            Rectangle rectHScrBar = exWin.GetClientRect(exWin.HWND_HScrBar);    //水平
            Rectangle rectVScrBar = exWin.GetClientRect(exWin.HWND_VScrBar);    //垂直
            Rectangle rectNavBar = exWin.GetClientRect(exWin.HWND_NavBar);     //工作表导航栏

            int x1 = (int)(pan.PointsToScreenPixelsY(Target.Top) - rectWrokbook.Top);
            int x2 = rectVScrBar.Left - rectWrokbook.Left - 1;
            int x3 = (int)(Target.Height * exWin.PixelsPerPointY);

            int y1 = (int)(pan.PointsToScreenPixelsX(Target.Left) - rectWrokbook.Left);
            int y2 = (int)(Target.Width * exWin.PixelsPerPointX);
            int y3 = rectHScrBar.Top - rectWrokbook.Top - 1;

            //绘制X、Y
            // g.DrawRectangles(new Pen(Color.Yellow, 12), new Rectangle[] { new Rectangle(0, x1, x2, x3) });


            g.DrawRectangles(
                 new Pen(System.Drawing.Color.Green, 2),
                 new Rectangle[]
                 {
                    //X方向
                    new Rectangle(0, x1,   x2, x3),
                    //Y方向
                    new Rectangle(y1, 0,y2,y3),
                 });
        }

        private void togButton_Click_1(object sender, RibbonControlEventArgs e)
        {
            exWin = new XlWorkbookHelper();
            app = Globals.ThisAddIn.Application;
            //range Target = Globals.ThisAddIn.Application.ActiveCell;

            //System.Drawing.Graphics g = exWin.GS_Excel;

            //g.DrawRectangles(new Pen(Color.Green, 2), new Rectangle[] { new Rectangle(0, 0,3000,170) });

            //g.FillRectangles(new Brush(), new Rectangle[] { new Rectangle(0, 170, 3000, 170) });

            //g.Dispose();
            //RibbonButtonlmpl button = sender as RibbonButtonlmpl;
            RibbonToggleButton button = sender as RibbonToggleButton;
            if (button.Checked)
            {
                app.SheetSelectionChange += app_SheetSelectionChange;
                app.SheetChange += app_SheetChange;
                app.SheetActivate += app_SheetActivate;
                app.WorkbookActivate += app_WorkbookActivate;
                app.WorkbookDeactivate += app_WorkbookDeactivate;
                app.WindowResize += app_WindowResize;

                app.WorkbookOpen += app_WorkbookOpen;
                app.WorkbookBeforeClose += app_WorkbookBeforeClose;
            }
            else
            {
                app.SheetSelectionChange -= app_SheetSelectionChange;
                app.SheetChange -= app_SheetChange;
                app.SheetActivate -= app_SheetActivate;

                app.WindowResize -= app_WindowResize;
                app.ScreenUpdating = true;

                app.WorkbookActivate -= app_WorkbookActivate;
                app.WorkbookDeactivate -= app_WorkbookDeactivate;
            }
        }

        private void button__RGB_LCHab_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "L";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "C";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "H";

            double[,] Lab = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            // L = 0.0; a = 0.0; b = 0.0;

            ColorRGB nRGB1;
            ColorLCHab nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nRGB1 = new ColorRGB(double.Parse(str[i, 0]) / 255, double.Parse(str[i, 1]) / 255, double.Parse(str[i, 2]) / 255);
                nLab2 = Converter.ToLCHab(nRGB1, WhitepointName.D65);
                Lab[i, 0] = nLab2.L;
                Lab[i, 1] = nLab2.C;
                Lab[i, 2] = nLab2.H;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

                //RGB2Lab(R, G, B, ref L, ref a, ref b);
                //Lab[i, 0] = L;
                //Lab[i, 1] = a;
                //Lab[i, 2] = b;
            }

            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Lab;

        }

        private void button_LCHab_RGB_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "R";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "G";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "B";

            double[,] RGB = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            //R= 0.0; G = 0.0; B = 0.0;

            ColorRGB nRGB1;
            ColorLCHab nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nLab2 = new ColorLCHab(new Whitepoint(WhitepointName.D65), double.Parse(str[i, 0]), double.Parse(str[i, 1]), double.Parse(str[i, 2]));
                //nLab2 = Converter.(nLab2, WhitepointName.D50);

                nRGB1 = Converter.ToRGB(nLab2, RGBSpaceName.sRGB);
                RGB[i, 0] = nRGB1.R * 255;
                RGB[i, 1] = nRGB1.G * 255;
                RGB[i, 2] = nRGB1.B * 255;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

            }
            //    for (i = 0; i < rows; i++)
            //{
            //    L = (double.Parse(str[i, 0]));
            //    a = (double.Parse(str[i, 1]));
            //    b = (double.Parse(str[i, 2]));
            //    Lab2RGB(L,a,b, ref R, ref G, ref B);
            //    RGB[i, 0] = R;
            //    RGB[i, 1] = G;
            //    RGB[i, 2] = B;
            //}
            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = RGB;
        }

        //****************************************************Color Difference Caculation************************************************
        private void button_CMC21_Click(object sender, RibbonControlEventArgs e)
        {

            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color 1";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "Color 2";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "CMC 2:1";

            ColorLab nLab1, nLab2;
            ColorRGB nRGB1, nRGB2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            double[,] result = new double[rows, 1];
            double L, a, b;
            int R, G, B;
            for (int i = 0; i < rows; i++)
            {
                L = (double.Parse(str[i, 0]));
                a = (double.Parse(str[i, 1]));
                b = (double.Parse(str[i, 2]));
                nLab1 = new ColorLab(new Whitepoint(WhitepointName.D65), L, a, b);
                nRGB1 = Converter.ToRGB(nLab1, RGBSpaceName.sRGB);
                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

                L = (double.Parse(str[i, 3]));
                a = (double.Parse(str[i, 4]));
                b = (double.Parse(str[i, 5]));
                nLab2 = new ColorLab(new Whitepoint(WhitepointName.D65), L, a, b);
                nRGB2 = Converter.ToRGB(nLab2, RGBSpaceName.sRGB);
                R = Convert.ToInt32((nRGB2.R * 255));
                G = Convert.ToInt32((nRGB2.G * 255));
                B = Convert.ToInt32((nRGB2.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols + 1]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

                result[i, 0] = CMC.GetDeltaE_CMC(nLab1, nLab2, CMCDifferenceMethod.Acceptability);

                //Given two Lab colors like described in Part III Made to use with D65
            }

            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 2];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 2];
            range range = worksheet.get_Range(c1, c2);
            range.Value = result;

        }

        private void toggleButton_GroupColor_Click(object sender, RibbonControlEventArgs e)
        {
            RibbonToggleButton button = sender as RibbonToggleButton;

            if (button.Checked)
            {
                group_ColorManagement.Visible = false;
            }
            else
            {
                group_ColorManagement.Visible = true;
            }
        }

        private void button_RGB2HSV_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "H";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "S";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "V";

            double[,] Lab = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            // L = 0.0; a = 0.0; b = 0.0;

            ColorRGB nRGB1;
            ColorHSV nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nRGB1 = new ColorRGB(double.Parse(str[i, 0]) / 255, double.Parse(str[i, 1]) / 255, double.Parse(str[i, 2]) / 255);
                nLab2 = Converter.ToHSV(nRGB1);
                //nLab2 = Converter.ToLCHab(nRGB1);
                Lab[i, 0] = nLab2.H;
                Lab[i, 1] = nLab2.S;
                Lab[i, 2] = nLab2.V;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

                //RGB2Lab(R, G, B, ref L, ref a, ref b);
                //Lab[i, 0] = L;
                //Lab[i, 1] = a;
                //Lab[i, 2] = b;
            }

            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = Lab;
        }

        private void button_HSV2RGB_Click(object sender, RibbonControlEventArgs e)
        {
            int rows = 1;
            int cols = 1;
            string[,] str = new string[1, 1];
            RangeData(ref str, ref rows, ref cols);

            worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            range activecells = Globals.ThisAddIn.Application.ActiveCell;
            int start_col = activecells.Column;
            int start_row = activecells.Row;

            ((range)worksheet.Cells[start_row - 1, start_col + cols]).Value2 = "Color";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 1]).Value2 = "R";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 2]).Value2 = "G";
            ((range)worksheet.Cells[start_row - 1, start_col + cols + 3]).Value2 = "B";

            double[,] RGB = new double[rows, cols];
            //int i;
            int R, G, B;
            //double L, a, b;
            //R= 0.0; G = 0.0; B = 0.0;

            ColorRGB nRGB1;
            ColorHSV nLab2;
            ColorDifference CMC = new ColorDifference();
            ColorManagment.ColorConverter Converter = new ColorManagment.ColorConverter();

            for (int i = 0; i < rows; i++)
            {
                //R = Convert.ToInt32((double.Parse(str[i, 0])));
                //G = Convert.ToInt32((double.Parse(str[i, 1])));
                //B = Convert.ToInt32((double.Parse(str[i, 2])));


                nLab2 = new ColorHSV(double.Parse(str[i, 0]), double.Parse(str[i, 1]), double.Parse(str[i, 2]));
                //nLab2 = Converter.(nLab2, WhitepointName.D50);

                nRGB1 = Converter.ToRGB(nLab2, RGBSpaceName.sRGB);
                RGB[i, 0] = nRGB1.R * 255;
                RGB[i, 1] = nRGB1.G * 255;
                RGB[i, 2] = nRGB1.B * 255;

                R = Convert.ToInt32((nRGB1.R * 255));
                G = Convert.ToInt32((nRGB1.G * 255));
                B = Convert.ToInt32((nRGB1.B * 255));
                ((range)worksheet.Cells[start_row + i, start_col + cols]).Interior.Color = System.Drawing.Color.FromArgb(255, R, G, B);

            }
            //    for (i = 0; i < rows; i++)
            //{
            //    L = (double.Parse(str[i, 0]));
            //    a = (double.Parse(str[i, 1]));
            //    b = (double.Parse(str[i, 2]));
            //    Lab2RGB(L,a,b, ref R, ref G, ref B);
            //    RGB[i, 0] = R;
            //    RGB[i, 1] = G;
            //    RGB[i, 2] = B;
            //}
            range c1 = (range)worksheet.Cells[start_row, start_col + cols + 1];
            range c2 = (range)worksheet.Cells[start_row + rows - 1, start_col + cols + 3];
            range range = worksheet.get_Range(c1, c2);
            range.Value = RGB;

        }
    }
}
