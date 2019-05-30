using System;
using System.Collections.Generic;
using System.Text;
using CSharpWin_JD.CaptureImage;
using System.Drawing;

namespace CaptureImageToolDemo
{
    /* 作者：Starts_2000
     * 日期：2009-09-08
     * 网站：http://www.csharpwin.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
     */

    public class ProfessionalCaptureImageToolColorTable : 
        CaptureImageToolColorTable
    {
        private static readonly Color _borderColor = Color.FromArgb(106, 255, 34);
        private static readonly Color _backColorNormal = Color.FromArgb(221, 255, 205);
        private static readonly Color _backColorHover = Color.FromArgb(106, 255, 34);
        private static readonly Color _backColorPressed = Color.FromArgb(74, 226, 0);
        private static readonly Color _foreColor = Color.FromArgb(41, 126, 0);

        public ProfessionalCaptureImageToolColorTable() { }

        public override Color BorderColor
        {
            get { return _borderColor; }
        }

        public override Color BackColorNormal
        {
            get { return _backColorNormal; }
        }

        public override Color BackColorHover
        {
            get { return _backColorHover; }
        }

        public override Color BackColorPressed
        {
            get { return _backColorPressed; }
        }

        public override Color ForeColor
        {
            get { return _foreColor; }
        }
    }
}
