using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pdf2image.Ghostscript
{
    public partial class Form_PDFtoPicture : Form
    {
        public Form_PDFtoPicture()
        {
            InitializeComponent();
        }

       
        /// 
        private void button1_Click(object sender, EventArgs e)
        {

            Program convertor = new Program();
            convertor.ConvertPDF2Image("C:\\Users\\Peter_Zhang\\Desktop\\JD20180204220346.bmp.pdf", "C:\\Users\\Peter_Zhang\\Desktop\\", "test", 1, 3, System.Drawing.Imaging.ImageFormat.Jpeg, Program.Definition.Three);

        }
    }
}
