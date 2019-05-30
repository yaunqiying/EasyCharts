using System;
using System.Linq;
using ColorManagment.Light;

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
    /// Handles conversions between color models and spaces
    /// </summary>
    public sealed class ColorConverter
    {
        //When adding new colors:
        //Add model(s) to region "Constants/Variables" as Color
        //Add model(s) to region "Conversionhandling" at method "Do"
        //Add model(s) to region "Conversionhandling" and create new method "Convert_NewModelName_"
        //Add model(s) to region "Conversion Collection" and create new method "_NewModelName_To_AllColors_"
        //Add model(s) to region "Conversions" and create new region like previous colors and add two methods for conversion and inverse conversion
        //Add model(s) to region "Converting Subroutines" at method "SetInputColor"
        //Add model(s) to region "Fast Conversion" at method "SetFastReferenceWhite"
        //Add model(s) to region "Fast Conversion" at method "SetFastAction"
        //Add model(s) to region "Fast Conversion" at method "ConvertFast"

        #region Settings and Init

        /// <summary>
        /// The chromatic adaption method that will be used for conversions
        /// </summary>
        public static AdaptionMethod ChromaticAdaptionMethod { get { return AD; } set { AD = value; MatrixPrecalculation(); } }
        /// <summary>
        /// The reference white that will be used for colors if not stated otherwise
        /// </summary>
        public static Whitepoint ReferenceWhite { get; set; }
        /// <summary>
        /// The standard colorspace that will be used if not stated otherwise
        /// </summary>
        public static RGBSpaceName StandardColorspace { get; set; }
        /// <summary>
        /// The preferred rendering intent that will be used if not stated otherwise and available within the ICC profile
        /// </summary>
        public static RenderingIntent PreferredRenderingIntent { get; set; }
        /// <summary>
        /// The standard colorspace that will be used for YCbCr colors if not stated otherwise
        /// </summary>
        public static YCbCrSpaceName StandardYCbCrSpace { get; set; }
        /// <summary>
        /// States if the library is initiated or not
        /// </summary>
        public static bool IsInit { get; private set; }

        private static AdaptionMethod AD;

        /// <summary>
        /// Creates a new instance of a color converter class
        /// </summary>
        public ColorConverter()
        {
            if (!IsInit) Init();
            ICCconverter = new ICC_Converter();
        }

        static ColorConverter()
        {
            if (!IsInit) Init();
        }

        /// <summary>
        /// Initiates the color conversion
        /// </summary>
        public static void Init()
        {
            if (!IsInit)
            {
                IsInit = true;

                AD = AdaptionMethod.Bradford;
                ReferenceWhite = new Whitepoint(WhitepointName.D50);
                StandardColorspace = RGBSpaceName.sRGB;
                PreferredRenderingIntent = RenderingIntent.RelativeColorimetric;
                StandardYCbCrSpace = YCbCrSpaceName.ITU_R_BT601_525;

                RGBSpaceName[] arr1 = (RGBSpaceName[])Enum.GetValues(typeof(RGBSpaceName));
                RGBspaceArr = new RGBColorspace[arr1.Length];
                arr1 = arr1.Where(t => t != RGBSpaceName.ICC).ToArray();
                for (int i = 0; i < arr1.Length; i++) RGBspaceArr[i] = RGBColorspace.GetColorspace(arr1[i]);

                YCbCrSpaceName[] arr2 = (YCbCrSpaceName[])Enum.GetValues(typeof(YCbCrSpaceName));
                YCbCrSpaceArr = new YCbCrColorspace[arr2.Length];
                arr2 = arr2.Where(t => t != YCbCrSpaceName.ICC).ToArray();
                for (int i = 0; i < arr2.Length; i++) YCbCrSpaceArr[i] = YCbCrColorspace.GetColorspace(arr2[i]);

                WhitepointName[] arr3 = (WhitepointName[])Enum.GetValues(typeof(WhitepointName));
                WhitepointArr = new Whitepoint[arr3.Length];
                arr3 = arr3.Where(t => t != WhitepointName.Custom).ToArray();
                for (int i = 0; i < arr3.Length; i++) WhitepointArr[i] = new Whitepoint(arr3[i]);

                MatrixPrecalculation();
            }
        }

        #endregion

        #region Constants/Variables

        #region Constants/Statics

        private const double Epsilon = 0.00885645167903563081717167575546;
        private const double Kappa = 903.2962962962962962962962962963;
        private const double KapEps = Kappa * Epsilon;
        private const double Pi180 = 0.01745329251994329576923690768489;    //Pi / 180
        private const double Pi180_1 = 57.295779513082320876798154814105;   //180 / Pi
        private const double Pi2 = 1.5707963267948966192313216916398;       //Pi / 2
        private const double cos16 = 0.96126169593831886191649704855706;    //Cos of 16°
        private const double sin16 = 0.2756373558169991856499715746113;     //Sin of 16°
        private const double cos26 = 0.89879404629916699278229567669579;    //Cos of 26°
        private const double sin26 = 0.43837114678907741745273454065827;    //Sin of 26°
        private const double cos50 = 0.64278760968653932632264340990726;    //Cos of 50°
        private const double sin50 = 0.76604444311897803520239265055542;    //Sin of 50°
        /// <summary>
        /// DIN99, DIN99b, DIN99c, DIN99d -> L1, L2, f, CG, Cd, angle
        /// </summary>
        private static readonly double[,] DIN99Vals = { { 105.51, 0.0158, 0.7, 0.045, 22.222222222222222222222222222222, 0 }, { 303.671, 0.0039, 0.83, 0.075, 23, 26 }, { 317.651, 0.0037, 0.94, 0.066, 23, 0 }, { 325.221, 0.0036, 1.14, 0.06, 22.5, 16 } };

        private static readonly double[,] XYZ_DEF_Matrix = { { 0.2053, 0.7125, 0.4670 }, { 1.8537, -1.2797, -0.4429 }, { -0.3655, 1.0120, -0.6104 } };
        private static readonly double[,] DEF_XYZ_Matrix = { { 0.671203, 0.495489, 0.153997 }, { 0.706165, 0.0247732, 0.522292 }, { 0.768864, -0.255621, -0.864558 } };

        #region Chromatic Adaption Matrix

        private static readonly double[,] Bradford_Ma = { { 0.8951, 0.2664, -0.1614 }, { -0.7502, 1.7135, 0.0367 }, { 0.0389, -0.0685, 1.0296 } };
        private static readonly double[,] Bradford_Ma1 = { { 0.9869929, -0.1470543, 0.1599627 }, { 0.4323053, 0.5183603, 0.0492912 }, { -0.0085287, 0.0400428, 0.9684867 } };
        private static readonly double[][,] Bradford = { Bradford_Ma, Bradford_Ma1 };

        private static readonly double[,] XYZScaling_Ma = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };
        private static readonly double[,] XYZScaling_Ma1 = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };
        private static readonly double[][,] XYZScaling = { XYZScaling_Ma, XYZScaling_Ma1 };

        private static readonly double[,] VonKries_Ma = { { 0.40024, 0.7076, -0.08081 }, { -0.2263, 1.16532, 0.0457 }, { 0.0, 0.0, 0.91822 } };
        private static readonly double[,] VonKries_Ma1 = { { 1.8599364, -1.1293816, 0.2198974 }, { 0.3611914, 0.6388125, -0.0000064 }, { 0.0, 0.0, 1.0890636 } };
        private static readonly double[][,] VonKries = { VonKries_Ma, VonKries_Ma1 };

        #endregion

        /// <summary>
        /// Precalculated adaption matrix: [sourceWP, destinationWP][x,y]
        /// </summary>
        private static double[,][,] PrecalcMatrix;
        /// <summary>
        /// List of all RGB colorspaces
        /// </summary>
        private static RGBColorspace[] RGBspaceArr;
        /// <summary>
        /// List of all YCbCr colorspaces
        /// </summary>
        private static YCbCrColorspace[] YCbCrSpaceArr;
        /// <summary>
        /// Array of all Whitepoints
        /// </summary>
        private static Whitepoint[] WhitepointArr;

        #endregion

        #region Conversion

        private Action FastAction;
        private ICC_Converter ICCconverter;
        private ColorModel InputModel;
        private ColorModel OutputModel;
        private Whitepoint InputWhitepoint;
        private RGBColorspace InputRGBSpace;
        private YCbCrColorspace InputYCbCrSpace;
        private RGBSpaceName RGBSpace = StandardColorspace;
        private YCbCrSpaceName YCbCrSpace = StandardYCbCrSpace;
        private Whitepoint OutReferenceWhite = ReferenceWhite;
        private bool DoAdaption;
        private bool IsRGBLinear;

        private MMath mmath = new MMath();
        private int EndArr;
        private double[] ValArr1;
        private double[] ValArr2;
        private double[] varArr;
        private double var1, var2, var3, var4, var5, var6, var7;

        #endregion

        #region Chromatic Adaption

        private double[][,] AM;
        private double[,] M = new double[3, 3];
        private double[,] output = new double[3, 3];
        private double[] S, D;
        private double[] c = new double[3];

        #endregion

        #endregion


        #region Public Conversion Methods Normal (Double)

        #region CIE based

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorXYZ ToXYZ(Color InColor)
        {
            OutputModel = ColorModel.CIEXYZ;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorXYZ(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorXYZ ToXYZ(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIEXYZ;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorXYZ(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorXYZ ToXYZ(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIEXYZ;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorXYZ(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorYxy ToYxy(Color InColor)
        {
            OutputModel = ColorModel.CIEYxy;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYxy(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorYxy ToYxy(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIEYxy;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYxy(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorYxy ToYxy(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIEYxy;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYxy(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLab ToLab(Color InColor)
        {
            OutputModel = ColorModel.CIELab;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLab ToLab(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIELab;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLab ToLab(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIELab;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLuv ToLuv(Color InColor)
        {
            OutputModel = ColorModel.CIELuv;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLuv ToLuv(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIELuv;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLuv ToLuv(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIELuv;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCHab ToLCHab(Color InColor)
        {
            OutputModel = ColorModel.CIELCHab;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLCHab ToLCHab(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIELCHab;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLCHab ToLCHab(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIELCHab;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCHuv ToLCHuv(Color InColor)
        {
            OutputModel = ColorModel.CIELCHuv;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLCHuv ToLCHuv(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.CIELCHuv;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorLCHuv ToLCHuv(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.CIELCHuv;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCHuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        #endregion

        #region RGB based

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorRGB ToRGB(Color InColor)
        {
            OutputModel = ColorModel.RGB;
            RGBSpace = StandardColorspace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorRGB(RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorRGB ToRGB(Color InColor, RGBSpaceName colorspace)
        {
            if (colorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = ColorModel.RGB;
            RGBSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(InColor);
            varArr = Do();
            return new ColorRGB(colorspace, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorHSL ToHSL(Color InColor)
        {
            OutputModel = ColorModel.HSL;
            RGBSpace = StandardColorspace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorHSL(RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorHSL ToHSL(Color InColor, RGBSpaceName colorspace)
        {
            if (colorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = ColorModel.HSL;
            RGBSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(InColor);
            varArr = Do();
            return new ColorHSL(colorspace, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorHSV ToHSV(Color InColor)
        {
            OutputModel = ColorModel.HSV;
            RGBSpace = StandardColorspace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorHSV(RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorHSV ToHSV(Color InColor, RGBSpaceName colorspace)
        {
            if (colorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = ColorModel.HSV;
            RGBSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(InColor);
            varArr = Do();
            return new ColorHSV(colorspace, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorYCbCr ToYCbCr(Color InColor)
        {
            OutputModel = ColorModel.YCbCr;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYCbCr(YCbCrSpace, RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorYCbCr ToYCbCr(Color InColor, YCbCrSpaceName colorspace)
        {
            if (colorspace == YCbCrSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = ColorModel.YCbCr;
            RGBSpace = StandardColorspace;
            YCbCrSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(StandardColorspace)];
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYCbCr(YCbCrSpace, RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="BaseColorspace">The colorspace the converted color will be based on</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorYCbCr ToYCbCr(Color InColor, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            if (colorspace == YCbCrSpaceName.ICC || BaseColorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = ColorModel.YCbCr;
            RGBSpace = BaseColorspace;
            YCbCrSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(BaseColorspace)];
            SetInputColor(InColor);
            varArr = Do();
            return new ColorYCbCr(YCbCrSpace, RGBSpace, varArr[0], varArr[1], varArr[2]);
        }

        #endregion

        #region Other

        /// <summary>
        /// Converts a color to a LCH99 color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCH99 ToLCH99(Color InColor)
        {
            OutReferenceWhite = WhitepointArr[(int)WhitepointName.D65];
            OutputModel = ColorModel.LCH99;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCH99(varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCH99b color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCH99b ToLCH99b(Color InColor)
        {
            OutReferenceWhite = WhitepointArr[(int)WhitepointName.D65];
            OutputModel = ColorModel.LCH99b;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCH99b(varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCH99c color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCH99c ToLCH99c(Color InColor)
        {
            OutReferenceWhite = WhitepointArr[(int)WhitepointName.D65];
            OutputModel = ColorModel.LCH99c;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCH99c(varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a LCH99d color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorLCH99d ToLCH99d(Color InColor)
        {
            OutReferenceWhite = WhitepointArr[(int)WhitepointName.D65];
            OutputModel = ColorModel.LCH99d;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorLCH99d(varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a DEF color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorDEF ToDEF(Color InColor)
        {
            OutputModel = ColorModel.DEF;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorDEF(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a DEF color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorDEF ToDEF(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.DEF;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorDEF(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a DEF color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorDEF ToDEF(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.DEF;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorDEF(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Bef color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorBef ToBef(Color InColor)
        {
            OutputModel = ColorModel.Bef;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBef(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Bef color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorBef ToBef(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.Bef;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBef(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Bef color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorBef ToBef(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.Bef;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBef(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a BCH color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorBCH ToBCH(Color InColor)
        {
            OutputModel = ColorModel.BCH;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBCH(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a BCH color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorBCH ToBCH(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.BCH;
            RGBSpace = StandardColorspace;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBCH(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }

        /// <summary>
        /// Converts a color to a BCH color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorBCH ToBCH(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.BCH;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorBCH(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
        }


        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public ColorGray ToGray(Color InColor)
        {
            OutputModel = ColorModel.Gray;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorGray(OutReferenceWhite, varArr[0]);
        }

        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorGray ToGray(Color InColor, Whitepoint RefWhite)
        {
            OutputModel = ColorModel.Gray;
            OutReferenceWhite = RefWhite;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorGray(OutReferenceWhite, varArr[0]);
        }

        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="RefWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public ColorGray ToGray(Color InColor, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = ColorModel.Gray;
            SetInputColor(InColor);
            varArr = Do();
            return new ColorGray(OutReferenceWhite, varArr[0]);
        }

        #endregion

        #region ICC

        /// <summary>
        /// Converts a color to the ICC PCS color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="profile">The profile the color should converted to</param>
        /// <returns>The converted color</returns>
        public Color ToICC_PCS(Color InColor, ICC profile)
        {
            if (InColor.ICCprofile != null)
            {
                if (InColor.IsPCScolor) { return ToICCcolor(InColor, profile); }  //directly convert to profile.PCS
                else { return ToICC_PCS(ToICC(InColor), profile); } //convert with InColor.icc to InColor.PCS, from InColor.PCS to profile.PCS
            }
            else { return ToICCcolor(InColor, profile); } //directly convert to profile.PCS
        }

        private Color ToICCcolor(Color InColor, ICC profile)
        {
            switch (profile.Header.PCS)
            {
                case ICCReader.ColorSpaceType.CIELAB: return ToLab(InColor, profile.ReferenceWhite);
                case ICCReader.ColorSpaceType.CIELUV: return ToLuv(InColor, profile.ReferenceWhite);
                case ICCReader.ColorSpaceType.CIEXYZ: return ToXYZ(InColor, profile.ReferenceWhite);
                case ICCReader.ColorSpaceType.CIEYxy: return ToYxy(InColor, profile.ReferenceWhite);
                case ICCReader.ColorSpaceType.Gray: return ToGray(InColor, profile.ReferenceWhite);
                case ICCReader.ColorSpaceType.HLS: return ToHSL(InColor);
                case ICCReader.ColorSpaceType.HSV: return ToHSV(InColor);
                case ICCReader.ColorSpaceType.RGB: return ToRGB(InColor);
                case ICCReader.ColorSpaceType.YCbCr: return ToYCbCr(InColor);

                default:
                    throw new ArgumentException("Cannot convert");
            }
        }

        /// <summary>
        /// Converts a color with the ICC profile it is in
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public Color ToICC(Color InColor)
        {
            if (InColor.ICCprofile != null)
            {
                if (InColor.IsPCScolor) { return ICCconverter.ToDevice(InColor.ICCprofile, InColor); }
                else { return ICCconverter.ToPCS(InColor.ICCprofile, InColor); }
            }
            else { throw new ArgumentException("No ICC profile available"); }
        }

        /// <summary>
        /// Converts a color to the ICC color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="profile">The icc file to be used for the conversion</param>
        /// <returns>The converted color</returns>
        public Color ToICC(Color InColor, ICC profile)
        {
            if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.DataColorspace)) { return ICCconverter.ToPCS(profile, InColor); }
            else if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.PCS)) { return ICCconverter.ToDevice(profile, InColor); }
            else { return ToICC(ToICC_PCS(InColor, profile), profile); }
        }

        /// <summary>
        /// Converts a color to the ICC color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="profile">The icc file to be used for the conversion</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <returns>The converted color</returns>
        public Color ToICC(Color InColor, ICC profile, RenderingIntent PrefRenderingIntent)
        {
            if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.DataColorspace)) { return ICCconverter.ToPCS(profile, InColor, PrefRenderingIntent); }
            else if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.PCS)) { return ICCconverter.ToDevice(profile, InColor, PrefRenderingIntent); }
            else { return ToICC(ToICC_PCS(InColor, profile), profile, PrefRenderingIntent); }
        }

        /// <summary>
        /// Converts a color to the ICC color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="profile">The icc file to be used for the conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <returns>The converted color</returns>
        public Color ToICC(Color InColor, ICC profile, ICCconversionType ConversionType, ICCconversionMethod ConversionMethod)
        {
            if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.DataColorspace)) { return ICCconverter.ToPCS(profile, InColor, ConversionMethod, ConversionType); }
            else if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.PCS)) { return ICCconverter.ToDevice(profile, InColor, ConversionMethod, ConversionType); }
            else { return ToICC(ToICC_PCS(InColor, profile), profile, ConversionType, ConversionMethod); }
        }

        /// <summary>
        /// Converts a color to the ICC color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="profile">The icc file to be used for the conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <returns>The converted color</returns>
        public Color ToICC(Color InColor, ICC profile, RenderingIntent PrefRenderingIntent, ICCconversionType ConversionType, ICCconversionMethod ConversionMethod)
        {
            if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.DataColorspace)) { return ICCconverter.ToPCS(profile, InColor, PrefRenderingIntent, ConversionMethod, ConversionType); }
            else if (ICC_Converter.IsSameSpace(InColor.Model, profile.Header.PCS)) { return ICCconverter.ToDevice(profile, InColor, PrefRenderingIntent, ConversionMethod, ConversionType); }
            else { return ToICC(ToICC_PCS(InColor, profile), profile, PrefRenderingIntent, ConversionType, ConversionMethod); }
        }

        #endregion

        #endregion
        
        #region Public Conversion Methods Light (Byte)

        #region CIE based

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorXYZ ToXYZ(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIEXYZ);
            return new BColorXYZ(OutReferenceWhite.Name, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorXYZ ToXYZ(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIEXYZ, ReferenceWhite);
            return new BColorXYZ(OutReferenceWhite.Name, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }


        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorYxy ToYxy(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIEYxy);
            return new BColorYxy(OutReferenceWhite.Name, (byte)(varArr[0] * 255), (sbyte)varArr[1], (sbyte)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorYxy ToYxy(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIEYxy, ReferenceWhite);
            return new BColorYxy(OutReferenceWhite.Name, (byte)(varArr[0] * 255), (sbyte)varArr[1], (sbyte)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLab ToLab(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELab);
            return new BColorLab(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (sbyte)varArr[1], (sbyte)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorLab ToLab(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELab, ReferenceWhite);
            return new BColorLab(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (sbyte)varArr[1], (sbyte)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLuv ToLuv(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELuv);
            return new BColorLuv(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (sbyte)varArr[1], (sbyte)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorLuv ToLuv(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELuv, ReferenceWhite);
            return new BColorLuv(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (sbyte)varArr[1], (sbyte)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCHab ToLCHab(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELCHab);
            return new BColorLCHab(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorLCHab ToLCHab(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELCHab, ReferenceWhite);
            return new BColorLCHab(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }


        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCHuv ToLCHuv(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELCHuv);
            return new BColorLCHuv(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorLCHuv ToLCHuv(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELCHuv, ReferenceWhite);
            return new BColorLCHuv(OutReferenceWhite.Name, (byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        #endregion

        #region RGB based

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorRGB ToRGB(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.RGB);
            return new BColorRGB(RGBSpace, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorRGB ToRGB(BColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.RGB, colorspace);
            return new BColorRGB(colorspace, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }


        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorHSL ToHSL(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.HSL);
            return new BColorHSL(RGBSpace, (byte)(varArr[0] * 0.70833), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorHSL ToHSL(BColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.HSL, colorspace);
            return new BColorHSL(colorspace, (byte)(varArr[0] * 0.70833), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }


        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorHSV ToHSV(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.HSV);
            return new BColorHSV(RGBSpace, (byte)(varArr[0] * 0.70833), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorHSV ToHSV(BColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.HSV, colorspace);
            return new BColorHSV(colorspace, (byte)(varArr[0] * 0.70833), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }


        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorYCbCr ToYCbCr(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.YCbCr);
            return new BColorYCbCr(YCbCrSpace, RGBSpace, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorYCbCr ToYCbCr(BColor InColor, YCbCrSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.YCbCr, StandardColorspace, colorspace);
            return new BColorYCbCr(YCbCrSpace, RGBSpace, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="BaseColorspace">The colorspace the converted color will be based on</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorYCbCr ToYCbCr(BColor InColor, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.YCbCr, BaseColorspace, colorspace);
            return new BColorYCbCr(YCbCrSpace, RGBSpace, (byte)(varArr[0] * 255), (byte)(varArr[1] * 255), (byte)(varArr[2] * 255));
        }

        #endregion

        #region Other

        /// <summary>
        /// Converts a color to a LCH99 color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCH99 ToLCH99(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99, WhitepointName.D65);
            return new BColorLCH99((byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        /// <summary>
        /// Converts a color to a LCH99b color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCH99b ToLCH99b(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99b, WhitepointName.D65);
            return new BColorLCH99b((byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        /// <summary>
        /// Converts a color to a LCH99c color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCH99c ToLCH99c(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99c, WhitepointName.D65);
            return new BColorLCH99c((byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }

        /// <summary>
        /// Converts a color to a LCH99d color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorLCH99d ToLCH99d(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99d, WhitepointName.D65);
            return new BColorLCH99d((byte)(varArr[0] * 2.55), (byte)(varArr[1] * 255), (byte)(varArr[2] * 0.70833));
        }


        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public BColorGray ToGray(BColor InColor)
        {
            varArr = Do(InColor, ColorModel.Gray);
            return new BColorGray(OutReferenceWhite.Name, (byte)(varArr[0] * 255));
        }

        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public BColorGray ToGray(BColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.Gray, ReferenceWhite);
            return new BColorGray(OutReferenceWhite.Name, (byte)(varArr[0] * 255));
        }

        #endregion

        #endregion

        #region Public Conversion Methods Light (Ushort)

        #region CIE based

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorXYZ ToXYZ(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIEXYZ);
            return new UColorXYZ(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to a XYZ color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorXYZ ToXYZ(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIEXYZ, ReferenceWhite);
            return new UColorXYZ(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }


        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorYxy ToYxy(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIEYxy);
            return new UColorYxy(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535), (short)varArr[1], (short)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Yxy color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorYxy ToYxy(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIEYxy, ReferenceWhite);
            return new UColorYxy(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535), (short)varArr[1], (short)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLab ToLab(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELab);
            return new UColorLab(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (short)varArr[1], (short)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Lab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorLab ToLab(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELab, ReferenceWhite);
            return new UColorLab(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (short)varArr[1], (short)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLuv ToLuv(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELuv);
            return new UColorLuv(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (short)varArr[1], (short)varArr[2]);
        }

        /// <summary>
        /// Converts a color to a Luv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorLuv ToLuv(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELuv, ReferenceWhite);
            return new UColorLuv(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (short)varArr[1], (short)varArr[2]);
        }


        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCHab ToLCHab(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELCHab);
            return new UColorLCHab(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        /// <summary>
        /// Converts a color to a LCHab color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorLCHab ToLCHab(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELCHab, ReferenceWhite);
            return new UColorLCHab(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }


        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCHuv ToLCHuv(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.CIELCHuv);
            return new UColorLCHuv(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        /// <summary>
        /// Converts a color to a LCHuv color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorLCHuv ToLCHuv(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.CIELCHuv, ReferenceWhite);
            return new UColorLCHuv(OutReferenceWhite.Name, (ushort)(varArr[0] * 655.35), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        #endregion

        #region RGB based

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorRGB ToRGB(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.RGB);
            return new UColorRGB(RGBSpace, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to an RGB color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorRGB ToRGB(UColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.RGB, colorspace);
            return new UColorRGB(colorspace, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }


        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorHSL ToHSL(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.HSL);
            return new UColorHSL(RGBSpace, (ushort)(varArr[0] * 182.041667), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to a HSL color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorHSL ToHSL(UColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.HSL, colorspace);
            return new UColorHSL(colorspace, (ushort)(varArr[0] * 182.041667), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }


        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorHSV ToHSV(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.HSV);
            return new UColorHSV(RGBSpace, (ushort)(varArr[0] * 182.041667), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to a HSV color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorHSV ToHSV(UColor InColor, RGBSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.HSV, colorspace);
            return new UColorHSV(colorspace, (ushort)(varArr[0] * 182.041667), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }


        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorYCbCr ToYCbCr(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.YCbCr);
            return new UColorYCbCr(YCbCrSpace, RGBSpace, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorYCbCr ToYCbCr(UColor InColor, YCbCrSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.YCbCr, StandardColorspace, colorspace);
            return new UColorYCbCr(YCbCrSpace, RGBSpace, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        /// <summary>
        /// Converts a color to a YCbCr color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="BaseColorspace">The colorspace the converted color will be based on</param>
        /// <param name="colorspace">The colorspace to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorYCbCr ToYCbCr(UColor InColor, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            varArr = Do(InColor, ColorModel.YCbCr, BaseColorspace, colorspace);
            return new UColorYCbCr(YCbCrSpace, RGBSpace, (ushort)(varArr[0] * 65535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 65535));
        }

        #endregion

        #region Other

        /// <summary>
        /// Converts a color to a LCH99 color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCH99 ToLCH99(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99, WhitepointName.D65);
            return new UColorLCH99((ushort)(varArr[0] * 65.535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        /// <summary>
        /// Converts a color to a LCH99b color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCH99b ToLCH99b(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99b, WhitepointName.D65);
            return new UColorLCH99b((ushort)(varArr[0] * 65.535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        /// <summary>
        /// Converts a color to a LCH99c color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCH99c ToLCH99c(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99c, WhitepointName.D65);
            return new UColorLCH99c((ushort)(varArr[0] * 65.535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }

        /// <summary>
        /// Converts a color to a LCH99d color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorLCH99d ToLCH99d(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.LCH99d, WhitepointName.D65);
            return new UColorLCH99d((ushort)(varArr[0] * 65.535), (ushort)(varArr[1] * 65535), (ushort)(varArr[2] * 182.041667));
        }


        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <returns>The converted color</returns>
        public UColorGray ToGray(UColor InColor)
        {
            varArr = Do(InColor, ColorModel.Gray);
            return new UColorGray(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535));
        }

        /// <summary>
        /// Converts a color to a gray color
        /// </summary>
        /// <param name="InColor">The color to convert</param>
        /// <param name="ReferenceWhite">The reference white to be used for the converted color</param>
        /// <returns>The converted color</returns>
        public UColorGray ToGray(UColor InColor, WhitepointName ReferenceWhite)
        {
            varArr = Do(InColor, ColorModel.Gray, ReferenceWhite);
            return new UColorGray(OutReferenceWhite.Name, (ushort)(varArr[0] * 65535));
        }

        #endregion

        #endregion


        #region Conversion Inputhandling

        #region Normal

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="BaseColorspace">The colorspace the output should be based on</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(Color inColor, ColorModel outModel, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = BaseColorspace;
            YCbCrSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(BaseColorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(Color inColor, ColorModel outModel, RGBSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = colorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(Color inColor, ColorModel outModel, Whitepoint RefWhite)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = RefWhite;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(Color inColor, ColorModel outModel, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(Color inColor, ColorModel outModel)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(inColor);
            return Do();
        }

        #endregion

        #region Byte

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="BaseColorspace">The colorspace the output should be based on</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(BColor inColor, ColorModel outModel, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = BaseColorspace;
            YCbCrSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(BaseColorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(BColor inColor, ColorModel outModel, RGBSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = colorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(BColor inColor, ColorModel outModel, Whitepoint RefWhite)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = RefWhite;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(BColor inColor, ColorModel outModel, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(BColor inColor, ColorModel outModel)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(inColor);
            return Do();
        }

        #endregion

        #region Ushort

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="BaseColorspace">The colorspace the output should be based on</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(UColor inColor, ColorModel outModel, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = BaseColorspace;
            YCbCrSpace = colorspace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(BaseColorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="colorspace">The colorspace the output should be in</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(UColor inColor, ColorModel outModel, RGBSpaceName colorspace)
        {
            OutputModel = outModel;
            RGBSpace = colorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(colorspace)];
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(UColor inColor, ColorModel outModel, Whitepoint RefWhite)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = RefWhite;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <param name="RefWhite">The reference white the output should have</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(UColor inColor, ColorModel outModel, WhitepointName RefWhite)
        {
            if (RefWhite != WhitepointName.Custom) OutReferenceWhite = WhitepointArr[(int)RefWhite];
            else OutReferenceWhite = ReferenceWhite;
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetInputColor(inColor);
            return Do();
        }

        /// <summary>
        /// Converts from one colorspace to another
        /// </summary>
        /// <param name="inColor">Input color</param>
        /// <param name="outModel">Output colormodel</param>
        /// <returns>Returns a color with the converted values from the input</returns>
        internal double[] Do(UColor inColor, ColorModel outModel)
        {
            OutputModel = outModel;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            OutReferenceWhite = ReferenceWhite;
            SetInputColor(inColor);
            return Do();
        }

        #endregion

        private double[] Do()
        {
            switch (InputModel)
            {
                case ColorModel.CIEXYZ: ConvertXYZ(); break;
                case ColorModel.CIEYxy: ConvertYxy(); break;
                case ColorModel.CIELab: ConvertLab(); break;
                case ColorModel.CIELuv: ConvertLuv(); break;
                case ColorModel.CIELCHab: ConvertLCHab(); break;
                case ColorModel.CIELCHuv: ConvertLCHuv(); break;
                case ColorModel.LCH99: ConvertLCH99(); break;
                case ColorModel.LCH99b: ConvertLCH99b(); break;
                case ColorModel.LCH99c: ConvertLCH99c(); break;
                case ColorModel.LCH99d: ConvertLCH99d(); break;
                case ColorModel.RGB: ConvertRGB(); break;
                case ColorModel.HSV: ConvertHSV(); break;
                case ColorModel.HSL: ConvertHSL(); break;
                case ColorModel.Gray: ConvertGray(); break;
                case ColorModel.YCbCr: ConvertYCbCr(); break;
                case ColorModel.DEF: ConvertDEF(); break;
                case ColorModel.Bef: ConvertBef(); break;
                case ColorModel.BCH: ConvertBCH(); break;

                default:
                    throw new NotImplementedException();
            }

            if (EndArr == 1) return ValArr1;
            else return ValArr2;
        }

        private void ConvertXYZ()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: XYZToXYZ(); break;
                case ColorModel.CIEYxy: XYZToYxy(); break;
                case ColorModel.CIELab: XYZToLab(); break;
                case ColorModel.CIELuv: XYZToLuv(); break;
                case ColorModel.CIELCHab: XYZToLCHab(); break;
                case ColorModel.CIELCHuv: XYZToLCHuv(); break;
                case ColorModel.RGB: XYZToRGB(); break;
                case ColorModel.HSV: XYZToHSV(); break;
                case ColorModel.HSL: XYZToHSL(); break;
                case ColorModel.Gray: XYZToGray(); break;
                case ColorModel.YCbCr: XYZToYCbCr(); break;
                case ColorModel.LCH99: XYZToLCH99(); break;
                case ColorModel.LCH99b: XYZToLCH99b(); break;
                case ColorModel.LCH99c: XYZToLCH99c(); break;
                case ColorModel.LCH99d: XYZToLCH99d(); break;
                case ColorModel.DEF: XYZToDEF(); break;
                case ColorModel.Bef: XYZToBef(); break;
                case ColorModel.BCH: XYZToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertYxy()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: YxyToXYZ(); break;
                case ColorModel.CIEYxy: YxyToYxy(); break;
                case ColorModel.CIELab: YxyToLab(); break;
                case ColorModel.CIELuv: YxyToLuv(); break;
                case ColorModel.CIELCHab: YxyToLCHab(); break;
                case ColorModel.CIELCHuv: YxyToLCHuv(); break;
                case ColorModel.RGB: YxyToRGB(); break;
                case ColorModel.HSV: YxyToHSV(); break;
                case ColorModel.HSL: YxyToHSL(); break;
                case ColorModel.Gray: YxyToGray(); break;
                case ColorModel.YCbCr: YxyToYCbCr(); break;
                case ColorModel.LCH99: YxyToLCH99(); break;
                case ColorModel.LCH99b: YxyToLCH99b(); break;
                case ColorModel.LCH99c: YxyToLCH99c(); break;
                case ColorModel.LCH99d: YxyToLCH99d(); break;
                case ColorModel.DEF: YxyToDEF(); break;
                case ColorModel.Bef: YxyToBef(); break;
                case ColorModel.BCH: YxyToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLab()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LabToXYZ(); break;
                case ColorModel.CIEYxy: LabToYxy(); break;
                case ColorModel.CIELab: LabToLab(); break;
                case ColorModel.CIELuv: LabToLuv(); break;
                case ColorModel.CIELCHab: LabToLCHab(); break;
                case ColorModel.CIELCHuv: LabToLCHuv(); break;
                case ColorModel.RGB: LabToRGB(); break;
                case ColorModel.HSV: LabToHSV(); break;
                case ColorModel.HSL: LabToHSL(); break;
                case ColorModel.Gray: LabToGray(); break;
                case ColorModel.YCbCr: LabToYCbCr(); break;
                case ColorModel.LCH99: LabToLCH99(); break;
                case ColorModel.LCH99b: LabToLCH99b(); break;
                case ColorModel.LCH99c: LabToLCH99c(); break;
                case ColorModel.LCH99d: LabToLCH99d(); break;
                case ColorModel.DEF: LabToDEF(); break;
                case ColorModel.Bef: LabToBef(); break;
                case ColorModel.BCH: LabToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLuv()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LuvToXYZ(); break;
                case ColorModel.CIEYxy: LuvToYxy(); break;
                case ColorModel.CIELab: LuvToLab(); break;
                case ColorModel.CIELuv: LuvToLuv(); break;
                case ColorModel.CIELCHab: LuvToLCHab(); break;
                case ColorModel.CIELCHuv: LuvToLCHuv(); break;
                case ColorModel.RGB: LuvToRGB(); break;
                case ColorModel.HSV: LuvToHSV(); break;
                case ColorModel.HSL: LuvToHSL(); break;
                case ColorModel.Gray: LuvToGray(); break;
                case ColorModel.YCbCr: LuvToYCbCr(); break;
                case ColorModel.LCH99: LuvToLCH99(); break;
                case ColorModel.LCH99b: LuvToLCH99b(); break;
                case ColorModel.LCH99c: LuvToLCH99c(); break;
                case ColorModel.LCH99d: LuvToLCH99d(); break;
                case ColorModel.DEF: LuvToDEF(); break;
                case ColorModel.Bef: LuvToBef(); break;
                case ColorModel.BCH: LuvToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCHab()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCHabToXYZ(); break;
                case ColorModel.CIEYxy: LCHabToYxy(); break;
                case ColorModel.CIELab: LCHabToLab(); break;
                case ColorModel.CIELuv: LCHabToLuv(); break;
                case ColorModel.CIELCHab: LCHabToLCHab(); break;
                case ColorModel.CIELCHuv: LCHabToLCHuv(); break;
                case ColorModel.RGB: LCHabToRGB(); break;
                case ColorModel.HSV: LCHabToHSV(); break;
                case ColorModel.HSL: LCHabToHSL(); break;
                case ColorModel.Gray: LCHabToGray(); break;
                case ColorModel.YCbCr: LCHabToYCbCr(); break;
                case ColorModel.LCH99: LCHabToLCH99(); break;
                case ColorModel.LCH99b: LCHabToLCH99b(); break;
                case ColorModel.LCH99c: LCHabToLCH99c(); break;
                case ColorModel.LCH99d: LCHabToLCH99d(); break;
                case ColorModel.DEF: LCHabToDEF(); break;
                case ColorModel.Bef: LCHabToBef(); break;
                case ColorModel.BCH: LCHabToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCHuv()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCHuvToXYZ(); break;
                case ColorModel.CIEYxy: LCHuvToYxy(); break;
                case ColorModel.CIELab: LCHuvToLab(); break;
                case ColorModel.CIELuv: LCHuvToLuv(); break;
                case ColorModel.CIELCHab: LCHuvToLCHab(); break;
                case ColorModel.CIELCHuv: LCHuvToLCHuv(); break;
                case ColorModel.RGB: LCHuvToRGB(); break;
                case ColorModel.HSV: LCHuvToHSV(); break;
                case ColorModel.HSL: LCHuvToHSL(); break;
                case ColorModel.Gray: LCHuvToGray(); break;
                case ColorModel.YCbCr: LCHuvToYCbCr(); break;
                case ColorModel.LCH99: LCHuvToLCH99(); break;
                case ColorModel.LCH99b: LCHuvToLCH99b(); break;
                case ColorModel.LCH99c: LCHuvToLCH99c(); break;
                case ColorModel.LCH99d: LCHuvToLCH99d(); break;
                case ColorModel.DEF: LCHuvToDEF(); break;
                case ColorModel.Bef: LCHuvToBef(); break;
                case ColorModel.BCH: LCHuvToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertRGB()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: RGBToXYZ(); break;
                case ColorModel.CIEYxy: RGBToYxy(); break;
                case ColorModel.CIELab: RGBToLab(); break;
                case ColorModel.CIELuv: RGBToLuv(); break;
                case ColorModel.CIELCHab: RGBToLCHab(); break;
                case ColorModel.CIELCHuv: RGBToLCHuv(); break;
                case ColorModel.RGB: RGBToRGB(); break;
                case ColorModel.HSV: RGBToHSV(); break;
                case ColorModel.HSL: RGBToHSL(); break;
                case ColorModel.Gray: RGBToGray(); break;
                case ColorModel.YCbCr: RGBToYCbCr(); break;
                case ColorModel.LCH99: RGBToLCH99(); break;
                case ColorModel.LCH99b: RGBToLCH99b(); break;
                case ColorModel.LCH99c: RGBToLCH99c(); break;
                case ColorModel.LCH99d: RGBToLCH99d(); break;
                case ColorModel.DEF: RGBToDEF(); break;
                case ColorModel.Bef: RGBToBef(); break;
                case ColorModel.BCH: RGBToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertHSV()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: HSVToXYZ(); break;
                case ColorModel.CIEYxy: HSVToYxy(); break;
                case ColorModel.CIELab: HSVToLab(); break;
                case ColorModel.CIELuv: HSVToLuv(); break;
                case ColorModel.CIELCHab: HSVToLCHab(); break;
                case ColorModel.CIELCHuv: HSVToLCHuv(); break;
                case ColorModel.RGB: HSVToRGB(); break;
                case ColorModel.HSV: HSVToHSV(); break;
                case ColorModel.HSL: HSVToHSL(); break;
                case ColorModel.Gray: HSVToGray(); break;
                case ColorModel.YCbCr: HSVToYCbCr(); break;
                case ColorModel.LCH99: HSVToLCH99(); break;
                case ColorModel.LCH99b: HSVToLCH99b(); break;
                case ColorModel.LCH99c: HSVToLCH99c(); break;
                case ColorModel.LCH99d: HSVToLCH99d(); break;
                case ColorModel.DEF: HSVToDEF(); break;
                case ColorModel.Bef: HSVToBef(); break;
                case ColorModel.BCH: HSVToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertHSL()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: HSLToXYZ(); break;
                case ColorModel.CIEYxy: HSLToYxy(); break;
                case ColorModel.CIELab: HSLToLab(); break;
                case ColorModel.CIELuv: HSLToLuv(); break;
                case ColorModel.CIELCHab: HSLToLCHab(); break;
                case ColorModel.CIELCHuv: HSLToLCHuv(); break;
                case ColorModel.RGB: HSLToRGB(); break;
                case ColorModel.HSV: HSLToHSV(); break;
                case ColorModel.HSL: HSLToHSL(); break;
                case ColorModel.Gray: HSLToGray(); break;
                case ColorModel.YCbCr: HSLToYCbCr(); break;
                case ColorModel.LCH99: HSLToLCH99(); break;
                case ColorModel.LCH99b: HSLToLCH99b(); break;
                case ColorModel.LCH99c: HSLToLCH99c(); break;
                case ColorModel.LCH99d: HSLToLCH99d(); break;
                case ColorModel.DEF: HSLToDEF(); break;
                case ColorModel.Bef: HSLToBef(); break;
                case ColorModel.BCH: HSLToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertGray()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: GrayToXYZ(); break;
                case ColorModel.CIEYxy: GrayToYxy(); break;
                case ColorModel.CIELab: GrayToLab(); break;
                case ColorModel.CIELuv: GrayToLuv(); break;
                case ColorModel.CIELCHab: GrayToLCHab(); break;
                case ColorModel.CIELCHuv: GrayToLCHuv(); break;
                case ColorModel.RGB: GrayToRGB(); break;
                case ColorModel.HSV: GrayToHSV(); break;
                case ColorModel.HSL: GrayToHSL(); break;
                case ColorModel.Gray: GrayToGray(); break;
                case ColorModel.YCbCr: GrayToYCbCr(); break;
                case ColorModel.LCH99: GrayToLCH99(); break;
                case ColorModel.LCH99b: GrayToLCH99b(); break;
                case ColorModel.LCH99c: GrayToLCH99c(); break;
                case ColorModel.LCH99d: GrayToLCH99d(); break;
                case ColorModel.DEF: GrayToDEF(); break;
                case ColorModel.Bef: GrayToBef(); break;
                case ColorModel.BCH: GrayToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertYCbCr()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: YCbCrToXYZ(); break;
                case ColorModel.CIEYxy: YCbCrToYxy(); break;
                case ColorModel.CIELab: YCbCrToLab(); break;
                case ColorModel.CIELuv: YCbCrToLuv(); break;
                case ColorModel.CIELCHab: YCbCrToLCHab(); break;
                case ColorModel.CIELCHuv: YCbCrToLCHuv(); break;
                case ColorModel.RGB: YCbCrToRGB(); break;
                case ColorModel.HSV: YCbCrToHSV(); break;
                case ColorModel.HSL: YCbCrToHSL(); break;
                case ColorModel.Gray: YCbCrToGray(); break;
                case ColorModel.YCbCr: YCbCrToYCbCr(); break;
                case ColorModel.LCH99: YCbCrToLCH99(); break;
                case ColorModel.LCH99b: YCbCrToLCH99b(); break;
                case ColorModel.LCH99c: YCbCrToLCH99c(); break;
                case ColorModel.LCH99d: YCbCrToLCH99d(); break;
                case ColorModel.DEF: YCbCrToDEF(); break;
                case ColorModel.Bef: YCbCrToBef(); break;
                case ColorModel.BCH: YCbCrToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCH99()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCH99ToXYZ(); break;
                case ColorModel.CIEYxy: LCH99ToYxy(); break;
                case ColorModel.CIELab: LCH99ToLab(); break;
                case ColorModel.CIELuv: LCH99ToLuv(); break;
                case ColorModel.CIELCHab: LCH99ToLCHab(); break;
                case ColorModel.CIELCHuv: LCH99ToLCHuv(); break;
                case ColorModel.RGB: LCH99ToRGB(); break;
                case ColorModel.HSV: LCH99ToHSV(); break;
                case ColorModel.HSL: LCH99ToHSL(); break;
                case ColorModel.Gray: LCH99ToGray(); break;
                case ColorModel.YCbCr: LCH99ToYCbCr(); break;
                case ColorModel.LCH99: LCH99ToLCH99(); break;
                case ColorModel.LCH99b: LCH99ToLCH99b(); break;
                case ColorModel.LCH99c: LCH99ToLCH99c(); break;
                case ColorModel.LCH99d: LCH99ToLCH99d(); break;
                case ColorModel.DEF: LCH99ToDEF(); break;
                case ColorModel.Bef: LCH99ToBef(); break;
                case ColorModel.BCH: LCH99ToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCH99b()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCH99bToXYZ(); break;
                case ColorModel.CIEYxy: LCH99bToYxy(); break;
                case ColorModel.CIELab: LCH99bToLab(); break;
                case ColorModel.CIELuv: LCH99bToLuv(); break;
                case ColorModel.CIELCHab: LCH99bToLCHab(); break;
                case ColorModel.CIELCHuv: LCH99bToLCHuv(); break;
                case ColorModel.RGB: LCH99bToRGB(); break;
                case ColorModel.HSV: LCH99bToHSV(); break;
                case ColorModel.HSL: LCH99bToHSL(); break;
                case ColorModel.Gray: LCH99bToGray(); break;
                case ColorModel.YCbCr: LCH99bToYCbCr(); break;
                case ColorModel.LCH99: LCH99bToLCH99(); break;
                case ColorModel.LCH99b: LCH99bToLCH99b(); break;
                case ColorModel.LCH99c: LCH99bToLCH99c(); break;
                case ColorModel.LCH99d: LCH99bToLCH99d(); break;
                case ColorModel.DEF: LCH99bToDEF(); break;
                case ColorModel.Bef: LCH99bToBef(); break;
                case ColorModel.BCH: LCH99bToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCH99c()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCH99cToXYZ(); break;
                case ColorModel.CIEYxy: LCH99cToYxy(); break;
                case ColorModel.CIELab: LCH99cToLab(); break;
                case ColorModel.CIELuv: LCH99cToLuv(); break;
                case ColorModel.CIELCHab: LCH99cToLCHab(); break;
                case ColorModel.CIELCHuv: LCH99cToLCHuv(); break;
                case ColorModel.RGB: LCH99cToRGB(); break;
                case ColorModel.HSV: LCH99cToHSV(); break;
                case ColorModel.HSL: LCH99cToHSL(); break;
                case ColorModel.Gray: LCH99cToGray(); break;
                case ColorModel.YCbCr: LCH99cToYCbCr(); break;
                case ColorModel.LCH99: LCH99cToLCH99(); break;
                case ColorModel.LCH99b: LCH99cToLCH99b(); break;
                case ColorModel.LCH99c: LCH99cToLCH99c(); break;
                case ColorModel.LCH99d: LCH99cToLCH99d(); break;
                case ColorModel.DEF: LCH99cToDEF(); break;
                case ColorModel.Bef: LCH99cToBef(); break;
                case ColorModel.BCH: LCH99cToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertLCH99d()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: LCH99dToXYZ(); break;
                case ColorModel.CIEYxy: LCH99dToYxy(); break;
                case ColorModel.CIELab: LCH99dToLab(); break;
                case ColorModel.CIELuv: LCH99dToLuv(); break;
                case ColorModel.CIELCHab: LCH99dToLCHab(); break;
                case ColorModel.CIELCHuv: LCH99dToLCHuv(); break;
                case ColorModel.RGB: LCH99dToRGB(); break;
                case ColorModel.HSV: LCH99dToHSV(); break;
                case ColorModel.HSL: LCH99dToHSL(); break;
                case ColorModel.Gray: LCH99dToGray(); break;
                case ColorModel.YCbCr: LCH99dToYCbCr(); break;
                case ColorModel.LCH99: LCH99dToLCH99(); break;
                case ColorModel.LCH99b: LCH99dToLCH99b(); break;
                case ColorModel.LCH99c: LCH99dToLCH99c(); break;
                case ColorModel.LCH99d: LCH99dToLCH99d(); break;
                case ColorModel.DEF: LCH99dToDEF(); break;
                case ColorModel.Bef: LCH99dToBef(); break;
                case ColorModel.BCH: LCH99dToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertDEF()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: DEFToXYZ(); break;
                case ColorModel.CIEYxy: DEFToYxy(); break;
                case ColorModel.CIELab: DEFToLab(); break;
                case ColorModel.CIELuv: DEFToLuv(); break;
                case ColorModel.CIELCHab: DEFToLCHab(); break;
                case ColorModel.CIELCHuv: DEFToLCHuv(); break;
                case ColorModel.RGB: DEFToRGB(); break;
                case ColorModel.HSV: DEFToHSV(); break;
                case ColorModel.HSL: DEFToHSL(); break;
                case ColorModel.Gray: DEFToGray(); break;
                case ColorModel.YCbCr: DEFToYCbCr(); break;
                case ColorModel.LCH99: DEFToLCH99(); break;
                case ColorModel.LCH99b: DEFToLCH99b(); break;
                case ColorModel.LCH99c: DEFToLCH99c(); break;
                case ColorModel.LCH99d: DEFToLCH99d(); break;
                case ColorModel.DEF: DEFToDEF(); break;
                case ColorModel.Bef: DEFToBef(); break;
                case ColorModel.BCH: DEFToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertBef()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: BefToXYZ(); break;
                case ColorModel.CIEYxy: BefToYxy(); break;
                case ColorModel.CIELab: BefToLab(); break;
                case ColorModel.CIELuv: BefToLuv(); break;
                case ColorModel.CIELCHab: BefToLCHab(); break;
                case ColorModel.CIELCHuv: BefToLCHuv(); break;
                case ColorModel.RGB: BefToRGB(); break;
                case ColorModel.HSV: BefToHSV(); break;
                case ColorModel.HSL: BefToHSL(); break;
                case ColorModel.Gray: BefToGray(); break;
                case ColorModel.YCbCr: BefToYCbCr(); break;
                case ColorModel.LCH99: BefToLCH99(); break;
                case ColorModel.LCH99b: BefToLCH99b(); break;
                case ColorModel.LCH99c: BefToLCH99c(); break;
                case ColorModel.LCH99d: BefToLCH99d(); break;
                case ColorModel.DEF: BefToDEF(); break;
                case ColorModel.Bef: BefToBef(); break;
                case ColorModel.BCH: BefToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ConvertBCH()
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: BCHToXYZ(); break;
                case ColorModel.CIEYxy: BCHToYxy(); break;
                case ColorModel.CIELab: BCHToLab(); break;
                case ColorModel.CIELuv: BCHToLuv(); break;
                case ColorModel.CIELCHab: BCHToLCHab(); break;
                case ColorModel.CIELCHuv: BCHToLCHuv(); break;
                case ColorModel.RGB: BCHToRGB(); break;
                case ColorModel.HSV: BCHToHSV(); break;
                case ColorModel.HSL: BCHToHSL(); break;
                case ColorModel.Gray: BCHToGray(); break;
                case ColorModel.YCbCr: BCHToYCbCr(); break;
                case ColorModel.LCH99: BCHToLCH99(); break;
                case ColorModel.LCH99b: BCHToLCH99b(); break;
                case ColorModel.LCH99c: BCHToLCH99c(); break;
                case ColorModel.LCH99d: BCHToLCH99d(); break;
                case ColorModel.DEF: BCHToDEF(); break;
                case ColorModel.Bef: BCHToBef(); break;
                case ColorModel.BCH: BCHToBCH(); break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Collection of Conversions

        #region XYZ

        private void XYZToXYZ()
        {
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void XYZToYxy()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToYxy(ValArr1, ValArr2); EndArr = 2; }
        }

        private void XYZToLab()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLab(ValArr1, ValArr2); EndArr = 2; }
        }

        private void XYZToLuv()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLuv(ValArr1, ValArr2); EndArr = 2; }
        }

        private void XYZToLCHab()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToLCHuv()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToRGB()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToRGB(ValArr1, ValArr2); EndArr = 2; }
        }

        private void XYZToHSV()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToHSL()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToYCbCr()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToGray()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToLCH99()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToLCH99b()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToDEF()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else { XYZToDEF(ValArr1, ref ValArr2); EndArr = 2; }
        }

        private void XYZToBef()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void XYZToBCH()
        {
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region Yxy

        private void YxyToXYZ()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption) { ChromaticAdaption(ValArr2, ref  ValArr1); EndArr = 1; }
            else { EndArr = 2; }
        }

        private void YxyToYxy()
        {
            if (DoAdaption)
            {
                YxyToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void YxyToLab()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLab(ValArr2, ValArr1); EndArr = 1; }
        }

        private void YxyToLuv()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLuv(ValArr2, ValArr1); EndArr = 1; }
        }

        private void YxyToLCHab()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToLCHuv()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToRGB()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YxyToHSV()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToHSL()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToYCbCr()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToGray()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToLCH99()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToLCH99b()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToDEF()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
            else { XYZToDEF(ValArr2, ref ValArr1); EndArr = 1; }
        }

        private void YxyToBef()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void YxyToBCH()
        {
            YxyToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        #endregion

        #region Lab

        private void LabToXYZ()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption) { ChromaticAdaption(ValArr2, ref  ValArr1); EndArr = 1; }
            else { EndArr = 2; }
        }

        private void LabToYxy()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToYxy(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LabToLab()
        {
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LabToLuv()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLuv(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LabToLCHab()
        {
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToLCHuv()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToRGB()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToRGB(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LabToHSV()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToHSL()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToYCbCr()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToGray()
        {
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToLCH99()
        {
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToLCH99b()
        {
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToDEF()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
            else { XYZToDEF(ValArr2, ref ValArr1); EndArr = 1; }
        }

        private void LabToBef()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LabToBCH()
        {
            LabToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        #endregion

        #region Luv

        private void LuvToXYZ()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption) { ChromaticAdaption(ValArr2, ref  ValArr1); EndArr = 1; }
            else { EndArr = 2; }
        }

        private void LuvToYxy()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToYxy(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LuvToLab()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLab(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LuvToLuv()
        {
            if (DoAdaption)
            {
                LuvToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LuvToLCHab()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToLCHuv()
        {
            if (DoAdaption)
            {
                LuvToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToRGB()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToRGB(ValArr2, ValArr1); EndArr = 1; }
        }

        private void LuvToHSV()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToHSL()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToYCbCr()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToGray()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToLCH99()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToLCH99b()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToDEF()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
            else { XYZToDEF(ValArr2, ref ValArr1); EndArr = 1; }
        }

        private void LuvToBef()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LuvToBCH()
        {
            LuvToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        #endregion

        #region LCHab

        private void LCHabToXYZ()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void LCHabToYxy()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHabToLab()
        {
            LCHabToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void LCHabToLuv()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHabToLCHab()
        {
            if (DoAdaption)
            {
                LCHabToLab(ValArr1, ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCHabToLCHuv()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToRGB()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHabToHSV()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToHSL()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToYCbCr()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToGray()
        {
            LCHabToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToLCH99()
        {
            LCHabToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToLCH99b()
        {
            LCHabToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToDEF()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCHabToBef()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHabToBCH()
        {
            LCHabToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region LCHuv

        private void LCHuvToXYZ()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void LCHuvToYxy()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHuvToLab()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHuvToLuv()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LuvToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void LCHuvToLCHab()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToLCHuv()
        {
            if (DoAdaption)
            {
                LCHuvToLuv(ValArr1, ValArr2);
                LuvToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCHuvToRGB()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHuvToHSV()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToHSL()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToYCbCr()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToGray()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCHuvToLCH99()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToLCH99b()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToDEF()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCHuvToBef()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCHuvToBCH()
        {
            LCHuvToLuv(ValArr1, ValArr2);
            LuvToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region RGB

        private void RGBToXYZ()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption) { ChromaticAdaption(ValArr2, ref ValArr1); EndArr = 1; }
            else { EndArr = 2; }
        }

        private void RGBToYxy()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToYxy(ValArr2, ValArr1); EndArr = 1; }
        }

        private void RGBToLab()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLab(ValArr2, ValArr1); EndArr = 1; }
        }

        private void RGBToLuv()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLuv(ValArr2, ValArr1); EndArr = 1; }
        }

        private void RGBToLCHab()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToLCHuv()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToRGB()
        {
            if (DoAdaption)
            {
                RGBToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void RGBToHSV()
        {
            if (DoAdaption)
            {
                RGBToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToHSL()
        {
            if (DoAdaption)
            {
                RGBToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToYCbCr()
        {
            if (DoAdaption)
            {
                RGBToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToGray()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToLCH99()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToLCH99b()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToDEF()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
            else { XYZToDEF(ValArr2, ref ValArr1); EndArr = 1; }
        }

        private void RGBToBef()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void RGBToBCH()
        {
            RGBToXYZ(ValArr1, ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        #endregion

        #region HSV

        private void HSVToXYZ()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void HSVToYxy()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToYxy(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSVToLab()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLab(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSVToLuv()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLuv(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSVToLCHab()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToLCHuv()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToRGB()
        {
            HSVToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void HSVToHSV()
        {
            if (DoAdaption)
            {
                HSVToRGB(ValArr1, ValArr2);
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void HSVToHSL()
        {
            HSVToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToYCbCr()
        {
            HSVToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToGray()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToLCH99()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToLCH99b()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToDEF()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void HSVToBef()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSVToBCH()
        {
            HSVToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region HSL

        private void HSLToXYZ()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void HSLToYxy()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToYxy(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSLToLab()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLab(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSLToLuv()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLuv(ValArr1, ValArr2); EndArr = 2; }
        }

        private void HSLToLCHab()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToLCHuv()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToRGB()
        {
            HSLToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void HSLToHSV()
        {
            HSLToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToHSL()
        {
            if (DoAdaption)
            {
                HSLToRGB(ValArr1, ValArr2);
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void HSLToYCbCr()
        {
            HSLToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToGray()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToLCH99()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToLCH99b()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToDEF()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void HSLToBef()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void HSLToBCH()
        {
            HSLToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region YCbCr

        private void YCbCrToXYZ()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void YCbCrToYxy()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToYxy(ValArr1, ValArr2); EndArr = 2; }
        }

        private void YCbCrToLab()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLab(ValArr1, ValArr2); EndArr = 2; }
        }

        private void YCbCrToLuv()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { XYZToLuv(ValArr1, ValArr2); EndArr = 2; }
        }

        private void YCbCrToLCHab()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToLCHuv()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToRGB()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void YCbCrToHSV()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToHSL()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            if (DoAdaption)
            {
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToYCbCr()
        {
            if (DoAdaption)
            {
                YCbCrToRGB(ValArr1, ValArr2);
                RGBToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void YCbCrToGray()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToLCH99()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToLCH99b()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToDEF()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void YCbCrToBef()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void YCbCrToBCH()
        {
            YCbCrToRGB(ValArr1, ValArr2);
            RGBToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region Gray

        private void GrayToXYZ()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void GrayToYxy()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void GrayToLab()
        {
            GrayToLab(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void GrayToLuv()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void GrayToLCHab()
        {
            GrayToLab(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToLCHuv()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToRGB()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void GrayToHSV()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToHSL()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToYCbCr()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToGray()
        {
            if (DoAdaption)
            {
                GrayToLab(ValArr1, ref ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void GrayToLCH99()
        {
            GrayToLab(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToLCH99b()
        {
            GrayToLab(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToDEF()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void GrayToBef()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void GrayToBCH()
        {
            GrayToLab(ValArr1, ref ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region LCH99

        private void LCH99ToXYZ()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void LCH99ToYxy()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99ToLab()
        {
            LCH99ToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void LCH99ToLuv()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99ToLCHab()
        {
            LCH99ToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToLCHuv()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToRGB()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99ToHSV()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToHSL()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToYCbCr()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToGray()
        {
            LCH99ToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToLCH99()
        {
            if (DoAdaption)
            {
                LCH99ToLab(ValArr1, ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99ToLCH99b()
        {
            LCH99ToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToDEF()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99ToBef()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99ToBCH()
        {
            LCH99ToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region LCH99b

        private void LCH99bToXYZ()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void LCH99bToYxy()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99bToLab()
        {
            LCH99bToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void LCH99bToLuv()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99bToLCHab()
        {
            LCH99bToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToLCHuv()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToRGB()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99bToHSV()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToHSL()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToYCbCr()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToGray()
        {
            LCH99bToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToLCH99()
        {
            LCH99bToLab(ValArr1, ValArr2);
            if (DoAdaption)
            {
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToLCH99b()
        {
            if (DoAdaption)
            {
                LCH99bToLab(ValArr1, ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99bToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToDEF()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99bToBef()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99bToBCH()
        {
            LCH99bToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region LCH99c

        private void LCH99cToXYZ()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99cToYxy()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99cToLab()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99cToLuv()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99cToLCHab()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToLCHuv()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToRGB()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99cToHSV()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToHSL()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToYCbCr()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToGray()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToLCH99()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToLCH99b()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToLCH99c()
        {
            if (DoAdaption)
            {
                LCH99cToLab(ValArr1, ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
                ChromaticAdaption(ValArr1, ref  ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99cToLCH99d()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToDEF()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99cToBef()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99cToBCH()
        {
            LCH99cToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.1 * ValArr1[2]) / 1.1;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region LCH99d

        private void LCH99dToXYZ()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99dToYxy()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99dToLab()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99dToLuv()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99dToLCHab()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToLCHuv()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToRGB()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99dToHSV()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToHSL()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToYCbCr()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToGray()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToLCH99()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToLCH99b()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToLCH99c()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToLCH99d()
        {
            if (DoAdaption)
            {
                LCH99dToLab(ValArr1, ValArr2);
                LabToXYZ(ValArr2, ValArr1);
                ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
                ChromaticAdaption(ValArr1, ref  ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void LCH99dToDEF()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
        }

        private void LCH99dToBef()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void LCH99dToBCH()
        {
            LCH99dToLab(ValArr1, ValArr2);
            LabToXYZ(ValArr2, ValArr1);
            ValArr1[0] = (ValArr1[0] + 0.12 * ValArr1[2]) / 1.12;
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        #endregion

        #region DEF

        private void DEFToXYZ()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption) { ChromaticAdaption(ValArr2, ref  ValArr1); EndArr = 1; }
            else { EndArr = 2; }
        }

        private void DEFToYxy()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToYxy(ValArr2, ValArr1); EndArr = 1; }
        }

        private void DEFToLab()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLab(ValArr2, ValArr1); EndArr = 1; }
        }

        private void DEFToLuv()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { XYZToLuv(ValArr2, ValArr1); EndArr = 1; }
        }

        private void DEFToLCHab()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToLCHuv()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToRGB()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void DEFToHSV()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToHSL()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToYCbCr()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToGray()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToLCH99()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToLCH99b()
        {
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            DEFToXYZ(ValArr1, ref ValArr2);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr2, ref ValArr1);
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToDEF()
        {
            if (DoAdaption)
            {
                DEFToXYZ(ValArr1, ref ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void DEFToBef()
        {
            if (DoAdaption)
            {
                DEFToXYZ(ValArr1, ref ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBef(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void DEFToBCH()
        {
            if (DoAdaption)
            {
                DEFToXYZ(ValArr1, ref ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToDEF(ValArr1, ref ValArr2);
                DEFToBCH(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        #endregion

        #region Bef

        private void BefToXYZ()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void BefToYxy()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BefToLab()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BefToLuv()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BefToLCHab()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToLCHuv()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToRGB()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BefToHSV()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToHSL()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToYCbCr()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToGray()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BefToLCH99()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToLCH99b()
        {
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            BefToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BefToDEF()
        {
            BefToDEF(ValArr1, ValArr2);
            if (DoAdaption)
            {
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void BefToBef()
        {
            if (DoAdaption)
            {
                BefToDEF(ValArr1, ValArr2);
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        private void BefToBCH()
        {
            BefToDEF(ValArr1, ValArr2);
            if (DoAdaption)
            {
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { DEFToBCH(ValArr2, ValArr1); EndArr = 1; }
        }

        #endregion

        #region BCH

        private void BCHToXYZ()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption) { ChromaticAdaption(ValArr1, ref  ValArr2); EndArr = 2; }
            else { EndArr = 1; }
        }

        private void BCHToYxy()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToYxy(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToYxy(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BCHToLab()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BCHToLuv()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BCHToLCHab()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCHab(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCHab(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToLCHuv()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLuv(ValArr2, ValArr1);
                LuvToLCHuv(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLuv(ValArr1, ValArr2);
                LuvToLCHuv(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToRGB()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BCHToHSV()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSV(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSV(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToHSL()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToHSL(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToHSL(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToYCbCr()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToRGB(ValArr2, ValArr1);
                RGBToYCbCr(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToRGB(ValArr1, ValArr2);
                RGBToYCbCr(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToGray()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                LabToXYZ(ValArr1, ValArr2);
                ChromaticAdaption(ValArr2, ref  ValArr1);
                XYZToLab(ValArr1, ValArr2);
                LabToGray(ValArr2, ValArr1);
                EndArr = 1;
            }
            else
            {
                LabToGray(ValArr1, ValArr2);
                EndArr = 2;
            }
        }

        private void BCHToLCH99()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToLCH99b()
        {
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99b(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99b(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToLCH99c()
        {
            //X'= 1.1 * X − 0.1 * Z
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.1 - ValArr2[2] * 0.1;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99c(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.1 - ValArr1[2] * 0.1;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99c(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToLCH99d()
        {
            //X'= 1.12 * X − 0.12 * Z
            BCHToDEF(ValArr1, ValArr2);
            DEFToXYZ(ValArr2, ref ValArr1);
            if (DoAdaption)
            {
                ChromaticAdaption(ValArr1, ref ValArr2);
                ValArr2[0] = ValArr2[0] * 1.12 - ValArr2[2] * 0.12;
                XYZToLab(ValArr2, ValArr1);
                LabToLCH99d(ValArr1, ValArr2);
                EndArr = 2;
            }
            else
            {
                ValArr1[0] = ValArr1[0] * 1.12 - ValArr1[2] * 0.12;
                XYZToLab(ValArr1, ValArr2);
                LabToLCH99d(ValArr2, ValArr1);
                EndArr = 1;
            }
        }

        private void BCHToDEF()
        {
            BCHToDEF(ValArr1, ValArr2);
            if (DoAdaption)
            {
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                EndArr = 1;
            }
            else { EndArr = 2; }
        }

        private void BCHToBef()
        {
            BCHToDEF(ValArr1, ValArr2);
            if (DoAdaption)
            {
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBef(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { DEFToBef(ValArr2, ValArr1); EndArr = 1; }
        }

        private void BCHToBCH()
        {
            if (DoAdaption)
            {
                BCHToDEF(ValArr1, ValArr2);
                DEFToXYZ(ValArr2, ref ValArr1);
                ChromaticAdaption(ValArr1, ref  ValArr2);
                XYZToDEF(ValArr2, ref ValArr1);
                DEFToBCH(ValArr1, ValArr2);
                EndArr = 2;
            }
            else { EndArr = 1; }
        }

        #endregion

        #endregion
        
        #region Fast Conversion

        /// <summary>
        /// Initiates the ConvertFast method.
        /// </summary>
        /// <param name="InColor">The type of color that will be used for the conversion</param>
        /// <param name="OutColor">The colormodel that is expected as output</param>
        public void SetFast(Color InColor, ColorModel OutColor)
        {
            OutputModel = OutColor;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetFastReferenceWhite(ReferenceWhite, RGBSpace);
            SetInputColor(InColor);
            SetFastAction();
        }

        /// <summary>
        /// Initiates the ConvertFast method.
        /// </summary>
        /// <param name="InColor">The type of color that will be used for the conversion</param>
        /// <param name="OutColor">The colormodel that is expected as output</param>
        /// <param name="OutRefWhite">The reference white for the output color</param>
        public void SetFast(Color InColor, ColorModel OutColor, Whitepoint OutRefWhite)
        {
            OutputModel = OutColor;
            RGBSpace = StandardColorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetFastReferenceWhite(OutRefWhite, RGBSpace);
            SetInputColor(InColor);
            SetFastAction();
        }

        /// <summary>
        /// Initiates the ConvertFast method.
        /// </summary>
        /// <param name="InColor">The type of color that will be used for the conversion</param>
        /// <param name="OutColor">The colormodel that is expected as output</param>
        /// <param name="colorspace">The colorspace for RGB output color</param>
        public void SetFast(Color InColor, ColorModel OutColor, RGBSpaceName colorspace)
        {
            if (colorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = OutColor;
            RGBSpace = colorspace;
            YCbCrSpace = StandardYCbCrSpace;
            SetFastReferenceWhite(ReferenceWhite, RGBSpace);
            SetInputColor(InColor);
            SetFastAction();
        }

        /// <summary>
        /// Initiates the ConvertFast method.
        /// </summary>
        /// <param name="InColor">The type of color that will be used for the conversion</param>
        /// <param name="OutColor">The colormodel that is expected as output</param>
        /// <param name="BaseColorspace">The base RGB colorspace for YCbCr output color</param>
        /// <param name="colorspace">The colorspace for YCbCr output color</param>
        public void SetFast(Color InColor, ColorModel OutColor, RGBSpaceName BaseColorspace, YCbCrSpaceName colorspace)
        {
            if (colorspace == YCbCrSpaceName.ICC || BaseColorspace == RGBSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = OutColor;
            RGBSpace = BaseColorspace;
            YCbCrSpace = colorspace;
            SetFastReferenceWhite(ReferenceWhite, RGBSpace);
            SetInputColor(InColor);
            SetFastAction();
        }

        /// <summary>
        /// Initiates the ConvertFast method.
        /// </summary>
        /// <param name="InColor">The type of color that will be used for the conversion</param>
        /// <param name="OutColor">The colormodel that is expected as output</param>
        /// <param name="colorspace">The colorspace for YCbCr output color</param>
        public void SetFast(Color InColor, ColorModel OutColor, YCbCrSpaceName colorspace)
        {
            if (colorspace == YCbCrSpaceName.ICC) throw new ArgumentException("ICC colorspace is not accepted with this method. Use the explicit ICC conversion.");
            OutputModel = OutColor;
            RGBSpace = StandardColorspace;
            YCbCrSpace = colorspace;
            SetFastReferenceWhite(ReferenceWhite, RGBSpace);
            SetInputColor(InColor);
            SetFastAction();
        }

        private void SetFastReferenceWhite(Whitepoint RefWhite, RGBSpaceName colorspace)
        {
            switch (OutputModel)
            {
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.CIELab:
                case ColorModel.CIELuv:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                case ColorModel.DEF:
                case ColorModel.Bef:
                case ColorModel.BCH:
                case ColorModel.Gray:
                    OutReferenceWhite = RefWhite;
                    break;

                case ColorModel.RGB:
                case ColorModel.HSV:
                case ColorModel.HSL:
                case ColorModel.YCbCr:
                    OutReferenceWhite = WhitepointArr[(int)RGBColorspace.GetWhitepointName(StandardColorspace)];
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SetFastAction()
        {
            switch (InputModel)
            {
                case ColorModel.CIEXYZ:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = XYZToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = XYZToYxy; break;
                        case ColorModel.CIELab: FastAction = XYZToLab; break;
                        case ColorModel.CIELuv: FastAction = XYZToLuv; break;
                        case ColorModel.CIELCHab: FastAction = XYZToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = XYZToLCHuv; break;
                        case ColorModel.RGB: FastAction = XYZToRGB; break;
                        case ColorModel.HSV: FastAction = XYZToHSV; break;
                        case ColorModel.HSL: FastAction = XYZToHSL; break;
                        case ColorModel.Gray: FastAction = XYZToGray; break;
                        case ColorModel.YCbCr: FastAction = XYZToYCbCr; break;
                        case ColorModel.LCH99: FastAction = XYZToLCH99; break;
                        case ColorModel.LCH99b: FastAction = XYZToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = XYZToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = XYZToLCH99d; break;
                        case ColorModel.DEF: FastAction = XYZToDEF; break;
                        case ColorModel.Bef: FastAction = XYZToBef; break;
                        case ColorModel.BCH: FastAction = XYZToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.CIEYxy:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = YxyToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = YxyToYxy; break;
                        case ColorModel.CIELab: FastAction = YxyToLab; break;
                        case ColorModel.CIELuv: FastAction = YxyToLuv; break;
                        case ColorModel.CIELCHab: FastAction = YxyToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = YxyToLCHuv; break;
                        case ColorModel.RGB: FastAction = YxyToRGB; break;
                        case ColorModel.HSV: FastAction = YxyToHSV; break;
                        case ColorModel.HSL: FastAction = YxyToHSL; break;
                        case ColorModel.Gray: FastAction = YxyToGray; break;
                        case ColorModel.YCbCr: FastAction = YxyToYCbCr; break;
                        case ColorModel.LCH99: FastAction = YxyToLCH99; break;
                        case ColorModel.LCH99b: FastAction = YxyToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = YxyToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = YxyToLCH99d; break;
                        case ColorModel.DEF: FastAction = YxyToDEF; break;
                        case ColorModel.Bef: FastAction = YxyToBef; break;
                        case ColorModel.BCH: FastAction = YxyToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.CIELab:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LabToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LabToYxy; break;
                        case ColorModel.CIELab: FastAction = LabToLab; break;
                        case ColorModel.CIELuv: FastAction = LabToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LabToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LabToLCHuv; break;
                        case ColorModel.RGB: FastAction = LabToRGB; break;
                        case ColorModel.HSV: FastAction = LabToHSV; break;
                        case ColorModel.HSL: FastAction = LabToHSL; break;
                        case ColorModel.Gray: FastAction = LabToGray; break;
                        case ColorModel.YCbCr: FastAction = LabToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LabToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LabToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LabToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LabToLCH99d; break;
                        case ColorModel.DEF: FastAction = LabToDEF; break;
                        case ColorModel.Bef: FastAction = LabToBef; break;
                        case ColorModel.BCH: FastAction = LabToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.CIELuv:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LuvToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LuvToYxy; break;
                        case ColorModel.CIELab: FastAction = LuvToLab; break;
                        case ColorModel.CIELuv: FastAction = LuvToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LuvToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LuvToLCHuv; break;
                        case ColorModel.RGB: FastAction = LuvToRGB; break;
                        case ColorModel.HSV: FastAction = LuvToHSV; break;
                        case ColorModel.HSL: FastAction = LuvToHSL; break;
                        case ColorModel.Gray: FastAction = LuvToGray; break;
                        case ColorModel.YCbCr: FastAction = LuvToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LuvToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LuvToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LuvToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LuvToLCH99d; break;
                        case ColorModel.DEF: FastAction = LuvToDEF; break;
                        case ColorModel.Bef: FastAction = LuvToBef; break;
                        case ColorModel.BCH: FastAction = LuvToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.CIELCHab:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCHabToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCHabToYxy; break;
                        case ColorModel.CIELab: FastAction = LCHabToLab; break;
                        case ColorModel.CIELuv: FastAction = LCHabToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCHabToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCHabToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCHabToRGB; break;
                        case ColorModel.HSV: FastAction = LCHabToHSV; break;
                        case ColorModel.HSL: FastAction = LCHabToHSL; break;
                        case ColorModel.Gray: FastAction = LCHabToGray; break;
                        case ColorModel.YCbCr: FastAction = LCHabToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCHabToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCHabToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCHabToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCHabToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCHabToDEF; break;
                        case ColorModel.Bef: FastAction = LCHabToBef; break;
                        case ColorModel.BCH: FastAction = LCHabToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.CIELCHuv:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCHuvToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCHuvToYxy; break;
                        case ColorModel.CIELab: FastAction = LCHuvToLab; break;
                        case ColorModel.CIELuv: FastAction = LCHuvToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCHuvToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCHuvToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCHuvToRGB; break;
                        case ColorModel.HSV: FastAction = LCHuvToHSV; break;
                        case ColorModel.HSL: FastAction = LCHuvToHSL; break;
                        case ColorModel.Gray: FastAction = LCHuvToGray; break;
                        case ColorModel.YCbCr: FastAction = LCHuvToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCHuvToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCHuvToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCHuvToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCHuvToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCHuvToDEF; break;
                        case ColorModel.Bef: FastAction = LCHuvToBef; break;
                        case ColorModel.BCH: FastAction = LCHuvToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.LCH99:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCH99ToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCH99ToYxy; break;
                        case ColorModel.CIELab: FastAction = LCH99ToLab; break;
                        case ColorModel.CIELuv: FastAction = LCH99ToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCH99ToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCH99ToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCH99ToRGB; break;
                        case ColorModel.HSV: FastAction = LCH99ToHSV; break;
                        case ColorModel.HSL: FastAction = LCH99ToHSL; break;
                        case ColorModel.Gray: FastAction = LCH99ToGray; break;
                        case ColorModel.YCbCr: FastAction = LCH99ToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCH99ToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCH99ToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCH99ToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCH99ToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCH99ToDEF; break;
                        case ColorModel.Bef: FastAction = LCH99ToBef; break;
                        case ColorModel.BCH: FastAction = LCH99ToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.LCH99b:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCH99bToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCH99bToYxy; break;
                        case ColorModel.CIELab: FastAction = LCH99bToLab; break;
                        case ColorModel.CIELuv: FastAction = LCH99bToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCH99bToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCH99bToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCH99bToRGB; break;
                        case ColorModel.HSV: FastAction = LCH99bToHSV; break;
                        case ColorModel.HSL: FastAction = LCH99bToHSL; break;
                        case ColorModel.Gray: FastAction = LCH99bToGray; break;
                        case ColorModel.YCbCr: FastAction = LCH99bToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCH99bToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCH99bToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCH99bToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCH99bToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCH99bToDEF; break;
                        case ColorModel.Bef: FastAction = LCH99bToBef; break;
                        case ColorModel.BCH: FastAction = LCH99bToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.LCH99c:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCH99cToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCH99cToYxy; break;
                        case ColorModel.CIELab: FastAction = LCH99cToLab; break;
                        case ColorModel.CIELuv: FastAction = LCH99cToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCH99cToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCH99cToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCH99cToRGB; break;
                        case ColorModel.HSV: FastAction = LCH99cToHSV; break;
                        case ColorModel.HSL: FastAction = LCH99cToHSL; break;
                        case ColorModel.Gray: FastAction = LCH99cToGray; break;
                        case ColorModel.YCbCr: FastAction = LCH99cToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCH99cToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCH99cToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCH99cToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCH99cToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCH99cToDEF; break;
                        case ColorModel.Bef: FastAction = LCH99cToBef; break;
                        case ColorModel.BCH: FastAction = LCH99cToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.LCH99d:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = LCH99dToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = LCH99dToYxy; break;
                        case ColorModel.CIELab: FastAction = LCH99dToLab; break;
                        case ColorModel.CIELuv: FastAction = LCH99dToLuv; break;
                        case ColorModel.CIELCHab: FastAction = LCH99dToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = LCH99dToLCHuv; break;
                        case ColorModel.RGB: FastAction = LCH99dToRGB; break;
                        case ColorModel.HSV: FastAction = LCH99dToHSV; break;
                        case ColorModel.HSL: FastAction = LCH99dToHSL; break;
                        case ColorModel.Gray: FastAction = LCH99dToGray; break;
                        case ColorModel.YCbCr: FastAction = LCH99dToYCbCr; break;
                        case ColorModel.LCH99: FastAction = LCH99dToLCH99; break;
                        case ColorModel.LCH99b: FastAction = LCH99dToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = LCH99dToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = LCH99dToLCH99d; break;
                        case ColorModel.DEF: FastAction = LCH99dToDEF; break;
                        case ColorModel.Bef: FastAction = LCH99dToBef; break;
                        case ColorModel.BCH: FastAction = LCH99dToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.RGB:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = RGBToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = RGBToYxy; break;
                        case ColorModel.CIELab: FastAction = RGBToLab; break;
                        case ColorModel.CIELuv: FastAction = RGBToLuv; break;
                        case ColorModel.CIELCHab: FastAction = RGBToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = RGBToLCHuv; break;
                        case ColorModel.RGB: FastAction = RGBToRGB; break;
                        case ColorModel.HSV: FastAction = RGBToHSV; break;
                        case ColorModel.HSL: FastAction = RGBToHSL; break;
                        case ColorModel.Gray: FastAction = RGBToGray; break;
                        case ColorModel.YCbCr: FastAction = RGBToYCbCr; break;
                        case ColorModel.LCH99: FastAction = RGBToLCH99; break;
                        case ColorModel.LCH99b: FastAction = RGBToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = RGBToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = RGBToLCH99d; break;
                        case ColorModel.DEF: FastAction = RGBToDEF; break;
                        case ColorModel.Bef: FastAction = RGBToBef; break;
                        case ColorModel.BCH: FastAction = RGBToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.HSV:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = HSVToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = HSVToYxy; break;
                        case ColorModel.CIELab: FastAction = HSVToLab; break;
                        case ColorModel.CIELuv: FastAction = HSVToLuv; break;
                        case ColorModel.CIELCHab: FastAction = HSVToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = HSVToLCHuv; break;
                        case ColorModel.RGB: FastAction = HSVToRGB; break;
                        case ColorModel.HSV: FastAction = HSVToHSV; break;
                        case ColorModel.HSL: FastAction = HSVToHSL; break;
                        case ColorModel.Gray: FastAction = HSVToGray; break;
                        case ColorModel.YCbCr: FastAction = HSVToYCbCr; break;
                        case ColorModel.LCH99: FastAction = HSVToLCH99; break;
                        case ColorModel.LCH99b: FastAction = HSVToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = HSVToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = HSVToLCH99d; break;
                        case ColorModel.DEF: FastAction = HSVToDEF; break;
                        case ColorModel.Bef: FastAction = HSVToBef; break;
                        case ColorModel.BCH: FastAction = HSVToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.HSL:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = HSLToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = HSLToYxy; break;
                        case ColorModel.CIELab: FastAction = HSLToLab; break;
                        case ColorModel.CIELuv: FastAction = HSLToLuv; break;
                        case ColorModel.CIELCHab: FastAction = HSLToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = HSLToLCHuv; break;
                        case ColorModel.RGB: FastAction = HSLToRGB; break;
                        case ColorModel.HSV: FastAction = HSLToHSV; break;
                        case ColorModel.HSL: FastAction = HSLToHSL; break;
                        case ColorModel.Gray: FastAction = HSLToGray; break;
                        case ColorModel.YCbCr: FastAction = HSLToYCbCr; break;
                        case ColorModel.LCH99: FastAction = HSLToLCH99; break;
                        case ColorModel.LCH99b: FastAction = HSLToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = HSLToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = HSLToLCH99d; break;
                        case ColorModel.DEF: FastAction = HSLToDEF; break;
                        case ColorModel.Bef: FastAction = HSLToBef; break;
                        case ColorModel.BCH: FastAction = HSLToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.Gray:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = GrayToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = GrayToYxy; break;
                        case ColorModel.CIELab: FastAction = GrayToLab; break;
                        case ColorModel.CIELuv: FastAction = GrayToLuv; break;
                        case ColorModel.CIELCHab: FastAction = GrayToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = GrayToLCHuv; break;
                        case ColorModel.RGB: FastAction = GrayToRGB; break;
                        case ColorModel.HSV: FastAction = GrayToHSV; break;
                        case ColorModel.HSL: FastAction = GrayToHSL; break;
                        case ColorModel.Gray: FastAction = GrayToGray; break;
                        case ColorModel.YCbCr: FastAction = GrayToYCbCr; break;
                        case ColorModel.LCH99: FastAction = GrayToLCH99; break;
                        case ColorModel.LCH99b: FastAction = GrayToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = GrayToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = GrayToLCH99d; break;
                        case ColorModel.DEF: FastAction = GrayToDEF; break;
                        case ColorModel.Bef: FastAction = GrayToBef; break;
                        case ColorModel.BCH: FastAction = GrayToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.YCbCr:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = YCbCrToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = YCbCrToYxy; break;
                        case ColorModel.CIELab: FastAction = YCbCrToLab; break;
                        case ColorModel.CIELuv: FastAction = YCbCrToLuv; break;
                        case ColorModel.CIELCHab: FastAction = YCbCrToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = YCbCrToLCHuv; break;
                        case ColorModel.RGB: FastAction = YCbCrToRGB; break;
                        case ColorModel.HSV: FastAction = YCbCrToHSV; break;
                        case ColorModel.HSL: FastAction = YCbCrToHSL; break;
                        case ColorModel.Gray: FastAction = YCbCrToGray; break;
                        case ColorModel.YCbCr: FastAction = YCbCrToYCbCr; break;
                        case ColorModel.LCH99: FastAction = YCbCrToLCH99; break;
                        case ColorModel.LCH99b: FastAction = YCbCrToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = YCbCrToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = YCbCrToLCH99d; break;
                        case ColorModel.DEF: FastAction = YCbCrToDEF; break;
                        case ColorModel.Bef: FastAction = YCbCrToBef; break;
                        case ColorModel.BCH: FastAction = YCbCrToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.DEF:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = DEFToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = DEFToYxy; break;
                        case ColorModel.CIELab: FastAction = DEFToLab; break;
                        case ColorModel.CIELuv: FastAction = DEFToLuv; break;
                        case ColorModel.CIELCHab: FastAction = DEFToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = DEFToLCHuv; break;
                        case ColorModel.RGB: FastAction = DEFToRGB; break;
                        case ColorModel.HSV: FastAction = DEFToHSV; break;
                        case ColorModel.HSL: FastAction = DEFToHSL; break;
                        case ColorModel.Gray: FastAction = DEFToGray; break;
                        case ColorModel.YCbCr: FastAction = DEFToYCbCr; break;
                        case ColorModel.LCH99: FastAction = DEFToLCH99; break;
                        case ColorModel.LCH99b: FastAction = DEFToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = DEFToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = DEFToLCH99d; break;
                        case ColorModel.DEF: FastAction = DEFToDEF; break;
                        case ColorModel.Bef: FastAction = DEFToBef; break;
                        case ColorModel.BCH: FastAction = DEFToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.Bef:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = BefToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = BefToYxy; break;
                        case ColorModel.CIELab: FastAction = BefToLab; break;
                        case ColorModel.CIELuv: FastAction = BefToLuv; break;
                        case ColorModel.CIELCHab: FastAction = BefToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = BefToLCHuv; break;
                        case ColorModel.RGB: FastAction = BefToRGB; break;
                        case ColorModel.HSV: FastAction = BefToHSV; break;
                        case ColorModel.HSL: FastAction = BefToHSL; break;
                        case ColorModel.Gray: FastAction = BefToGray; break;
                        case ColorModel.YCbCr: FastAction = BefToYCbCr; break;
                        case ColorModel.LCH99: FastAction = BefToLCH99; break;
                        case ColorModel.LCH99b: FastAction = BefToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = BefToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = BefToLCH99d; break;
                        case ColorModel.DEF: FastAction = BefToDEF; break;
                        case ColorModel.Bef: FastAction = BefToBef; break;
                        case ColorModel.BCH: FastAction = BefToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case ColorModel.BCH:
                    switch (OutputModel)
                    {
                        case ColorModel.CIEXYZ: FastAction = BCHToXYZ; break;
                        case ColorModel.CIEYxy: FastAction = BCHToYxy; break;
                        case ColorModel.CIELab: FastAction = BCHToLab; break;
                        case ColorModel.CIELuv: FastAction = BCHToLuv; break;
                        case ColorModel.CIELCHab: FastAction = BCHToLCHab; break;
                        case ColorModel.CIELCHuv: FastAction = BCHToLCHuv; break;
                        case ColorModel.RGB: FastAction = BCHToRGB; break;
                        case ColorModel.HSV: FastAction = BCHToHSV; break;
                        case ColorModel.HSL: FastAction = BCHToHSL; break;
                        case ColorModel.Gray: FastAction = BCHToGray; break;
                        case ColorModel.YCbCr: FastAction = BCHToYCbCr; break;
                        case ColorModel.LCH99: FastAction = BCHToLCH99; break;
                        case ColorModel.LCH99b: FastAction = BCHToLCH99b; break;
                        case ColorModel.LCH99c: FastAction = BCHToLCH99c; break;
                        case ColorModel.LCH99d: FastAction = BCHToLCH99d; break;
                        case ColorModel.DEF: FastAction = BCHToDEF; break;
                        case ColorModel.Bef: FastAction = BCHToBef; break;
                        case ColorModel.BCH: FastAction = BCHToBCH; break;

                        default:
                            throw new NotImplementedException();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts a color faster than the normal approach. This method has to be initiated first with the SetFast method.
        /// </summary>
        /// <param name="InColor">The color that will be converter</param>
        /// <returns>the converted color</returns>
        public Color ConvertFast(Color InColor)
        {
            if (InColor.Model != InputModel) throw new ArgumentException("Input color model does not match initiated model");

            if (InputModel != ColorModel.Gray)
            {
                ValArr1 = InColor.ColorArray;
                ValArr2 = InColor.ColorArray;
            }
            else
            {
                ValArr1 = new double[] { ((ColorGray)InColor).G, 0, 0 };
                ValArr2 = new double[] { ValArr1[0], 0, 0 };
            }

            FastAction();
            if (EndArr == 1) varArr = ValArr1;
            else varArr = ValArr2;

            switch (OutputModel)
            {
                case ColorModel.CIEXYZ: return new ColorXYZ(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.CIEYxy: return new ColorYxy(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.CIELab: return new ColorLab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.CIELuv: return new ColorLuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.CIELCHab: return new ColorLCHab(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.CIELCHuv: return new ColorLCHuv(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.RGB: return new ColorRGB(RGBSpace, varArr[0], varArr[1], varArr[2]);
                case ColorModel.HSV: return new ColorHSV(RGBSpace, varArr[0], varArr[1], varArr[2]);
                case ColorModel.HSL: return new ColorHSL(RGBSpace, varArr[0], varArr[1], varArr[2]);
                case ColorModel.Gray: return new ColorGray(OutReferenceWhite, varArr[0]);
                case ColorModel.YCbCr: return new ColorYCbCr(YCbCrSpace, RGBSpace, varArr[0], varArr[1], varArr[2]);
                case ColorModel.LCH99: return new ColorLCH99(varArr[0], varArr[1], varArr[2]);
                case ColorModel.LCH99b: return new ColorLCH99b(varArr[0], varArr[1], varArr[2]);
                case ColorModel.LCH99c: return new ColorLCH99c(varArr[0], varArr[1], varArr[2]);
                case ColorModel.LCH99d: return new ColorLCH99d(varArr[0], varArr[1], varArr[2]);
                case ColorModel.DEF: return new ColorDEF(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.Bef: return new ColorBef(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);
                case ColorModel.BCH: return new ColorBCH(OutReferenceWhite, varArr[0], varArr[1], varArr[2]);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Conversion Code

        #region Yxy-XYZ

        private void YxyToXYZ(double[] InArr, double[] OutArr)
        {
            OutArr[1] = InArr[0];
            if (InArr[2] == 0)
            {
                OutArr[0] = 0;
                OutArr[1] = 0;
                OutArr[2] = 0;
            }
            else
            {
                OutArr[0] = (InArr[1] * InArr[0]) / InArr[2];
                OutArr[2] = ((1 - InArr[1] - InArr[2]) * InArr[0]) / InArr[2];
            }
        }

        private void XYZToYxy(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[1];
            if (InArr[0] + InArr[1] + InArr[2] == 0)
            {
                OutArr[1] = OutReferenceWhite.Cx;
                OutArr[2] = OutReferenceWhite.Cy;
            }
            else
            {
                OutArr[1] = InArr[0] / (InArr[0] + InArr[1] + InArr[2]);
                OutArr[2] = InArr[1] / (InArr[0] + InArr[1] + InArr[2]);
            }
        }

        #endregion

        #region Lab-XYZ

        private void LabToXYZ(double[] InArr, double[] OutArr)
        {
            var1 = Fx_LabToXYZ(InArr[1], InArr[0]);
            var2 = Fz_LabToXYZ(InArr[2], InArr[0]);
            var3 = Pow3(var1); var3 = (var3 > Epsilon) ? var3 : ((116d * var1) - 16d) / Kappa;
            var4 = (InArr[0] > Kappa * Epsilon) ? Pow3((InArr[0] + 16d) / 116d) : InArr[0] / Kappa;
            var5 = Pow3(var2); var5 = (var5 > Epsilon) ? var5 : ((116d * var2) - 16d) / Kappa;
            OutArr[0] = var3 * InputWhitepoint.X;
            OutArr[1] = var4 * InputWhitepoint.Y;
            OutArr[2] = var5 * InputWhitepoint.Z;
        }

        private void XYZToLab(double[] InArr, double[] OutArr)
        {
            var1 = InArr[1] / OutReferenceWhite.Y;
            OutArr[0] = (116 * Fn_XYZToLab(var1)) - 16;
            OutArr[1] = 500 * (Fn_XYZToLab(InArr[0] / OutReferenceWhite.X) - Fn_XYZToLab(var1));
            OutArr[2] = 200 * (Fn_XYZToLab(var1) - Fn_XYZToLab(InArr[2] / OutReferenceWhite.Z));
        }

        #endregion

        #region RGB-XYZ

        private void RGBToXYZ(double[] InArr, double[] OutArr)
        {
            if (!IsRGBLinear)
            {
                InArr[0] = InputRGBSpace.ToLinear(InArr[0]);
                InArr[1] = InputRGBSpace.ToLinear(InArr[1]);
                InArr[2] = InputRGBSpace.ToLinear(InArr[2]);
                IsRGBLinear = true;
            }
            varArr = mmath.MultiplyMatrix(InputRGBSpace.CM, InArr);
            OutArr[0] = varArr[0]; OutArr[1] = varArr[1]; OutArr[2] = varArr[2];
        }

        private void XYZToRGB(double[] InArr, double[] OutArr)
        {
            varArr = mmath.MultiplyMatrix(RGBspaceArr[(int)RGBSpace].ICM, InArr);
            OutArr[0] = varArr[0]; OutArr[1] = varArr[1]; OutArr[2] = varArr[2];

            OutArr[0] = RGBspaceArr[(int)RGBSpace].ToNonLinear(OutArr[0]);
            OutArr[1] = RGBspaceArr[(int)RGBSpace].ToNonLinear(OutArr[1]);
            OutArr[2] = RGBspaceArr[(int)RGBSpace].ToNonLinear(OutArr[2]);
            IsRGBLinear = false;
        }

        #endregion

        #region Luv-XYZ

        private void LuvToXYZ(double[] InArr, double[] OutArr)
        {
            OutArr[1] = (InArr[0] > KapEps) ? Pow3((InArr[0] + 16) / 116d) : InArr[0] / Kappa;
            varArr = InputWhitepoint.ValueArray;
            var4 = (4 * varArr[0]) / (varArr[0] + 15 * varArr[1] + 3 * varArr[2]);
            var5 = (9 * varArr[1]) / (varArr[0] + 15 * varArr[1] + 3 * varArr[2]);
            var1 = (((52 * InArr[0]) / (InArr[1] + 13 * InArr[0] * var4)) - 1) / 3d;
            var2 = -5 * OutArr[1];
            var3 = (((39 * InArr[0]) / (InArr[2] + 13 * InArr[0] * var5)) - 5) * OutArr[1];
            OutArr[0] = (var3 - var2) / (var1 + 0.33333333333333333333333333333333);
            OutArr[2] = OutArr[0] * var1 + var2;
        }

        private void XYZToLuv(double[] InArr, double[] OutArr)
        {
            varArr = OutReferenceWhite.ValueArray;
            var1 = InArr[1] / varArr[1];
            var2 = (4 * InArr[0]) / (InArr[0] + 15 * InArr[1] + 3 * InArr[2]);
            var3 = (9 * InArr[1]) / (InArr[0] + 15 * InArr[1] + 3 * InArr[2]);
            var4 = (4 * varArr[0]) / (varArr[0] + 15 * varArr[1] + 3 * varArr[2]);
            var5 = (9 * varArr[1]) / (varArr[0] + 15 * varArr[1] + 3 * varArr[2]);
            OutArr[0] = (var1 > Epsilon) ? (116d * Math.Pow(var1, 0.33333333333333333333333333333333)) - 16d : Kappa * var1;
            OutArr[1] = 13 * OutArr[0] * (var2 - var4);
            OutArr[2] = 13 * OutArr[0] * (var3 - var5);
        }

        #endregion

        #region LCHab-Lab

        private void LCHabToLab(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[0];
            OutArr[1] = InArr[1] * Math.Cos(InArr[2] * Pi180);
            OutArr[2] = InArr[1] * Math.Sin(InArr[2] * Pi180);
        }

        private void LabToLCHab(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[0];
            OutArr[1] = Math.Sqrt(Pow2(InArr[1]) + Pow2(InArr[2]));
            OutArr[2] = Math.Atan2(InArr[2], InArr[1]) * Pi180_1;
        }

        #endregion

        #region LCHuv-Luv

        private void LCHuvToLuv(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[0];
            OutArr[1] = InArr[1] * Math.Cos(InArr[2] * Pi180);
            OutArr[2] = InArr[1] * Math.Sin(InArr[2] * Pi180);
        }

        private void LuvToLCHuv(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[0];
            OutArr[1] = Math.Sqrt(Pow2(InArr[1]) + Pow2(InArr[2]));
            OutArr[2] = Math.Atan2(InArr[2], InArr[1]) * Pi180_1;
        }

        #endregion

        #region LCH99-Lab

        private void LCH99ToLab(double[] InArr, double[] OutArr)
        {
            DIN99Lab(0, InArr);
            OutArr[1] = var5;
            OutArr[2] = var6;
            OutArr[0] = var7;
        }

        private void LabToLCH99(double[] InArr, double[] OutArr)
        {
            LabDIN99(0, InArr);
            OutArr[0] = var1;
            OutArr[1] = var4;
            OutArr[2] = var5;
        }

        #endregion

        #region LCH99b-Lab

        private void LCH99bToLab(double[] InArr, double[] OutArr)
        {
            DIN99Lab(1, InArr);
            OutArr[1] = var5;
            OutArr[2] = var6;
            OutArr[0] = var7;
        }

        private void LabToLCH99b(double[] InArr, double[] OutArr)
        {
            LabDIN99(1, InArr);
            OutArr[0] = var1;
            OutArr[1] = var4;
            OutArr[2] = var5;
        }

        #endregion

        #region LCH99c-Lab

        private void LCH99cToLab(double[] InArr, double[] OutArr)
        {
            DIN99Lab(2, InArr);
            OutArr[1] = var5;
            OutArr[2] = var6;
            OutArr[0] = var7;
        }

        private void LabToLCH99c(double[] InArr, double[] OutArr)
        {
            LabDIN99(2, InArr);
            OutArr[0] = var1;
            OutArr[1] = var4;
            OutArr[2] = var5;
        }

        #endregion

        #region LCH99d-Lab

        private void LCH99dToLab(double[] InArr, double[] OutArr)
        {
            DIN99Lab(3, InArr);
            OutArr[1] = var5;
            OutArr[2] = var6;
            OutArr[0] = var7;
        }

        private void LabToLCH99d(double[] InArr, double[] OutArr)
        {
            LabDIN99(3, InArr);
            OutArr[0] = var1;
            OutArr[1] = var4;
            OutArr[2] = var5;
        }

        #endregion

        #region HSV-RGB

        private void RGBToHSV(double[] InArr, double[] OutArr)
        {
            if (IsRGBLinear)
            {
                InArr[0] = InputRGBSpace.ToNonLinear(InArr[0]);
                InArr[1] = InputRGBSpace.ToNonLinear(InArr[1]);
                InArr[2] = InputRGBSpace.ToNonLinear(InArr[2]);
                IsRGBLinear = false;
            }

            var1 = Math.Max(InArr[0], Math.Max(InArr[1], InArr[2]));
            var2 = Math.Min(InArr[0], Math.Min(InArr[1], InArr[2]));

            if (Math.Round(var1, 6) == Math.Round(var2, 6)) { OutArr[2] = var2; OutArr[0] = 0; }
            else
            {
                var3 = (InArr[0] == var2) ? InArr[1] - InArr[2] : ((InArr[2] == var2) ? InArr[0] - InArr[1] : InArr[2] - InArr[0]);
                var4 = (InArr[0] == var2) ? 3d : ((InArr[2] == var2) ? 1d : 5d);
                OutArr[0] = 60d * (var4 - var3 / (var1 - var2));
                OutArr[2] = var1;
            }
            OutArr[1] = (var1 == 0) ? 0 : (var1 - var2) / var1;
        }

        private void HSVToRGB(double[] InArr, double[] OutArr)
        {
            if (InArr[1] == 0)
            {
                OutArr[0] = InArr[2];
                OutArr[1] = InArr[2];
                OutArr[2] = InArr[2];
            }
            else
            {
                var1 = (InArr[0] / 360d) * 6;
                var2 = Math.Floor(var1);
                var3 = InArr[2] * (1 - InArr[1]);
                var4 = InArr[2] * (1 - InArr[1] * (var1 - var2));
                var5 = InArr[2] * (1 - InArr[1] * (1 - (var1 - var2)));

                switch ((int)var2)
                {
                    case 6:
                    case 0: OutArr[0] = InArr[2]; OutArr[1] = var5; OutArr[2] = var3; break;
                    case 1: OutArr[0] = var4; OutArr[1] = InArr[2]; OutArr[2] = var3; break;
                    case 2: OutArr[0] = var3; OutArr[1] = InArr[2]; OutArr[2] = var5; break;
                    case 3: OutArr[0] = var3; OutArr[1] = var4; OutArr[2] = InArr[2]; break;
                    case 4: OutArr[0] = var5; OutArr[1] = var3; OutArr[2] = InArr[2]; break;
                    default: OutArr[0] = InArr[2]; OutArr[1] = var3; OutArr[2] = var4; break;
                }
            }
            IsRGBLinear = false;
        }

        #endregion

        #region HSL-RGB

        private void RGBToHSL(double[] InArr, double[] OutArr)
        {
            if (IsRGBLinear)
            {
                InArr[0] = InputRGBSpace.ToNonLinear(InArr[0]);
                InArr[1] = InputRGBSpace.ToNonLinear(InArr[1]);
                InArr[2] = InputRGBSpace.ToNonLinear(InArr[2]);
                IsRGBLinear = false;
            }

            var1 = Math.Max(InArr[0], Math.Max(InArr[1], InArr[2]));
            var2 = Math.Min(InArr[0], Math.Min(InArr[1], InArr[2]));

            if (Math.Round(var1, 6) == Math.Round(var2, 6)) { OutArr[1] = 0; OutArr[0] = 0; }
            else
            {
                if ((var1 + var2) / 2d <= 0.5) { OutArr[1] = (var1 - var2) / (var1 + var2); }
                else { OutArr[1] = (var1 - var2) / (2 - var1 - var2); }

                if (InArr[0] == var1) { OutArr[0] = (InArr[1] - InArr[2]) / (var1 - var2); }
                else if (InArr[1] == var1) { OutArr[0] = 2 + (InArr[2] - InArr[0]) / (var1 - var2); }
                else { OutArr[0] = 4 + (InArr[0] - InArr[1]) / (var1 - var2); }    //InArr[2] == max
                OutArr[0] *= 60;
            }
            OutArr[2] = (var1 + var2) / 2d;
        }

        private void HSLToRGB(double[] InArr, double[] OutArr)
        {
            if (InArr[1] == 0)
            {
                OutArr[0] = InArr[2];
                OutArr[1] = InArr[2];
                OutArr[2] = InArr[2];
            }
            else
            {
                if (InArr[2] < 0.5) { var1 = InArr[2] * (1 + InArr[1]); }
                else { var1 = (InArr[2] + InArr[1]) - (InArr[1] * InArr[2]); }

                var2 = 2 * InArr[2] - var1;

                OutArr[0] = HueToRGB(var2, var1, InArr[0] + 120);
                OutArr[1] = HueToRGB(var2, var1, InArr[0]);
                OutArr[2] = HueToRGB(var2, var1, InArr[0] - 120);
            }
            IsRGBLinear = false;
        }

        #endregion

        #region Gray-Lab

        private void GrayToLab(double[] InArr, ref double[] OutArr)
        {
            OutArr[0] = InArr[0] * 100;
        }

        private void LabToGray(double[] InArr, double[] OutArr)
        {
            OutArr[0] = InArr[0] / 100d;
        }

        #endregion

        #region YCbCr-RGB

        private void RGBToYCbCr(double[] InArr, double[] OutArr)
        {
            if (!IsRGBLinear)
            {
                InArr[0] = RGBspaceArr[(int)RGBSpace].ToLinear(InArr[0]);
                InArr[1] = RGBspaceArr[(int)RGBSpace].ToLinear(InArr[1]);
                InArr[2] = RGBspaceArr[(int)RGBSpace].ToLinear(InArr[2]);
                IsRGBLinear = true;
            }
            OutArr[0] = YCbCrSpaceArr[(int)YCbCrSpace].KR * InArr[0] + YCbCrSpaceArr[(int)YCbCrSpace].KG * InArr[1] + YCbCrSpaceArr[(int)YCbCrSpace].KB * InArr[2];
            OutArr[1] = (((InArr[2] - OutArr[0]) / (1 - YCbCrSpaceArr[(int)YCbCrSpace].KB)) / 2) + 0.5;
            OutArr[2] = (((InArr[0] - OutArr[0]) / (1 - YCbCrSpaceArr[(int)YCbCrSpace].KR)) / 2) + 0.5;
        }

        private void YCbCrToRGB(double[] InArr, double[] OutArr)
        {
            OutArr[2] = -2 * InArr[1] * (InputYCbCrSpace.KB - 1) + InputYCbCrSpace.KB + InArr[0] - 1;
            OutArr[0] = -2 * InArr[2] * (InputYCbCrSpace.KR - 1) + InputYCbCrSpace.KR + InArr[0] - 1;
            OutArr[1] = ((-OutArr[2] * InputYCbCrSpace.KB) - (InputYCbCrSpace.KR * OutArr[0]) + InArr[0]) / InputYCbCrSpace.KG;

            OutArr[0] = InputRGBSpace.ToNonLinear(OutArr[0]);
            OutArr[1] = InputRGBSpace.ToNonLinear(OutArr[1]);
            OutArr[2] = InputRGBSpace.ToNonLinear(OutArr[2]);
            IsRGBLinear = false;
        }

        #endregion

        #region DEF-XYZ

        private void DEFToXYZ(double[] InArr, ref double[] OutArr)
        {
            OutArr = mmath.MultiplyMatrix(DEF_XYZ_Matrix, InArr);
        }

        private void XYZToDEF(double[] InArr, ref double[] OutArr)
        {
            OutArr = mmath.MultiplyMatrix(XYZ_DEF_Matrix, InArr);
        }

        #endregion

        #region Bef-DEF

        private void BefToDEF(double[] InArr, double[] OutArr)
        {
            OutArr[1] = InArr[1] * InArr[0];
            OutArr[2] = InArr[2] * InArr[0];
            OutArr[0] = Math.Sqrt(Pow2(InArr[0]) - Pow2(OutArr[1]) - Pow2(OutArr[2]));
        }

        private void DEFToBef(double[] InArr, double[] OutArr)
        {
            OutArr[0] = Math.Sqrt(Pow2(InArr[0]) + Pow2(InArr[1]) + Pow2(InArr[2]));
            OutArr[1] = InArr[1] / OutArr[0];
            OutArr[2] = InArr[2] / OutArr[0];
        }

        #endregion

        #region BCH-DEF

        private void BCHToDEF(double[] InArr, double[] OutArr)
        {
            var1 = InArr[0] * Math.Sin(InArr[1]);
            OutArr[0] = InArr[0] * Math.Sin(Pi2 - InArr[1]);
            OutArr[1] = var1 * Math.Cos(InArr[2]);
            OutArr[2] = var1 * Math.Sin(InArr[2]);
        }

        private void DEFToBCH(double[] InArr, double[] OutArr)
        {
            OutArr[0] = Math.Sqrt(Pow2(InArr[0]) + Pow2(InArr[1]) + Pow2(InArr[2]));
            OutArr[1] = Math.Asin((Math.Sqrt(Pow2(InArr[1]) + Pow2(InArr[2])) / OutArr[0]) * Sign(InArr[2]));
            OutArr[2] = Math.Acos(InArr[1] / Math.Sqrt(Pow2(InArr[1]) + Pow2(InArr[2])) * Sign(InArr[2]));

            if (double.IsNaN(OutArr[0])) OutArr[0] = 0;
            if (double.IsNaN(OutArr[1])) OutArr[1] = 0;
            if (double.IsNaN(OutArr[2])) OutArr[2] = 0;
        }

        #endregion

        #endregion

        #region Converting Subroutines

        private double Fn_XYZToLab(double val)
        {
            return (val <= Epsilon) ? ((Kappa * val) + 16d) / 116d : Math.Pow(val, 0.33333333333333333333333333333333);
        }

        private double Fx_LabToXYZ(double a, double L)
        {
            return (a / 500d) + Fy_LabToXYZ(L);
        }

        private double Fy_LabToXYZ(double L)
        {
            return (L + 16) / 116d;
        }

        private double Fz_LabToXYZ(double b, double L)
        {
            return Fy_LabToXYZ(L) - (b / 200d);
        }

        private double HueToRGB(double v1, double v2, double vH)
        {
            if (vH > 360) { vH -= 360; }
            else if (vH < 0) { vH += 360; }

            if (vH < 60) { v1 = v1 + (v2 - v1) * vH / 60; }
            else if (vH < 180) { v1 = v2; }
            else if (vH < 240) { v1 = v1 + (v2 - v1) * (240 - vH) / 60; }

            return v1;
        }

        private void SetInputColor(Color inColor)
        {
            InputModel = inColor.Model;
            DoAdaption = inColor.ReferenceWhite != OutReferenceWhite;
            if (InputModel != ColorModel.Gray)
            {
                ValArr1 = inColor.ColorArray;
                ValArr2 = inColor.ColorArray;
            }
            else
            {
                ValArr1 = new double[] { ((ColorGray)inColor).G, 0, 0 };
                ValArr2 = new double[] { ValArr1[0], 0, 0 };
            }

            switch (InputModel)
            {
                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.Gray:
                case ColorModel.DEF:
                case ColorModel.Bef:
                case ColorModel.BCH:
                    if (inColor.ReferenceWhite.Name == WhitepointName.Custom || OutReferenceWhite.Name == WhitepointName.Custom) DoAdaption = true;
                    InputWhitepoint = inColor.ReferenceWhite;
                    break;

                case ColorModel.HSL:
                    InputRGBSpace = ((ColorHSL)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.HSV:
                    InputRGBSpace = ((ColorHSV)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.RGB:
                    InputRGBSpace = ((ColorRGB)inColor).Space;
                    IsRGBLinear = ((ColorRGB)inColor).IsLinear;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.YCbCr:
                    InputRGBSpace = ((ColorYCbCr)inColor).BaseSpace;
                    InputYCbCrSpace = ((ColorYCbCr)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionYCbCr();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SetInputColor(BColor inColor)
        {
            InputModel = inColor.Model;
            DoAdaption = inColor.ReferenceWhite != OutReferenceWhite.Name;

            if (InputModel != ColorModel.Gray)
            {
                ValArr1 = inColor.DoubleColorArray;
                ValArr2 = inColor.DoubleColorArray;
            }
            else
            {
                ValArr1 = new double[] { ((BColorGray)inColor).G / 255d, 0, 0 };
                ValArr2 = new double[] { ((BColorGray)inColor).G / 255d, 0, 0 };
            }

            switch (InputModel)
            {
                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.Gray:
                case ColorModel.DEF:
                case ColorModel.Bef:
                case ColorModel.BCH:
                    if (inColor.ReferenceWhite == WhitepointName.Custom || OutReferenceWhite.Name == WhitepointName.Custom) DoAdaption = true;
                    InputWhitepoint = WhitepointArr[(int)inColor.ReferenceWhite];
                    break;

                case ColorModel.HSL:
                    InputRGBSpace = ((BColorHSL)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.HSV:
                    InputRGBSpace = ((BColorHSV)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.RGB:
                    InputRGBSpace = ((BColorRGB)inColor).Space;
                    IsRGBLinear = ((BColorRGB)inColor).IsLinear;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.YCbCr:
                    InputRGBSpace = ((BColorYCbCr)inColor).BaseSpace;
                    InputYCbCrSpace = ((BColorYCbCr)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionYCbCr();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SetInputColor(UColor inColor)
        {
            InputModel = inColor.Model;
            DoAdaption = inColor.ReferenceWhite != OutReferenceWhite.Name;
            if (InputModel != ColorModel.Gray)
            {
                ValArr1 = inColor.DoubleColorArray;
                ValArr2 = inColor.DoubleColorArray;
            }
            else
            {
                ValArr1 = new double[] { ((UColorGray)inColor).G / 65535d, 0, 0 };
                ValArr2 = new double[] { ((UColorGray)inColor).G / 65535d, 0, 0 };
            }

            switch (InputModel)
            {
                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.Gray:
                case ColorModel.DEF:
                case ColorModel.Bef:
                case ColorModel.BCH:
                    if (inColor.ReferenceWhite == WhitepointName.Custom || OutReferenceWhite.Name == WhitepointName.Custom) DoAdaption = true;
                    InputWhitepoint = WhitepointArr[(int)inColor.ReferenceWhite];
                    break;

                case ColorModel.HSL:
                    InputRGBSpace = ((UColorHSL)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.HSV:
                    InputRGBSpace = ((UColorHSV)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.RGB:
                    InputRGBSpace = ((UColorRGB)inColor).Space;
                    IsRGBLinear = ((UColorRGB)inColor).IsLinear;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionRGB();
                    break;
                case ColorModel.YCbCr:
                    InputRGBSpace = ((UColorYCbCr)inColor).BaseSpace;
                    InputYCbCrSpace = ((UColorYCbCr)inColor).Space;
                    InputWhitepoint = InputRGBSpace.ReferenceWhite;
                    DoAdaptionYCbCr();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void DoAdaptionRGB()
        {
            if (OutputModel == ColorModel.RGB || OutputModel == ColorModel.HSV || OutputModel == ColorModel.HSL) DoAdaption = InputRGBSpace.Name != RGBSpace;
            else DoAdaption = InputWhitepoint.Name != OutReferenceWhite.Name;
        }

        private void DoAdaptionYCbCr()
        {
            if (OutputModel == ColorModel.YCbCr) DoAdaption = InputRGBSpace.Name != RGBSpace || InputYCbCrSpace.Name != YCbCrSpace;
            else DoAdaption = InputWhitepoint.Name != OutReferenceWhite.Name;
        }

        private void LabDIN99(int model, double[] Lab)
        {
            var1 = DIN99Vals[model, 0] * Math.Log(1 + DIN99Vals[model, 1] * Lab[0]);      //L
            switch (model)
            {
                case 0:     //DIN99
                    var2 = Lab[1] * cos16 + Lab[2] * sin16;                             //e
                    var3 = DIN99Vals[model, 2] * (-Lab[1] * sin16 + Lab[2] * cos16);    //f
                    break;
                case 1:     //DIN99b
                    var2 = Lab[1] * cos26 + Lab[2] * sin26;
                    var3 = DIN99Vals[model, 2] * (-Lab[1] * sin26 + Lab[2] * cos26);
                    break;
                case 2:     //DIN99c
                    var2 = Lab[1];
                    var3 = DIN99Vals[model, 2] * Lab[2];
                    break;
                case 3:     //DIN99d
                    var2 = Lab[1] * cos50 + Lab[2] * sin50;
                    var3 = DIN99Vals[model, 2] * (-Lab[1] * sin50 + Lab[2] * cos50);
                    break;
            }
            var4 = Math.Log(1 + DIN99Vals[model, 3] * Math.Sqrt(Pow2(var2) + Pow2(var3))) * DIN99Vals[model, 4];  //C
            var5 = (Math.Atan2(var3, var2) * Pi180_1) + DIN99Vals[model, 5];      //H
        }

        private void DIN99Lab(int model, double[] LCH)
        {
            var1 = (Math.Exp(LCH[1] / DIN99Vals[model, 4]) - 1) / DIN99Vals[model, 3];  //G
            var4 = (LCH[2] - DIN99Vals[model, 5]) * Pi180;
            var2 = var1 * Math.Cos(var4);   //e
            var3 = var1 * Math.Sin(var4);   //f
            switch (model)
            {
                case 0:     //DIN99
                    var5 = var2 * cos16 - (var3 / DIN99Vals[model, 2]) * sin16;
                    var6 = var2 * sin16 + (var3 / DIN99Vals[model, 2]) * cos16;
                    break;
                case 1:     //DIN99b
                    var5 = var2 * cos26 - (var3 / DIN99Vals[model, 2]) * sin26;
                    var6 = var2 * sin26 + (var3 / DIN99Vals[model, 2]) * cos26;
                    break;
                case 2:     //DIN99c
                    var5 = var2;
                    var6 = var3 / DIN99Vals[model, 2];
                    break;
                case 3:     //DIN99d
                    var5 = var2 * cos50 - (var3 / DIN99Vals[model, 2]) * sin50;
                    var6 = var2 * sin50 + (var3 / DIN99Vals[model, 2]) * cos50;
                    break;
            }
            var7 = (Math.Exp(LCH[0] / DIN99Vals[model, 0]) - 1) / DIN99Vals[model, 1];
        }

        private static double Pow2(double p)
        {
            return p * p;
        }

        private static double Pow3(double p)
        {
            return p * p * p;
        }

        private static int Sign(double d)
        {
            if (d < 0) return -1;
            else return 1;
        }

        #endregion


        #region Chromatic Adaption

        private void ChromaticAdaption(double[] InArr, ref double[] OutArr)
        {
            if (InputWhitepoint.Name == WhitepointName.Custom || OutReferenceWhite.Name == WhitepointName.Custom)
            {
                AM = GetAdaptionMatrix();
                S = mmath.MultiplyMatrix(AM[0], InputWhitepoint.ValueArray);
                D = mmath.MultiplyMatrix(AM[0], OutReferenceWhite.ValueArray);
                M = new double[,] { { D[0] / S[0], 0, 0 }, { 0, D[1] / S[1], 0 }, { 0, 0, D[2] / S[2] } };
                OutArr = mmath.MultiplyMatrix(mmath.MultiplyMatrix(mmath.MultiplyMatrix(AM[1], M), AM[0]), InArr);
            }
            else { OutArr = mmath.MultiplyMatrix(PrecalcMatrix[(int)InputWhitepoint.Name, (int)OutReferenceWhite.Name], InArr); }
        }

        private static double[][,] GetAdaptionMatrix()
        {
            switch (ChromaticAdaptionMethod)
            {
                case AdaptionMethod.Bradford:
                    return Bradford;
                case AdaptionMethod.VonKries:
                    return VonKries;
                case AdaptionMethod.XYZScaling:
                    return XYZScaling;

                default:
                    throw new Exception("Adaption method not found");
            }
        }

        /// <summary>
        /// Calculates the adaption matrices for all whitepoints with the specified adaption method
        /// </summary>
        private static void MatrixPrecalculation()
        {
            double[][,] AM;
            double[,] M;
            double[] S, D;
            int[] WPvals = (int[])Enum.GetValues(typeof(WhitepointName));
            PrecalcMatrix = new double[WPvals.Length, WPvals.Length][,];
            AM = GetAdaptionMatrix();
            for (int i = 0; i < WPvals.Length; i++)
            {
                if ((WhitepointName)WPvals[i] != WhitepointName.Custom)
                {
                    Whitepoint WPsource = WhitepointArr[(int)(WhitepointName)WPvals[i]];
                    for (int j = 0; j < WPvals.Length; j++)
                    {
                        if ((WhitepointName)WPvals[j] != WhitepointName.Custom)
                        {
                            Whitepoint WPdest = WhitepointArr[(int)(WhitepointName)WPvals[j]];
                            if (WPsource != WPdest)
                            {
                                S = MMath.StaticMultiplyMatrix(AM[0], WPsource.ValueArray);
                                D = MMath.StaticMultiplyMatrix(AM[0], WPdest.ValueArray);
                                M = new double[,] { { D[0] / S[0], 0, 0 }, { 0, D[1] / S[1], 0 }, { 0, 0, D[2] / S[2] } };
                                PrecalcMatrix[i, j] = MMath.StaticMultiplyMatrix(MMath.StaticMultiplyMatrix(AM[1], M), AM[0]);
                            }
                            else { PrecalcMatrix[i, j] = new double[,] { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } }; }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
