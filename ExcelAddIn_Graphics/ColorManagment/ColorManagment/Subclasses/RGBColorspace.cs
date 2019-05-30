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
    /// Represents a colorspace with all the data
    /// </summary>
    public abstract class RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public abstract RGBSpaceName Name { get; }
        /// <summary>
        /// The reference white
        /// </summary>
        public abstract Whitepoint ReferenceWhite { get; }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public abstract double Gamma { get; }

        /// <summary>
        /// Red primary
        /// </summary>
        internal abstract double[] Cr { get; }
        /// <summary>
        /// Green primary
        /// </summary>
        internal abstract double[] Cg { get; }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal abstract double[] Cb { get; }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal abstract double[,] CM { get; }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal abstract double[,] ICM { get; }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal abstract double ToLinear(double c);
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal abstract double ToNonLinear(double c);


        /// <summary>
        /// Get a colorspace from the name
        /// </summary>
        /// <param name="name">The name of the colorspace</param>
        /// <returns>The named colorspace</returns>
        public static RGBSpaceName GetColorspaceName(string name)
        {
            switch (name.Trim().ToLower().Replace(" ", ""))
            {
                case "adobergb":
                case "adobergb(1998)":
                case "adobergb1998":
                    return RGBSpaceName.AdobeRGB;
                case "prophotorgb":
                    return RGBSpaceName.ProPhotoRGB;
                case "srgb":
                    return RGBSpaceName.sRGB;
                case "applergb":
                    return RGBSpaceName.AppleRGB;
                case "brucergb":
                    return RGBSpaceName.BruceRGB;
                case "ciergb":
                    return RGBSpaceName.CIERGB;
                case "ntscrgb":
                    return RGBSpaceName.NTSCRGB;
                case "widegamut":
                case "adobewide":
                case "widegamutrgb":
                case "adobewidegamutrgb":
                    return RGBSpaceName.WideGamutRGB;
                case "bestrgb":
                    return RGBSpaceName.BestRGB;
                case "betargb":
                    return RGBSpaceName.BetaRGB;
                case "colormatchrgb":
                    return RGBSpaceName.ColorMatchRGB;
                case "donrgb4":
                    return RGBSpaceName.DonRGB4;
                case "ektaspaceps5":
                    return RGBSpaceName.EktaSpacePS5;
                case "palsecamrgb":
                    return RGBSpaceName.PAL_SECAMRGB;
                case "smptecrgb":
                    return RGBSpaceName.SMPTE_C_RGB;

                default:
                    throw new Exception("Colorspace not found");
            }
        }

        /// <summary>
        /// Get a colorspace from the name
        /// </summary>
        /// <param name="name">The name of the colorspace</param>
        /// <returns>The named colorspace</returns>
        public static RGBColorspace GetColorspace(RGBSpaceName name)
        {
            switch (name)
            {
                case RGBSpaceName.AdobeRGB:
                    return new AdobeRGB();
                case RGBSpaceName.ProPhotoRGB:
                    return new ProPhotoRGB();
                case RGBSpaceName.sRGB:
                    return new sRGB();
                case RGBSpaceName.AppleRGB:
                    return new AppleRGB();
                case RGBSpaceName.BruceRGB:
                    return new BruceRGB();
                case RGBSpaceName.CIERGB:
                    return new CIERGB();
                case RGBSpaceName.NTSCRGB:
                    return new NTSCRGB();
                case RGBSpaceName.WideGamutRGB:
                    return new WideGamutRGB();
                case RGBSpaceName.BestRGB:
                    return new BestRGB();
                case RGBSpaceName.BetaRGB:
                    return new BetaRGB();
                case RGBSpaceName.ColorMatchRGB:
                    return new ColorMatchRGB();
                case RGBSpaceName.DonRGB4:
                    return new DonRGB4();
                case RGBSpaceName.EktaSpacePS5:
                    return new EktaSpacePS5();
                case RGBSpaceName.PAL_SECAMRGB:
                    return new PAL_SECAMRGB();
                case RGBSpaceName.SMPTE_C_RGB:
                    return new SMPTE_C_RGB();

                case RGBSpaceName.ICC:
                    throw new Exception("ICC cannot be created without arguments");
                default:
                    throw new Exception("Colorspace not found");
            }
        }

        /// <summary>
        /// Get the name of the whitespace a colorspace is in
        /// </summary>
        /// <param name="name">The name of the colorspace</param>
        /// <returns>The name of the whitepoint</returns>
        public static WhitepointName GetWhitepointName(RGBSpaceName name)
        {
            switch (name)
            {
                case RGBSpaceName.AdobeRGB: return WhitepointName.D65;
                case RGBSpaceName.ProPhotoRGB: return WhitepointName.D50;
                case RGBSpaceName.sRGB: return WhitepointName.D65;
                case RGBSpaceName.AppleRGB: return WhitepointName.D65;
                case RGBSpaceName.BruceRGB: return WhitepointName.D65;
                case RGBSpaceName.CIERGB: return WhitepointName.E;
                case RGBSpaceName.NTSCRGB: return WhitepointName.C;
                case RGBSpaceName.WideGamutRGB: return WhitepointName.D50;
                case RGBSpaceName.BestRGB: return WhitepointName.D50;
                case RGBSpaceName.BetaRGB: return WhitepointName.D50;
                case RGBSpaceName.ColorMatchRGB: return WhitepointName.D50;
                case RGBSpaceName.DonRGB4: return WhitepointName.D50;
                case RGBSpaceName.EktaSpacePS5: return WhitepointName.D50;
                case RGBSpaceName.PAL_SECAMRGB: return WhitepointName.D65;
                case RGBSpaceName.SMPTE_C_RGB: return WhitepointName.D65;

                case RGBSpaceName.ICC:
                    throw new Exception("Check ICC for whitepoint");
                default:
                    throw new Exception("Colorspace not found");
            }
        }

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            RGBColorspace c = obj as RGBColorspace;
            if ((Object)c == null) { return false; }

            return Name == c.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RGBColorspace a, RGBColorspace b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.Name == b.Name;
        }

        public static bool operator !=(RGBColorspace a, RGBColorspace b)
        {
            return !(a == b);
        }

        #endregion
    }


    /// <summary>
    /// Custom RGB colorspace
    /// </summary>
    public sealed class CustomRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return (RGBSpaceName)(-1); } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value
        /// </summary>
        public override double Gamma { get { return gamma; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { xr, yr }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { xg, yg }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { xb, yb }; } }

        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return _CM; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return _ICM; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, gamma);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / gamma);
        }

        private Whitepoint wp;
        private double xr, yr, xg, yg, xb, yb, gamma;
        private double[,] _CM, _ICM;

        /// <summary> 
        /// Creates an instance of a custom RGB colorspace
        /// </summary>
        /// <param name="xr">X-Chromaticity coordinate for Red</param>
        /// <param name="yr">Y-Chromaticity coordinate for Red</param>
        /// <param name="xg">X-Chromaticity coordinate for Green</param>
        /// <param name="yg">Y-Chromaticity coordinate for Green</param>
        /// <param name="xb">X-Chromaticity coordinate for Blue</param>
        /// <param name="yb">Y-Chromaticity coordinate for Blue</param>
        /// <param name="Gamma">The gamme value</param>
        /// <param name="ReferenceWhite">The reference white</param>
        public CustomRGB(double xr, double yr, double xg, double yg, double xb, double yb, double Gamma, Whitepoint ReferenceWhite)
        {
            wp = ReferenceWhite;
            this.gamma = Gamma;
            this.xr = xr;
            this.yr = yr;
            this.xg = xg;
            this.yg = yg;
            this.xb = xb;
            this.yb = yb;
            _CM = GetM();
            _ICM = MMath.StaticInvertMatrix(_CM);
        }

        private double[,] GetM()
        {
            double Xr = xr / yr;
            double Zr = (1 - xr - yr) / yr;
            double Xg = xg / yg;
            double Zg = (1 - xg - yg) / yg;
            double Xb = xb / yb;
            double Zb = (1 - xb - yb) / yb;

            double[] S = MMath.StaticMultiplyMatrix(MMath.StaticInvertMatrix(new double[,] { { Xr, Xg, Xb }, { 1, 1, 1 }, { Zr, Zg, Zb } }), wp.ValueArray);
            return new double[,] { { S[0] * Xr, S[1] * Xg, S[2] * Xb }, { S[0], S[1], S[2] }, { S[0] * Zr, S[1] * Zg, S[2] * Zb } };
        }
    }

    /// <summary>
    /// The sRGB colorspace
    /// </summary>
    public sealed class sRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.sRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.3, 0.6 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.4124564, 0.3575761, 0.1804375 }, { 0.2126729, 0.7151522, 0.072175 }, { 0.0193339, 0.119192, 0.9503041 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 3.2404542, -1.5371385, -0.4985314 }, { -0.969266, 1.8760108, 0.041556 }, { 0.0556434, -0.2040259, 1.0572252 } }; } }
        

        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return (c > 0.04045) ? Math.Pow((c + 0.055) / 1.055, 2.4) : (c / 12.92);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return (c <= 0.0031308) ? 12.92 * c : 1.055 * Math.Pow(c, 1 / 2.4) - 0.055;
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the sRGB colorspace
        /// </summary>
        public sRGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The NTSC RGB colorspace
    /// </summary>
    public sealed class NTSCRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.NTSCRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.67, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.21, 0.71 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.14, 0.08 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.6068909, 0.1735011, 0.200348 }, { 0.2989164, 0.586599, 0.1144845 }, { 0.0, 0.0660957, 1.1162243 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.9099961, -0.5324542, -0.2882091 }, { -0.9846663, 1.999171, -0.0283082 }, { 0.0583056, -0.1183781, 0.8975535 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the NTSCRGB colorspace
        /// </summary>
        public NTSCRGB()
        {
            wp = new Whitepoint(WhitepointName.C);
        }
    }

    /// <summary>
    /// The Bruce RGB colorspace
    /// </summary>
    public sealed class BruceRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.BruceRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.28, 0.65 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.4674162, 0.2944512, 0.1886026 }, { 0.2410115, 0.6835475, 0.075441 }, { 0.0219101, 0.0736128, 0.9933071 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.7454669, -1.1358136, -0.4350269 }, { -0.969266, 1.8760108, 0.041556 }, { 0.0112723, -0.1139754, 1.0132541 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the BruceRGB colorspace
        /// </summary>
        public BruceRGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The CIE RGB colorspace
    /// </summary>
    public sealed class CIERGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.CIERGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.735, 0.265 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.274, 0.717 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.167, 0.009 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.488718, 0.3106803, 0.2006017 }, { 0.1762044, 0.8129847, 0.0108109 }, { 0.0, 0.0102048, 0.9897952 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.3706743, -0.9000405, -0.4706338 }, { -0.513885, 1.4253036, 0.0885814 }, { 0.0052982, -0.0146949, 1.0093968 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the CIERGB colorspace
        /// </summary>
        public CIERGB()
        {
            wp = new Whitepoint(WhitepointName.E);
        }
    }

    /// <summary>
    /// The Adobe RGB colorspace
    /// </summary>
    public sealed class AdobeRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.AdobeRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.21, 0.71 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.5767309, 0.185554, 0.1881852 }, { 0.2973769, 0.6273491, 0.0752741 }, { 0.0270343, 0.0706872, 0.9911085 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.041369, -0.5649464, -0.3446944 }, { -0.969266, 1.8760108, 0.041556 }, { 0.0134474, -0.1183897, 1.0154096 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the AdobeRGB colorspace
        /// </summary>
        public AdobeRGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The Apple RGB colorspace
    /// </summary>
    public sealed class AppleRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.AppleRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 1.8; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.625, 0.34 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.28, 0.595 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.155, 0.07 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.4497288, 0.3162486, 0.1844926 }, { 0.2446525, 0.6720283, 0.0833192 }, { 0.0251848, 0.1411824, 0.9224628 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.9515373, -1.2894116, -0.4738445 }, { -1.0851093, 1.9908566, 0.0372026 }, { 0.0854934, -0.2694964, 1.0912975 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 1.8d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 1.8d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the AppleRGB colorspace
        /// </summary>
        public AppleRGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The Pro Photo RGB colorspace
    /// </summary>
    public sealed class ProPhotoRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.ProPhotoRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 1.8; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.7347, 0.2653 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.1596, 0.8404 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.0366, 0.0001 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.7976749, 0.1351917, 0.0313534 }, { 0.2880402, 0.7118741, 0.0000857 }, { 0.0, 0.0, 0.8252100 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.3459433, -0.2556075, -0.0511118 }, { -0.5445989, 1.5081673, 0.0205351 }, { 0.0, 0.0, 1.2118128 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return (c > 1 / 32d) ? Math.Pow(c, 1.8d) : c / 16d;
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return (c > 1 / 521d) ? Math.Pow(c, 1 / 1.8d) : c * 16d;
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ProPhotoRGB colorspace
        /// </summary>
        public ProPhotoRGB()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The Adobe Wide Gamut RGB colorspace
    /// </summary>
    public sealed class WideGamutRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.WideGamutRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.735, 0.265 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.115, 0.826 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.157, 0.018 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.7161046, 0.1009296, 0.1471858 }, { 0.2581874, 0.7249378, 0.0168748 }, { 0.0, 0.0517813, 0.7734287 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.4628067, -0.1840623, -0.2743606 }, { -0.5217933, 1.4472381, 0.0677227 }, { 0.0349342, -0.096893, 1.2884099 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the WideGamutRGB colorspace
        /// </summary>
        public WideGamutRGB()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The Best RGB colorspace
    /// </summary>
    public sealed class BestRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.BestRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.7347, 0.2653 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.215, 0.775 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.13, 0.035 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.6326696, 0.2045558, 0.1269946 }, { 0.2284569, 0.7373523, 0.0341908 }, { 0.0000000, 0.0095142, 0.8156958 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.7552599, -0.4836786, -0.2530000 }, { -0.5441336, 1.5068789, 0.0215528 }, { 0.0063467, -0.0175761, 1.2256959 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the BestRGB colorspace
        /// </summary>
        public BestRGB()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The Beta RGB colorspace
    /// </summary>
    public sealed class BetaRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.BetaRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.6888, 0.3112 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.1986, 0.7551 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.1265, 0.0352 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.6712537, 0.1745834, 0.1183829 }, { 0.3032726, 0.6637861, 0.0329413 }, { 0.0, 0.040701, 0.784509 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.683227, -0.4282363, -0.2360185 }, { -0.7710229, 1.7065571, 0.04469 }, { 0.0400013, -0.0885376, 1.272364 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the BetaRGB colorspace
        /// </summary>
        public BetaRGB()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The ColorMatchRGB colorspace
    /// </summary>
    public sealed class ColorMatchRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.ColorMatchRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 1.8; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.63, 0.34 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.295, 0.605 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.075 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.5093439, 0.3209071, 0.1339691 }, { 0.2748840, 0.6581315, 0.0669845 }, { 0.0242545, 0.1087821, 0.6921735 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.6422874, -1.2234270, -0.3930143 }, { -1.1119763, 2.0590183, 0.0159614 }, { 0.0821699, -0.2807254, 1.4559877 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 1.8d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 1.8d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ColorMatchRGB colorspace
        /// </summary>
        public ColorMatchRGB()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The Don RGB 4 colorspace
    /// </summary>
    public sealed class DonRGB4 : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.DonRGB4; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.696, 0.3 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.215, 0.765 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.13, 0.035 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.6457711, 0.1933511, 0.1250978 }, { 0.2783496, 0.6879702, 0.0336802 }, { 0.0037113, 0.0179861, 0.8035125 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 1.7603902, -0.4881198, -0.2536126 }, { -0.7126288, 1.6527432, 0.0416715 }, { 0.0078207, -0.0347411, 1.2447743 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the DonRGB4 colorspace
        /// </summary>
        public DonRGB4()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The Ekta Space PS5 colorspace
    /// </summary>
    public sealed class EktaSpacePS5 : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.EktaSpacePS5; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.695, 0.305 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.26, 0.7 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.11, 0.005 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.5938914, 0.2729801, 0.0973485 }, { 0.2606286, 0.7349465, 0.0044249 }, { 0.0, 0.0419969, 0.7832131 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 2.0043819, -0.7304844, -0.2450052 }, { -0.7110285, 1.6202126, 0.0792227 }, { 0.0381263, -0.086878, 1.2725438 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the EktaSpacePS5 colorspace
        /// </summary>
        public EktaSpacePS5()
        {
            wp = new Whitepoint(WhitepointName.D50);
        }
    }

    /// <summary>
    /// The PAL/SECAM RGB colorspace
    /// </summary>
    public sealed class PAL_SECAMRGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.PAL_SECAMRGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.29, 0.6 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.430619, 0.3415419, 0.1783091 }, { 0.2220379, 0.7066384, 0.0713236 }, { 0.0201853, 0.1295504, 0.9390944 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 3.0628971, -1.3931791, -0.4757517 }, { -0.969266, 1.8760108, 0.041556 }, { 0.0678775, -0.2288548, 1.069349 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the PAL_SECAMRGB colorspace
        /// </summary>
        public PAL_SECAMRGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The SMPTE-C RGB colorspace
    /// </summary>
    public sealed class SMPTE_C_RGB : RGBColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override RGBSpaceName Name { get { return RGBSpaceName.SMPTE_C_RGB; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// The gamma value (may be approximate)
        /// </summary>
        public override double Gamma { get { return 2.2; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.63, 0.34 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.31, 0.595 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.155, 0.07 }; } }
        
        /// <summary>
        /// The conversion matrix
        /// </summary>
        internal override double[,] CM { get { return new double[,] { { 0.3935891, 0.3652497, 0.1916313 }, { 0.2124132, 0.7010437, 0.0865432 }, { 0.0187423, 0.1119313, 0.9581563 } }; } }
        /// <summary>
        /// The inverse conversion matrix
        /// </summary>
        internal override double[,] ICM { get { return new double[,] { { 3.505396, -1.7394894, -0.5439640 }, { -1.0690722, 1.9778245, 0.0351722 }, { 0.05632, -0.1970226, 1.0502026 } }; } }


        /// <summary>
        /// Linearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The linear value from given color</returns>
        internal override double ToLinear(double c)
        {
            return Math.Pow(c, 2.19921875d);
        }
        /// <summary>
        /// Delinearises a color
        /// </summary>
        /// <param name="c">A channel from a color</param>
        /// <returns>The non-linear value from given color</returns>
        internal override double ToNonLinear(double c)
        {
            return Math.Pow(c, 1 / 2.19921875d);
        }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the SMPTE_C_RGB colorspace
        /// </summary>
        public SMPTE_C_RGB()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }    
}
