using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ColorManagment;

/*  This GUI converts between colormodels and spaces that are defined in the
    ColorManagment library and measures the time it takes to do so.
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

namespace SpeedGUI
{
    public partial class MainForm : Form
    {
        //When adding new colors:
        //Add model(s) to region "In/Out-Color UI Elements" at methods "ColorFrom/ToDroDo_SelectedIndexChanged" 
        //Add model(s) to region "Subroutines" at methods "GetFrom/ToColor"
        //Add model(s) to region "Backgroundworker" at method "MainWorker_DoWork"

        #region Variables

        bool IsInit = false;
        int PrevToSpace;
        int PrevYCbCrToSpace;
        int PrevFromSpace;
        int PrevYCbCrFromSpace;
        int Threadcount = 1;
        ICC FromICC;
        ICC ToICC;
        ICC FromYCbCrICC;
        ICC ToYCbCrICC;

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

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (MainWorker.IsBusy) { MainWorker.CancelAsync(); }
            else
            {
                object[] args = new object[] { (int)IterationUpDo.Value, GetFromColor(), GetToColor(), FastChBox.Checked };
                SettingsBox.Enabled = false;
                MainWorker.RunWorkerAsync(args); StartStopButton.Text = "Stop";
            }
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

        private void AutoThreadChBox_CheckedChanged(object sender, EventArgs e)
        {
            ThreadUpDo.Enabled = !AutoThreadChBox.Checked;
            if (AutoThreadChBox.Checked) ThreadUpDo.Value = Environment.ProcessorCount;
        }

        private void ThreadUpDo_ValueChanged(object sender, EventArgs e)
        {
            Threadcount = (int)ThreadUpDo.Value;
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
                case ColorModel.CIELab: return new ColorLab(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.CIELCHab: return new ColorLCHab(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.CIELCHuv: return new ColorLCHuv(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.CIELuv: return new ColorLuv(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.CIEXYZ: return new ColorXYZ(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.CIEYxy: return new ColorYxy(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.Gray: return new ColorGray(new Whitepoint((WhitepointName)RefWhiteFromDroDo.SelectedIndex));
                case ColorModel.LCH99: return new ColorLCH99();
                case ColorModel.LCH99b: return new ColorLCH99b();
                case ColorModel.LCH99c: return new ColorLCH99c();
                case ColorModel.LCH99d: return new ColorLCH99d();
                case ColorModel.HSL:
                    if (cn == RGBSpaceName.ICC) return new ColorHSL(FromICC);
                    else return new ColorHSL(cn);
                case ColorModel.HSV:
                    if (cn == RGBSpaceName.ICC) return new ColorHSV(FromICC);
                    else return new ColorHSV(cn);
                case ColorModel.RGB:
                    if (cn == RGBSpaceName.ICC) return new ColorRGB(FromICC);
                    else return new ColorRGB(cn);
                case ColorModel.YCbCr:
                    if ((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex == YCbCrSpaceName.ICC) return new ColorYCbCr(FromYCbCrICC);
                    else if (cn != RGBSpaceName.ICC) return new ColorYCbCr((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex, cn);
                    else throw new NotSupportedException();

                case ColorModel.CMY: return new ColorCMY(FromICC);
                case ColorModel.CMYK: return new ColorCMYK(FromICC);

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
                    return new ColorX(FromICC);

                default:
                    throw new NotSupportedException();
            }
        }

        private Color GetToColor()
        {
            RGBSpaceName cn = (RGBSpaceName)ColorspaceToDroDo.SelectedIndex;
            switch ((ColorModel)ColorToDroDo.SelectedIndex)
            {
                case ColorModel.CIELab: return new ColorLab(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.CIELCHab: return new ColorLCHab(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.CIELCHuv: return new ColorLCHuv(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.CIELuv: return new ColorLuv(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.CIEXYZ: return new ColorXYZ(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.CIEYxy: return new ColorYxy(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.Gray: return new ColorGray(new Whitepoint((WhitepointName)RefWhiteToDroDo.SelectedIndex));
                case ColorModel.LCH99: return new ColorLCH99();
                case ColorModel.LCH99b: return new ColorLCH99b();
                case ColorModel.LCH99c: return new ColorLCH99c();
                case ColorModel.LCH99d: return new ColorLCH99d();
                case ColorModel.HSL:
                    if (cn == RGBSpaceName.ICC) return new ColorHSL(ToICC);
                    else return new ColorHSL(cn);
                case ColorModel.HSV:
                    if (cn == RGBSpaceName.ICC) return new ColorHSV(ToICC);
                    else return new ColorHSV(cn);
                case ColorModel.RGB:
                    if (cn == RGBSpaceName.ICC) return new ColorRGB(ToICC);
                    else return new ColorRGB(cn);
                case ColorModel.YCbCr:
                    if ((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex == YCbCrSpaceName.ICC) return new ColorYCbCr(ToYCbCrICC);
                    else if (cn != RGBSpaceName.ICC) return new ColorYCbCr((YCbCrSpaceName)YCbCrSpaceToDroDo.SelectedIndex, cn);
                    else throw new NotSupportedException();

                case ColorModel.CMY: return new ColorCMY(ToICC);
                case ColorModel.CMYK: return new ColorCMYK(ToICC);

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
                    return new ColorX(ToICC);

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region Backgroundworker

        private static long time;
        private static int progress;

        private void Measure(object Data)
        {
            int end = (int)((object[])Data)[0];
            Color FromColor = (Color)((object[])Data)[1];
            Color ToColor = (Color)((object[])Data)[2];
            bool DoFast = (bool)((object[])Data)[3];
            Stopwatch watch = new Stopwatch();
            Color From2Color = FromColor;
            ColorConverter conv = new ColorConverter();

            if (DoFast)
            {
                RGBSpaceName n = ColorConverter.StandardColorspace;
                YCbCrSpaceName m = ColorConverter.StandardYCbCrSpace;
                if (ToColor.Model == ColorModel.YCbCr)
                {
                    n = ((ColorYCbCr)ToColor).BaseSpaceName;
                    m = ((ColorYCbCr)ToColor).SpaceName;
                }
                else if (ToColor.Model == ColorModel.RGB) n = ((ColorRGB)ToColor).SpaceName;
                else if (ToColor.Model == ColorModel.HSV) n = ((ColorRGB)ToColor).SpaceName;
                else if (ToColor.Model == ColorModel.HSL) n = ((ColorRGB)ToColor).SpaceName;

                if (n != ColorConverter.StandardColorspace) conv.SetFast(FromColor, ToColor.Model, n, m);
                else conv.SetFast(FromColor, ToColor.Model, ToColor.ReferenceWhite);
            }
            
            for (int i = 0; i < end / Threadcount && !MainWorker.CancellationPending; i++)
            {
                if (DoFast)
                {
                    watch.Start();
                    conv.ConvertFast(FromColor);
                    watch.Stop();
                }
                else
                {
                    if (FromColor.IsICCcolor && !FromColor.IsPCScolor)
                    {
                        watch.Start();
                        From2Color = conv.ToICC(FromColor);
                        watch.Stop();
                    }

                    switch (ToColor.Model)
                    {
                        case ColorModel.CIELab:
                            watch.Start();
                            conv.ToLab(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.CIELCHab:
                            watch.Start();
                            conv.ToLCHab(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.CIELCHuv:
                            watch.Start();
                            conv.ToLCHuv(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.CIELuv:
                            watch.Start();
                            conv.ToLuv(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.CIEXYZ:
                            watch.Start();
                            conv.ToXYZ(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.CIEYxy:
                            watch.Start();
                            conv.ToYxy(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.Gray:
                            watch.Start();
                            conv.ToGray(From2Color, ToColor.ReferenceWhite);
                            watch.Stop();
                            break;
                        case ColorModel.HSL:
                            RGBSpaceName n = ((ColorHSL)ToColor).SpaceName;
                            if (n == RGBSpaceName.ICC) goto default;
                            watch.Start();
                            conv.ToHSL(From2Color, n);
                            watch.Stop();
                            break;
                        case ColorModel.HSV:
                            n = ((ColorHSV)ToColor).SpaceName;
                            if (n == RGBSpaceName.ICC) goto default;
                            watch.Start();
                            conv.ToHSV(From2Color, n);
                            watch.Stop();
                            break;
                        case ColorModel.RGB:
                            n = ((ColorRGB)ToColor).SpaceName;
                            if (n == RGBSpaceName.ICC) goto default;
                            watch.Start();
                            conv.ToRGB(From2Color, n);
                            watch.Stop();
                            break;
                        case ColorModel.YCbCr:
                            n = ((ColorYCbCr)ToColor).BaseSpaceName;
                            YCbCrSpaceName m = ((ColorYCbCr)ToColor).SpaceName;
                            if (m == YCbCrSpaceName.ICC) goto default;
                            watch.Start();
                            conv.ToYCbCr(From2Color, n, m);
                            watch.Stop();
                            break;
                        case ColorModel.LCH99:
                            watch.Start();
                            conv.ToLCH99(From2Color);
                            watch.Stop();
                            break;
                        case ColorModel.LCH99b:
                            watch.Start();
                            conv.ToLCH99b(From2Color);
                            watch.Stop();
                            break;
                        case ColorModel.LCH99c:
                            watch.Start();
                            conv.ToLCH99c(From2Color);
                            watch.Stop();
                            break;
                        case ColorModel.LCH99d:
                            watch.Start();
                            conv.ToLCH99d(From2Color);
                            watch.Stop();
                            break;


                        default:
                            watch.Start();
                            conv.ToICC(conv.ToICC_PCS(From2Color, ToICC), ToICC);
                            watch.Stop();
                            break;
                    }
                }

                progress++;
                time += watch.ElapsedTicks;
                if (progress % 100 == 0) MainWorker.ReportProgress(progress * 100 / (end - 1));
            }
        }

        private void MainWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int end = (int)((object[])e.Argument)[0];
            time = 0;
            progress = 0;

            Thread[] threads = new Thread[Threadcount];
            for (int i = 0; i < Threadcount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(Measure));
                threads[i].Start(e.Argument);
            }
            for (int i = 0; i < Threadcount; i++) threads[i].Join();
            
            e.Result = (long)(((time / (double)Stopwatch.Frequency) * 1000d) / (double)end);
            e.Cancel = MainWorker.CancellationPending;
        }

        private void MainWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MainProgressBar.Value = e.ProgressPercentage;
        }

        private void MainWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeTotalLabel.Text = (e.Cancelled) ? "Cancelled" : (long)e.Result + "ms";
            TimeIterationLabel.Text = (e.Cancelled) ? "Cancelled" : ((((long)e.Result) / IterationUpDo.Value) * 1000).ToString("n6") + "µs";
            StartStopButton.Text = "Start";
            MainProgressBar.Value = 0;
            SettingsBox.Enabled = true;
            GC.Collect();
        }

        #endregion
    }
}
