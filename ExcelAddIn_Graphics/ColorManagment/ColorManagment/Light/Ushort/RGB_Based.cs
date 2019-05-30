
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
    public sealed class UColorRGB : UColor
    {
        /// <summary>
        /// Red: 0 to 65535
        /// </summary>
        public ushort R
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Green: 0 to 65535
        /// </summary>
        public ushort G
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Blue: 0 to 65535
        /// </summary>
        public ushort B
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 65535d, ColorValues[1] / 65535d, ColorValues[2] / 65535d }; } }
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
        public UColorRGB()
            : this(ColorConverter.StandardColorspace, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public UColorRGB(bool IsLinear)
            : this(ColorConverter.StandardColorspace, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        public UColorRGB(ushort R, ushort G, ushort B)
            : this(ColorConverter.StandardColorspace, R, G, B, false)
        { }

        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public UColorRGB(ushort R, ushort G, ushort B, bool IsLinear)
            : this(ColorConverter.StandardColorspace, R, G, B, IsLinear)
        { }
        
        #endregion

        #region Colorspace

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorRGB(RGBSpaceName Space)
            : this(Space, 0, 0, 0, false)
        { }

        /// <summary>
        /// Creates a new instance of a linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public UColorRGB(RGBSpaceName Space, bool IsLinear)
            : this(Space, 0, 0, 0, IsLinear)
        { }

        /// <summary>
        /// Creates a new instance of a non-linear RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        public UColorRGB(RGBSpaceName Space, ushort R, ushort G, ushort B)
            : this(Space, R, G, B, false)
        { }
        
        /// <summary>
        /// Creates a new instance of a RGB Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="R">The red value (0 - 65535)</param>
        /// <param name="G">The green value (0 - 65535)</param>
        /// <param name="B">The blue value (0 - 65535)</param>
        /// <param name="IsLinear">States if the given values are linear or not</param>
        public UColorRGB(RGBSpaceName Space, ushort R, ushort G, ushort B, bool IsLinear)
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
        public UColorRGB ToNonLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (IsLinear) { return new UColorRGB(SpaceName, (ushort)(Space.ToNonLinear(R) * 65535), (ushort)(Space.ToNonLinear(G) * 65535), (ushort)(Space.ToNonLinear(B) * 65535), false); }
                else { return this; }
            }
            else { return this; }
        }

        /// <summary>
        /// Converts the color to a linear color.
        /// </summary>
        /// <returns>A linear RGB color</returns>
        public UColorRGB ToLinear()
        {
            if (SpaceName != RGBSpaceName.ICC)
            {
                if (!IsLinear) { return new UColorRGB(SpaceName, (ushort)(Space.ToLinear(R) * 65535), (ushort)(Space.ToLinear(G) * 65535), (ushort)(Space.ToLinear(B) * 65535), true); }
                else { return this; }
            }
            else { return this; }
        }
    }

    /// <summary>
    /// Hue-Saturation based Color
    /// </summary>
    public abstract class UColorHSx : UColor
    {
        /// <summary>
        /// Hue: 0 to 65535
        /// </summary>
        public ushort H
        {
            get { return (ushort)(((ColorValues[0] % ushort.MaxValue) + ushort.MaxValue) % ushort.MaxValue); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Saturation: 0 to 65535
        /// </summary>
        public ushort S
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] * 0.00549324788281071183337148088808, ColorValues[1] / 65535d, ColorValues[2] / 65535d }; } }

        internal RGBColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        public UColorHSx()
            : this(ColorConverter.StandardColorspace, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorHSx(RGBSpaceName Space)
            : this(Space, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a HSx Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0 - 65535)</param>
        public UColorHSx(RGBSpaceName Space, ushort H, ushort S)
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
    public sealed class UColorHSV : UColorHSx
    {
        /// <summary>
        /// Value: 0 to 65535
        /// </summary>
        public ushort V
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
        public UColorHSV()
            : base()
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorHSV(RGBSpaceName Space)
            : base(Space)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0 - 65535)</param>
        /// <param name="V">Value (0 - 65535)</param>
        public UColorHSV(ushort H, ushort S, ushort V)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.V = V;
        }

        /// <summary>
        /// Creates a new instance of a HSV Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0 - 65535)</param>
        /// <param name="V">Value (0 - 65535)</param>
        public UColorHSV(RGBSpaceName Space, ushort H, ushort S, ushort V)
            : base(Space, H, S)
        {
            this.V = V;
        }
        
        #endregion

    }

    /// <summary>
    /// HSL Color
    /// </summary>
    public sealed class UColorHSL : UColorHSx
    {
        /// <summary>
        /// Lightness: 0 to 65535
        /// </summary>
        public ushort L
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
        public UColorHSL()
            : base()
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorHSL(RGBSpaceName Space)
            : base(Space)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0 - 65535)</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        public UColorHSL(ushort H, ushort S, ushort L)
            : base(ColorConverter.StandardColorspace, H, S)
        {
            this.L = L;
        }

        /// <summary>
        /// Creates a new instance of a HSL Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="H">Hue (0.0 - 360.0)</param>
        /// <param name="S">Saturation (0 - 65535)</param>
        /// <param name="L">Lightness (0 - 65535)</param>
        public UColorHSL(RGBSpaceName Space, ushort H, ushort S, ushort L)
            : base(Space, H, S)
        {
            this.L = L;
        }
        
        #endregion

    }

    /// <summary>
    /// YCbCr Color
    /// </summary>
    public sealed class UColorYCbCr : UColor
    {
        /// <summary>
        /// Luma: 0 to 65535
        /// </summary>
        public ushort Y
        {
            get { return ColorValues[1]; }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Blue-Yellow Chrominance: 0 to 65535
        /// </summary>
        public ushort Cb
        {
            get { return ColorValues[1]; }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Red-Green Chrominance: 0 to 65535
        /// </summary>
        public ushort Cr
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
        public override ushort[] ColorArray { get { return new ushort[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 65535d, ColorValues[1] / 65535d, ColorValues[2] / 65535d }; } }

        internal RGBColorspace BaseSpace;
        internal YCbCrColorspace Space;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        public UColorYCbCr()
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public UColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace)
            : this(Space, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorYCbCr(YCbCrSpaceName Space)
            : this(Space, ColorConverter.StandardColorspace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public UColorYCbCr(RGBSpaceName BaseSpace)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 - 65535)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 - 65535)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 - 65535)</param>
        public UColorYCbCr(ushort Y, ushort Cb, ushort Cr)
            : this(ColorConverter.StandardYCbCrSpace, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 - 65535)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 - 65535)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 - 65535)</param>
        /// <param name="Space">The colorspace this color is in</param>
        public UColorYCbCr(YCbCrSpaceName Space, ushort Y, ushort Cb, ushort Cr)
            : this(Space, ColorConverter.StandardColorspace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 - 65535)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 - 65535)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 - 65535)</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public UColorYCbCr(RGBSpaceName BaseSpace, ushort Y, ushort Cb, ushort Cr)
            : this(ColorConverter.StandardYCbCrSpace, BaseSpace, Y, Cb, Cr)
        { }

        /// <summary>
        /// Creates a new instance of a YCbCr Color
        /// </summary>
        /// <param name="Y">Luma-value (0 - 65535)</param>
        /// <param name="Cb">Blue-Yellow Chrominance-value (0 - 65535)</param>
        /// <param name="Cr">Red-Green Chrominance-value (0 - 65535)</param>
        /// <param name="Space">The colorspace this color is in</param>
        /// <param name="BaseSpace">The colorspace this color is based on</param>
        public UColorYCbCr(YCbCrSpaceName Space, RGBSpaceName BaseSpace, ushort Y, ushort Cb, ushort Cr)
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