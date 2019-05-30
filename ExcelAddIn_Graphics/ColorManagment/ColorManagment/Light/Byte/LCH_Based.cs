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
    public abstract class BColorLCH : BColor
    {
        /// <summary>
        /// Lightness: 0 to 255
        /// </summary>
        public byte L
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Chroma: 0 to 255
        /// </summary>
        public byte C
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Hue Angel: 0 to 255
        /// </summary>
        public byte H
        {
            get { return (byte)(((ColorValues[2] % byte.MaxValue) + byte.MaxValue) % byte.MaxValue); }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 2.55d, ColorValues[1] / 255d, ColorValues[2] * 1.4117647058823529411764705882353 }; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        public BColorLCH()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH(byte L, byte C, byte H)
            : this(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorLCH(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH(WhitepointName ReferenceWhite, byte L, byte C, byte H)
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
    public sealed class BColorLCHab : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHab; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        public BColorLCHab()
            : base(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCHab(byte L, byte C, byte H)
            : base(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorLCHab(WhitepointName ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCHab(WhitepointName ReferenceWhite, byte L, byte C, byte H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// CIE LCH Color based on a CIE L*u*v* Color
    /// </summary>
    public sealed class BColorLCHuv : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHuv; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        public BColorLCHuv()
            : base(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCHuv(byte L, byte C, byte H)
            : base(ColorConverter.ReferenceWhite.Name, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorLCHuv(WhitepointName ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCHuv(WhitepointName ReferenceWhite, byte L, byte C, byte H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }


    /// <summary>
    /// LCH Color based on the DIN 99 formula
    /// </summary>
    public sealed class BColorLCH99 : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        public BColorLCH99()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH99(byte L, byte C, byte H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99b formula
    /// </summary>
    public sealed class BColorLCH99b : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99b; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        public BColorLCH99b()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH99b(byte L, byte C, byte H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99c formula
    /// </summary>
    public sealed class BColorLCH99c : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99c; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        public BColorLCH99c()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH99c(byte L, byte C, byte H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99d formula
    /// </summary>
    public sealed class BColorLCH99d : BColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99d; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        public BColorLCH99d()
            : base(WhitepointName.D65, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="C">Chroma (0 - 255)</param>
        /// <param name="H">Hue (0 - 255)</param>
        public BColorLCH99d(byte L, byte C, byte H)
            : base(WhitepointName.D65, L, C, H)
        { }

        #endregion
    }
}
