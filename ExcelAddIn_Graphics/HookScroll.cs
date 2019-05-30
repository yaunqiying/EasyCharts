using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcelAddIn_Graphics
{
    public partial class HookScroll
    {
        const int WM_LBUTTONDOWN = 0x201;
        const int WM_LBUTTONDBLCLK = 0x203;
        const int WM_RBUTTONDOWN = 0x204;
        const int WM_RBUTTONDBLCLK = 0x206;

        const int WM_NCHITTEST = 0x84;                  //发送到窗口，确定窗口的部分对应于特定屏幕坐标
        const int VK_LBUTTON = 0x01;                    //指鼠标左键
        const int WM_MOUSEMOVE = 0x200;                 //
        const int WM_LBUTTONUP = 0x202;                 //
        const int WM_RBUTTONUP = 0x205;                 //



        const int GWL_WNDPROC = -4;                     //为窗口过程设定一个新的地址
        private IntPtr HTCAPTION = new IntPtr(0x2);     //在标题区
        private IntPtr HTCLIENT = new IntPtr(0x1);      //在客户区

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);

        [DllImport("User32.dll")]
        protected static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32", EntryPoint = "CallWindowProc")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint uMSG, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);

        public delegate IntPtr WndProcHandler(IntPtr hWnd, uint msg, int wParam, int lParam);
        public delegate void DrawHandler();

        /////////////////////////////////////////////////////////////
        /// 创建
        /////////////////////////////////////////////////////////////
        private IntPtr hWnd;

        public IntPtr lpPrevWndProc;
        public DrawHandler draw;

        public HookScroll(IntPtr hWnd,DrawHandler draw)
        {
            this.hWnd = hWnd;
            this.draw = draw;
        }

        public void Hook()
        {
            lpPrevWndProc = SetWindowLong(hWnd, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(new WndProcHandler(WindowProc)));
        }

       public void UnHook()
        {
            SetWindowLong(hWnd, GWL_WNDPROC, lpPrevWndProc);
        }

       const int WM_HSCROLL = 0x0114;
       const int WM_VSCROLL = 0x0115;
       const int WM_MOUSEWHEEL = 0x20A; 

        public IntPtr WindowProc(IntPtr hWnd, uint uMsg, int wParam, int lParam)
        {
            switch ((int)uMsg)
            {
                case WM_HSCROLL:
                case WM_VSCROLL:
                    //System.Windows.Forms.MessageBox.Show("WM_HSCROLL");
                    //draw();
                    break;

                case  WM_MOUSEWHEEL:
                    //System.Windows.Forms.MessageBox.Show("Test");
                    draw();
                    break;

                default:
                    break;
            }

            //DoEvents
            //System.Threading.Thread.Sleep(10);
            return CallWindowProc(lpPrevWndProc, hWnd, uMsg, wParam, lParam);
        }

    }
}
