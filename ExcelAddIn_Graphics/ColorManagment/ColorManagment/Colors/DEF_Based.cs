
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
    /// DEF Color
    /// </summary>
    public sealed class ColorDEF : Color
    {
        /// <summary>
        /// D-value
        /// </summary>
        public double D
        {
            get { return (ColorValues[0] > Dmax) ? Dmax : ((ColorValues[0] < Dmin) ? Dmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// E-value
        /// </summary>
        public double E
        {
            get { return (ColorValues[1] > Emax) ? Emax : ((ColorValues[1] < Emin) ? Emin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// F-value
        /// </summary>
        public double F
        {
            get { return (ColorValues[2] > Fmax) ? Fmax : ((ColorValues[2] < Fmin) ? Fmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for D
        /// </summary>
        public const double Dmin = 0;
        /// <summary>
        /// Minimum value for E
        /// </summary>
        public const double Emin = double.MinValue;
        /// <summary>
        /// Minimum value for F
        /// </summary>
        public const double Fmin = double.MinValue;
        /// <summary>
        /// Maximum value for D
        /// </summary>
        public const double Dmax = double.MaxValue;
        /// <summary>
        /// Maximum value for E
        /// </summary>
        public const double Emax = double.MaxValue;
        /// <summary>
        /// Maximum value for F
        /// </summary>
        public const double Fmax = double.MaxValue;

        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.DEF; } }
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
        /// Creates a new instance of a DEF Color
        /// </summary>
        public ColorDEF()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a DEF Color
        /// </summary>
        /// <param name="D">D-value</param>
        /// <param name="E">E-value</param>
        /// <param name="F">F-value</param>
        public ColorDEF(double D, double E, double F)
            : this(ColorConverter.ReferenceWhite, D, E, F)
        { }

        /// <summary>
        /// Creates a new instance of a DEF Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorDEF(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a DEF Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="D">D-value</param>
        /// <param name="E">E-value</param>
        /// <param name="F">F-value</param>
        public ColorDEF(Whitepoint ReferenceWhite, double D, double E, double F)
            : base()
        {
            this.wp = ReferenceWhite;
            this.D = D;
            this.E = E;
            this.F = F;
        }

        #endregion
    }
    
    /// <summary>
    /// Bef Color
    /// </summary>
    public sealed class ColorBef : Color
    {
        /// <summary>
        /// B-value
        /// </summary>
        public double B
        {
            get { return (ColorValues[0] > Bmax) ? Bmax : ((ColorValues[0] < Bmin) ? Bmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// e-value
        /// </summary>
        public double e
        {
            get { return (ColorValues[1] > eMax) ? eMax : ((ColorValues[1] < eMin) ? eMin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// f-value
        /// </summary>
        public double f
        {
            get { return (ColorValues[2] > fMax) ? fMax : ((ColorValues[2] < fMin) ? fMin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// Minimum value for B
        /// </summary>
        public const double Bmin = 0;
        /// <summary>
        /// Minimum value for e
        /// </summary>
        public const double eMin = double.MinValue;
        /// <summary>
        /// Minimum value for f
        /// </summary>
        public const double fMin = double.MinValue;
        /// <summary>
        /// Maximum value for B
        /// </summary>
        public const double Bmax = double.MaxValue;
        /// <summary>
        /// Maximum value for e
        /// </summary>
        public const double eMax = double.MaxValue;
        /// <summary>
        /// Maximum value for f
        /// </summary>
        public const double fMax = double.MaxValue;

        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.Bef; } }
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
        /// Creates a new instance of a Bef Color
        /// </summary>
        public ColorBef()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a Bef Color
        /// </summary>
        /// <param name="B">B-value</param>
        /// <param name="e">e-value</param>
        /// <param name="f">f-value</param>
        public ColorBef(double B, double e, double f)
            : this(ColorConverter.ReferenceWhite, B, e, f)
        { }

        /// <summary>
        /// Creates a new instance of a Bef Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorBef(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a Bef Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="B">B-value</param>
        /// <param name="e">e-value</param>
        /// <param name="f">f-value</param>
        public ColorBef(Whitepoint ReferenceWhite, double B, double e, double f)
            : base()
        {
            this.wp = ReferenceWhite;
            this.B = B;
            this.e = e;
            this.f = f;
        }

        #endregion
    }

    /// <summary>
    /// BCH Color
    /// </summary>
    public sealed class ColorBCH : Color
    {
        /// <summary>
        /// Brightness: 0 to 100
        /// </summary>
        public double B
        {
            get { return (ColorValues[0] > Bmax) ? Bmax : ((ColorValues[0] < Bmin) ? Bmin : ColorValues[0]); }
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
        /// Minimum value for B
        /// </summary>
        public const double Bmin = 0;
        /// <summary>
        /// Minimum value for C
        /// </summary>
        public const double Cmin = double.MinValue;
        /// <summary>
        /// Minimum value for H
        /// </summary>
        public const double Hmin = 0;
        /// <summary>
        /// Maximum value for B
        /// </summary>
        public const double Bmax = 100;
        /// <summary>
        /// Maximum value for C
        /// </summary>
        public const double Cmax = double.MaxValue;
        /// <summary>
        /// Maximum value for H
        /// </summary>
        public const double Hmax = 360;
        
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.BCH; } }
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
        /// Creates a new instance of a BCH Color
        /// </summary>
        public ColorBCH()
            : this(ColorConverter.ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a BCH Color
        /// </summary>
        /// <param name="B">Brightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorBCH(double B, double C, double H)
            : this(ColorConverter.ReferenceWhite, B, C, H)
        { }

        /// <summary>
        /// Creates a new instance of a BCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorBCH(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a BCH Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        /// <param name="B">Brightness (0 - 100)</param>
        /// <param name="C">Chroma</param>
        /// <param name="H">Hue (0 - 360)</param>
        public ColorBCH(Whitepoint ReferenceWhite, double B, double C, double H)
            : base()
        {
            this.wp = ReferenceWhite;
            this.B = B;
            this.C = C;
            this.H = H;
        }

        #endregion
    }
}
