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
    /// Represents a color
    /// </summary>
    public abstract class Color
    {
        //When adding new colors:
        //Add model(s) to region "Conversion" and add conversion methods like previous colors "To_NewColorName_"
        //Add model(s) to region "Constructor" to the method "GetColor" in the switch/case block
        //Add model(s) to method "Copy" in the switch/case block
        
        /// <summary>
        /// The values from each color channel
        /// </summary>
        protected double[] ColorValues;
        /// <summary>
        /// The whitepoint of this color
        /// </summary>
        protected Whitepoint wp;
        /// <summary>
        /// The ICC profile of this color
        /// </summary>
        protected ICC ICC_Profile;

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
        public Whitepoint ReferenceWhite { get { return (ICC_Profile != null) ? ICC_Profile.ReferenceWhite : wp; } }
        /// <summary>
        /// The ICC profile linked to this color
        /// </summary>
        public ICC ICCprofile { get { return ICC_Profile; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public abstract double[] ColorArray { get; }
        /// <summary>
        /// If the color is initiated from an ICC profile, this returns true
        /// </summary>
        public bool IsICCcolor { get { return ICC_Profile != null; } }
        /// <summary>
        /// States if the color is the PCS or device colormodel
        /// </summary>
        public bool IsPCScolor { get; private set; }
        
        #region Constructor

        /// <summary>
        /// Creates a new instance of a specific color with the standard settings
        /// </summary>
        /// <param name="Model">The colormodel the new color will be in</param>
        /// <returns>A color in the given colormodel or null if not possible to create color</returns>
        public static Color GetColor(ColorModel Model)
        {
            switch (Model)
            {
                case ColorModel.CIELab: return new ColorLab();
                case ColorModel.CIELCHab: return new ColorLCHab();
                case ColorModel.CIELCHuv: return new ColorLCHuv();
                case ColorModel.CIELuv: return new ColorLuv();
                case ColorModel.CIEXYZ: return new ColorXYZ();
                case ColorModel.CIEYxy: return new ColorYxy();
                case ColorModel.Gray: return new ColorGray();
                case ColorModel.HSL: return new ColorHSL();
                case ColorModel.HSV: return new ColorHSV();
                case ColorModel.LCH99: return new ColorLCH99();
                case ColorModel.LCH99b: return new ColorLCH99b();
                case ColorModel.LCH99c: return new ColorLCH99c();
                case ColorModel.LCH99d: return new ColorLCH99d();
                case ColorModel.RGB: return new ColorRGB();
                case ColorModel.YCbCr: return new ColorYCbCr();

                default: return null;
            }
        }

        /// <summary>
        /// Creates a new instance of a color
        /// </summary>
        protected Color()
        {
            ColorValues = new double[ChannelCount];
        }

        /// <summary>
        /// Creates a new instance of a color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        protected Color(ICC profile)
        {
            if (ICC_Converter.IsSameSpace(Model, profile.Header.DataColorspace)) { IsPCScolor = false; }
            else if (ICC_Converter.IsSameSpace(Model, profile.Header.PCS)) { IsPCScolor = true; }
            else { throw new ArgumentException("Profile device space or PCS has to be the same as this color"); }
            ICC_Profile = profile;
            ColorValues = new double[ChannelCount];
        }

        #endregion

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            Color c = obj as Color;
            if ((Object)c == null) { return false; }

            return ColorValues == c.ColorValues && ReferenceWhite == c.ReferenceWhite;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Color a, Color b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.ColorValues == b.ColorValues && a.ReferenceWhite == b.ReferenceWhite;
        }

        public static bool operator !=(Color a, Color b)
        {
            return !(a == b);
        }

        #endregion
        
        /// <summary>
        /// Creates a new color with the same values as this color
        /// </summary>
        /// <returns>A copy of this color</returns>
        public Color Copy()
        {
            switch (this.Model)
            {
                case ColorModel.CIELab: return new ColorLab(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIELuv: return new ColorLuv(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIEXYZ: return new ColorXYZ(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIEYxy: return new ColorYxy(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIELCHab: return new ColorLCHab(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CIELCHuv: return new ColorLCHuv(wp.Copy(), ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99: return new ColorLCH99(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99b: return new ColorLCH99b(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99c: return new ColorLCH99c(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.LCH99d: return new ColorLCH99d(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.DEF: return new ColorDEF(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.Bef: return new ColorBef(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.BCH: return new ColorBCH(ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.Gray:
                    if (ICC_Profile == null) return new ColorGray(ICC_Profile, ColorValues[0]);
                    else return new ColorGray(wp.Copy(), ColorValues[0]);
                case ColorModel.RGB:
                    if (ICC_Profile == null) return new ColorRGB(((ColorRGB)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                    else return new ColorRGB(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSL:
                    if (ICC_Profile == null) return new ColorHSL(((ColorHSL)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                    else return new ColorHSL(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.HSV:
                    if (ICC_Profile == null) return new ColorHSV(((ColorHSV)this).Space.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                    else return new ColorHSV(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.YCbCr:
                    if (ICC_Profile == null) return new ColorYCbCr(((ColorYCbCr)this).Space.Name, ((ColorYCbCr)this).BaseSpace.Name, ColorValues[0], ColorValues[1], ColorValues[2]);
                    else return new ColorYCbCr(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2]);

                case ColorModel.CMY: return new ColorCMY(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2]);
                case ColorModel.CMYK: return new ColorCMYK(ICC_Profile, ColorValues[0], ColorValues[1], ColorValues[2], ColorValues[3]);

                case ColorModel.Color2:
                case ColorModel.Color3:
                case ColorModel.Color4:
                case ColorModel.Color5:
                case ColorModel.Color6:
                case ColorModel.Color7:
                case ColorModel.Color8:
                case ColorModel.Color9:
                case ColorModel.Color10:
                case ColorModel.Color11:
                case ColorModel.Color12:
                case ColorModel.Color13:
                case ColorModel.Color14:
                case ColorModel.Color15: return new ColorX(ICC_Profile, (double[])ColorArray.Clone());

                default: throw new NotImplementedException();
            }
        }
    }
}
