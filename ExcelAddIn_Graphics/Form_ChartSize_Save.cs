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
    public partial class Form_ChartSize : Form
    {
        public Microsoft.Office.Tools.Excel.Worksheet worksheet;
        public Excel.Chart chart;
        public Form_ChartSize()
        {
            InitializeComponent();
        }

        private void Form_ChartSize_Load(object sender, EventArgs e)
        {
            try
            {
                worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
                chart = Globals.ThisAddIn.Application.ActiveChart;
                textBox_ChartHeight.Text = Convert.ToString(Math.Ceiling(chart.ChartArea.Height));
                textBox_ChartWidth.Text = Convert.ToString(Math.Ceiling(chart.ChartArea.Width));
                textBox_PlotAreaHeight.Text = Convert.ToString(Math.Ceiling(chart.PlotArea.Height));
                textBox_PlotAreaWidth.Text = Convert.ToString(Math.Ceiling(chart.PlotArea.Width));
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
   
        }

        //private void textBox_ChartHeight_TextChanged(object sender, EventArgs e)
        //{
        //    if (textBox_ChartHeight.Text == "") return;
        //    chart.ChartArea.Height = double.Parse(textBox_ChartHeight.Text); ;
        //    //if (ratio == 0) return;
        //}

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (textBox_ChartHeight.Text == "") return;
            chart.ChartArea.Height = double.Parse(textBox_ChartHeight.Text);

            if (textBox_ChartWidth.Text == "") return;
            chart.ChartArea.Width = double.Parse(textBox_ChartWidth.Text);

            if (textBox_PlotAreaHeight.Text == "") return;
            chart.PlotArea.Height = double.Parse(textBox_PlotAreaHeight.Text);

            if (textBox_PlotAreaWidth.Text == "") return;
            chart.PlotArea.Width = double.Parse(textBox_PlotAreaWidth.Text);

            //this.Close();
            if (checkBox_AllChart.ThreeState==true)
            {


            }
        }

    }
}
