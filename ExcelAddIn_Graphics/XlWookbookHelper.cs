using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddIn_Graphics
{
    public partial class XlWorkbookHelper
    {
        /////////////////////////////////////////////////////////
        /// 私变区
        /////////////////////////////////////////////////////////
        //应用程序
        private Excel.Application app;
        private Excel.Window win;
        public Excel.Window Win
        {
            get { return win; }
            set { win = value; }
        }

        /////////////////////////////////////////////////////////
        /// 创建区
        /////////////////////////////////////////////////////////
        public XlWorkbookHelper()
        {
            app = Globals.ThisAddIn.Application;
            win = app.ActiveWindow;
            //pan = win.ActivePane;
            //cell = win.ActiveCell;
        }

        /////////////////////////////////////////////////////////
        /// 字段区
        /////////////////////////////////////////////////////////
        //【窗口句柄】
        public IntPtr HWND_Application
        {
            get
            {
                return new IntPtr(app.Hwnd);
            }
        }

        public IntPtr HWND_Desktop
        {
            get
            {
                string path = "XLDESK";
                return XlWorkbookHelper.GetHandle(HWND_Application, path);
            }
        }


        public IntPtr HWND_WinWorkbook
        {
            get
            {
                string path = "XLDESK/EXCEL7";
                return XlWorkbookHelper.GetHandle(HWND_Application, path);
            }
        }

        public IntPtr HWND_HScrBar
        {
            get
            {
                return XlWorkbookHelper.GetHandle(HWND_WinWorkbook, "NUIScrollbar", "水平");
            }
        }

        public IntPtr HWND_VScrBar
        {
            get
            {
                return XlWorkbookHelper.GetHandle(HWND_WinWorkbook, "NUIScrollbar", "垂直");
            }
        }

        public IntPtr HWND_NavBar
        {
            get
            {
                return XlWorkbookHelper.GetHandle(HWND_WinWorkbook, "XLCTL", null);
            }
        }



        //【屏幕参数】
        //沿屏幕宽度每逻辑英寸的像素数
        private float PixelsPerInchX
        {
            get
            {
                return GetDeviceCaps(GetWindowDC(HWND_DESKTOP), LOGPIXELSX);
            }
        }

        //沿屏幕高度每逻辑英寸的像素数
        private float PixelsPerInchY
        {
            get
            {
                return GetDeviceCaps(GetWindowDC(HWND_DESKTOP), LOGPIXELSY);
            }
        }

        //X方向第磅像素值
        public double PixelsPerPointX
        {
            get
            {
                return PixelsPerInchX / PoundsPerInch * (win.Zoom / 100);
            }
        }

        //Y方向第磅像素值
        public double PixelsPerPointY
        {
            get
            {
                return PixelsPerInchY / PoundsPerInch * (win.Zoom / 100);
            }
        }



        //屏幕
        /////////////////////////////////////////////////////////
        /// 方法区
        /////////////////////////////////////////////////////////
        public static IntPtr GetHandle(IntPtr handle, string path)
        {
            IntPtr retHandel = handle;
            string[] nodes = path.Split('/');
            foreach (var item in nodes)
            {
                retHandel = FindWindowEx(retHandel, IntPtr.Zero, item, null);
                if (retHandel == IntPtr.Zero) return retHandel;
            }

            return retHandel;
        }

        public static IntPtr GetHandle(IntPtr handle, string className, string title)
        {
            return FindWindowEx(handle, IntPtr.Zero, className, title);
        }

        public Rectangle GetWindowRect(IntPtr handle)
        {
            RECT winRect;
            //GetClientRect(handle, out winRect);
            GetWindowRect(handle, out winRect);

            Rectangle rect = new Rectangle(
                winRect.Left, 
                winRect.Top, 
                winRect.Right - winRect.Left, 
                winRect.Bottom - winRect.Top);

            return rect;
        }

        public Rectangle GetClientRect(IntPtr handle)
        {
            RECT winRect; GetClientRect(handle, out winRect);
            POINT winPoint; ClientToScreen(handle, out winPoint);

            Rectangle rect = new Rectangle(winPoint.Left, winPoint.Top, winRect.Right, winRect.Bottom);
            return rect;
        }

        //绘图区
        private Pen pen;
        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }

        public int GetColor(IntPtr handle, int x, int y)
        {
            IntPtr dc = GetDC(handle);
            int color = (int)GetPixel(dc, x, y);

            return color;
        }

        public void DrawRectangles()
        {
            //////////////////////////////////////
            /// 采用 .net 绘制
            //////////////////////////////////////
            //初始
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(HWND_WinWorkbook);
            Rectangle rectExcel = GetClientRect(HWND_WinWorkbook);     //主区域

            //参考线
            int left = 40;
            int top = 40;
            Pen pen = new Pen(Color.Red, 4);

            g.DrawRectangles(pen,
                new Rectangle[] 
                {
                    //大框
                    new Rectangle
                        (
                            0,
                            0,
                            rectExcel.Width - 1,
                            rectExcel.Height - 1
                        ),
                    //参考线
                    new Rectangle
                        (
                            0 + left,
                            0 + top,
                            rectExcel.Width - left*2 - 1,
                            rectExcel.Height - top*2 - 1
                        ),
                    //参考线
                    new Rectangle
                        (
                            0 + left * 2,
                            0 + top * 2,
                            rectExcel.Width - left*4 - 1,
                            rectExcel.Height - top*4 - 1
                        ),
                });
            //End of DrawRectangles
        }

        //获得所有句柄
        public static List<IntPtr> GetAllChildWindows(IntPtr handle)
        {
            List<IntPtr> hList = new List<IntPtr>();
            hList.Add(handle);

            //enum all child windows   
            EnumChildWindows
            (
                handle,
                delegate(IntPtr hWnd, int lParam)
                {
                    hList.Add(hWnd);
                    return true;
                },
                0
            );

            return hList;
        }

        //测试用
        public void DrawTest()
        {
            //////////////////////////////////////
            /// 采用 .net 绘制
            //////////////////////////////////////
            //初始
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(HWND_WinWorkbook);
            Rectangle rectExcel = GetClientRect(HWND_WinWorkbook);     //主区域

            Pen pen = new Pen(Color.Red, 20);
            g.DrawRectangles(pen,
                new Rectangle[] 
                {
                    //大框
                    new Rectangle
                        (
                            60,
                            60,
                            rectExcel.Width - 60 * 2 - 1,
                            rectExcel.Height - 60 * 2 - 1
                        ),
                });
        }

        public int ColorTest()
        {
            return GetColor(HWND_WinWorkbook, 60+0, 60+0);
        }

        //public Graphics GS_Excel
        //{
        //    get
        //    {
        //        IntPtr handle = GetDC(HWND_WinWorkbook);

        //        return Graphics.FromHdc(handle);
        //    }
        //}
    }
}
