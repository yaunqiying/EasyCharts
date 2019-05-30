using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSharpWin_JD.CaptureImage;

namespace MyTest
{
    public partial class Form_Camera : Form
    {
        public Form_Camera()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CaptureImageTool capture = new CaptureImageTool();
            //capture.SelectCursor = new Cursor(Properties.Resources.Arrow_M.Handle); 
            if (capture.ShowDialog() == DialogResult.OK)
            {
                Image image = capture.Image;
                pictureBox1.Width = image.Width;
                pictureBox1.Height = image.Height;
                pictureBox1.Image = image;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.Alt|HotKey.KeyModifiers.Ctrl, Keys.S);

        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            HotKey.UnregisterHotKey(Handle, 102);
        }

        /// 
        /// 监视Windows消息
        /// 重载WndProc方法，用于实现热键响应
        /// 
        /// 
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;//如果m.Msg的值为0x0312那么表示用户按下了热键
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Shift+S
                            //此处填写快捷键响应代码         
                            break;
                        case 101:    //按下的是Ctrl+B
                            //此处填写快捷键响应代码
                            break;
                        case 102:    //按下的是Ctrl+Alt+S
                            CaptureImageTool capture = new CaptureImageTool();

                            if (capture.ShowDialog() == DialogResult.OK)
                            {
                                Image image = capture.Image;
                                pictureBox1.Width = image.Width;
                                pictureBox1.Height = image.Height;
                                pictureBox1.Image = image;
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }


    }
}
