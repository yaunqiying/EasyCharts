
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
    /// RGB Color
    /// </summary>
    public sealed class ColorRGB : Color
    {
        /// <summary>
        /// Red: 0.0 to 1.0
        /// </summary>
        public double R
        {
            get { return (ColorValues[0] > Rmax) ? Rmax : ((ColorValues[0] < Rmin) ? Rmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Green: 0.0 to 1.0
        /// </summary>
        public double G
        {
            get { return (ColorValues[01] > Gmax) ? Gmax : ((ColorValues[1] < Gmin) ? Gmin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Blue: 0.0 to 1.0
        /// </summary>
        public double B
        {
            get { return (ColorValues[2] > Bmax) ? Bmax : ((ColorValues[2] < Bmin) ? Bmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for R
        /// </summary>
        public const double Rmin = 0;
        /// <summary>
        /// Minimum value for G
        /// </summary>
        public const double Gmin = 0;
        /// <summary>
        /// Minimum value for B
        /// </summary>
        public const double Bmin = 0;
        /// <summary>
        /// Maximum value for R
        /// </summary>
        public const double Rmax = 1;
        /// <summary>
        /// Maximum value for G
        /// </summary>
        public const double Gmax = 1;
        /// <summary>
        /// Maximum value for B
        /// </summary>
        public const double Bmax = 1;

        /// <summary>
        /// The colormodel of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.RGB; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// The colorspace of this color
        /// </summary>
        public RGBSpaceName SpaceName { get { return (Space == null) ? RGBSpaceName.ICC : Space.Name; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// States if the gamma value is calulated in (non linear) or not (linear)
        /// </summary>
        public bool IsLinear { get; private set; }

        internal RGBColorspace Space;

        #region Constructor

        #region Blank

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        public ColorRGB()
            : this(ColorConverter.StandardColorspace, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(bool IsLinear)
            : this(ColorConverter.StandardColorspace, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        public ColorRGB(ushort R, ushort G, ushort B)
            : this(ColorConverter.StandardColorspace, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(ushort R, ushort G, ushort B, bool IsLinear)
            : this(ColorConverter.StandardColorspace, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        public ColorRGB(byte R, byte G, byte B)
            : this(ColorConverter.StandardColorspace, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(byte R, byte G, byte B, bool IsLinear)
            : this(ColorConverter.StandardColorspace, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        public ColorRGB(double R, double G, double B)
            : this(ColorConverter.StandardColorspace, R, G, B, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(double R, double G, double B, bool IsLinear)
            : this(ColorConverter.StandardColorspace, R, G, B, IsLinear)
        { }

        #endregion

        #region Colorspace

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorRGB(RGBSpaceName Space)
            : this(Space, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(RGBSpaceName Space, bool IsLinear)
            : this(Space, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        public ColorRGB(RGBSpaceName Space, ushort R, ushort G, ushort B)
            : this(Space, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(RGBSpaceName Space, ushort R, ushort G, ushort B, bool IsLinear)
            : this(Space, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        public ColorRGB(RGBSpaceName Space, byte R, byte G, byte B)
            : this(Space, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(RGBSpaceName Space, byte R, byte G, byte B, bool IsLinear)
            : this(Space, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        public ColorRGB(RGBSpaceName Space, double R, double G, double B)
            : this(Space, R, G, B, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(RGBSpaceName Space, double R, double G, double B, bool IsLinear)
            : base()
        {
            this.Space = RGBColorspace.GetColorspace(Space);
            wp = this.Space.ReferenceWhite;
            this.R = R;
            this.G = G;
            this.B = B;
            this.IsLinear = IsLinear;
        }

        #endregion

        #region ICC

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        public ColorRGB(ICC ICCprofile)
            : this(ICCprofile, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(ICC ICCprofile, bool IsLinear)
            : this(ICCprofile, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        public ColorRGB(ICC ICCprofile, byte R, byte G, byte B)
            : this(ICCprofile, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(ICC ICCprofile, byte R, byte G, byte B, bool IsLinear)
            : this(ICCprofile, R / (double)byte.MaxValue, G / (double)byte.MaxValue, B / (double)byte.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        public ColorRGB(ICC ICCprofile, ushort R, ushort G, ushort B)
            : this(ICCprofile, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(ICC ICCprofile, ushort R, ushort G, ushort B, bool IsLinear)
            : this(ICCprofile, R / (double)ushort.MaxValue, G / (double)ushort.MaxValue, B / (double)ushort.MaxValue, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        public ColorRGB(ICC ICCprofile, double R, double G, double B)
            : this(ICCprofile, R, G, B, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="R">The red value (0.0 - 1.0)</param>
        /// <param name="G">The green value (0.0 - 1.0)</param>
        /// <param name="B">The blue value (0.0 - 1.0)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public ColorRGB(ICC ICCprofile, double R, double G, double B, bool IsLinear)
            : base(ICCprofile)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.IsLinear = IsLinear;
        }

        #endregion

        #endregion

        /// <summary>
        /// Converts the color to a non linear color.
        /// </summary>
        /// <returns>A non-linear RGB color</returns>
        public ColorRGB ToNonLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (IsLinear) { return new ColorRGB(SpaceName, Space.ToNonLinear(R), Space.ToNonLinear(G), Space.ToNonLinear(B), false); }
                else { return this; }
            }
            else { return this; }
        }

        /// <summary>
        /// Converts the color to a linear color.
        /// </summary>
        /// <returns>A linear RGB color</returns>
        public ColorRGB ToLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (!IsLinear) { return new ColorRGB(SpaceName, Space.ToLinear(R), Space.ToLinear(G), Space.ToLinear(B), true); }
                else { return this; }
            }
            else { return this; }
        }
    }

    /// <summary>
    /// Hue-Saturation based Color
    /// </summary>
    public abstract class ColorHSx : Color
    {
        /// <summary>
        /// Hue: 0.0 to 360.0
        /// </summary>
        public double H
        {
            get { return ((ColorValues[0] % Hmax) + Hmax) % Hmax; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Saturation: 0.0 to 1.0
        /// </summary>
        public double S
        {
            get { return (ColorValues[1] > Smax) ? Smax : ((ColorValues[1] < Smin) ? Smin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        
        /// <summary>
        /// Minimum value for H
        /// </summary>
        public const double Hmin = 0;
        /// <summary>
        /// Minimum value for S
        /// </summary>
        public const double Smin = 0;
        /// <summary>
        /// Maximum value for H
        /// </summary>
        public const double Hmax = 360;
        /// <summary>
        /// Maximum value for S
        /// </summary>
        public const double Smax = 1;

        /// <summary>
        /// The colorspace of this color
        /// </summary>
        public RGBSpaceName SpaceName { get { return (Space == null) ? RGBSpaceName.ICC : Space.Name; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        internal RGBColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        public ColorHSx()
            : this(ColorConverter.StandardColorspace, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorHSx(RGBSpaceName Space)
            : this(Space, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        public ColorHSx(RGBSpaceName Space, double H, double S)
            : base()
        {
            this.Space = RGBColorspace.GetColorspace(Space);
            wp = this.Space.ReferenceWhite;
            this.H = H;
            this.S = S;
        }


        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        public ColorHSx(ICC ICCprofile)
            : this(ICCprofile, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        public ColorHSx(ICC ICCprofile, double H, double S)
            : base(ICCprofile)
        {
            this.H = H;
            this.S = S;
        }

        #endregion
    }

    /// <summary>
    /// HSV Color
    /// </summary>
    public sealed class ColorHSV : ColorHSx
    {
        /// <summary>
        /// Value: 0.0 to 1.0
        /// </summary>
        public double V
        {
            get { return (ColorValues[2] > Vmax) ? Vmax : ((ColorValues[2] < Vmin) ? Vmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for V
        /// </summary>
        public const double Vmin = 0;
        /// <summary>
        /// Maximum value for V
        /// </summary>
        public const double Vmax = 1;

        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override ColorModel Model { get { return ColorModel.HSV; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        public ColorHSV()
            : base()
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorHSV(RGBSpaceName Space)
            : base(Space)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="V">Value (0.0 - 1.0)</param>
        public ColorHSV(double H, double S, double V)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="V">Value (0.0 - 1.0)</param>
        public ColorHSV(RGBSpaceName Space, double H, double S, double V)
            : base(Space, H, S)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        public ColorHSV(ICC ICCprofile)
            : base(ICCprofile)
        { }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="V">Value (0.0 - 1.0)</param>
        public ColorHSV(ICC ICCprofile, double H, double S, double V)
            : base(ICCprofile, H, S)
        {
            this.V = V;
        }

        #endregion

    }

    /// <summary>
    /// HSL Color
    /// </summary>
    public sealed class ColorHSL : ColorHSx
    {
        /// <summary>
        /// Lightness: 0.0 to 1.0
        /// </summary>
        public double L
        {
            get { return (ColorValues[2] > Lmax) ? Lmax : ((ColorValues[2] < Lmin) ? Lmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for L
        /// </summary>
        public const double Lmin = 0;
        /// <summary>
        /// Maximum value for L
        /// </summary>
        public const double Lmax = 1;

        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override ColorModel Model { get { return ColorModel.HSL; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        public ColorHSL()
            : base()
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorHSL(RGBSpaceName Space)
            : base(Space)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="L">Lightness (0.0 - 1.0)</param>
        public ColorHSL(double H, double S, double L)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="L">Lightness (0.0 - 1.0)</param>
        public ColorHSL(RGBSpaceName Space, double H, double S, double L)
            : base(Space, H, S)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        public ColorHSL(ICC ICCprofile)
            : base(ICCprofile)
        { }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="ICCprofile">The ICC profile for this color</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0.0 - 1.0)</param>
        /// <param name="L">Lightness (0.0 - 1.0)</param>
        public ColorHSL(ICC ICCprofile, double H, double S, double L)
            : base(ICCprofile, H, S)
        {
            this.L = L;
        }

        #endregion

    }

    /// <summary>
    /// YCbCr Color
    /// </summary>
    public sealed class ColorYCbCr : Color
    {
        /// <summary>
        /// Luma: 0.0 to 1.0
        /// </summary>
        public double Y
        {
            get { return (ColorValues[0] > Ymax) ? Ymax : ((ColorValues[0] < Ymin) ? Ymin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Blue-Yellow Chrominance: 0.0 to 1.0
        /// </summary>
        public double Cb
        {
            get { return (ColorValues[1] > Cbmax) ? Cbmax : ((ColorValues[1] < Cbmin) ? Cbmin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Red-Green Chrominance: 0.0 to 1.0
        /// </summary>
        public double Cr
        {
            get { return (ColorValues[2] > Crmax) ? Crmax : ((ColorValues[2] < Crmin) ? Crmin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }

        /// <summary>
        /// Minimum value for Y
        /// </summary>
        public const double Ymin = 0;
        /// <summary>
        /// Minimum value for Cb
        /// </summary>
        public const double Cbmin = 0;
        /// <summary>
        /// Minimum value for Cr
        /// </summary>
        public const double Crmin = 0;
        /// <summary>
        /// Maximum value for Y
        /// </summary>
        public const double Ymax = 1;
        /// <summary>
        /// Maximum value for Cb
        /// </summary>
        public const double Cbmax = 1;
        /// <summary>
        /// Maximum value for Cr
        /// </summary>
        public const double Crmax = 1;

        /// <summary>
        /// The colormodel of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.YCbCr; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// The colorspace this color derived from
        /// </summary>
        public RGBSpaceName BaseSpaceName { get { return (Space == null) ? RGBSpaceName.ICC : BaseSpace.Name; } }
        /// <summary>
        /// The colorspace of this color
        /// </summary>
        public YCbCrSpaceName SpaceName { get { return (Space == null) ? YCbCrSpaceName.ICC : Space.Name; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        internal RGBColorspace BaseSpace;
        internal YCbCrColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        public ColorYCbCr()
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public ColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace)
            : this(Space, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorYCbCr(YCbCrSpaceName Space)
            : this(Space, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public ColorYCbCr(RGBSpaceName BaseSpace)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0.0 - 1.0)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0.0 - 1.0)</param>
        public ColorYCbCr(double Y, double Cb, double Cr)
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0.0 - 1.0)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Space">The colorspace this color is in</param>
        public ColorYCbCr(YCbCrSpaceName Space, double Y, double Cb, double Cr)
            : this(Space, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0.0 - 1.0)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0.0 - 1.0)</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public ColorYCbCr(RGBSpaceName BaseSpace, double Y, double Cb, double Cr)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0.0 - 1.0)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public ColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace, double Y, double Cb, double Cr)
            : base()
        {
            this.Y = Y;
            this.Cb = Cb;
            this.Cr = Cr;
            this.Space = YCbCrColorspace.GetColorspace(Space);
            this.BaseSpace = RGBColorspace.GetColorspace(BaseSpace);
            wp = this.Space.ReferenceWhite;
        }


        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        public ColorYCbCr(ICC profile)
            : base(profile)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0.0 - 1.0)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0.0 - 1.0)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0.0 - 1.0)</param>
        /// <param name="profile">The ICC profile for this color</param>
        public ColorYCbCr(ICC profile, double Y, double Cb, double Cr)
            : base(profile)
        {
            this.Y = Y;
            this.Cb = Cb;
            this.Cr = Cr;
        }

        #endregion
    }
}