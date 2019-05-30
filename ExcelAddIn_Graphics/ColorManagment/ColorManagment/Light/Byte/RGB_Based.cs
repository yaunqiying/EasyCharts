
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
    /// RGB Color
    /// </summary>
    public sealed class BColorRGB : BColor
    {
        /// <summary>
        /// Red: 0 to 255
        /// </summary>
        public byte R
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Green:  0 to 255
        /// </summary>
        public byte G
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Blue:  0 to 255
        /// </summary>
        public byte B
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }

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
        public RGBSpaceName SpaceName { get { return Space.Name; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 255d, ColorValues[1] / 255d, ColorValues[2] / 255d }; } }
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
        public BColorRGB()
            : this(ColorConverter.StandardColorspace, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public BColorRGB(bool IsLinear)
            : this(ColorConverter.StandardColorspace, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        public BColorRGB(byte R, byte G, byte B)
            : this(ColorConverter.StandardColorspace, R, G, B, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public BColorRGB(byte R, byte G, byte B, bool IsLinear)
            : this(ColorConverter.StandardColorspace, R, G, B, IsLinear)
        { }
        
        #endregion

        #region Colorspace

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorRGB(RGBSpaceName Space)
            : this(Space, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public BColorRGB(RGBSpaceName Space, bool IsLinear)
            : this(Space, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        public BColorRGB(RGBSpaceName Space, byte R, byte G, byte B)
            : this(Space, R, G, B, false)
        { }
        
        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 255)</param>
        /// <param name="G">The green value (0 - 255)</param>
        /// <param name="B">The blue value (0 - 255)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public BColorRGB(RGBSpaceName Space, byte R, byte G, byte B, bool IsLinear)
            : base()
        {
            this.Space = RGBColorspace.GetColorspace(Space);
            wp = this.Space.ReferenceWhite.Name;
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
        public BColorRGB ToNonLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (IsLinear) { return new BColorRGB(SpaceName, (byte)(Space.ToNonLinear(R) * 255), (byte)(Space.ToNonLinear(G) * 255), (byte)(Space.ToNonLinear(B) * 255), false); }
                else { return this; }
            }
            else { return this; }
        }

        /// <summary>
        /// Converts the color to a linear color.
        /// </summary>
        /// <returns>A linear RGB color</returns>
        public BColorRGB ToLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (!IsLinear) { return new BColorRGB(SpaceName, (byte)(Space.ToLinear(R) * 255), (byte)(Space.ToLinear(G) * 255), (byte)(Space.ToLinear(B) * 255), true); }
                else { return this; }
            }
            else { return this; }
        }
    }

    /// <summary>
    /// Hue-Saturation based Color
    /// </summary>
    public abstract class BColorHSx : BColor
    {
        /// <summary>
        /// Hue: 0 to 255
        /// </summary>
        public byte H
        {
            get { return (byte)(((ColorValues[0] % byte.MaxValue) + byte.MaxValue) % byte.MaxValue); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Saturation: 0 to 255
        /// </summary>
        public byte S
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }

        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        
        /// <summary>
        /// The colorspace of this color
        /// </summary>
        public RGBSpaceName SpaceName { get { return Space.Name; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] * 1.4117647058823529411764705882353, ColorValues[1] / 255d, ColorValues[2] / 255d }; } }

        internal RGBColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        public BColorHSx()
            : this(ColorConverter.StandardColorspace, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorHSx(RGBSpaceName Space)
            : this(Space, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0 - 255)</param>
        /// <param name="S">Saturation (0 - 255)</param>
        public BColorHSx(RGBSpaceName Space, byte H, byte S)
            : base()
        {
            this.Space = RGBColorspace.GetColorspace(Space);
            wp = this.Space.ReferenceWhite.Name;
            this.H = H;
            this.S = S;
        }
        
        #endregion
    }

    /// <summary>
    /// HSV Color
    /// </summary>
    public sealed class BColorHSV : BColorHSx
    {
        /// <summary>
        /// Value: 0 to 255
        /// </summary>
        public byte V
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override ColorModel Model { get { return ColorModel.HSV; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        public BColorHSV()
            : base()
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorHSV(RGBSpaceName Space)
            : base(Space)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="H">Hue (0 - 255)</param>
        /// <param name="S">Saturation (0 - 255)</param>
        /// <param name="V">Value (0 - 255)</param>
        public BColorHSV(byte H, byte S, byte V)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0 - 255)</param>
        /// <param name="S">Saturation (0 - 255)</param>
        /// <param name="V">Value (0 - 255)</param>
        public BColorHSV(RGBSpaceName Space, byte H, byte S, byte V)
            : base(Space, H, S)
        {
            this.V = V;
        }
        
        #endregion

    }

    /// <summary>
    /// HSL Color
    /// </summary>
    public sealed class BColorHSL : BColorHSx
    {
        /// <summary>
        /// Lightness: 0 to 255
        /// </summary>
        public byte L
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override ColorModel Model { get { return ColorModel.HSL; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        public BColorHSL()
            : base()
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorHSL(RGBSpaceName Space)
            : base(Space)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="H">Hue (0 - 255)</param>
        /// <param name="S">Saturation (0 - 255)</param>
        /// <param name="L">Lightness (0 - 255)</param>
        public BColorHSL(byte H, byte S, byte L)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0 - 255)</param>
        /// <param name="S">Saturation (0 - 255)</param>
        /// <param name="L">Lightness (0 - 255)</param>
        public BColorHSL(RGBSpaceName Space, byte H, byte S, byte L)
            : base(Space, H, S)
        {
            this.L = L;
        }
        
        #endregion
    }

    /// <summary>
    /// YCbCr Color
    /// </summary>
    public sealed class BColorYCbCr : BColor
    {
        /// <summary>
        /// Luma: 0 to 255
        /// </summary>
        public byte Y
        {
            get { return ColorValues[1]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Blue-Yellow Chrominance: 0 to 255
        /// </summary>
        public byte Cb
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Red-Green Chrominance: 0 to 255
        /// </summary>
        public byte Cr
        {
            get { return ColorValues[2]; }
            set { ColorValues[2] = value; }
        }
        
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
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 255d, ColorValues[1] / 255d, ColorValues[2] / 255d }; } }

        internal RGBColorspace BaseSpace;
        internal YCbCrColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        public BColorYCbCr()
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public BColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace)
            : this(Space, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorYCbCr(YCbCrSpaceName Space)
            : this(Space, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public BColorYCbCr(RGBSpaceName BaseSpace)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 to 255)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 to 255)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 to 255)</param>
        public BColorYCbCr(byte Y, byte Cb, byte Cr)
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 to 255)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 to 255)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 to 255)</param>
        /// <param name="Space">The colorspace this color is in</param>
        public BColorYCbCr(YCbCrSpaceName Space, byte Y, byte Cb, byte Cr)
            : this(Space, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 to 255)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 to 255)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 to 255)</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public BColorYCbCr(RGBSpaceName BaseSpace, byte Y, byte Cb, byte Cr)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 to 255)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 to 255)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 to 255)</param>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public BColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace, byte Y, byte Cb, byte Cr)
            : base()
        {
            this.Y = Y;
            this.Cb = Cb;
            this.Cr = Cr;
            this.Space = YCbCrColorspace.GetColorspace(Space);
            this.BaseSpace = RGBColorspace.GetColorspace(BaseSpace);
            wp = this.Space.ReferenceWhite.Name;
        }
        
        #endregion
    }
}