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
    /// CIE XYZ (CIE 1931 2°) Color
    /// </summary>
    public sealed class BColorXYZ : BColor
    {
        /// <summary>
        /// X-value: 0 to 255
        /// </summary>
        public byte X
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Y-value: 0 to 255
        /// </summary>
        public byte Y
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Z-value: 0 to 255
        /// </summary>
        public byte Z
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CIEXYZ; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 255d, ColorValues[1] / 255d, ColorValues[2] / 255d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        public BColorXYZ()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="X">X-value (0 - 255)</param>
        /// <param name="Y">Y-value (0 - 255)</param>
        /// <param name="Z">Z-value (0 - 255)</param>
        public BColorXYZ(byte X, byte Y, byte Z)
            : this(ColorConverter.ReferenceWhite.Name, X, Y, Z)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorXYZ(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="X">X-value (0 - 255)</param>
        /// <param name="Y">Y-value (0 - 255)</param>
        /// <param name="Z">Z-value (0 - 255)</param>
        public BColorXYZ(WhitepointName ReferenceWhite, byte X, byte Y, byte Z)
            : base()
        {
            this.wp = ReferenceWhite;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        #endregion
    }

    /// <summary>
    /// CIE Yxy (CIE 1931 2°) Color
    /// </summary>
    public sealed class BColorYxy : BColor
    {
        /// <summary>
        /// Y-value: 0 to 255
        /// </summary>
        public byte Y
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// x-value: -128 to 127
        /// </summary>
        public sbyte x
        {
            get { return (sbyte)(ColorValues[1] - 128); }
            set { ColorValues[1] = (byte)(value + 128); }
        }
        /// <summary>
        /// y-value: -128 to 127
        /// </summary>
        public sbyte y
        {
            get { return (sbyte)(ColorValues[2] - 128); }
            set { ColorValues[2] = (byte)(value + 128); }
        }
        
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CIEYxy; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 255d, ColorValues[1] - 128d, ColorValues[2] - 128d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        public BColorYxy()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="Y">Y-value (0 - 255)</param>
        /// <param name="x">x-value (-128 - 127)</param>
        /// <param name="y">y-value (-128 - 127)</param>
        public BColorYxy(byte Y, sbyte x, sbyte y)
            : this(ColorConverter.ReferenceWhite.Name, Y, x, y)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorYxy(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="Y">Y-value (0 - 255)</param>
        /// <param name="x">x-value (-128 - 127)</param>
        /// <param name="y">y-value (-128 - 127)</param>
        public BColorYxy(WhitepointName ReferenceWhite, byte Y, sbyte x, sbyte y)
            : base()
        {
            this.wp = ReferenceWhite;
            this.Y = Y;
            this.x = x;
            this.y = y;
        }

        #endregion
    }

    /// <summary>
    /// CIE L*a*b* (CIE 1976) Color
    /// </summary>
    public sealed class BColorLab : BColor
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
        /// a-axis: -128 to 127
        /// </summary>
        public sbyte a
        {
            get { return (sbyte)(ColorValues[1] - 128); }
            set { ColorValues[1] = (byte)(value + 128); }
        }
        /// <summary>
        /// b-axis: -128 to 127
        /// </summary>
        public sbyte b
        {
            get { return (sbyte)(ColorValues[2] - 128); }
            set { ColorValues[2] = (byte)(value + 128); }
        }
        
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CIELab; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 2.55d, ColorValues[1] - 128d, ColorValues[2] - 128d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        public BColorLab()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="a">a-value (green/magenta) (-128 - 127)</param>
        /// <param name="b">b-value (blue/yellow) (-128 - 127)</param>
        public BColorLab(byte L, sbyte a, sbyte b)
            : this(ColorConverter.ReferenceWhite.Name, L, a, b)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorLab(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="a">a-value (green/magenta) (-128 - 127)</param>
        /// <param name="b">b-value (blue/yellow) (-128 - 127)</param>
        public BColorLab(WhitepointName ReferenceWhite, byte L, sbyte a, sbyte b)
            : base()
        {
            this.wp = ReferenceWhite;
            this.L = L;
            this.a = a;
            this.b = b;
        }

        #endregion
    }

    /// <summary>
    /// CIE L*u*v* (CIE 1976) Color
    /// </summary>
    public sealed class BColorLuv : BColor
    {
        /// <summary>
        /// Lightness: 0 to 100
        /// </summary>
        public byte L
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// u-axis: -128 to 127
        /// </summary>
        public sbyte u
        {
            get { return (sbyte)(ColorValues[1] - 128); }
            set { ColorValues[1] = (byte)(value + 128); }
        }
        /// <summary>
        /// v-axis: -128 to 127
        /// </summary>
        public sbyte v
        {
            get { return (sbyte)(ColorValues[2] - 128); }
            set { ColorValues[2] = (byte)(value + 128); }
        }
        
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CIELuv; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 2.55d, ColorValues[1] - 128d, ColorValues[2] - 128d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        public BColorLuv()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="u">u-value (red/green) (-128 - 127)</param>
        /// <param name="v">v-value (blue/yellow) (-128 - 127)</param>
        public BColorLuv(byte L, sbyte u, sbyte v)
            : this(ColorConverter.ReferenceWhite.Name, L, u, v)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorLuv(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 255)</param>
        /// <param name="u">u-value (red/green) (-128 - 127)</param>
        /// <param name="v">v-value (blue/yellow) (-128 - 127)</param>
        public BColorLuv(WhitepointName ReferenceWhite, byte L, sbyte u, sbyte v)
            : base()
        {
            this.wp = ReferenceWhite;
            this.L = L;
            this.u = u;
            this.v = v;
        }

        #endregion
    }
}
