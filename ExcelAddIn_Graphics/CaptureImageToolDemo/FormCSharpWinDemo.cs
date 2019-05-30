using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CSharpWin_JD.CaptureImage;
using CSharpWin;

namespace CaptureImageToolDemo
{
    public partial class FormCSharpWinDemo : Form
    {
        #region Fileds

        private SystemMenuNativeWindow _systemMenuNativeWindow;
        private ProfessionalCaptureImageToolColorTable _colorTable = 
            new ProfessionalCaptureImageToolColorTable();

        #endregion

        #region Constructors

        public FormCSharpWinDemo()
        {
            InitializeComponent();
            InitEvents();
        }

        #endregion

        #region Properties

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = string.Format(
                    "CS 程序员之窗 - {0}", value);
            }
        }

        #endregion

        #region Override Methods

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (_systemMenuNativeWindow == null)
            {
                _systemMenuNativeWindow = new SystemMenuNativeWindow(this);
            }

            _systemMenuNativeWindow.AppendSeparator();
            _systemMenuNativeWindow.AppendMenu(
                1001,
                "访问 www.csharpwin.com",
                delegate(object sender, EventArgs e)
                {
                    Process.Start("www.csharpwin.com");
                });

            _systemMenuNativeWindow.AppendMenu(
                1000,
                "关于...(&A)",
                delegate(object sender, EventArgs e)
                {
                    AboutBoxCSharpWinDemo about = new AboutBoxCSharpWinDemo();
                    about.ShowDialog();
                });
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (_systemMenuNativeWindow != null)
            {
                _systemMenuNativeWindow.Dispose();
                _systemMenuNativeWindow = null;
            }
        }

        #endregion

        #region Private Methods

        private void InitEvents()
        {
            linkLabelCSharpWin.Click += delegate(object sender, EventArgs e)
            {
                Process.Start("www.csharpwin.com");
            };

            buttonAbout.Click += delegate(object sender, EventArgs e)
            {
                AboutBoxCSharpWinDemo about = new AboutBoxCSharpWinDemo();
                about.ShowDialog();
            };

            buttonCaptureImage.Click += delegate(object sender, EventArgs e)
            {
                if (checkBoxHide.Checked)
                {
                    Hide();
                    System.Threading.Thread.Sleep(30);
                }
                CaptureImageTool capture = new CaptureImageTool();
                if (checkBoxCursor.Checked)
                {
                    capture.SelectCursor = CursorManager.ArrowNew;
                    capture.DrawCursor = CursorManager.CrossNew;
                }
                else
                {
                    capture.SelectCursor = CursorManager.Arrow;
                    capture.DrawCursor = CursorManager.Cross;
                }
                if (checkBoxColorTable.Checked)
                {
                    capture.ColorTable = _colorTable;
                }

                if (capture.ShowDialog() == DialogResult.OK)
                {
                    Image image = capture.Image;
                    pictureBox.Width = image.Width;
                    pictureBox.Height = image.Height;
                    pictureBox.Image = image;
                }

                if (!Visible)
                {
                    Show();
                }
            };
        }

        private void ButtonAboutClick(object sender, EventArgs e)
        {
            AboutBoxCSharpWinDemo f = new AboutBoxCSharpWinDemo();
            f.ShowDialog();
        }

        #endregion

    }
}