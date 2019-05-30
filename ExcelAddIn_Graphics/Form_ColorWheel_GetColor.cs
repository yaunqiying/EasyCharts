using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelAddIn_Graphics
{
    public partial class Form_ColorWheel_GetColor : Form
    {
        public Form_ColorWheel_GetColor()
        {
            InitializeComponent();
        }

        private void Form_ColorWheel_GetColor_Load(object sender, EventArgs e)
        {
            MaximizedBounds = Screen.PrimaryScreen.Bounds;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;

            Opacity = 0.01;
        }

        private void Form_ColorWheel_GetColor_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private void Form_ColorWheel_GetColor_MouseMove(object sender, MouseEventArgs e)
        //{
        //    //Form_ColorWheel Mymainform = (Form_ColorWheel)this.Tag;
        //    //if (e.X > Mymainform.Left && e.X < Mymainform.Left + Mymainform.Width && e.Y > Mymainform.Top && e.Y < Mymainform.Top + Mymainform.Width)
        //    //{
        //    //    Mymainform.Location = Mymainform.Location == Mymainform.formLeft ? Mymainform.formRight : Mymainform.formLeft;
        //    //}
        //}
    }
}
