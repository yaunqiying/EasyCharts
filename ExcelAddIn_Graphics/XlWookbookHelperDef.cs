using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcelAddIn_Graphics
{
    public partial class XlWorkbookHelper
    {
        //http://blog.csdn.net/ke_yang/article/details/5417552

        //屏幕参数
        //CreateGraphics().DpiX;
        private readonly int LOGPIXELSX = 88;       //沿屏幕宽度每逻辑英寸的像素数，在多显示器系统中，该值对所显示器相同
        private readonly int LOGPIXELSY = 90;       //沿屏幕高度每逻辑英寸的像素数，在多显示器系统中，该值对所显示器相同

        //GetSystemMetrics要用到的，获取窗口的边框大小，X表示横轴方向，Y表示纵轴方向
        private readonly int SM_CXBORDER = 5;
        private readonly int SM_CYBORDER = 6;
        private readonly int SM_CXDLGFRAME = 7;
        private readonly int SM_CYDLGFRAME = 8;
        private readonly int SM_CXFRAME = 32;
        private readonly int SM_CYFRAME = 33;
        private readonly int SM_CXSIZEFRAME = 32;
        private readonly int SM_CYSIZEFRAME = 33;

        //
        private readonly double PoundsPerInch = 72;    //1 Inch = 72 Pound//

        //
        private IntPtr HWND_DESKTOP = IntPtr.Zero;  //桌面句柄参数
        private IntPtr HWND_SCREEN = IntPtr.Zero;   //屏幕句柄参数

        /////////////////////////////////////////////////////////
        /// API区
        /////////////////////////////////////////////////////////
        //搜索窗口
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx
        (
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow
        );

        //回调函数
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public delegate bool EnumChildProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(int hwndParent, EnumChildProc EnumFunc, IntPtr lParam);

        bool EnumCP(IntPtr hwnd, IntPtr lParam)
        {

            System.Text.StringBuilder sbClassName = new StringBuilder(255);

            GetClassName(hwnd, sbClassName, 255);

            if ("Static" == sbClassName.ToString())
            {

                return false;

            }

            return true;

        }

        //获取笔刷
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        //获取设备句柄
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        //颜色
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        //获取设备参数
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDC, int index);


        //获取尺寸
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        //获取坐标
        public struct POINT
        {
            public int Left;
            public int Top;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, out POINT pt);

        //得到被定义的系统数据或者系统配置信息
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int smIndex);

        //获得所有子窗口
        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, int lParam);
    }
}
