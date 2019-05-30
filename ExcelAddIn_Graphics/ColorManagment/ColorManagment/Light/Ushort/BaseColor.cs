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
    public abstract class UColor
    {
        /// <summary>
        /// The values from each color channel
        /// </summary>
        protected ushort[] ColorValues;
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
        public abstract ushort[] ColorArray { get; }
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
        public static UColor GetColor(ColorModel Model)
        {
            switch (Model)
            {
                case ColorModel.CIELab: return new UColorLab();
                case ColorModel.CIELCHab: return new UColorLCHab();
                case ColorModel.CIELCHuv: return new UColorLCHuv();
                case ColorModel.CIELuv: return new UColorLuv();
                case ColorModel.CIEXYZ: return new UColorXYZ();
                case ColorModel.CIEYxy: return new UColorYxy();
                case ColorModel.Gray: return new UColorGray();
                case ColorModel.HSL: return new UColorHSL();
                case ColorModel.HSV: return new UColorHSV();
                case ColorModel.LCH99: return new UColorLCH99();
                case ColorModel.LCH99b: return new UColorLCH99b();
                case ColorModel.LCH99c: return new UColorLCH99c();
                case ColorModel.LCH99d: return new UColorLCH99d();
                case ColorModel.RGB: return new UColorRGB();
                case ColorModel.YCbCr: return new UColorYCbCr();

                default: return null;
            }
        }

        /// <summary>
        /// Creates a new instance of a color
        /// </summary>
        protected UColor()
        {
            ColorValues = new ushort[ChannelCount];
        }
        
        #endregion

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            UColor c = obj as UColor;
            if ((Object)c == null) { return false; }

            return ColorValues == c.ColorValues && ReferenceWhite == c.ReferenceWhite;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(UColor a, UColor b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.ColorValues == b.ColorValues && a.ReferenceWhite == b.ReferenceWhite;
        }

        public static bool operator !=(UColor a, UColor b)
        {
            return !(a == b);
        }

        #endregion
        
        /// <summary>
        /// Creates a new color with the same values as this color
        /// </summary>
        /// <returns>A copy of this color</returns>
        public UColor Copy()
        {
            switch (this.Model)
            {
                case ColorModel.CIELab: return new UColorLab(wp, ColorValues[0], (short)(ColorValues[1] - 32768), (short)(ColorValues[2] - 32768));
                case ColorModel.CIELuv: return new UColorLuv(wp, ColorValues[0], (short)(ColorValues[1] - 32768), (short)(ColorValues[2] - 32768));
                case ColorModel.CIEXYZ: return new UColorXYZ(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIEYxy: return new UColorYxy(wp, ColorValues[0], (short)(ColorValues[1] - 32768), (short)(ColorValues[2] - 32768));
                case ColorModel.CIELCHab: return new UColorLCHab(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIELCHuv: return new UColorLCHuv(wp, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99: return new UColorLCH99(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99b: return new UColorLCH99b(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99c: return new UColorLCH99c(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99d: return new UColorLCH99d(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.Gray: return new UColorGray(wp, ColorValues[0]);
                case ColorModel.RGB: return new UColorRGB(((UColorRGB)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSL: return new UColorHSL(((UColorHSL)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSV: return new UColorHSV(((UColorHSV)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.YCbCr:  return new UColorYCbCr(((UColorYCbCr)this).Space.Name, ((UColorYCbCr)this).BaseSpace.Name, ColorValues[0], ColorValues[1], ColorValues[2]);

                default: throw new NotImplementedException();
            }
        }
    }
}
