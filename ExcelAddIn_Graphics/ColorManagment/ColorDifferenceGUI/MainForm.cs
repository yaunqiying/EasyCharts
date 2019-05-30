using System;
using System.Windows.Forms;
using System.Globalization;
using ColorManagment;

/*  This GUI compares the different color-difference-algorithms.
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

namespace ColorDifferenceGUI
{
    public partial class MainForm : Form
    {
        //When adding new colors:
        //Add model(s) to region "Subroutines" at methods "ColorFrom/ToDroDo_SelectedIndexChanged"
        //Add model(s) to region "Backgroundworker" at method "GetFrom/ToColor"
        //Add model(s) to region "Backgroundworker" at method "SetMaxUpDoValueFrom/To"

        #region Variables

        bool IsInit = false;
        int PrevToSpace;
        int PrevYCbCrToSpace;
        int PrevFromSpace;
        int PrevYCbCrFromSpace;
        ICC FromICC;
        ICC ToICC;
        ICC FromYCbCrICC;
        ICC ToYCbCrICC;
        double[] FromValues;
        double[] ToValues;
        ColorConverter Converter = new ColorConverter();
        CultureInfo cInfo = new CultureInfo("en-US");

        #endregion

        public MainForm()
        {
            InitializeComponent();
            RenderIntentDroDo.Items.AddRange(Enum.GetNames(typeof(RenderingIntent)));
            RenderIntentDroDo.SelectedIndex = (int)ColorConverter.PreferredRenderingIntent;

            ChromAdaptDroDo.Items.AddRange(Enum.GetNames(typeof(AdaptionMethod)));
            ChromAdaptDroDo.SelectedIndex = (int)ColorConverter.ChromaticAdaptionMethod;

            ColorspaceToDroDo.Items.AddRange(Enum.GetNames(typeof(RGBSpaceName)));
            ColorspaceToDroDo.SelectedIndex = (int)ColorConverter.StandardColorspace;
            ColorspaceFromDroDo.Items.AddRange(Enum.GetNames(typeof(RGBSpaceName)));
            ColorspaceFromDroDo.SelectedIndex = (int)ColorConverter.StandardColorspace;
            GenColorspaceDroDo.Items.AddRange(Enum.GetNames(typeof(RGBSpaceName)));
            GenColorspaceDroDo.SelectedIndex = (int)ColorConverter.StandardColorspace;

            YCbCrSpaceFromDroDo.Items.AddRange(Enum.GetNames(typeof(YCbCrSpaceName)));
            YCbCrSpaceFromDroDo.SelectedIndex = (int)ColorConverter.StandardYCbCrSpace;
            YCbCrSpaceToDroDo.Items.AddRange(Enum.GetNames(typeof(YCbCrSpaceName)));
            YCbCrSpaceToDroDo.SelectedIndex = (int)ColorConverter.StandardYCbCrSpace;
            GenYCbCrDroDo.Items.AddRange(Enum.GetNames(typeof(YCbCrSpaceName)));
            GenYCbCrDroDo.SelectedIndex = (int)ColorConverter.StandardYCbCrSpace;

            RefWhiteFromDroDo.Items.AddRange(Enum.GetNames(typeof(WhitepointName)));
            RefWhiteFromDroDo.SelectedIndex = (int)ColorConverter.ReferenceWhite.Name;
            RefWhiteToDroDo.Items.AddRange(Enum.GetNames(typeof(WhitepointName)));
            RefWhiteToDroDo.SelectedIndex = (int)ColorConverter.ReferenceWhite.Name;

            ColorFromDroDo.Items.AddRange(Enum.GetNames(typeof(ColorModel)));
            ColorFromDroDo.SelectedIndex = 0;
            ColorToDroDo.Items.AddRange(Enum.GetNames(typeof(ColorModel)));
            ColorToDroDo.SelectedIndex = 0;

            IsInit = true;
        }

        private void CompareButton_Click(object sender, EventArgs e)
        {
            Color FromColor = GetFromColor();
            Color ToColor = GetToColor();
            if (FromColor.IsICCcolor && !FromColor.IsPCScolor) { FromColor = Converter.ToICC(FromColor); }
            if (ToColor.IsICCcolor && !ToColor.IsPCScolor) { ToColor = Converter.ToICC(ToColor); }

            ColorLCH DIN99From = Converter.ToLCH99(FromColor);
            ColorLCH DIN99To = Converter.ToLCH99(ToColor);
            DIN99_E.Text = ColorDifference.GetDeltaE_DIN99((ColorLCH99)DIN99From, (ColorLCH99)DIN99To).ToString("n5", cInfo);
            DIN99_H.Text = ColorDifference.GetDeltaH_DIN99((ColorLCH99)DIN99From, (ColorLCH99)DIN99To).ToString("n5", cInfo);
            DIN99_C.Text = ColorDifference.GetDeltaC_DIN99((ColorLCH99)DIN99From, (ColorLCH99)DIN99To).ToString("n5", cInfo);
            DIN99From = Converter.ToLCH99b(FromColor);
            DIN99To = Converter.ToLCH99b(ToColor);
            DIN99b_E.Text = ColorDifference.GetDeltaE_DIN99((ColorLCH99b)DIN99From, (ColorLCH99b)DIN99To).ToString("n5", cInfo);
            DIN99b_H.Text = ColorDifference.GetDeltaH_DIN99((ColorLCH99b)DIN99From, (ColorLCH99b)DIN99To).ToString("n5", cInfo);
            DIN99b_C.Text = ColorDifference.GetDeltaC_DIN99((ColorLCH99b)DIN99From, (ColorLCH99b)DIN99To).ToString("n5", cInfo);
            DIN99From = Converter.ToLCH99c(FromColor);
            DIN99To = Converter.ToLCH99c(ToColor);
            DIN99c_E.Text = ColorDifference.GetDeltaE_DIN99((ColorLCH99c)DIN99From, (ColorLCH99c)DIN99To).ToString("n5", cInfo);
            DIN99c_H.Text = ColorDifference.GetDeltaH_DIN99((ColorLCH99c)DIN99From, (ColorLCH99c)DIN99To).ToString("n5", cInfo);
            DIN99c_C.Text = ColorDifference.GetDeltaC_DIN99((ColorLCH99c)DIN99From, (ColorLCH99c)DIN99To).ToString("n5", cInfo);
            DIN99From = Converter.ToLCH99d(FromColor);
            DIN99To = Converter.ToLCH99d(ToColor);
            DIN99d_E.Text = ColorDifference.GetDeltaE_DIN99((ColorLCH99d)DIN99From, (ColorLCH99d)DIN99To).ToString("n5", cInfo);
            DIN99d_H.Text = ColorDifference.GetDeltaH_DIN99((ColorLCH99d)DIN99From, (ColorLCH99d)DIN99To).ToString("n5", cInfo);
            DIN99d_C.Text = ColorDifference.GetDeltaC_DIN99((ColorLCH99d)DIN99From, (ColorLCH99d)DIN99To).ToString("n5", cInfo);

            ColorLab LabFrom = Converter.ToLab(FromColor);
            ColorLab LabTo = Converter.ToLab(ToColor);
            CIE76_E.Text = ColorDifference.GetDeltaE_CIE76(LabFrom, LabTo).ToString("n5", cInfo);
            CIE76_H.Text = "N/A"; //CIE76_H.Text = ColorDifference.GetDeltaH_CIE76(LabFrom, LabTo).ToString("n5", cInfo);
            CIE76_C.Text = "N/A";//CIE76_C.Text = ColorDifference.GetDeltaC_CIE76(LabFrom, LabTo).ToString("n5", cInfo);

            CIE94g_E.Text = ColorDifference.GetDeltaE_CIE94(LabFrom, LabTo, CIE94DifferenceMethod.GraphicArts).ToString("n5", cInfo);
            CIE94t_E.Text = ColorDifference.GetDeltaE_CIE94(LabFrom, LabTo, CIE94DifferenceMethod.Textiles).ToString("n5", cInfo);
            CIE94g_H.Text = CIE94t_H.Text = ColorDifference.GetDeltaH_CIE94(LabFrom, LabTo).ToString("n5", cInfo);
            CIE94g_C.Text = CIE94t_C.Text = ColorDifference.GetDeltaC_CIE94(LabFrom, LabTo).ToString("n5", cInfo);

            CIEDE2000_E.Text = ColorDifference.GetDeltaE_CIEDE2000(LabFrom, LabTo).ToString("n5", cInfo);
            CIEDE2000_H.Text = "N/A"; //CIEDE2000_H.Text = ColorDifference.GetDeltaH_CIEDE2000(LabFrom, LabTo).ToString("n5", cInfo);
            CIEDE2000_C.Text = "N/A"; //CIEDE2000_C.Text = ColorDifference.GetDeltaC_CIEDE2000(LabFrom, LabTo).ToString("n5", cInfo);

            CMC11_E.Text = ColorDifference.GetDeltaE_CMC(LabFrom, LabTo, CMCDifferenceMethod.Perceptibility).ToString("n5", cInfo);
            CMC21_E.Text = ColorDifference.GetDeltaE_CMC(LabFrom, LabTo, CMCDifferenceMethod.Acceptability).ToString("n5", cInfo);
            CMC11_H.Text = CMC21_H.Text = ColorDifference.GetDeltaH_CMC(LabFrom, LabTo).ToString("n5", cInfo);
            CMC11_C.Text = CMC21_C.Text = ColorDifference.GetDeltaC_CMC(LabFrom, LabTo).ToString("n5", cInfo);
        }

        #region In/Out-Color UI Elements

        private void ColorspaceFromDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (ColorspaceFromDroDo.SelectedIndex == (int)RGBSpaceName.ICC)
                {
                    if ((ColorModel)ColorFromDroDo.SelectedIndex == ColorModel.YCbCr) ColorspaceFromDroDo.SelectedIndex = PrevFromSpace;
                    else LoadICC(true, true);
                }
                else { RefWhiteFromDroDo.SelectedIndex = (int)RGBColorspace.GetColorspace((RGBSpaceName)ColorspaceFromDroDo.SelectedIndex).ReferenceWhite.Name; }
                PrevFromSpace = ColorspaceFromDroDo.SelectedIndex;
            }
        }

        private void ColorspaceToDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (ColorspaceToDroDo.SelectedIndex == (int)RGBSpaceName.ICC)
                {
                    if ((ColorModel)ColorToDroDo.SelectedIndex == ColorModel.YCbCr) ColorspaceToDroDo.SelectedIndex = PrevToSpace;
                    else LoadICC(false, true);
                }
                else { RefWhiteToDroDo.SelectedIndex = (int)RGBColorspace.GetColorspace((RGBSpaceName)ColorspaceToDroDo.SelectedIndex).ReferenceWhite.Name; }
                PrevToSpace = ColorspaceToDroDo.SelectedIndex;
            }
        }

        private void ColorFromDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((ColorModel)ColorFromDroDo.SelectedIndex)
            {
                case ColorModel.YCbCr:
                    ColorspaceFromDroDo.Enabled = true;
                    YCbCrSpaceFromDroDo.Enabled = true;
                    RefWhiteFromDroDo.Enabled = false;
                    ColorspaceFromDroDo_SelectedIndexChanged(null, null);
                    break;
                case ColorModel.RGB:
                case ColorModel.HSL:
                case ColorModel.HSV:
                    ColorspaceFromDroDo.Enabled = true;
                    YCbCrSpaceFromDroDo.Enabled = false;
                    RefWhiteFromDroDo.Enabled = false;
                    ColorspaceFromDroDo_SelectedIndexChanged(null, null);
                    break;

                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.Gray:
                    ColorspaceFromDroDo.Enabled = false;
                    YCbCrSpaceFromDroDo.Enabled = false;
                    RefWhiteFromDroDo.Enabled = true;
                    break;

                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                    ColorspaceFromDroDo.Enabled = false;
                    YCbCrSpaceFromDroDo.Enabled = false;
                    RefWhiteFromDroDo.Enabled = false;
                    break;

                default:
                    ColorspaceFromDroDo.Enabled = false;
                    YCbCrSpaceFromDroDo.Enabled = false;
                    RefWhiteFromDroDo.Enabled = false;
                    ColorspaceFromDroDo.SelectedIndex = (int)RGBSpaceName.ICC;
                    break;
            }
            SetMaxUpDoValueFrom();
            FromValues = new double[(int)FromColorChannelUpDo.Maximum];
            FromColorChannelUpDo_ValueChanged(null, null);
        }

        private void ColorToDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((ColorModel)ColorToDroDo.SelectedIndex)
            {
                case ColorModel.YCbCr:
                    ColorspaceToDroDo.Enabled = true;
                    YCbCrSpaceToDroDo.Enabled = true;
                    RefWhiteToDroDo.Enabled = false;
                    ColorspaceToDroDo_SelectedIndexChanged(null, null);
                    break;
                case ColorModel.RGB:
                case ColorModel.HSL:
                case ColorModel.HSV:
                    ColorspaceToDroDo.Enabled = true;
                    YCbCrSpaceToDroDo.Enabled = false;
                    RefWhiteToDroDo.Enabled = false;
                    ColorspaceToDroDo_SelectedIndexChanged(null, null);
                    break;

                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.Gray:
                    ColorspaceToDroDo.Enabled = false;
                    YCbCrSpaceToDroDo.Enabled = false;
                    RefWhiteToDroDo.Enabled = true;
                    break;

                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                    ColorspaceToDroDo.Enabled = false;
                    YCbCrSpaceToDroDo.Enabled = false;
                    RefWhiteToDroDo.Enabled = false;
                    break;

                default:
                    ColorspaceToDroDo.Enabled = false;
                    YCbCrSpaceToDroDo.Enabled = false;
                    RefWhiteToDroDo.Enabled = false;
                    ColorspaceToDroDo.SelectedIndex = (int)RGBSpaceName.ICC;
                    break;
            }
            SetMaxUpDoValueTo();
            ToValues = new double[(int)FromColorChannelUpDo.Maximum];
            ToColorChannelUpDo_ValueChanged(null, null);
        }

        private void YCbCrSpaceFromDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (YCbCrSpaceFromDroDo.SelectedIndex == (int)YCbCrSpaceName.ICC) { LoadICC(true, false); }
                PrevYCbCrFromSpace = YCbCrSpaceFromDroDo.SelectedIndex;
            }
        }

        private void YCbCrSpaceToDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (YCbCrSpaceToDroDo.SelectedIndex == (int)YCbCrSpaceName.ICC) { LoadICC(false, false); }
                PrevYCbCrToSpace = YCbCrSpaceToDroDo.SelectedIndex;
            }
        }

        private void FromColorChannelUpDo_ValueChanged(object sender, EventArgs e)
        {
            FromColorValueBox.Text = FromValues[(int)FromColorChannelUpDo.Value - 1].ToString("n4");
        }

        private void FromColorValueBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string a = FromColorValueBox.Text.Replace(',', '.');
                FromValues[(int)FromColorChannelUpDo.Value - 1] = Convert.ToDouble(a, cInfo);
            }
            catch (FormatException) { MessageBox.Show("Not a number!"); FromColorValueBox.Text = (FromColorValueBox.Text != "") ? FromColorValueBox.Text.Substring(0, FromColorValueBox.Text.Length - 1) : "0"; }
        }

        private void ToColorChannelUpDo_ValueChanged(object sender, EventArgs e)
        {
            ToColorValueBox.Text = ToValues[(int)ToColorChannelUpDo.Value - 1].ToString("n4");
        }

        private void ToColorValueBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string a = ToColorValueBox.Text.Replace(',', '.');
                ToValues[(int)ToColorChannelUpDo.Value - 1] = Convert.ToDouble(a, cInfo);
            }
            catch (FormatException) { MessageBox.Show("Not a number!"); ToColorValueBox.Text = (ToColorValueBox.Text != "") ? ToColorValueBox.Text.Substring(0, ToColorValueBox.Text.Length - 1) : "0"; }
        }

        #endregion

        #region Settings UI Elements

        private void ChromAdaptDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.ChromaticAdaptionMethod = (AdaptionMethod)ChromAdaptDroDo.SelectedIndex;
        }

        private void RenderIntentDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.PreferredRenderingIntent = (RenderingIntent)RenderIntentDroDo.SelectedIndex;
        }

        private void GenColorspaceDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.StandardColorspace = (RGBSpaceName)GenColorspaceDroDo.SelectedIndex;
        }

        private void GenYCbCrDroDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorConverter.StandardYCbCrSpace = (YCbCrSpaceName)GenYCbCrDroDo.SelectedIndex;
        }

        #endregion

        #region Subroutines

        private void LoadICC(bool From, bool IsRGB)
        {
            ICC nICC = null;
            iccOpenDialog.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Profiles");
            iccOpenDialog.Title = "Choose ICC";
            if (iccOpenDialog.ShowDialog() == DialogResult.OK) { nICC = new ICC(iccOpenDialog.FileName); }
            else
            {
                if (From)
                {
                    if (IsRGB) ColorspaceFromDroDo.SelectedIndex = PrevFromSpace;
                    else YCbCrSpaceFromDroDo.SelectedIndex = PrevYCbCrFromSpace;
                    ICCboxFrom.Text = String.Empty;
                }
                else
                {
                    if (IsRGB) ColorspaceToDroDo.SelectedIndex = PrevToSpace;
                    else YCbCrSpaceToDroDo.SelectedIndex = PrevYCbCrToSpace;
                    ICCboxTo.Text = String.Empty;
                }
            }

            if (nICC != null)
            {
                if (From)
                {
                    if (IsRGB) { FromICC = nICC; ICCboxFrom.Text = nICC.ProfileName; }
                    else { FromYCbCrICC = nICC; ICCYCbCrFromBox.Text = nICC.ProfileName; }
                }
                else
                {
                    if (IsRGB) { ToICC = nICC; ICCboxTo.Text = nICC.ProfileName; }
                    else { ToYCbCrICC = nICC; ICCYCbCrToBox.Text = nICC.ProfileName; }
                }
            }
        }

        private Color GetFromColor()
        {
            RGBSpaceName cn = (RGBSpaceName)ColorspaceFromDroDo.SelectedIndex;
            switch ((ColorModel)ColorFromDroDo.SelectedIndex)
            {
                case ColorModel.CIELab: return new ColorLab(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CIELCHab: return new ColorLCHab(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CIELCHuv: return new ColorLCHuv(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CIELuv: return new ColorLuv(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CIEXYZ: return new ColorXYZ(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CIEYxy: return new ColorYxy(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.Gray: return new ColorGray(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex), FromValues[0]);
                case ColorModel.LCH99: return new ColorLCH99(FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.LCH99b: return new ColorLCH99b(FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.LCH99c: return new ColorLCH99c(FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.LCH99d: return new ColorLCH99d(FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.HSL:
                    if (cn == RGBSpaceName.ICC) return new ColorHSL(FromICC, FromValues[0], FromValues[1], FromValues[2]);
                    else return new ColorHSL(cn, FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.HSV:
                    if (cn == RGBSpaceName.ICC) return new ColorHSV(FromICC, FromValues[0], FromValues[1], FromValues[2]);
                    else return new ColorHSV(cn, FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.RGB:
                    if (cn == RGBSpaceName.ICC) return new ColorRGB(FromICC, FromValues[0], FromValues[1], FromValues[2]);
                    else return new ColorRGB(cn, FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.YCbCr:
                    if ((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex == YCbCrSpaceName.ICC) return new ColorYCbCr(FromYCbCrICC, FromValues[0], FromValues[1], FromValues[2]);
                    else if (cn != RGBSpaceName.ICC) return new ColorYCbCr((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex, cn, FromValues[0], FromValues[1], FromValues[2]);
                    else throw new NotSupportedException();

                case ColorModel.CMY: return new ColorCMY(FromICC, FromValues[0], FromValues[1], FromValues[2]);
                case ColorModel.CMYK: return new ColorCMYK(FromICC, FromValues[0], FromValues[1], FromValues[2], FromValues[3]);

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
                    return new ColorX(FromICC, FromValues);

                default:
                    throw new NotSupportedException();
            }
        }

        private Color GetToColor()
        {
            RGBSpaceName cn = (RGBSpaceName)ColorspaceToDroDo.SelectedIndex;
            switch ((ColorModel)ColorToDroDo.SelectedIndex)
            {
                case ColorModel.CIELab: return new ColorLab(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CIELCHab: return new ColorLCHab(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CIELCHuv: return new ColorLCHuv(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CIELuv: return new ColorLuv(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CIEXYZ: return new ColorXYZ(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CIEYxy: return new ColorYxy(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.Gray: return new ColorGray(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex), ToValues[0]);
                case ColorModel.LCH99: return new ColorLCH99(ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.LCH99b: return new ColorLCH99b(ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.LCH99c: return new ColorLCH99c(ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.LCH99d: return new ColorLCH99d(ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.HSL:
                    if (cn == RGBSpaceName.ICC) return new ColorHSL(ToICC, ToValues[0], ToValues[1], ToValues[2]);
                    else return new ColorHSL(cn, ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.HSV:
                    if (cn == RGBSpaceName.ICC) return new ColorHSV(ToICC, ToValues[0], ToValues[1], ToValues[2]);
                    else return new ColorHSV(cn, ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.RGB:
                    if (cn == RGBSpaceName.ICC) return new ColorRGB(ToICC, ToValues[0], ToValues[1], ToValues[2]);
                    else return new ColorRGB(cn, ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.YCbCr:
                    if ((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex == YCbCrSpaceName.ICC) return new ColorYCbCr(ToYCbCrICC, ToValues[0], ToValues[1], ToValues[2]);
                    else if (cn != RGBSpaceName.ICC) return new ColorYCbCr((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex, cn, ToValues[0], ToValues[1], ToValues[2]);
                    else throw new NotSupportedException();

                case ColorModel.CMY: return new ColorCMY(ToICC, ToValues[0], ToValues[1], ToValues[2]);
                case ColorModel.CMYK: return new ColorCMYK(ToICC, ToValues[0], ToValues[1], ToValues[2], ToValues[3]);
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
                    return new ColorX(ToICC, ToValues);

                default:
                    throw new NotSupportedException();
            }
        }

        private void SetMaxUpDoValueFrom()
        {
            switch ((ColorModel)ColorFromDroDo.SelectedIndex)
            {
                case ColorModel.Gray: FromColorChannelUpDo.Maximum = 1; break;

                case ColorModel.Color2: FromColorChannelUpDo.Maximum = 2; break;

                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.HSL:
                case ColorModel.HSV:
                case ColorModel.RGB:
                case ColorModel.YCbCr:
                case ColorModel.CMY:
                case ColorModel.Color3:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                    FromColorChannelUpDo.Maximum = 3; break;

                case ColorModel.CMYK:
                case ColorModel.Color4:
                    FromColorChannelUpDo.Maximum = 4; break;

                case ColorModel.Color5: FromColorChannelUpDo.Maximum = 5; break;
                case ColorModel.Color6: FromColorChannelUpDo.Maximum = 6; break;
                case ColorModel.Color7: FromColorChannelUpDo.Maximum = 7; break;
                case ColorModel.Color8: FromColorChannelUpDo.Maximum = 8; break;
                case ColorModel.Color9: FromColorChannelUpDo.Maximum = 9; break;
                case ColorModel.Color10: FromColorChannelUpDo.Maximum = 10; break;
                case ColorModel.Color11: FromColorChannelUpDo.Maximum = 11; break;
                case ColorModel.Color12: FromColorChannelUpDo.Maximum = 12; break;
                case ColorModel.Color13: FromColorChannelUpDo.Maximum = 13; break;
                case ColorModel.Color14: FromColorChannelUpDo.Maximum = 14; break;
                case ColorModel.Color15: FromColorChannelUpDo.Maximum = 15; break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void SetMaxUpDoValueTo()
        {
            switch ((ColorModel)ColorToDroDo.SelectedIndex)
            {
                case ColorModel.Gray: ToColorChannelUpDo.Maximum = 1; break;

                case ColorModel.Color2: ToColorChannelUpDo.Maximum = 2; break;

                case ColorModel.CIELab:
                case ColorModel.CIELCHab:
                case ColorModel.CIELCHuv:
                case ColorModel.CIELuv:
                case ColorModel.CIEXYZ:
                case ColorModel.CIEYxy:
                case ColorModel.HSL:
                case ColorModel.HSV:
                case ColorModel.RGB:
                case ColorModel.YCbCr:
                case ColorModel.CMY:
                case ColorModel.Color3:
                case ColorModel.LCH99:
                case ColorModel.LCH99b:
                case ColorModel.LCH99c:
                case ColorModel.LCH99d:
                    ToColorChannelUpDo.Maximum = 3; break;

                case ColorModel.CMYK:
                case ColorModel.Color4:
                    ToColorChannelUpDo.Maximum = 4; break;

                case ColorModel.Color5: ToColorChannelUpDo.Maximum = 5; break;
                case ColorModel.Color6: ToColorChannelUpDo.Maximum = 6; break;
                case ColorModel.Color7: ToColorChannelUpDo.Maximum = 7; break;
                case ColorModel.Color8: ToColorChannelUpDo.Maximum = 8; break;
                case ColorModel.Color9: ToColorChannelUpDo.Maximum = 9; break;
                case ColorModel.Color10: ToColorChannelUpDo.Maximum = 10; break;
                case ColorModel.Color11: ToColorChannelUpDo.Maximum = 11; break;
                case ColorModel.Color12: ToColorChannelUpDo.Maximum = 12; break;
                case ColorModel.Color13: ToColorChannelUpDo.Maximum = 13; break;
                case ColorModel.Color14: ToColorChannelUpDo.Maximum = 14; break;
                case ColorModel.Color15: ToColorChannelUpDo.Maximum = 15; break;

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion
    }
}
