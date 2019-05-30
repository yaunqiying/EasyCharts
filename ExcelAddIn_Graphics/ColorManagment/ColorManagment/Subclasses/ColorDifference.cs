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
    public  class ColorDifference
    {
        #region Variables

        private const double Pi2 = 6.283185307179586476925286766559;        //2 * Pi
        private const double Pi180 = 0.01745329251994329576923690768489;    //Pi / 180
        private static double var1, var2, var3, var4, var5, var6, var7, var8, var9, var10,
            var11, var12, var13, var14, var15, var16, var17, var18, var19, var20, var21, var22;

        public static ColorLab nLab1;
        public static  ColorLab nLab2;
        private static ColorConverter Converter = new ColorConverter();
        private static Whitepoint nw = new Whitepoint(WhitepointName.D65);

        #endregion
        
        #region Public Methods

        #region Delta E

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_DIN99(ColorLCH99 Color1, ColorLCH99 Color2)
        {
            return GetDeltaE_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_DIN99(ColorLCH99b Color1, ColorLCH99b Color2)
        {
            return GetDeltaE_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_DIN99(ColorLCH99c Color1, ColorLCH99c Color2)
        {
            return GetDeltaE_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_DIN99(ColorLCH99d Color1, ColorLCH99d Color2)
        {
            return GetDeltaE_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_CIE76(ColorLab Color1, ColorLab Color2)
        {
            return Math.Sqrt(Math.Pow(Color2.L - Color1.L, 2) + Math.Pow(Color2.a - Color1.a, 2) + Math.Pow(Color2.b - Color1.b, 2));
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <param name="DiffMethod">The specific way to calculate the difference</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_CIE94(ColorLab Color1, ColorLab Color2, CIE94DifferenceMethod DiffMethod)
        {
            var1 = (DiffMethod == CIE94DifferenceMethod.Textiles) ? 2 : 1;             //SL
            var2 = Math.Sqrt(Math.Pow(Color1.a, 2) + Math.Pow(Color1.b, 2));           //C1
            var3 = Math.Sqrt(Math.Pow(Color2.a, 2) + Math.Pow(Color2.b, 2));           //C2
            var4 = (DiffMethod == CIE94DifferenceMethod.GraphicArts) ? 0.045 : 0.048;  //K1
            var5 = (DiffMethod == CIE94DifferenceMethod.GraphicArts) ? 0.015 : 0.014;  //K2
            var6 = Math.Pow(Color1.a - Color2.a, 2) + Math.Pow(Color1.b - Color2.b, 2) - Math.Pow(var2 - var3, 2);//Delta H

            return Math.Sqrt(Math.Pow((Color1.L - Color2.L) / var1, 2) + Math.Pow((var2 - var3) / (1 + var4 * var2), 2) + var6 / Math.Pow((1 + var5 * var2), 2));
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_CIEDE2000(ColorLab Color1, ColorLab Color2)
        {
            var1 = (Color1.L + Color2.L) / 2d;   //L_
            var2 = Math.Sqrt(Math.Pow(Color1.a, 2) + Math.Pow(Color1.b, 2));   //C1
            var3 = Math.Sqrt(Math.Pow(Color2.a, 2) + Math.Pow(Color2.b, 2));   //C2
            var4 = (var2 + var3) / 2d;   //C_
            var5 = (1 - Math.Sqrt((Math.Pow(var4, 7)) / (Math.Pow(var4, 7) + 6103515625))) / 2d;   //G
            var6 = Color1.a * (1 + var5);   //a1'
            var7 = Color2.a * (1 + var5);   //a2'
            var8 = Math.Sqrt(Math.Pow(var6, 2) + Math.Pow(Color1.b, 2));   //C1'
            var9 = Math.Sqrt(Math.Pow(var7, 2) + Math.Pow(Color2.b, 2));   //C2'
            var10 = (var8 + var9) / 2d;  //C'_
            var11 = Math.Atan2(Color1.b, var6);  //h1'
            var11 = (var11 < 0) ? var11 + Pi2 : (var11 >= Pi2) ? var11 - Pi2 : var11;
            var12 = Math.Atan2(Color2.b, var7);  //h2'
            var12 = (var12 < 0) ? var12 + Pi2 : (var12 >= Pi2) ? var12 - Pi2 : var12;
            var13 = (Math.Abs(var11 - var12) > Math.PI) ? (var11 + var12 + Pi2) / 2d : (var11 + var12) / 2d;  //H'_
            var14 = 1 - 0.17 * Math.Cos(var13 - 0.5236) + 0.24 * Math.Cos(2 * var13) + 0.32 * Math.Cos(3 * var13 + 0.10472) - 0.2 * Math.Cos(4 * var13 - 1.0995574);  //T
            var15 = var12 - var11;  //Delta h'
            var15 = (Math.Abs(var15) > Math.PI && var12 <= var11) ? var15 + Pi2 : (Math.Abs(var15) > Math.PI && var12 > var11) ? var15 - Pi2 : var15;
            var16 = 2 * Math.Sqrt(var8 * var9) * Math.Sin(var15 / 2d);  //Delta H'
            var17 = 1 + ((0.015 * Math.Pow(var1 - 50, 2)) / (Math.Sqrt(20 + Math.Pow(var1 - 50, 2))));  //SL
            var18 = 1 + 0.045 * var10;  //SC
            var19 = 1 + 0.015 * var10 * var14;  //SH            
            var20 = 1.0471976 * Math.Exp(-Math.Pow((var13 - 4.799655) / 0.436332313, 2));  //Delta O
            var21 = 2 * Math.Sqrt(Math.Pow(var10, 7) / (Math.Pow(var10, 7) + 6103515625));  //RC            
            var22 = -var21 * Math.Sin(2 * var20);  //RT
            
            return Math.Sqrt(Math.Pow((Color2.L - Color1.L) / var17, 2) + Math.Pow((var9 - var8) / var18, 2) + Math.Pow(var16 / var19, 2) + var22 * ((var9 - var8) / var18) * ((var16) / var19));
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <param name="DiffMethod">The specific way to calculate the difference</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public double GetDeltaE_CMC(ColorLab Color1, ColorLab Color2, CMCDifferenceMethod DiffMethod)
        {
            if (Color1.ReferenceWhite.Name != WhitepointName.D65) {
                nLab1 = Converter.ToLab(Color1, nw); }
            else
            {
                nLab1 = Color1;
            }
            if (Color2.ReferenceWhite.Name != WhitepointName.D65)
            {
                nLab2 = Converter.ToLab(Color2, nw);
            }
            else
            {
                nLab2= Color2;
            }

            var1 = (DiffMethod == CMCDifferenceMethod.Acceptability) ? 2 : 1;
            var2 = Math.Sqrt(Math.Pow(nLab1.a, 2) + Math.Pow(nLab1.b, 2));   //C1
            var3 = Math.Sqrt(Math.Pow(nLab2.a, 2) + Math.Pow(nLab2.b, 2));   //C2
            var4 = Math.Pow(nLab1.a - nLab2.a, 2) + Math.Pow(nLab1.b - nLab2.b, 2) - Math.Pow(var2 - var3, 2);
            var5 = (var4 < 0) ? 0 : Math.Sqrt(var4);   //Delta H
            var6 = (nLab1.L < 16) ? 0.511 : (nLab1.L * 0.040975) / (1 + nLab1.L * 0.01765);   //SL
            var7 = ((0.0638 * var2) / (1 + 0.0131 * var2)) + 0.638;   //SC
            var8 = Math.Atan2(nLab1.b, nLab1.a);   //H1
            var8 = (var8 < 0) ? var8 + Pi2 : (var8 >= Pi2) ? var8 - Pi2 : var8;
            var9 = (var8 <= 6.0213859193804370403867331512857 && var8 >= 2.8623399732707005061548528603213) ? 0.56 + Math.Abs(0.2 * Math.Cos(var8 + 2.9321531433504736892318004910609)) : 0.36 + Math.Abs(0.4 * Math.Cos(var8 + 0.61086523819801535192329176897101));   //T
            var10 = Math.Sqrt(Math.Pow(var2, 4) / (Math.Pow(var2, 4) + 1900));  //F
            var11 = var7 * (var10 * var9 + 1 - var10);  //SH

            return Math.Sqrt(Math.Pow((nLab1.L - nLab2.L) / (var1 * var6), 2) + Math.Pow((var2 - var3) / var7, 2) + Math.Pow(var5 / var11, 2));
        }

        /// <summary>
        /// Get the difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <param name="l">luma</param>
        /// <param name="c">chromaticity</param>
        /// <returns>The difference between Color1 and Color2</returns>
        public static double GetDeltaE_CMC(ColorLab Color1, ColorLab Color2, double l, double c)
        {
            if (Color1.ReferenceWhite.Name != WhitepointName.D65) nLab1 = Converter.ToLab(Color1, nw);
            if (Color2.ReferenceWhite.Name != WhitepointName.D65) nLab2 = Converter.ToLab(Color2, nw);

            var2 = Math.Sqrt(Math.Pow(nLab1.a, 2) + Math.Pow(nLab1.b, 2));   //C1
            var3 = Math.Sqrt(Math.Pow(nLab2.a, 2) + Math.Pow(nLab2.b, 2));   //C2
            var4 = Math.Pow(nLab1.a - nLab2.a, 2) + Math.Pow(nLab1.b - nLab2.b, 2) - Math.Pow(var2 - var3, 2);
            var5 = (var4 < 0) ? 0 : Math.Sqrt(var4);   //Delta H
            var6 = (nLab1.L < 16) ? 0.511 : (nLab1.L * 0.040975) / (1 + nLab1.L * 0.01765);   //SL
            var7 = ((0.0638 * var2) / (1 + 0.0131 * var2)) + 0.638;   //SC
            var8 = Math.Atan2(nLab1.b, nLab1.a);   //H1
            var8 = (var8 < 0) ? var8 + Pi2 : (var8 >= Pi2) ? var8 - Pi2 : var8;
            var9 = (var8 <= 6.0213859193804370403867331512857 && var8 >= 2.8623399732707005061548528603213) ? 0.56 + Math.Abs(0.2 * Math.Cos(var8 + 2.9321531433504736892318004910609)) : 0.36 + Math.Abs(0.4 * Math.Cos(var8 + 0.61086523819801535192329176897101));   //T
            var10 = Math.Sqrt(Math.Pow(var2, 4) / (Math.Pow(var2, 4) + 1900));  //F
            var11 = var7 * (var10 * var9 + 1 - var10);  //SH

            return Math.Sqrt(Math.Pow((nLab1.L - nLab2.L) / (l * var6), 2) + Math.Pow((var2 - var3) / (c * var7), 2) + Math.Pow(var5 / var11, 2));
        }
        
        #endregion

        #region Delta H

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_DIN99(ColorLCH99 Color1, ColorLCH99 Color2)
        {
            return GetDeltaH_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_DIN99(ColorLCH99b Color1, ColorLCH99b Color2)
        {
            return GetDeltaH_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_DIN99(ColorLCH99c Color1, ColorLCH99c Color2)
        {
            return GetDeltaH_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_DIN99(ColorLCH99d Color1, ColorLCH99d Color2)
        {
            return GetDeltaH_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_CIE94(ColorLab Color1, ColorLab Color2)
        {
            var2 = Math.Sqrt(Math.Pow(Color1.a, 2) + Math.Pow(Color1.b, 2));         //C1
            var3 = Math.Sqrt(Math.Pow(Color2.a, 2) + Math.Pow(Color2.b, 2));         //C2
            var1 = Math.Pow(Color1.a - Color2.a, 2) + Math.Pow(Color1.b - Color2.b, 2) - Math.Pow(var2 - var3, 2);
            return (var1 < 0) ? 0 : Math.Sqrt(var1);
        }

        /// <summary>
        /// Get the Hue difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Hue difference between Color1 and Color2</returns>
        public static double GetDeltaH_CMC(ColorLab Color1, ColorLab Color2)
        {
            if (Color1.ReferenceWhite.Name != WhitepointName.D65) nLab1 = Converter.ToLab(Color1, nw);
            if (Color2.ReferenceWhite.Name != WhitepointName.D65) nLab2 = Converter.ToLab(Color2, nw);

            var2 = Math.Sqrt(Math.Pow(nLab1.a, 2) + Math.Pow(nLab1.b, 2));         //C1
            var3 = Math.Sqrt(Math.Pow(nLab2.a, 2) + Math.Pow(nLab2.b, 2));         //C2
            var1 = Math.Pow(nLab1.a - nLab2.a, 2) + Math.Pow(nLab1.b - nLab2.b, 2) - Math.Pow(var2 - var3, 2);
            return (var1 < 0) ? 0 : Math.Sqrt(var1);
        }

        #endregion

        #region Delta C

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_DIN99(ColorLCH99 Color1, ColorLCH99 Color2)
        {
            return GetDeltaC_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_DIN99(ColorLCH99b Color1, ColorLCH99b Color2)
        {
            return GetDeltaC_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_DIN99(ColorLCH99c Color1, ColorLCH99c Color2)
        {
            return GetDeltaC_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_DIN99(ColorLCH99d Color1, ColorLCH99d Color2)
        {
            return GetDeltaC_DIN99_Base(Color1, Color2);
        }

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_CIE94(ColorLab Color1, ColorLab Color2)
        {
            var1 = Math.Sqrt(Math.Pow(Color1.a, 2) + Math.Pow(Color1.b, 2));         //C1
            var2 = Math.Sqrt(Math.Pow(Color2.a, 2) + Math.Pow(Color2.b, 2));         //C2
            return var1 - var2;
        }

        /// <summary>
        /// Get the Chroma difference between two colors
        /// </summary>
        /// <param name="Color1">First color</param>
        /// <param name="Color2">Second color</param>
        /// <returns>The Chroma difference between Color1 and Color2</returns>
        public static double GetDeltaC_CMC(ColorLab Color1, ColorLab Color2)
        {
            if (Color1.ReferenceWhite.Name != WhitepointName.D65) nLab1 = Converter.ToLab(Color1, nw);
            if (Color2.ReferenceWhite.Name != WhitepointName.D65) nLab2 = Converter.ToLab(Color2, nw);

            var1 = Math.Sqrt(Math.Pow(nLab1.a, 2) + Math.Pow(nLab1.b, 2));         //C1
            var2 = Math.Sqrt(Math.Pow(nLab2.a, 2) + Math.Pow(nLab2.b, 2));         //C2
            return var1 - var2;
        }

        #endregion

        #endregion

        #region Subroutines

        private static double GetDeltaE_DIN99_Base(ColorLCH Color1, ColorLCH Color2)
        {
            var1 = Color1.C * Math.Cos(Color1.H * Pi180);   //a1
            var2 = Color1.C * Math.Sin(Color1.H * Pi180);   //b1
            var3 = Color2.C * Math.Cos(Color2.H * Pi180);   //a2
            var4 = Color2.C * Math.Sin(Color2.H * Pi180);   //b2
            return Math.Sqrt(Math.Pow(Color1.L - Color2.L, 2) + Math.Pow(var1 - var3, 2) + Math.Pow(var2 - var4, 2));
        }

        private static double GetDeltaH_DIN99_Base(ColorLCH Color1, ColorLCH Color2)
        {
            var1 = Color1.C * Math.Cos(Color1.H * Pi180);
            var2 = Color1.C * Math.Sin(Color1.H * Pi180);
            var3 = Color2.C * Math.Cos(Color2.H * Pi180);
            var4 = Color2.C * Math.Sin(Color2.H * Pi180);
            var5 = Math.Sqrt(0.5 * (Color2.C * Color1.C + var3 * var1 + var4 * var2));

            return (var5 == 0) ? 0 : (var1 * var4 - var3 * var2) / var5;
        }

        private static double GetDeltaC_DIN99_Base(ColorLCH Color1, ColorLCH Color2)
        {
            return Color1.C - Color2.C;
        }

        #endregion
    }
}
