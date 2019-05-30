using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ExcelAddIn_Graphics
{
    public partial class XlWorkbookHelper
    {
        //【屏幕参数】
        public int CXFRAME
        {
            get
            {
                //获得系统窗口X方向边框大小
                return GetSystemMetrics(SM_CXFRAME);
            }
        }

        public int CYFRAME
        {
            get
            {
                //获得系统窗口Y方向边框大小
                return GetSystemMetrics(SM_CYFRAME);
            }
        }

        //【图形接口】
        public System.Drawing.Graphics GS_Screen
        {
            get
            {
                IntPtr handle = GetDC(HWND_SCREEN);

                return System.Drawing.Graphics.FromHdc(handle);
            }
        }

        public System.Drawing.Graphics GS_Excel
        {
            get
            {
                IntPtr handle = GetDC(HWND_WinWorkbook);

                return System.Drawing.Graphics.FromHdc(handle);
            }
        }
    }
}
