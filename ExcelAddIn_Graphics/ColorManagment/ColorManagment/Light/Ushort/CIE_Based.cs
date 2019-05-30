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
    public sealed class UColorXYZ : UColor
    {
        /// <summary>
        /// X-value: 0 to 65535
        /// </summary>
        public ushort X
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Y-value: 0 to 65535
        /// </summary>
        public ushort Y
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Z-value: 0 to 65535
        /// </summary>
        public ushort Z
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 65535d, ColorValues[1] / 65535d, ColorValues[2] / 65535d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        public UColorXYZ()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="X">X-value (0 - 65535)</param>
        /// <param name="Y">Y-value (0 - 65535)</param>
        /// <param name="Z">Z-value (0 - 65535)</param>
        public UColorXYZ(ushort X, ushort Y, ushort Z)
            : this(ColorConverter.ReferenceWhite.Name, X, Y, Z)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorXYZ(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="X">X-value (0 - 65535)</param>
        /// <param name="Y">Y-value (0 - 65535)</param>
        /// <param name="Z">Z-value (0 - 65535)</param>
        public UColorXYZ(WhitepointName ReferenceWhite, ushort X, ushort Y, ushort Z)
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
    public sealed class UColorYxy : UColor
    {
        /// <summary>
        /// Y-value: 0 to 65535
        /// </summary>
        public ushort Y
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// x-value: -32768 to 32767
        /// </summary>
        public short x
        {
            get { return (short)(ColorValues[1] - 32768); }
            set { ColorValues[1] = (ushort)(value + 32768); }
        }
        /// <summary>
        /// y-value: -32768 to 32767
        /// </summary>
        public short y
        {
            get { return (short)(ColorValues[2] - 32768); }
            set { ColorValues[2] = (ushort)(value + 32768); }
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 65535d, ColorValues[1] - 32768d, ColorValues[2] - 32768d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        public UColorYxy()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="Y">Y-value (0 - 65535)</param>
        /// <param name="x">x-value (-32768 - 32767)</param>
        /// <param name="y">y-value (-32768 - 32767)</param>
        public UColorYxy(ushort Y, short x, short y)
            : this(ColorConverter.ReferenceWhite.Name, Y, x, y)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorYxy(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="Y">Y-value (0 - 65535)</param>
        /// <param name="x">x-value (-32768 - 32767)</param>
        /// <param name="y">y-value (-32768 - 32767)</param>
        public UColorYxy(WhitepointName ReferenceWhite, ushort Y, short x, short y)
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
    public sealed class UColorLab : UColor
    {
        /// <summary>
        /// Lightness: 0.0 to 65535.0
        /// </summary>
        public ushort L
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// a-axis: -32768 to 32767
        /// </summary>
        public short a
        {
            get { return (short)(ColorValues[1] - 32768); }
            set { ColorValues[1] = (ushort)(value + 32768); }
        }
        /// <summary>
        /// b-axis: -32768 to 32767
        /// </summary>
        public short b
        {
            get { return (short)(ColorValues[2] - 32768); }
            set { ColorValues[2] = (ushort)(value + 32768); }
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 655.35d, ColorValues[1] - 32768d, ColorValues[2] - 32768d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        public UColorLab()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="a">a-value (green/magenta) (-32768 - 32767)</param>
        /// <param name="b">b-value (blue/yellow) (-32768 - 32767)</param>
        public UColorLab(ushort L, short a, short b)
            : this(ColorConverter.ReferenceWhite.Name, L, a, b)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorLab(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="a">a-value (green/magenta) (-32768 - 32767)</param>
        /// <param name="b">b-value (blue/yellow) (-32768 - 32767)</param>
        public UColorLab(WhitepointName ReferenceWhite, ushort L, short a, short b)
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
    public sealed class UColorLuv : UColor
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
        /// u-axis: -32768 to 32767
        /// </summary>
        public short u
        {
            get { return (short)(ColorValues[1] - 32768); }
            set { ColorValues[1] = (ushort)(value + 32768); }
        }
        /// <summary>
        /// v-axis: -32768 to 32767
        /// </summary>
        public short v
        {
            get { return (short)(ColorValues[1] - 32768); }
            set { ColorValues[1] = (ushort)(value + 32768); }
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 655.35d, ColorValues[1] - 32768d, ColorValues[2] - 32768d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        public UColorLuv()
            : this(ColorConverter.ReferenceWhite.Name, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="u">u-value (red/green) (-32768 - 32767)</param>
        /// <param name="v">v-value (blue/yellow) (-32768 - 32767)</param>
        public UColorLuv(ushort L, short u, short v)
            : this(ColorConverter.ReferenceWhite.Name, L, u, v)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public UColorLuv(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        /// <param name="u">u-value (red/green) (-32768 - 32767)</param>
        /// <param name="v">v-value (blue/yellow) (-32768 - 32767)</param>
        public UColorLuv(WhitepointName ReferenceWhite, ushort L, short u, short v)
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
