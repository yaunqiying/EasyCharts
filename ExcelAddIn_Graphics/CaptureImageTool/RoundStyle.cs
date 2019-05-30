using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin_JD.CaptureImage
{
    /* 作者：Starts_2000
     *      （涂剑凯修改 http://www.cnblogs.com/bdstjk/）
     * 日期：2009-07-31
     * 网站：http://www.csharpwin.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
     */

    /// <summary>
    /// 建立圆角路径的样式。
    /// </summary>
    public enum RoundStyle
    {
        /// <summary>
        /// 四个角都不是圆角。
        /// </summary>
        None = 0,
        /// <summary>
        /// 四个角都为圆角。
        /// </summary>
        All = 1,
        /// <summary>
        /// 左边两个角为圆角。
        /// </summary>
        Left = 2,
        /// <summary>
        /// 右边两个角为圆角。
        /// </summary>
        Right = 3,
        /// <summary>
        /// 上边两个角为圆角。
        /// </summary>
        Top = 4,
        /// <summary>
        /// 下边两个角为圆角。
        /// </summary>
        Bottom = 5
    }
}
