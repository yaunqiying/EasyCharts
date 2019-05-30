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

namespace ColorManagment
{
    /// <summary>
    /// LCH Color
    /// </summary>
    public abstract class ColorLCH : Color
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
        /// Chroma
        /// </summary>
        public double C
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Hue Angel: 0 to 360
        /// </summary>
        public double H
        {
            get { return ((ColorValues[2] % Hmax) + Hmax) % Hmax; }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// Minimum value for L
        /// </summary>
        public const double Lmin = 0;
        /// <summary>
        /// Minimum value for C
        /// </summary>
        public const double Cmin = double.MinValue;
        /// <summary>
        /// Minimum value for H
        /// </summary>
        public const double Hmin = 0;
        /// <summary>
        /// Maximum value for L
        /// </summary>
        public const double Lmax = 100;
        /// <summary>
        /// Maximum value for C
        /// </summary>
        public const double Cmax = double.MaxValue;
        /// <summary>
        /// Maximum value for H
        /// </summary>
        public const double Hmax = 360;

        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        public ColorLCH()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH(double L, double C, double H)
            : this(ColorConverter.ReferenceWhite, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorLCH(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH(Whitepoint ReferenceWhite, double L, double C, double H)
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
    public sealed class ColorLCHab : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHab; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        public ColorLCHab()
            : base(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCHab(double L, double C, double H)
            : base(ColorConverter.ReferenceWhite, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorLCHab(Whitepoint ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHab Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCHab(Whitepoint ReferenceWhite, double L, double C, double H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// CIE LCH Color based on a CIE L*u*v* Color
    /// </summary>
    public sealed class ColorLCHuv : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.CIELCHuv; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        public ColorLCHuv()
            : base(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCHuv(double L, double C, double H)
            : base(ColorConverter.ReferenceWhite, L, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorLCHuv(Whitepoint ReferenceWhite)
            : base(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CIE LCHuv Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCHuv(Whitepoint ReferenceWhite, double L, double C, double H)
            : base(ReferenceWhite, L, C, H)
        { }

        #endregion
    }


    /// <summary>
    /// LCH Color based on the DIN 99 formula
    /// </summary>
    public sealed class ColorLCH99 : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        public ColorLCH99()
            : base(new Whitepoint(WhitepointName.D65), 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99 Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH99(double L, double C, double H)
            : base(new Whitepoint(WhitepointName.D65), L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99b formula
    /// </summary>
    public sealed class ColorLCH99b : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99b; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        public ColorLCH99b()
            : base(new Whitepoint(WhitepointName.D65), 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99b Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH99b(double L, double C, double H)
            : base(new Whitepoint(WhitepointName.D65), L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99c formula
    /// </summary>
    public sealed class ColorLCH99c : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99c; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        public ColorLCH99c()
            : base(new Whitepoint(WhitepointName.D65), 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99c Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH99c(double L, double C, double H)
            : base(new Whitepoint(WhitepointName.D65), L, C, H)
        { }

        #endregion
    }

    /// <summary>
    /// LCH Color based on the DIN 99d formula
    /// </summary>
    public sealed class ColorLCH99d : ColorLCH
    {
        public override ColorModel Model { get { return ColorModel.LCH99d; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        public ColorLCH99d()
            : base(new Whitepoint(WhitepointName.D65), 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a LCH99d Color
        /// </summary>
        /// <param name="L">Lightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorLCH99d(double L, double C, double H)
            : base(new Whitepoint(WhitepointName.D65), L, C, H)
        { }

        #endregion
    }
}
