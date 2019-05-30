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
    public abstract class BColor
    {
        /// <summary>
        /// The values from each color channel
        /// </summary>
        protected byte[] ColorValues;
        /// <summary>
        /// The whitepoint of this color
        /// </summary>
        protected WhitepointName wp;

        /// <summary>
        /// The colormodel of this color
        /// </summary>
        public abstract ColorModel Model { get; }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public abstract byte ChannelCount { get; }
        /// <summary>
        /// The name of the reference white
        /// </summary>
        public WhitepointName ReferenceWhite { get { return wp; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public abstract byte[] ColorArray { get; }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public abstract double[] DoubleColorArray { get; }
        
        #region Constructor

        /// <summary>
        /// Creates a new instance of a specific color with the standard settings
        /// </summary>
        /// <param name="Model">The colormodel the new color will be in</param>
        /// <returns>A color in the given colormodel or null if not possible to create color</returns>
        public static BColor GetColor(ColorModel Model)
        {
            switch (Model)
            {
                case ColorModel.CIELab: return new BColorLab();
                case ColorModel.CIELCHab: return new BColorLCHab();
                case ColorModel.CIELCHuv: return new BColorLCHuv();
                case ColorModel.CIELuv: return new BColorLuv();
                case ColorModel.CIEXYZ: return new BColorXYZ();
                case ColorModel.CIEYxy: return new BColorYxy();
                case ColorModel.Gray: return new BColorGray();
                case ColorModel.HSL: return new BColorHSL();
                case ColorModel.HSV: return new BColorHSV();
                case ColorModel.LCH99: return new BColorLCH99();
                case ColorModel.LCH99b: return new BColorLCH99b();
                case ColorModel.LCH99c: return new BColorLCH99c();
                case ColorModel.LCH99d: return new BColorLCH99d();
                case ColorModel.RGB: return new BColorRGB();
                case ColorModel.YCbCr: return new BColorYCbCr();

                default: return null;
            }
        }

        /// <summary>
        /// Creates a new instance of a color
        /// </summary>
        protected BColor()
        {
            ColorValues = new byte[ChannelCount];
        }
        
        #endregion

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            BColor c = obj as BColor;
            if ((Object)c == null) { return false; }

            return ColorValues == c.ColorValues && ReferenceWhite == c.ReferenceWhite;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(BColor a, BColor b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.ColorValues == b.ColorValues && a.ReferenceWhite == b.ReferenceWhite;
        }

        public static bool operator !=(BColor a, BColor b)
        {
            return !(a == b);
        }

        #endregion
        
        /// <summary>
        /// Creates a new color with the same values as this color
        /// </summary>
        /// <returns>A copy of this color</returns>
        public BColor Copy()
        {
            switch (this.Model)
            {
                case ColorModel.CIELab: return new BColorLab(wp, ColorValues[0], (sbyte)(ColorValues[1] - 128), (sbyte)(ColorValues[2] - 128));
                case ColorModel.CIELuv: return new BColorLuv(wp, ColorValues[0], (sbyte)(ColorValues[1] - 128), (sbyte)(ColorValues[2] - 128));
                case ColorModel.CIEXYZ: return new BColorXYZ(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIEYxy: return new BColorYxy(wp, ColorValues[0], (sbyte)(ColorValues[1] - 128), (sbyte)(ColorValues[2] - 128));
                case ColorModel.CIELCHab: return new BColorLCHab(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIELCHuv: return new BColorLCHuv(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99: return new BColorLCH99(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99b: return new BColorLCH99b(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99c: return new BColorLCH99c(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99d: return new BColorLCH99d(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.Gray: return new BColorGray(wp, ColorValues[0]);
                case ColorModel.RGB: return new BColorRGB(((BColorRGB)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSL: return new BColorHSL(((BColorHSL)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSV: return new BColorHSV(((BColorHSV)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.YCbCr:  return new BColorYCbCr(((BColorYCbCr)this).Space.Name, ((BColorYCbCr)this).BaseSpace.Name, ColorValues[0], ColorValues[1], ColorValues[2]);

                default: throw new NotImplementedException();
            }
        }
    }
}
