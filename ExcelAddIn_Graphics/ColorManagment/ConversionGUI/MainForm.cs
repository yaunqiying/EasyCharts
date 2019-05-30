using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using ColorManagment;

/*  This GUI converts between colormodels and spaces that are defined in the ColorManagment library.
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

namespace ConversionGUI
{
    public partial class MainForm : Form
    {
        //When adding new colors:
        //Add model(s) to GUI like previous colors
        //Add model(s) to region "Variables" as Color, as ICC (if compatible), as Whitepoint (if compatible), as Colorspace (if compatible)
        //Add model(s) to region "Get Value from Box" like previous colors
        //Add model(s) to region "Subroutines" at method "FillFields"
        //Add model(s) to region "Subroutines" at method "Conversion"
        //Add model(s) to region "Subroutines" at method "SetRefWhiteList" or "SetSpaceList"
        //Add model(s) to region "Other GUI Elements" at method "Convert_Button_Click"
        //Add model(s) to region "Subroutines" at method "SetICCStuff" if compatible with ICC

        #region Variables

        private ColorConverter Converter = new ColorConverter();
        private ColorRGB ColRGB;
        private ColorXYZ ColXYZ;
        private ColorLab ColLab;
        private ColorLuv ColLuv;
        private ColorLCHab ColLCHab;
        private ColorLCHuv ColLCHuv;
        private ColorLCH99 ColLCH99;
        private ColorLCH99b ColLCH99b;
        private ColorLCH99c ColLCH99c;
        private ColorLCH99d ColLCH99d;
        private ColorYxy ColYxy;
        private ColorHSL ColHSL;
        private ColorHSV ColHSV;
        private ColorCMY ColCMY;
        private ColorCMYK ColCMYK;
        private ColorYCbCr ColYCbCr;
        private ColorGray ColGray;
        private ColorX ColX;
        private ColorDEF ColDEF;
        private ColorBef ColBef;
        private ColorBCH ColBCH;

        private ICC RGB_ICC;
        private ICC HSL_ICC;
        private ICC HSV_ICC;
        private ICC CMY_ICC;
        private ICC CMYK_ICC;
        private ICC YCbCr_ICC;
        private ICC Gray_ICC;
        private ICC XColor_ICC;

        private int PrevSpace;
        private int PrevYCbCrSpace;
        CultureInfo cInfo = new CultureInfo("en-US");
        bool IsInit = false;

        List<ComboBox> SpaceCoBoxes = new List<ComboBox>();
        List<ComboBox> RefWhiteCoBoxes = new List<ComboBox>();

        private RGBSpaceName BaseSpace { get { return (RGBSpaceName)General_SpaceDrDo.SelectedIndex; } }
        private RGBSpaceName RGBSpace { get { return (RGBSpaceName)RGB_CoBox.SelectedIndex; } }
        private RGBSpaceName HSVSpace { get { return (RGBSpaceName)HSV_CoBox.SelectedIndex; } }
        private RGBSpaceName HSLSpace { get { return (RGBSpaceName)HSL_CoBox.SelectedIndex; } }
        private YCbCrSpaceName YCbCrSpace { get { return (YCbCrSpaceName)YCbCr_CoBox.SelectedIndex; } }

        private Whitepoint Basewp { get { return new Whitepoint((WhitepointName)RefWhiteDrDo.SelectedIndex); } }
        private Whitepoint XYZwp { get { return new Whitepoint((WhitepointName)XYZ_CoBox.SelectedIndex); } }
        private Whitepoint Labwp { get { return new Whitepoint((WhitepointName)Lab_CoBox.SelectedIndex); } }
        private Whitepoint Yxywp { get { return new Whitepoint((WhitepointName)Yxy_CoBox.SelectedIndex); } }
        private Whitepoint LCHabwp { get { return new Whitepoint((WhitepointName)LCHab_CoBox.SelectedIndex); } }
        private Whitepoint LCHuvwp { get { return new Whitepoint((WhitepointName)LCHuv_CoBox.SelectedIndex); } }
        private Whitepoint Luvwp { get { return new Whitepoint((WhitepointName)Luv_CoBox.SelectedIndex); } }
        private Whitepoint DEFwp { get { return new Whitepoint((WhitepointName)DEF_CoBox.SelectedIndex); } }
        private Whitepoint Befwp { get { return new Whitepoint((WhitepointName)Bef_CoBox.SelectedIndex); } }
        private Whitepoint BCHwp { get { return new Whitepoint((WhitepointName)BCH_CoBox.SelectedIndex); } }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            SetSpaceList();
            SetRefWhiteList();
            foreach (ComboBox c in SpaceCoBoxes)
            {
                c.Items.AddRange(Enum.GetNames(typeof(RGBSpaceName)));
                c.SelectedIndex = (int)ColorConverter.StandardColorspace;
            }
            foreach (ComboBox c in RefWhiteCoBoxes)
            {
                c.Items.AddRange(Enum.GetNames(typeof(WhitepointName)));
                c.SelectedIndex = (int)ColorConverter.ReferenceWhite.Name;
            }
            YCbCr_CoBox.Items.AddRange(Enum.GetNames(typeof(YCbCrSpaceName)));
            YCbCr_CoBox.SelectedIndex = (int)ColorConverter.StandardYCbCrSpace;
            RenderIntentCoBox.Items.AddRange(Enum.GetNames(typeof(RenderingIntent)));
            RenderIntentCoBox.SelectedIndex = (int)ColorConverter.PreferredRenderingIntent;
            ChroAdaptCoBox.Items.AddRange(Enum.GetNames(typeof(AdaptionMethod)));
            ChroAdaptCoBox.SelectedIndex = (int)ColorConverter.ChromaticAdaptionMethod;

            IsInit = true;
        }

        #region Get Value from Box

        private double[] RGB
        {
            get
            {
                string a = RGB_R.Text.Replace(',', '.');
                string b = RGB_G.Text.Replace(',', '.');
                string c = RGB_B.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] Lab
        {
            get
            {
                string a = Lab_L.Text.Replace(',', '.');
                string b = Lab_a.Text.Replace(',', '.');
                string c = Lab_b.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] XYZ
        {
            get
            {
                string a = XYZ_X.Text.Replace(',', '.');
                string b = XYZ_Y.Text.Replace(',', '.');
                string c = XYZ_Z.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] Luv
        {
            get
            {
                string a = Luv_L.Text.Replace(',', '.');
                string b = Luv_u.Text.Replace(',', '.');
                string c = Luv_v.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] LCHab
        {
            get
            {
                string a = LCHab_L.Text.Replace(',', '.');
                string b = LCHab_C.Text.Replace(',', '.');
                string c = LCHab_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] LCHuv
        {
            get
            {
                string a = LCHuv_L.Text.Replace(',', '.');
                string b = LCHuv_C.Text.Replace(',', '.');
                string c = LCHuv_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] Yxy
        {
            get
            {
                string a = Yxy_Y.Text.Replace(',', '.');
                string b = Yxy_x.Text.Replace(',', '.');
                string c = Yxy_sy.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] HSV
        {
            get
            {
                string a = HSV_H.Text.Replace(',', '.');
                string b = HSV_S.Text.Replace(',', '.');
                string c = HSV_V.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] HSL
        {
            get
            {
                string a = HSL_H.Text.Replace(',', '.');
                string b = HSL_S.Text.Replace(',', '.');
                string c = HSL_L.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] CMY
        {
            get
            {
                string a = CMY_C.Text.Replace(',', '.');
                string b = CMY_M.Text.Replace(',', '.');
                string c = CMY_Y.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] CMYK
        {
            get
            {
                string a = CMYK_C.Text.Replace(',', '.');
                string b = CMYK_M.Text.Replace(',', '.');
                string c = CMYK_Y.Text.Replace(',', '.');
                string d = CMYK_K.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo), Convert.ToDouble(d, cInfo) };
            }
        }

        private double[] YCbCr
        {
            get
            {
                string a = YCbCr_Y.Text.Replace(',', '.');
                string b = YCbCr_Cb.Text.Replace(',', '.');
                string c = YCbCr_Cr.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] Gray
        {
            get
            {
                string a = Gray_G.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo) };
            }
        }

        private double[] LCH99
        {
            get
            {
                string a = LCH99_L.Text.Replace(',', '.');
                string b = LCH99_C.Text.Replace(',', '.');
                string c = LCH99_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] LCH99b
        {
            get
            {
                string a = LCH99b_L.Text.Replace(',', '.');
                string b = LCH99b_C.Text.Replace(',', '.');
                string c = LCH99b_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] LCH99c
        {
            get
            {
                string a = LCH99c_L.Text.Replace(',', '.');
                string b = LCH99c_C.Text.Replace(',', '.');
                string c = LCH99c_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] LCH99d
        {
            get
            {
                string a = LCH99d_L.Text.Replace(',', '.');
                string b = LCH99d_C.Text.Replace(',', '.');
                string c = LCH99d_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] DEF
        {
            get
            {
                string a = DEF_D.Text.Replace(',', '.');
                string b = DEF_E.Text.Replace(',', '.');
                string c = DEF_F.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] Bef
        {
            get
            {
                string a = Bef_B.Text.Replace(',', '.');
                string b = Bef_e.Text.Replace(',', '.');
                string c = Bef_f.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] BCH
        {
            get
            {
                string a = BCH_B.Text.Replace(',', '.');
                string b = BCH_C.Text.Replace(',', '.');
                string c = BCH_H.Text.Replace(',', '.');
                return new double[] { Convert.ToDouble(a, cInfo), Convert.ToDouble(b, cInfo), Convert.ToDouble(c, cInfo) };
            }
        }

        private double[] XColor = new double[15];
        private int XColor_Channels = 15;

        #endregion

        #region Subroutines

        private bool IsXColor(ColorModel model)
        {
            switch (model)
            {
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
                case ColorModel.Color15:
                    return true;

                default: return false;
            }
        }

        private void FillFields()
        {
            string precision = "n6";

            RGB_R.Text = ColRGB.R.ToString(precision);
            RGB_G.Text = ColRGB.G.ToString(precision);
            RGB_B.Text = ColRGB.B.ToString(precision);

            Lab_L.Text = ColLab.L.ToString(precision);
            Lab_a.Text = ColLab.a.ToString(precision);
            Lab_b.Text = ColLab.b.ToString(precision);

            XYZ_X.Text = ColXYZ.X.ToString(precision);
            XYZ_Y.Text = ColXYZ.Y.ToString(precision);
            XYZ_Z.Text = ColXYZ.Z.ToString(precision);

            Luv_L.Text = ColLuv.L.ToString(precision);
            Luv_u.Text = ColLuv.u.ToString(precision);
            Luv_v.Text = ColLuv.v.ToString(precision);

            LCHab_L.Text = ColLCHab.L.ToString(precision);
            LCHab_C.Text = ColLCHab.C.ToString(precision);
            LCHab_H.Text = ColLCHab.H.ToString(precision);

            LCHuv_L.Text = ColLCHuv.L.ToString(precision);
            LCHuv_C.Text = ColLCHuv.C.ToString(precision);
            LCHuv_H.Text = ColLCHuv.H.ToString(precision);

            Yxy_Y.Text = ColYxy.Y.ToString(precision);
            Yxy_x.Text = ColYxy.x.ToString(precision);
            Yxy_sy.Text = ColYxy.y.ToString(precision);

            HSV_H.Text = ColHSV.H.ToString(precision);
            HSV_S.Text = ColHSV.S.ToString(precision);
            HSV_V.Text = ColHSV.V.ToString(precision);

            HSL_H.Text = ColHSL.H.ToString(precision);
            HSL_S.Text = ColHSL.S.ToString(precision);
            HSL_L.Text = ColHSL.L.ToString(precision);

            LCH99_L.Text = ColLCH99.L.ToString(precision);
            LCH99_C.Text = ColLCH99.C.ToString(precision);
            LCH99_H.Text = ColLCH99.H.ToString(precision);

            LCH99b_L.Text = ColLCH99b.L.ToString(precision);
            LCH99b_C.Text = ColLCH99b.C.ToString(precision);
            LCH99b_H.Text = ColLCH99b.H.ToString(precision);

            LCH99c_L.Text = ColLCH99c.L.ToString(precision);
            LCH99c_C.Text = ColLCH99c.C.ToString(precision);
            LCH99c_H.Text = ColLCH99c.H.ToString(precision);

            LCH99d_L.Text = ColLCH99d.L.ToString(precision);
            LCH99d_C.Text = ColLCH99d.C.ToString(precision);
            LCH99d_H.Text = ColLCH99d.H.ToString(precision);

            CMY_C.Text = (ColCMY == null) ? "N/A" : ColCMY.C.ToString(precision);
            CMY_M.Text = (ColCMY == null) ? "N/A" : ColCMY.M.ToString(precision);
            CMY_Y.Text = (ColCMY == null) ? "N/A" : ColCMY.Y.ToString(precision);

            CMYK_C.Text = (ColCMYK == null) ? "N/A" : ColCMYK.C.ToString("n4");
            CMYK_M.Text = (ColCMYK == null) ? "N/A" : ColCMYK.M.ToString("n4");
            CMYK_Y.Text = (ColCMYK == null) ? "N/A" : ColCMYK.Y.ToString("n4");
            CMYK_K.Text = (ColCMYK == null) ? "N/A" : ColCMYK.K.ToString("n4");

            YCbCr_Y.Text = (ColYCbCr == null) ? "N/A" : ColYCbCr.Y.ToString(precision);
            YCbCr_Cb.Text = (ColYCbCr == null) ? "N/A" : ColYCbCr.Cb.ToString(precision);
            YCbCr_Cr.Text = (ColYCbCr == null) ? "N/A" : ColYCbCr.Cr.ToString(precision);

            Gray_G.Text = (ColGray == null) ? "N/A" : ColGray.G.ToString(precision);

            XColor_ChannelUpDo_ValueChanged(null, null);

            DEF_D.Text = ColDEF.D.ToString(precision);
            DEF_E.Text = ColDEF.E.ToString(precision);
            DEF_F.Text = ColDEF.F.ToString(precision);

            Bef_B.Text = ColBef.B.ToString(precision);
            Bef_e.Text = ColBef.e.ToString(precision);
            Bef_f.Text = ColBef.f.ToString(precision);

            BCH_B.Text = ColBCH.B.ToString(precision);
            BCH_C.Text = ColBCH.C.ToString(precision);
            BCH_H.Text = ColBCH.H.ToString(precision);

            int R = (int)Math.Round(ColRGB.R * 255, MidpointRounding.AwayFromZero);
            int G = (int)Math.Round(ColRGB.G * 255, MidpointRounding.AwayFromZero);
            int B = (int)Math.Round(ColRGB.B * 255, MidpointRounding.AwayFromZero);
            ColorPanel.BackColor = System.Drawing.Color.FromArgb(R, G, B);
            Hex_Label.Text = "Hex: #" + Convert.ToString(R, 16).PadLeft(2, '0') + Convert.ToString(G, 16).PadLeft(2, '0') + Convert.ToString(B, 16).PadLeft(2, '0');
            RGB_Label.Text = "R: " + R + " G: " + G + " B: " + B;
        }

        private void Conversion(Color inColor)
        {
            Color inColor2 = inColor;
            if (inColor.IsICCcolor && !inColor.IsPCScolor) { inColor2 = Converter.ToICC(inColor); }

            if (inColor.Model != ColorModel.CIEXYZ) ColXYZ = Converter.ToXYZ(inColor2, XYZwp);
            if (inColor.Model != ColorModel.CIELab) ColLab = Converter.ToLab(inColor2, Labwp);
            if (inColor.Model != ColorModel.CIELuv) ColLuv = Converter.ToLuv(inColor2, Luvwp);
            if (inColor.Model != ColorModel.CIELCHab) ColLCHab = Converter.ToLCHab(inColor2, LCHabwp);
            if (inColor.Model != ColorModel.CIELCHuv) ColLCHuv = Converter.ToLCHuv(inColor2, LCHuvwp);
            if (inColor.Model != ColorModel.CIEYxy) ColYxy = Converter.ToYxy(inColor2, Yxywp);
            if (inColor.Model != ColorModel.LCH99) ColLCH99 = Converter.ToLCH99(inColor2);
            if (inColor.Model != ColorModel.LCH99b) ColLCH99b = Converter.ToLCH99b(inColor2);
            if (inColor.Model != ColorModel.LCH99c) ColLCH99c = Converter.ToLCH99c(inColor2);
            if (inColor.Model != ColorModel.LCH99d) ColLCH99d = Converter.ToLCH99d(inColor2);
            if (inColor.Model != ColorModel.DEF) ColDEF = Converter.ToDEF(inColor2);
            if (inColor.Model != ColorModel.Bef) ColBef = Converter.ToBef(inColor2);
            if (inColor.Model != ColorModel.BCH) ColBCH = Converter.ToBCH(inColor2);

            if (inColor.Model != ColorModel.RGB)
            {
                if (RGBSpace != RGBSpaceName.ICC) ColRGB = Converter.ToRGB(inColor2, RGBSpace);
                else ColRGB = (ColorRGB)Converter.ToICC(Converter.ToICC_PCS(inColor2, RGB_ICC), RGB_ICC);
            }
            if (inColor.Model != ColorModel.HSV)
            {
                if (HSVSpace != RGBSpaceName.ICC) ColHSV = Converter.ToHSV(inColor2, HSVSpace);
                else ColHSV = (ColorHSV)Converter.ToICC(Converter.ToICC_PCS(inColor2, HSV_ICC), HSV_ICC);
            }
            if (inColor.Model != ColorModel.HSL)
            {
                if (HSLSpace != RGBSpaceName.ICC) ColHSL = Converter.ToHSL(inColor2, HSLSpace);
                else ColHSL = (ColorHSL)Converter.ToICC(Converter.ToICC_PCS(inColor2, HSL_ICC), HSL_ICC);
            }
            if (inColor.Model != ColorModel.CMY && CMY_ICC != null) ColCMY = (ColorCMY)Converter.ToICC(Converter.ToICC_PCS(inColor2, CMY_ICC), CMY_ICC);
            if (inColor.Model != ColorModel.CMYK && CMYK_ICC != null) ColCMYK = (ColorCMYK)Converter.ToICC(Converter.ToICC_PCS(inColor2, CMYK_ICC), CMYK_ICC);
            if (inColor.Model != ColorModel.YCbCr)
            {
                if (YCbCrSpace != YCbCrSpaceName.ICC) ColYCbCr = Converter.ToYCbCr(inColor2, YCbCrSpace);
                else ColYCbCr = (ColorYCbCr)Converter.ToICC(Converter.ToICC_PCS(inColor2, YCbCr_ICC), YCbCr_ICC);
            }
            if (inColor.Model != ColorModel.Gray)
            {
                if (CMY_ICC != null) ColGray = (ColorGray)Converter.ToICC(Converter.ToICC_PCS(inColor2, Gray_ICC), Gray_ICC);
                else ColGray = Converter.ToGray(inColor2);
            }
            if (!IsXColor(inColor.Model) && XColor_ICC != null) ColX = (ColorX)Converter.ToICC(Converter.ToICC_PCS(inColor2, XColor_ICC), XColor_ICC);

            FillFields();
        }

        private void SetSpaceList()
        {
            SpaceCoBoxes.Add(General_SpaceDrDo);
            SpaceCoBoxes.Add(RGB_CoBox);
            SpaceCoBoxes.Add(HSV_CoBox);
            SpaceCoBoxes.Add(HSL_CoBox);
        }

        private void SetRefWhiteList()
        {
            RefWhiteCoBoxes.Add(RefWhiteDrDo);
            RefWhiteCoBoxes.Add(XYZ_CoBox);
            RefWhiteCoBoxes.Add(Lab_CoBox);
            RefWhiteCoBoxes.Add(Luv_CoBox);
            RefWhiteCoBoxes.Add(LCHab_CoBox);
            RefWhiteCoBoxes.Add(LCHuv_CoBox);
            RefWhiteCoBoxes.Add(Yxy_CoBox);
            RefWhiteCoBoxes.Add(DEF_CoBox);
            RefWhiteCoBoxes.Add(Bef_CoBox);
            RefWhiteCoBoxes.Add(BCH_CoBox);
        }

        private void SetICCStuff(bool Enabled, ICC nICC, string Name)
        {
            switch (Name)
            {
                case "General_SpaceDrDo":
                    if (IsInit)
                    {
                        foreach (ComboBox c in SpaceCoBoxes)
                        {
                            if (c.Name != General_SpaceDrDo.Name) c.SelectedIndex = (int)BaseSpace;
                            else ColorConverter.StandardColorspace = BaseSpace;
                        }
                    }
                    break;
                case "RGB_CoBox":
                    RGB_ICCbox.Enabled = Enabled;
                    RGB_ICC = nICC;
                    RGB_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "HSL_CoBox":
                    HSL_ICCbox.Enabled = Enabled;
                    HSL_ICC = nICC;
                    HSL_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "HSV_CoBox":
                    HSV_ICCbox.Enabled = Enabled;
                    HSV_ICC = nICC;
                    HSV_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "YCbCr_CoBox":
                    YCbCr_ICCbox.Enabled = Enabled;
                    YCbCr_ICC = nICC;
                    YCbCr_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;

                case "CMY_ChICC":
                    CMY_ICCbox.Enabled = Enabled;
                    CMY_ICC = nICC;
                    CMY_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "CMYK_ChICC":
                    CMYK_ICCbox.Enabled = Enabled;
                    CMYK_ICC = nICC;
                    CMYK_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "Gray_ChICC":
                    Gray_ICCbox.Enabled = Enabled;
                    Gray_ICC = nICC;
                    Gray_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
                case "ColorX_ChICC":
                    XColor_ICCbox.Enabled = Enabled;
                    XColor_ICC = nICC;
                    XColor_ICCbox.Text = (nICC == null) ? String.Empty : nICC.ProfileName;
                    break;
            }
        }

        #endregion

        #region Other GUI Elements

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (TextBox t in this.Controls.OfType<TextBox>()) { if (t.Name != XColor_Channel.Name && t.Name != DescriptionBox.Name) t.Clear(); }
        }

        private void ColorPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (ColorSelectDialog.ShowDialog() == DialogResult.OK)
            {
                if (ColRGB == null)
                {
                    if (RGBSpace != RGBSpaceName.ICC) ColRGB = new ColorRGB(RGBSpace);
                    else ColRGB = new ColorRGB(RGB_ICC);
                }
                ColRGB.R = ColorSelectDialog.Color.R / 255d;
                ColRGB.G = ColorSelectDialog.Color.G / 255d;
                ColRGB.B = ColorSelectDialog.Color.B / 255d;
                RGB_R.Text = ColRGB.R.ToString("n4");
                RGB_G.Text = ColRGB.G.ToString("n4");
                RGB_B.Text = ColRGB.B.ToString("n4");
                ColorPanel.BackColor = ColorSelectDialog.Color;
                Hex_Label.Text = "Hex: #" + Convert.ToString(ColorSelectDialog.Color.R, 16).PadLeft(2, '0') + Convert.ToString(ColorSelectDialog.Color.G, 16).PadLeft(2, '0') + Convert.ToString(ColorSelectDialog.Color.B, 16).PadLeft(2, '0');
                RGB_Label.Text = "R: " + ColorSelectDialog.Color.R + " G: " + ColorSelectDialog.Color.G + " B: " + ColorSelectDialog.Color.B;
            }
        }

        private void XColor_Channel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int t = Convert.ToInt32(XColor_Channel.Text);
                XColor_Channels = (t < 2) ? 2 : (t > 15) ? 15 : t;
                XColor_Channel.Text = XColor_Channels.ToString();
                XColor_ChannelUpDo.Maximum = XColor_Channels;
            }
            catch (FormatException) { MessageBox.Show("Not a number!"); XColor_Channel.Text = (XColor_Channel.Text != "") ? XColor_Channel.Text.Substring(0, XColor_Channel.Text.Length - 1) : "2"; }
        }

        private void XColor_X_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (XColor_X.Text != "N/A")
                {
                    string a = XColor_X.Text.Replace(',', '.');
                    XColor[(int)XColor_ChannelUpDo.Value - 1] = Convert.ToDouble(a, cInfo);
                }
            }
            catch (FormatException) { MessageBox.Show("Not a number!"); XColor_X.Text = (XColor_X.Text != "") ? XColor_X.Text.Substring(0, XColor_X.Text.Length - 1) : "0"; }
        }

        private void XColor_ChannelUpDo_ValueChanged(object sender, EventArgs e)
        {
            XColor_Channel.Text = XColor_Channels.ToString();
            XColor_X.Text = (XColor_ICC == null) ? "N/A" : XColor[(int)XColor_ChannelUpDo.Value - 1].ToString("n6");
        }

        private void RefWhiteDrDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                foreach (ComboBox c in RefWhiteCoBoxes)
                {
                    if (c.Name != RefWhiteDrDo.Name) c.SelectedIndex = (int)Basewp.Name;
                    else ColorConverter.ReferenceWhite = Basewp;
                }
            }
        }

        private void Space_CoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool en = false; ICC nICC = null;
            if (((RGBSpaceName)((ComboBox)sender).SelectedIndex) == RGBSpaceName.ICC)
            {
                if (((ComboBox)sender).Name != General_SpaceDrDo.Name)
                {
                    iccOpenDialog.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Profiles");
                    iccOpenDialog.Title = "Choose ICC for " + ((ComboBox)sender).Name.Substring(0, ((ComboBox)sender).Name.IndexOf('_'));
                    if (iccOpenDialog.ShowDialog() == DialogResult.OK) { en = true; nICC = new ICC(iccOpenDialog.FileName); }
                    else { ((ComboBox)sender).SelectedIndex = PrevSpace; }
                }
            }
            else { en = false; }
            SetICCStuff(en, nICC, ((ComboBox)sender).Name);
            PrevSpace = ((ComboBox)sender).SelectedIndex;
        }

        private void YCbCr_CoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool en = false; ICC nICC = null;
            if (((YCbCrSpaceName)((ComboBox)sender).SelectedIndex) == YCbCrSpaceName.ICC)
            {
                iccOpenDialog.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Profiles");
                iccOpenDialog.Title = "Choose ICC for YCbCr";
                if (iccOpenDialog.ShowDialog() == DialogResult.OK) { en = true; nICC = new ICC(iccOpenDialog.FileName); }
                else { ((ComboBox)sender).SelectedIndex = PrevSpace; }
            }
            else { en = false; }
            SetICCStuff(en, nICC, ((ComboBox)sender).Name);
            PrevYCbCrSpace = YCbCr_CoBox.SelectedIndex;
        }

        private void RenderIntentCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.PreferredRenderingIntent = (RenderingIntent)RenderIntentCoBox.SelectedIndex;
        }

        private void ChroAdaptCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.ChromaticAdaptionMethod = (AdaptionMethod)ChroAdaptCoBox.SelectedIndex;
        }

        private void ChooseICC_Click(object sender, EventArgs e)
        {
            bool en = false; ICC nICC = null;
            iccOpenDialog.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Profiles");
            iccOpenDialog.Title = "Choose ICC for " + ((Button)sender).Name.Substring(0, ((Button)sender).Name.IndexOf('_'));
            if (iccOpenDialog.ShowDialog() == DialogResult.OK) { en = true; nICC = new ICC(iccOpenDialog.FileName); }
            SetICCStuff(en, nICC, ((Button)sender).Name);
        }

        private void Convert_Button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "RGB_Button":
                    try
                    {
                        if (RGBSpace != RGBSpaceName.ICC) ColRGB = new ColorRGB(RGBSpace, RGB[0], RGB[1], RGB[2]);
                        else ColRGB = new ColorRGB(RGB_ICC, RGB[0], RGB[1], RGB[2]);
                        Conversion(ColRGB);
                    }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "XYZ_Button":
                    try { ColXYZ = new ColorXYZ(XYZwp, XYZ[0], XYZ[1], XYZ[2]); Conversion(ColXYZ); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "Lab_Button":
                    try { ColLab = new ColorLab(Labwp, Lab[0], Lab[1], Lab[2]); Conversion(ColLab); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "Luv_Button":
                    try { ColLuv = new ColorLuv(Luvwp, Luv[0], Luv[1], Luv[2]); Conversion(ColLuv); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "Yxy_Button":
                    try { ColYxy = new ColorYxy(Yxywp, Yxy[0], Yxy[1], Yxy[2]); Conversion(ColYxy); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCHab_Button":
                    try { ColLCHab = new ColorLCHab(LCHabwp, LCHab[0], LCHab[1], LCHab[2]); Conversion(ColLCHab); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCHuv_Button":
                    try { ColLCHuv = new ColorLCHuv(LCHuvwp, LCHuv[0], LCHuv[1], LCHuv[2]); Conversion(ColLCHuv); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCH99_Button":
                    try { ColLCH99 = new ColorLCH99(LCH99[0], LCH99[1], LCH99[2]); Conversion(ColLCH99); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCH99b_Button":
                    try { ColLCH99b = new ColorLCH99b(LCH99b[0], LCH99b[1], LCH99b[2]); Conversion(ColLCH99b); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCH99c_Button":
                    try { ColLCH99c = new ColorLCH99c(LCH99c[0], LCH99c[1], LCH99c[2]); Conversion(ColLCH99c); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "LCH99d_Button":
                    try { ColLCH99d = new ColorLCH99d(LCH99d[0], LCH99d[1], LCH99d[2]); Conversion(ColLCH99d); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "HSV_Button":
                    try
                    {
                        if (HSVSpace != RGBSpaceName.ICC) ColHSV = new ColorHSV(HSVSpace, HSV[0], HSV[1], HSV[2]);
                        else ColHSV = new ColorHSV(HSV_ICC, HSV[0], HSV[1], HSV[2]);
                        Conversion(ColHSV);
                    }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "HSL_Button":
                    try
                    {
                        if (HSLSpace != RGBSpaceName.ICC) ColHSL = new ColorHSL(HSLSpace, HSL[0], HSL[1], HSL[2]);
                        else ColHSL = new ColorHSL(HSL_ICC, HSL[0], HSL[1], HSL[2]);
                        Conversion(ColHSL);
                    }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "CMY_Button":
                    try { ColCMY = new ColorCMY(CMY_ICC, CMY[0], CMY[1], CMY[2]); Conversion(ColCMY); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "CMYK_Button":
                    try { ColCMYK = new ColorCMYK(CMYK_ICC, CMYK[0], CMYK[1], CMYK[2], CMYK[3]); Conversion(ColCMYK); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "Gray_Button":
                    try
                    {
                        if (Gray_ICC != null) ColGray = new ColorGray(Gray_ICC, Gray[0]);
                        else ColGray = new ColorGray(Gray[0]);
                        Conversion(ColGray);
                    }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "YCbCr_Button":
                    try
                    {
                        if (YCbCrSpace == YCbCrSpaceName.ICC) ColYCbCr = new ColorYCbCr(YCbCr_ICC, YCbCr[0], YCbCr[1], YCbCr[2]);
                        else ColYCbCr = new ColorYCbCr(YCbCrSpace, YCbCr[0], YCbCr[1], YCbCr[2]);
                        Conversion(ColYCbCr);
                    }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "XColor_Button":
                    try { ColX = new ColorX(XColor_ICC, XColor.Take(XColor_Channels).ToArray()); Conversion(ColX); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "DEF_Button":
                    try { ColDEF = new ColorDEF(DEFwp, DEF[0], DEF[1], DEF[2]); Conversion(ColDEF); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "Bef_Button":
                    try { ColBef = new ColorBef(Befwp, Bef[0], Bef[1], Bef[2]); Conversion(ColBef); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;

                case "BCH_Button":
                    try { ColBCH = new ColorBCH(BCHwp, BCH[0], BCH[1], BCH[2]); Conversion(ColBCH); }
                    catch (FormatException) { MessageBox.Show("Not a number!"); }
                    catch (Exception ex) { MessageBox.Show("Error:" + Environment.NewLine + ex.Message); }
                    break;
            }
        }

        #endregion
    }
}
