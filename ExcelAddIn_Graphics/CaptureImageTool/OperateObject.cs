using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSharpWin_JD.CaptureImage
{
    /* 作者：Starts_2000
     *      （涂剑凯修改 http://www.cnblogs.com/bdstjk/）
     * 日期：2009-09-08
     * 网站：http://www.csharpwin.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
     */

    internal class OperateObject
    {
        private OperateType _operateType;
        private Color _color;
        private object _data;

        public OperateObject() { }

        public OperateObject(
            OperateType operateType, Color color, object data)
        {
            _operateType = operateType;
            _color = color;
            _data = data;
        }

        public OperateType OperateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
