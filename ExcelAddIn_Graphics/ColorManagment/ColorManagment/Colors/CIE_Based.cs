
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

namespace ColorManagment
{
    /// <summary>
    /// CIE XYZ (CIE 1931 2°) Color
    /// </summary>
    public sealed class ColorXYZ : Color
    {
        /// <summary>
        /// X-value: 0.0 to 1.0
        /// </summary>
        public double X
        {
            get { return (ColorValues[0] > Xmax) ? Xmax : ((ColorValues[0] < Xmin) ? Xmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Y-value: 0.0 to 1.0
        /// </summary>
        public double Y
        {
            get { return (ColorValues[1] > Ymax) ? Ymax : ((ColorValues[1] < Ymin) ? Ymin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Z-value: 0.0 to 1.0
        /// </summary>
        public double Z
        {
            get { return (ColorValues[2] > Zmax) ? Zmax : ((ColorValues[2] < Zmin) ? Zmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// Minimum value for X
        /// </summary>
        public const double Xmin = 0;
        /// <summary>
        /// Minimum value for Y
        /// </summary>
        public const double Ymin = 0;
        /// <summary>
        /// Minimum value for Z
        /// </summary>
        public const double Zmin = 0;
        /// <summary>
        /// Maximum value for x
        /// </summary>
        public const double Xmax = 1;
        /// <summary>
        /// Maximum value for Y
        /// </summary>
        public const double Ymax = 1;
        /// <summary>
        /// Maximum value for Z
        /// </summary>
        public const double Zmax = 1;

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
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        public ColorXYZ()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="X">X-value (0.0 - 1.0)</param>
        /// <param name="Y">Y-value (0.0 - 1.0)</param>
        /// <param name="Z">Z-value (0.0 - 1.0)</param>
        public ColorXYZ(double X, double Y, double Z)
            : this(ColorConverter.ReferenceWhite, X, Y, Z)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorXYZ(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE XYZ Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="X">X-value (0.0 - 1.0)</param>
        /// <param name="Y">Y-value (0.0 - 1.0)</param>
        /// <param name="Z">Z-value (0.0 - 1.0)</param>
        public ColorXYZ(Whitepoint ReferenceWhite, double X, double Y, double Z)
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
    public sealed class ColorYxy : Color
    {
        /// <summary>
        /// Y-value: 0.0 to 1.0
        /// </summary>
        public double Y
        {
            get { return (ColorValues[0] > Ymax) ? Ymax : ((ColorValues[0] < Ymin) ? Ymin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// x-value
        /// </summary>
        public double x
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// y-value
        /// </summary>
        public double y
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for Y
        /// </summary>
        public const double Ymin = 0;
        /// <summary>
        /// Minimum value for x
        /// </summary>
        public const double xMin = double.MinValue;
        /// <summary>
        /// Minimum value for y
        /// </summary>
        public const double yMin = double.MinValue;
        /// <summary>
        /// Maximum value for Y
        /// </summary>
        public const double Ymax = 1;
        /// <summary>
        /// Maximum value for x
        /// </summary>
        public const double xMax = double.MaxValue;
        /// <summary>
        /// Maximum value for y
        /// </summary>
        public const double yMax = double.MaxValue;

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
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        public ColorYxy()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="Y">Y-value (0.0 - 1.0)</param>
        /// <param name="x">x-value</param>
        /// <param name="y">y-value</param>
        public ColorYxy(double Y, double x, double y)
            : this(ColorConverter.ReferenceWhite, Y, x, y)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorYxy(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE Yxy Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="Y">Y-value (0.0 - 1.0)</param>
        /// <param name="x">x-value</param>
        /// <param name="y">y-value</param>
        public ColorYxy(Whitepoint ReferenceWhite, double Y, double x, double y)
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
    public sealed class ColorLab : Color
    {
        /// <summary>
        /// Lightness: 0.0 to 100.0
        /// </summary>
        public double L
        {
            get { return (ColorValues[0] > Lmax) ? Lmax : ((ColorValues[0] < Lmin) ? Lmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// a-axis
        /// </summary>
        public double a
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// b-axis
        /// </summary>
        public double b
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for L
        /// </summary>
        public const double Lmin = 0;
        /// <summary>
        /// Minimum value for a
        /// </summary>
        public const double aMin = double.MinValue;
        /// <summary>
        /// Minimum value for b
        /// </summary>
        public const double bMin = double.MinValue;
        /// <summary>
        /// Maximum value for L
        /// </summary>
        public const double Lmax = 100;
        /// <summary>
        /// Maximum value for a
        /// </summary>
        public const double aMax = double.MaxValue;
        /// <summary>
        /// Maximum value for b
        /// </summary>
        public const double bMax = double.MaxValue;

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
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        public ColorLab()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="a">a-value (green/magenta)</param>
        /// <param name="b">b-value (blue/yellow)</param>
        public ColorLab(double L, double a, double b)
            : this(ColorConverter.ReferenceWhite, L, a, b)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorLab(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*a*b* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="a">a-value (green/magenta)</param>
        /// <param name="b">b-value (blue/yellow)</param>
        public ColorLab(Whitepoint ReferenceWhite, double L, double a, double b)
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
    public sealed class ColorLuv : Color
    {
        /// <summary>
        /// Lightness: 0 to 100
        /// </summary>
        public double L
        {
            get { return (ColorValues[0] > Lmax) ? Lmax : ((ColorValues[0] < Lmin) ? Lmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// u-axis
        /// </summary>
        public double u
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// v-axis
        /// </summary>
        public double v
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for L
        /// </summary>
        public const double Lmin = 0;
        /// <summary>
        /// Minimum value for u
        /// </summary>
        public const double uMin = double.MinValue;
        /// <summary>
        /// Minimum value for v
        /// </summary>
        public const double vMin = double.MinValue;
        /// <summary>
        /// Maximum value for L
        /// </summary>
        public const double Lmax = 100;
        /// <summary>
        /// Maximum value for u
        /// </summary>
        public const double uMax = double.MaxValue;
        /// <summary>
        /// Maximum value for v
        /// </summary>
        public const double vMax = double.MaxValue;

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
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        public ColorLuv()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="u">u-value (red/green)</param>
        /// <param name="v">v-value (blue/yellow)</param>
        public ColorLuv(double L, double u, double v)
            : this(ColorConverter.ReferenceWhite, L, u, v)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorLuv(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE L*u*v* Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="u">u-value (red/green)</param>
        /// <param name="v">v-value (blue/yellow)</param>
        public ColorLuv(Whitepoint ReferenceWhite, double L, double u, double v)
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
