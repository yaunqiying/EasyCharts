using System;

/*  This library handles colormodels and spaces and the conversion between those.
    Copyright (C) 2013  Johannes Bildstein

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

namespace ColorManagment.Light
{
    /// <summary>
    /// LCH Color
    /// </summary>
    public abstract class UColorLCH : UColor
    {
        /// <summary>
        /// Lightness: 0 to 65535
        /// </summary>
        public ushort L
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Chroma
        /// </summary>
        public ushort C
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Hue Angel: 0 to 65535
        /// </summary>
        public ushort H
        {
            get { return (ushort)(((ColorValues[2] % ushort.MaxValue) + ushort.MaxValue) % ushort.MaxValue); }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 655.35d, ColorValues[1] / 65535d, ColorValues[2] * 0.00549324788281071183337148088808 }; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        public UColorLCH()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH(ushort L, ushort C, ushort H)
            : this(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorLCH(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH(WhitepointName ReferenceWhite, ushort L, ushort C, ushort H)
            : base()
        {
            this.wp = ReferenceWhite;
            this.L = L;
            this.C = C;
            this.H = H;
        }

        #endregion
    }


    /// <summary>
    /// CIE LCH Color based on a CIE L*a*b* Color
    /// </summary>
    public sealed class UColorLCHab : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHab; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        public UColorLCHab()
            : base(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCHab(ushort L, ushort C, ushort H)
            : base(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorLCHab(WhitepointName ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCHab(WhitepointName ReferenceWhite, ushort L, ushort C, ushort H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// CIE LCH Color based on a CIE L*u*v* Color
    /// </summary>
    public sealed class UColorLCHuv : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHuv; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        public UColorLCHuv()
            : base(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCHuv(ushort L, ushort C, ushort H)
            : base(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorLCHuv(WhitepointName ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCHuv(WhitepointName ReferenceWhite, ushort L, ushort C, ushort H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }


    /// <summary>
    /// LCH Color based on the DIN 99 formula
    /// </summary>
    public sealed class UColorLCH99 : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        public UColorLCH99()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH99(ushort L, ushort C, ushort H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99b formula
    /// </summary>
    public sealed class UColorLCH99b : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99b; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        public UColorLCH99b()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH99b(ushort L, ushort C, ushort H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99c formula
    /// </summary>
    public sealed class UColorLCH99c : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99c; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        public UColorLCH99c()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH99c(ushort L, ushort C, ushort H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99d formula
    /// </summary>
    public sealed class UColorLCH99d : UColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99d; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        public UColorLCH99d()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public UColorLCH99d(ushort L, ushort C, ushort H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }
}
