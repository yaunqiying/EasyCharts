using System;
using System.Linq;
using ICCReader;

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
    /// Handles conversions between colors specified in an ICC profile
    /// </summary>
    internal sealed class ICC_Converter
    {
        #region Variables

        private double[] InValues;
        private ColorModel InModel;
        private ICC Profile;
        private RenderingIntent PreferredRenderingIntent;
        private ICCconversionMethod ConversionMethod;
        private ICCconversionType ConversionType;
        private bool IsDefault;
        private MMath mmath = new MMath();

        #region Outsourced Variables

        private int dowhat;
        private double sic, sout;
        private double[] Mr, Mb, Mg, ic, output, result, t;
        private double[] c2 = new double[3];
        private double[,] M, tmp3x3 = new double[3, 3];
        private TagSignature sig;
        private TagDataEntry TagEntry1,TagEntry2,TagEntry3;

        #endregion

        #endregion

        #region Profile Descriptions

        /* Abstract:
         *  AToB0Tag
         *      DToB0Tag
         */

        /* Device Link:
        *  profileSequenceDescTag
        *  AToB0Tag
        *  colorantTableTag which is required only if the data colour space field is xCLR, where x is hexadecimal 2 to F (see 7.2.6);
        *  colorantTableOutTag, required only if the PCS field is xCLR, where x is hexadecimal 2 to F (see 7.2.6)
        *      DToB0Tag
        */

        /* Display Device:
        *  N-component LUT based
        *      AToB0 and BToA0
        *          AToB1Tag, AToB2Tag, BToA1Tag, and BToA2Tag -> Table 25
        *          DToB0Tag, DToB1Tag, DToB2Tag, DToB3Tag, BToD0Tag, BToD1Tag, BToD2Tag, BToD3Tag
        *          gamutTag (9.2.28)
        *          
        *  Three-component matrix based (usage: F.3)
        *      redMatrixColumnTag
        *      greenMatrixColumnTag
        *      blueMatrixColumnTag
        *      redTRCTag
        *      greenTRCTag
        *      blueTRCTag
        *          AToB1, AToB2, BToA0, BToA1, BToA2 -> Table 25
        *          gamutTag (9.2.28)
        * 
        *  monochrome (usage: F.2)
        *      grayTRCTag
        *          AToB0Tag, AToB1Tag, AToB2Tag, BToA0Tag, BToA1Tag, and BToA2Tag -> Table 25
        */

        /* Input Device
         *  N-component LUT based
         *      AToB0
         *          AToB1Tag, AToB2Tag, BToA1Tag, and BToA2Tag -> Table 25
         *          DToB0Tag, DToB1Tag, DToB2Tag, DToB3Tag, BToD0Tag, BToD1Tag, BToD2Tag, BToD3Tag
         *          gamutTag (9.2.28)
         * 
         *  Three-component matrix based (usage: F.3)
         *      redMatrixColumnTag
         *      greenMatrixColumnTag
         *      blueMatrixColumnTag
         *      redTRCTag
         *      greenTRCTag
         *      blueTRCTag
         *          AToB1, AToB2, BToA0, BToA1, BToA2 -> Table 25
         *          gamutTag (9.2.28)
         * 
         *  monochrome (usage: F.2)
         *      grayTRCTag
         *          AToB0Tag, AToB1Tag, AToB2Tag, BToA0Tag, BToA1Tag, and BToA2Tag -> Table 25
         */

        /* Output Device
         *  N-component LUT based
         *      AToB0Tag
         *      AToB1Tag
         *      AToB2Tag
         *      BToA0Tag
         *      BToA1Tag
         *      BToA2Tag
         *      gamutTag
         *      colorantTableTag, only for the xCLR colour spaces (see 7.2.6)
         * 
         *  monochrome (usage: F.2)
         *      grayTRCTag
         *          AToB0Tag, AToB1Tag, AToB2Tag, BToA0Tag, BToA1Tag, and BToA2Tag -> Table 25
         */

        /* Colorspace
         *  BToA0Tag
         *  AToB0Tag
         *      AToB1Tag, AToB2Tag, BToA1Tag, BToA2Tag, DToB0Tag, DToB1Tag, DToB2Tag, DToB3Tag, BToD0Tag, BToD1Tag, BToD2Tag, and BToD3Tag
         *      gamutTag (see 9.2.28)
         */

        #endregion

        #region Conversion Methods

        /// <summary>
        /// Converts the device color into the PCS color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="inColor">The device color (has to match the profiles device color type)</param>
        /// <returns>The converted color in the PCS color type</returns>
        public Color ToPCS(ICC Profile, Color inColor)
        {
            InValues = inColor.ColorArray;
            InModel = inColor.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = ColorConverter.PreferredRenderingIntent;
            IsDefault = true;
            return Do_Device();
        }

        /// <summary>
        /// Converts the device color into the PCS color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="inColor">The device color (has to match the profiles device color type)</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <returns>The converted color in the PCS color type</returns>
        public Color ToPCS(ICC Profile, Color inColor, RenderingIntent PrefRenderingIntent)
        {
            InValues = inColor.ColorArray;
            InModel = inColor.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = PrefRenderingIntent;
            IsDefault = true;
            return Do_Device();
        }

        /// <summary>
        /// Converts the device color into the PCS color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="inColor">The device color (has to match the profiles device color type)</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <returns>The converted color in the PCS color type</returns>
        public Color ToPCS(ICC Profile, Color inColor, ICCconversionMethod ConversionMethod, ICCconversionType ConversionType)
        {
            InValues = inColor.ColorArray;
            InModel = inColor.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = ColorConverter.PreferredRenderingIntent;
            this.ConversionMethod = ConversionMethod;
            this.ConversionType = ConversionType;
            IsDefault = false;
            return Do_Device();
        }

        /// <summary>
        /// Converts the device color into the PCS color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="inColor">The device color (has to match the profiles device color type)</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <returns>The converted color in the PCS color type</returns>
        public Color ToPCS(ICC Profile, Color inColor, RenderingIntent PrefRenderingIntent, ICCconversionMethod ConversionMethod, ICCconversionType ConversionType)
        {
            InValues = inColor.ColorArray;
            InModel = inColor.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = PrefRenderingIntent;
            this.ConversionMethod = ConversionMethod;
            this.ConversionType = ConversionType;
            IsDefault = false;
            return Do_Device();
        }


        /// <summary>
        /// Converts the PCS color into the device color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="pcs">The PCS color (has to match the profiles PCS color type)</param>
        /// <returns>The converted color in the device color type</returns>
        public Color ToDevice(ICC Profile, Color pcs)
        {
            InValues = pcs.ColorArray;
            InModel = pcs.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = ColorConverter.PreferredRenderingIntent;
            IsDefault = true;
            return Do_PCS();
        }

        /// <summary>
        /// Converts the PCS color into the device color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="pcs">The PCS color (has to match the profiles PCS color type)</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <returns>The converted color in the device color type</returns>
        public Color ToDevice(ICC Profile, Color pcs, RenderingIntent PrefRenderingIntent)
        {
            InValues = pcs.ColorArray;
            InModel = pcs.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = PrefRenderingIntent;
            IsDefault = true;
            return Do_PCS();
        }

        /// <summary>
        /// Converts the PCS color into the device color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="pcs">The PCS color (has to match the profiles PCS color type)</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <returns>The converted color in the device color type</returns>
        public Color ToDevice(ICC Profile, Color pcs, ICCconversionMethod ConversionMethod, ICCconversionType ConversionType)
        {
            InValues = pcs.ColorArray;
            InModel = pcs.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = ColorConverter.PreferredRenderingIntent;
            this.ConversionMethod = ConversionMethod;
            this.ConversionType = ConversionType;
            IsDefault = false;
            return Do_PCS();
        }

        /// <summary>
        /// Converts the PCS color into the device color
        /// </summary>
        /// <param name="Profile">The profile that will be used for the conversion</param>
        /// <param name="pcs">The PCS color (has to match the profiles PCS color type)</param>
        /// <param name="PrefRenderingIntent">The preferred rendering intent</param>
        /// <param name="ConversionMethod">The method of conversion</param>
        /// <param name="ConversionType">The type of conversion</param>
        /// <returns>The converted color in the device color type</returns>
        public Color ToDevice(ICC Profile, Color pcs, RenderingIntent PrefRenderingIntent, ICCconversionMethod ConversionMethod, ICCconversionType ConversionType)
        {
            InValues = pcs.ColorArray;
            InModel = pcs.Model;
            this.Profile = Profile;
            PreferredRenderingIntent = PrefRenderingIntent;
            this.ConversionMethod = ConversionMethod;
            this.ConversionType = ConversionType;
            IsDefault = false;
            return Do_PCS();
        }

        #endregion

        #region Conversion Handling

        private Color Do_PCS()
        {
            if (!IsSameSpace(InModel, Profile.Header.PCS)) { throw new ArgumentException("Input color does not match profile PCS color"); }

            switch (Profile.ICCType)
            {
                case ProfileClassName.Abstract: return DoAbstract();
                case ProfileClassName.DeviceLink: return DoDeviceLink();
                case ProfileClassName.DisplayDevice: return DoDisplayDevice_PCS();
                case ProfileClassName.InputDevice: return DoInputDevice_PCS();
                case ProfileClassName.OutputDevice: return DoOutputDevice_PCS();
                case ProfileClassName.ColorSpace: return DoColorspace_PCS();

                case ProfileClassName.NamedColor:
                default:
                    throw new ArgumentException("This type of ICC profile cannot be used to convert a color");
            }
        }

        private Color Do_Device()
        {
            if (!IsSameSpace(InModel, Profile.Header.DataColorspace)) { throw new ArgumentException("Input color does not match profile device color"); }

            switch (Profile.ICCType)
            {
                case ProfileClassName.Abstract: return DoAbstract();
                case ProfileClassName.DeviceLink: return DoDeviceLink();
                case ProfileClassName.DisplayDevice: return DoDisplayDevice_Device();
                case ProfileClassName.InputDevice: return DoInputDevice_Device();
                case ProfileClassName.OutputDevice: return DoOutputDevice_Device();
                case ProfileClassName.ColorSpace: return DoColorspace_Device();

                case ProfileClassName.NamedColor:
                default:
                    throw new ArgumentException("This type of ICC profile cannot be used to convert a color");
            }
        }


        private Color DoAbstract()
        {
            if (ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasDToB0) { result = PCS_Multiprocess_PCS(); }
            else if (Profile.HasAToB0) { result = PCS_LUT_PCS(); }
            else { throw new ArgumentException("Conversiondata of ICC not found"); }
            return ToColor(result, Profile.Header.DataColorspace);
        }

        private Color DoDeviceLink()
        {
            if (ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasDToB0) { result = Device_Multiprocess_Device(); }
            else if (Profile.HasAToB0) { result = Device_LUT_Device(); }
            else { throw new ArgumentException("Conversiondata of ICC not found"); }
            return ToColor(result, Profile.Header.PCS);
        }


        private Color DoDisplayDevice_PCS()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasBToA0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
                else if (Profile.HasRedTRC && ConversionType == ICCconversionType.ThreeComponentMatrix) { dowhat = 2; }
            }
            else
            {
                if (Profile.HasRedTRC) { dowhat = 2; }
                else if (Profile.HasBToA0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasBToA0) { result = PCS_LUT_Device(); }
                    else { result = PCS_Gray_Device(); }
                    break;
                case 1:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasBToD0) { result = PCS_Multiprocess_Device(); }
                    else { result = PCS_LUT_Device(); }
                    break;
                case 2:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasBToA1) { result = PCS_LUT_Device(); }
                    else { result = PCS_MatrixTRC_Device(); }
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.DataColorspace);
        }

        private Color DoInputDevice_PCS()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasBToA0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
                else if (Profile.HasRedTRC && ConversionType == ICCconversionType.ThreeComponentMatrix) { dowhat = 2; }
            }
            else
            {
                if (Profile.HasRedTRC) { dowhat = 2; }
                else if (Profile.HasBToA0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasBToA0) { result = PCS_LUT_Device(); }
                    else { result = PCS_Gray_Device(); }
                    break;
                case 1:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasBToD0) { result = PCS_Multiprocess_Device(); }
                    else { result = PCS_LUT_Device(); }
                    break;
                case 2:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasBToA1) { result = PCS_LUT_Device(); }
                    else { result = PCS_MatrixTRC_Device(); }
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.DataColorspace);
        }

        private Color DoOutputDevice_PCS()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasBToA0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
            }
            else
            {
                if (Profile.HasBToA0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasBToA0) { result = PCS_LUT_Device(); }
                    else { result = PCS_Gray_Device(); }
                    break;
                case 1:
                    result = PCS_LUT_Device();
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.DataColorspace);
        }

        private Color DoColorspace_PCS()
        {
            if (Profile.HasBToD0 && ConversionMethod == ICCconversionMethod.MultiprocessElement) { result = PCS_Multiprocess_Device(); }
            else if (Profile.HasBToA0) { result = PCS_LUT_Device(); }
            else { throw new ArgumentException("Conversiondata of ICC not found"); }
            return ToColor(result, Profile.Header.DataColorspace);
        }


        private Color DoDisplayDevice_Device()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasAToB0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
                else if (Profile.HasRedTRC && ConversionType == ICCconversionType.ThreeComponentMatrix) { dowhat = 2; }
            }
            else
            {
                if (Profile.HasRedTRC) { dowhat = 2; }
                else if (Profile.HasAToB0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasAToB0) { result = Device_LUT_PCS(); }
                    else { return Device_Gray_PCS(); }
                    break;
                case 1:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasDToB0) { result = Device_Multiprocess_PCS(); }
                    else { result = Device_LUT_PCS(); }
                    break;
                case 2:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasAToB1) { result = Device_LUT_PCS(); }
                    else { result = Device_MatrixTRC_PCS(); }
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.PCS);
        }

        private Color DoInputDevice_Device()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasAToB0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
                else if (Profile.HasRedTRC && ConversionType == ICCconversionType.ThreeComponentMatrix) { dowhat = 2; }
            }
            else
            {
                if (Profile.HasRedTRC) { dowhat = 2; }
                else if (Profile.HasAToB0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasAToB0) { result = Device_LUT_PCS(); }
                    else { return Device_Gray_PCS(); }
                    break;
                case 1:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.MultiprocessElement && Profile.HasDToB0) { result = Device_Multiprocess_PCS(); }
                    else { result = Device_LUT_PCS(); }
                    break;
                case 2:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasAToB1) { result = Device_LUT_PCS(); }
                    else { result = Device_MatrixTRC_PCS(); }
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.PCS);
        }

        private Color DoOutputDevice_Device()
        {
            dowhat = -1;
            if (!IsDefault)
            {
                if (Profile.HasGrayTRC && ConversionType == ICCconversionType.Monochrome) { dowhat = 0; }
                else if (Profile.HasAToB0 && ConversionType == ICCconversionType.NComponentLUT) { dowhat = 1; }
            }
            else
            {
                if (Profile.HasAToB0) { dowhat = 1; }
                else if (Profile.HasGrayTRC) { dowhat = 0; }
            }

            switch (dowhat)
            {
                case 0:
                    if (!IsDefault && ConversionMethod == ICCconversionMethod.LUT && Profile.HasAToB0) { result = Device_LUT_PCS(); }
                    else { return Device_Gray_PCS(); }
                    break;
                case 1:
                    result = Device_LUT_PCS();
                    break;

                default:
                    throw new ArgumentException("Conversiondata of ICC not found");
            }
            return ToColor(result, Profile.Header.PCS);
        }

        private Color DoColorspace_Device()
        {
            if (Profile.HasDToB0 && ConversionMethod == ICCconversionMethod.MultiprocessElement) { result = Device_Multiprocess_PCS(); }
            else if (Profile.HasAToB0) { result = Device_LUT_PCS(); }
            else { throw new ArgumentException("Conversiondata of ICC not found"); }
            return ToColor(result, Profile.Header.PCS);
        }

        #endregion

        #region Conversion

        #region Output-/Display-/Input-Device Profile

        private double[] PCS_MatrixTRC_Device()
        {
            output = new double[3];
            AdjustColor();
            if (ic.Length != 3) { throw new ArgumentException("Input color has wrong number of channels"); }
            if (InModel != ColorModel.CIEXYZ) { throw new ArgumentException("Profile connection space has to be XYZ"); }
            Mr = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.redMatrixColumnTag)).Data[0].GetArray();
            Mg = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.greenMatrixColumnTag)).Data[0].GetArray();
            Mb = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.blueMatrixColumnTag)).Data[0].GetArray();
            M = new double[3, 3] { { Mr[0], Mg[0], Mb[0] }, { Mr[1], Mg[1], Mb[1] }, { Mr[2], Mg[2], Mb[2] } };
            t = mmath.MultiplyMatrix(mmath.InvertMatrix(M), ic);
            
            TagEntry1 = Profile.GetEntry(TagSignature.redTRCTag);
            if (TagEntry1.Signature == TypeSignature.curve) { output[0] = ((curveTagDataEntry)TagEntry1).GetValueInverted(t[0]); }
            else if (TagEntry1.Signature == TypeSignature.parametricCurve) { output[0] = ((parametricCurveTagDataEntry)TagEntry1).Curve.InverseFunction(t[0]); }
            else { throw new CorruptProfileException("redTRCTag has wrong type"); }

            TagEntry2 = Profile.GetEntry(TagSignature.greenTRCTag);
            if (TagEntry2.Signature == TypeSignature.curve) { output[1] = ((curveTagDataEntry)TagEntry2).GetValueInverted(t[1]); }
            else if (TagEntry2.Signature == TypeSignature.parametricCurve) { output[1] = ((parametricCurveTagDataEntry)TagEntry2).Curve.InverseFunction(t[1]); }
            else { throw new CorruptProfileException("greenTRCTag has wrong type"); }

            TagEntry3 = Profile.GetEntry(TagSignature.blueTRCTag);
            if (TagEntry3.Signature == TypeSignature.curve) { output[2] = ((curveTagDataEntry)TagEntry3).GetValueInverted(t[2]); }
            else if (TagEntry3.Signature == TypeSignature.parametricCurve) { output[2] = ((parametricCurveTagDataEntry)TagEntry3).Curve.InverseFunction(t[2]); }
            else { throw new CorruptProfileException("blueTRCTag has wrong type"); }

            return output;
        }

        private double[] PCS_LUT_Device()
        {
            AdjustColor();
            if (ic.Length != 3) { throw new ArgumentException("Input color has wrong number of channels"); }
            if (InModel != ColorModel.CIEXYZ && InModel != ColorModel.CIELab) { throw new ArgumentException("Profile connection space has to be either XYZ or Lab"); }

            if (PreferredRenderingIntent == RenderingIntent.RelativeColorimetric) { sig = TagSignature.BToA1Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.Saturation) { sig = TagSignature.BToA2Tag; }
            else { sig = TagSignature.BToA0Tag; }
            TagEntry1 = Profile.GetEntry(sig);
            if (TagEntry1 == null) { TagEntry1 = Profile.GetEntry(TagSignature.BToA0Tag); }

            if (TagEntry1.Signature == TypeSignature.lut8) { output = ((lut8TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lut16) { output = ((lut16TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lutBToA) { output = ((lutBToATagDataEntry)TagEntry1).GetValue(ic); }
            else { throw new CorruptProfileException("BToATag has wrong type"); }

            return output;
        }

        private double[] PCS_Multiprocess_Device()
        {
            AdjustColor();
            if (ic.Length != 3) { throw new ArgumentException("Input color has wrong number of channels"); }
            if (InModel != ColorModel.CIEXYZ && InModel != ColorModel.CIELab) { throw new ArgumentException("Profile connection space has to be either XYZ or Lab"); }

            if (PreferredRenderingIntent == RenderingIntent.RelativeColorimetric) { sig = TagSignature.BToD1Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.Saturation) { sig = TagSignature.BToD2Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.AbsoluteColorimetric) { sig = TagSignature.BToD3Tag; }
            else { sig = TagSignature.BToD0Tag; }
            TagEntry1 = Profile.GetEntry(sig);
            if (TagEntry1 == null) { TagEntry1 = Profile.GetEntry(TagSignature.BToD0Tag); }

            return ((multiProcessElementsTagDataEntry)TagEntry1).GetValue(ic);
        }

        private double[] PCS_Gray_Device()
        {
            if (Profile.Header.PCS == ColorSpaceType.CIEXYZ) { sic = InValues[1]; }
            else if (Profile.Header.PCS == ColorSpaceType.CIELAB) { sic = InValues[0] / 100d; }
            else { throw new ArgumentException("Profile connection space is corrupt! (has to be either XYZ or Lab)"); }

            output = new double[1];
            TagEntry1 = Profile.GetEntry(TagSignature.grayTRCTag);
            if (TagEntry1.Signature == TypeSignature.curve) { output[0] = ((curveTagDataEntry)TagEntry1).GetValueInverted(sic); }
            else if (TagEntry1.Signature == TypeSignature.parametricCurve) { output[0] = ((parametricCurveTagDataEntry)TagEntry1).Curve.InverseFunction(sic); }
            else { throw new CorruptProfileException("grayTRCTag has wrong type"); }

            return output;
        }


        private double[] Device_MatrixTRC_PCS()
        {
            t = new double[3];
            AdjustColor();
            if (ic.Length != 3) { throw new ArgumentException("Input color has wrong number of channels"); }

            TagEntry1 = Profile.GetEntry(TagSignature.redTRCTag);
            if (TagEntry1.Signature == TypeSignature.curve) { t[0] = ((curveTagDataEntry)TagEntry1).GetValue(ic[0]); }
            else if (TagEntry1.Signature == TypeSignature.parametricCurve) { t[0] = ((parametricCurveTagDataEntry)TagEntry1).Curve.Function(ic[0]); }
            else { throw new CorruptProfileException("redTRCTag has wrong type"); }

            TagEntry2 = Profile.GetEntry(TagSignature.greenTRCTag);
            if (TagEntry2.Signature == TypeSignature.curve) { t[1] = ((curveTagDataEntry)TagEntry2).GetValue(ic[1]); }
            else if (TagEntry2.Signature == TypeSignature.parametricCurve) { t[1] = ((parametricCurveTagDataEntry)TagEntry2).Curve.Function(ic[1]); }
            else { throw new CorruptProfileException("greenTRCTag has wrong type"); }

            TagEntry3 = Profile.GetEntry(TagSignature.blueTRCTag);
            if (TagEntry3.Signature == TypeSignature.curve) { t[2] = ((curveTagDataEntry)TagEntry3).GetValue(ic[2]); }
            else if (TagEntry3.Signature == TypeSignature.parametricCurve) { t[2] = ((parametricCurveTagDataEntry)TagEntry3).Curve.Function(ic[2]); }
            else { throw new CorruptProfileException("blueTRCTag has wrong type"); }

            Mr = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.redMatrixColumnTag)).Data[0].GetArray();
            Mg = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.greenMatrixColumnTag)).Data[0].GetArray();
            Mb = ((XYZTagDataEntry)Profile.GetEntry(TagSignature.blueMatrixColumnTag)).Data[0].GetArray();
            M = new double[3, 3] { { Mr[0], Mg[0], Mb[0] }, { Mr[1], Mg[1], Mb[1] }, { Mr[2], Mg[2], Mb[2] } };

            return mmath.MultiplyMatrix(M, t);
        }

        private double[] Device_LUT_PCS()
        {
            AdjustColor();
            if (PreferredRenderingIntent == RenderingIntent.RelativeColorimetric) { sig = TagSignature.AToB1Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.Saturation) { sig = TagSignature.AToB2Tag; }
            else { sig = TagSignature.AToB0Tag; }
            TagEntry1 = Profile.GetEntry(sig);
            if (TagEntry1 == null) { TagEntry1 = Profile.GetEntry(TagSignature.AToB0Tag); }

            if (TagEntry1.Signature == TypeSignature.lut8) { output = ((lut8TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lut16) { output = ((lut16TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lutAToB) { output = ((lutAToBTagDataEntry)TagEntry1).GetValue(ic); }
            else { throw new CorruptProfileException("AToBTag has wrong type"); }

            return output;
        }

        private double[] Device_Multiprocess_PCS()
        {
            if (PreferredRenderingIntent == RenderingIntent.RelativeColorimetric) { sig = TagSignature.DToB1Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.Saturation) { sig = TagSignature.DToB2Tag; }
            else if (PreferredRenderingIntent == RenderingIntent.AbsoluteColorimetric) { sig = TagSignature.DToB3Tag; }
            else { sig = TagSignature.DToB0Tag; }
            TagEntry1 = Profile.GetEntry(sig);
            if (TagEntry1 == null) { TagEntry1 = Profile.GetEntry(TagSignature.DToB0Tag); }

            AdjustColor();
            return ((multiProcessElementsTagDataEntry)TagEntry1).GetValue(ic);
        }

        private Color Device_Gray_PCS()
        {
            TagEntry1 = Profile.GetEntry(TagSignature.grayTRCTag);
            if (TagEntry1.Signature == TypeSignature.curve) { sout = ((curveTagDataEntry)TagEntry1).GetValue(InValues[0]); }
            else if (TagEntry1.Signature == TypeSignature.parametricCurve) { sout = ((parametricCurveTagDataEntry)TagEntry1).Curve.Function(InValues[0]); }
            else { throw new CorruptProfileException("grayTRCTag has wrong type"); }

            if (Profile.Header.PCS == ColorSpaceType.CIEXYZ) { return new ColorXYZ(Profile.ReferenceWhite, 0, sout, 0); }
            else if (Profile.Header.PCS == ColorSpaceType.CIELAB) { return new ColorLab(Profile.ReferenceWhite, sout * 100, 0, 0); }
            else { throw new ArgumentException("Profile connection space is corrupt! (has to be either XYZ or Lab)"); }
        }

        #endregion

        #region Device Link Profile

        private double[] Device_LUT_Device()
        {
            TagEntry1 = Profile.GetEntry(TagSignature.AToB0Tag);
            AdjustColor();
            if (TagEntry1.Signature == TypeSignature.lut8) { output = ((lut8TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lut16) { output = ((lut16TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lutAToB) { output = ((lutAToBTagDataEntry)TagEntry1).GetValue(ic); }
            else { throw new CorruptProfileException("AToBTag has wrong type"); }

            return output;
        }

        private double[] Device_Multiprocess_Device()
        {
            TagEntry1 = Profile.GetEntry(TagSignature.DToB0Tag);
            AdjustColor();
            return ((multiProcessElementsTagDataEntry)TagEntry1).GetValue(ic);
        }

        #endregion

        #region Abstract Profile

        private double[] PCS_LUT_PCS()
        {
            TagEntry1 = Profile.GetEntry(TagSignature.AToB0Tag);
            AdjustColor();
            if (TagEntry1.Signature == TypeSignature.lut8) { output = ((lut8TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lut16) { output = ((lut16TagDataEntry)TagEntry1).GetValue(ic); }
            else if (TagEntry1.Signature == TypeSignature.lutAToB) { output = ((lutAToBTagDataEntry)TagEntry1).GetValue(ic); }
            else { throw new CorruptProfileException("AToBTag has wrong type"); }

            return output;
        }

        private double[] PCS_Multiprocess_PCS()
        {
            TagEntry1 = Profile.GetEntry(TagSignature.DToB0Tag);
            AdjustColor();
            return ((multiProcessElementsTagDataEntry)TagEntry1).GetValue(ic);
        }

        #endregion

        #endregion

        #region Converting Subroutines

        private void AdjustColor()
        {
            ic = (double[])InValues.Clone();
            if (InModel == ColorModel.CIELab || InModel == ColorModel.CIELuv) { ic[0] /= 100; ic[1] = (ic[1] + 140) / 280d; ic[2] = (ic[2] + 140) / 280d; }
            else if (InModel == ColorModel.CMY || InModel == ColorModel.CMYK) { ic = ic.Select(t => t / 100d).ToArray(); }
            else if (InModel == ColorModel.HSV) { ic[0] /= 360d; }
            else if (InModel == ColorModel.HSL) { ic[0] /= 360d; ic[1] = InValues[2]; ic[2] = InValues[1]; }
        }

        internal static bool IsSameSpace(ColorModel Model, ColorSpaceType Space)
        {
            if (Enum.GetName(typeof(ColorModel), Model).ToLower() == Enum.GetName(typeof(ColorSpaceType), Space).ToLower()) return true;
            else return false;
        }

        private Color ToColor(double[] inValues, ColorSpaceType outType)
        {
            switch (outType)
            {
                case ColorSpaceType.RGB:
                    return new ColorRGB(Profile, inValues[0], inValues[1], inValues[2], false);
                case ColorSpaceType.HSV:
                    return new ColorHSV(Profile, inValues[0], inValues[1], inValues[2]);
                case ColorSpaceType.HLS:
                    return new ColorHSL(Profile, inValues[0], inValues[2], inValues[1]);
                case ColorSpaceType.CIEYxy:
                    return new ColorYxy(Profile.ReferenceWhite, inValues[0], inValues[1], inValues[2]);
                case ColorSpaceType.CIEXYZ:
                    return new ColorXYZ(Profile.ReferenceWhite, inValues[0], inValues[1], inValues[2]);
                case ColorSpaceType.CIELUV:
                    return new ColorLuv(Profile.ReferenceWhite, inValues[0] * 100, inValues[1], inValues[2]);
                case ColorSpaceType.CIELAB:
                    return new ColorLab(Profile.ReferenceWhite, inValues[0] * 100, inValues[1], inValues[2]);
                case ColorSpaceType.CMY:
                    return new ColorCMY(Profile, inValues[0] * 100, inValues[1] * 100, inValues[2] * 100);
                case ColorSpaceType.CMYK:
                    return new ColorCMYK(Profile, inValues[0] * 100, inValues[1] * 100, inValues[2] * 100, inValues[3] * 100);
                case ColorSpaceType.Gray:
                    return new ColorGray(Profile, inValues[0]);
                case ColorSpaceType.YCbCr:
                    return new ColorYCbCr(Profile, inValues[0], inValues[1], inValues[2]);
                case ColorSpaceType.Color2:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color3:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color4:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color5:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color6:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color7:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color8:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color9:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color10:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color11:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color12:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color13:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color14:
                    return new ColorX(Profile, inValues);
                case ColorSpaceType.Color15:
                    return new ColorX(Profile, inValues);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
