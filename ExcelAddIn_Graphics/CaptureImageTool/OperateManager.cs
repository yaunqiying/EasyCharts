using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
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

    internal class OperateManager : IDisposable
    {
        private List<OperateObject> _operateList;

        private static readonly int MaxOperateCount = 1000;

        public OperateManager()
        {
        }

        public List<OperateObject> OperateList
        {
            get
            {
                if (_operateList == null)
                {
                    _operateList = new List<OperateObject>(100);
                }
                return _operateList;
            }
        }

        public int OperateCount
        {
            get { return OperateList.Count; }
        }

        public void AddOperate(
            OperateType operateType, 
            Color color,
            object data)
        {
            OperateObject obj = new OperateObject(
                operateType, color, data);
            if (OperateList.Count > MaxOperateCount)
            {
                OperateList.RemoveAt(0);
            }
            OperateList.Add(obj);
        }

        public bool RedoOperate()
        {
            if (OperateList.Count > 0)
            {
                OperateList.RemoveAt(OperateList.Count - 1);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            OperateList.Clear();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (_operateList != null)
            {
                _operateList.Clear();
                _operateList = null;
            }
        }

        #endregion
    }
}
