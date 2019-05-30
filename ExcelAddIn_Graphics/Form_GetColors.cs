using System;
using System.Windows.Forms;

namespace ExcelAddIn_Graphics
{
    public partial class Form_GetColors : Form
    {
        public Form_GetColors()
        {
            InitializeComponent();
        }

        private void Form_GetColors_Load(object sender, EventArgs e)
        {
            MaximizedBounds = Screen.PrimaryScreen.Bounds;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;

            Opacity = 0.01;
        }

        private void Form_GetColors_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_GetColors_MouseMove(object sender, MouseEventArgs e)
        {
            Form_ColorPixel Mymainform = (Form_ColorPixel)Tag;
            if (e.X > Mymainform.Left && e.X < Mymainform.Left + Mymainform.Width && e.Y > Mymainform.Top && e.Y < Mymainform.Top + Mymainform.Width)
            {
                Mymainform.Location = Mymainform.Location == Mymainform.formLeft ? Mymainform.formRight : Mymainform.formLeft;
            }
        }
    }
}
