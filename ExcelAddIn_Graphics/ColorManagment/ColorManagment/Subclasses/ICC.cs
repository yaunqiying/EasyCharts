using System;
using System.IO;
using ICCReader;
using System.Globalization;

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
    /// A ICC profile
    /// </summary>
    public sealed class ICC
    {
        #region Variables

        /// <summary>
        /// Name of the profile
        /// </summary>
        public string ProfileName { get; private set; }

        /// <summary>
        /// ICC Profile Type
        /// </summary>
        public ProfileClassName ICCType { get { return profile.Header.ProfileClass; } }
        /// <summary>
        /// ICC Header
        /// </summary>
        public ICCHeader Header { get { return profile.Header; } }
        /// <summary>
        /// ICC Tag Table
        /// </summary>
        public ICCTagTable TagTable { get { return profile.TagTable; } }
        /// <summary>
        /// ICC Data
        /// </summary>
        public TagDataEntry[] TagData { get { return profile.TagData; } }


        /// <summary>
        /// The reference white
        /// </summary>
        public Whitepoint ReferenceWhite { get { return wp; } }
        /// <summary>
        /// Description of this profile in localized strings
        /// </summary>
        public LocalizedString[] ProfileDescription { get { return des; } }
        /// <summary>
        /// Copyright information of this profile in localized strings
        /// </summary>
        public LocalizedString[] CopyrightInformation { get { return cprs; } }
        /// <summary>
        /// Chromatic adaption matrix
        /// </summary>
        public double[,] Ma { get { return ma; } }
        /// <summary>
        /// Inverted chromatic adaption matrix
        /// </summary>
        public double[,] Ma1 { get { return ma1; } }


        #region Has profile Tag-X

        /// <summary>
        /// States if a conversion is possible with Matrix TRC
        /// </summary>
        public bool MatrixTRCpossible { get { return (HasRedTRC) ? true : false; } }
        /// <summary>
        /// States if a conversion to the PCS is possible with Multiprocess Elements
        /// </summary>
        public bool MultiprocessPossible_ToPCS { get { return (HasDToB0) ? true : false; } }
        /// <summary>
        /// States if a conversion to the device color is possible with Multiprocess Elements
        /// </summary>
        public bool MultiprocessPossible_ToDevice { get { return (HasBToD0) ? true : false; } }
        /// <summary>
        /// States if a conversion to the PCS is possible with an N-LUT
        /// </summary>
        public bool nLUTpossible_ToPCS { get { return (HasAToB0) ? true : false; } }
        /// <summary>
        /// States if a conversion to the device color is possible with an N-LUT
        /// </summary>
        public bool nLUTpossible_ToDevice { get { return (HasBToA0) ? true : false; } }
        /// <summary>
        /// States if a monochrome conversion is possible
        /// </summary>
        public bool MonochromePossible { get { return (HasGrayTRC) ? true : false; } }

        /// <summary>
        /// States if the profile has a gray TRC tag
        /// </summary>
        public bool HasGrayTRC { get; private set; }
        /// <summary>
        /// States if the profile has a red TRC tag
        /// </summary>
        public bool HasRedTRC { get; private set; }
        /// <summary>
        /// States if the profile has a green TRC tag
        /// </summary>
        public bool HasGreenTRC { get; private set; }
        /// <summary>
        /// States if the profile has a blue TRC tag
        /// </summary>
        public bool HasBlueTRC { get; private set; }

        /// <summary>
        /// States if the profile has a B to A0 tag (PCS->Device: perceptual)
        /// </summary>
        public bool HasBToA0 { get; private set; }
        /// <summary>
        /// States if the profile has a B to A1 tag (PCS->Device: relative colorimetric)
        /// </summary>
        public bool HasBToA1 { get; private set; }
        /// <summary>
        /// States if the profile has a B to A2 tag (PCS->Device: saturation)
        /// </summary>
        public bool HasBToA2 { get; private set; }

        /// <summary>
        /// States if the profile has a A to B0 tag (Device->PCS: perceptual)
        /// </summary>
        public bool HasAToB0 { get; private set; }
        /// <summary>
        /// States if the profile has a A to B1 tag (Device->PCS: relative colorimetric)
        /// </summary>
        public bool HasAToB1 { get; private set; }
        /// <summary>
        /// States if the profile has a A to B2 tag (Device->PCS: saturation)
        /// </summary>
        public bool HasAToB2 { get; private set; }

        /// <summary>
        /// States if the profile has a B to D0 tag (PCS->Device: perceptual)
        /// </summary>
        public bool HasBToD0 { get; private set; }
        /// <summary>
        /// States if the profile has a B to D1 tag (PCS->Device: relative colorimetric)
        /// </summary>
        public bool HasBToD1 { get; private set; }
        /// <summary>
        /// States if the profile has a B to D2 tag (PCS->Device: saturation)
        /// </summary>
        public bool HasBToD2 { get; private set; }
        /// <summary>
        /// States if the profile has a B to D3 tag (PCS->Device: absolute colorimetric)
        /// </summary>
        public bool HasBToD3 { get; private set; }

        /// <summary>
        /// States if the profile has a D to B0 tag (Device->PCS: perceptual)
        /// </summary>
        public bool HasDToB0 { get; private set; }
        /// <summary>
        /// States if the profile has a D to B1 tag (Device->PCS: relative colorimetric)
        /// </summary>
        public bool HasDToB1 { get; private set; }
        /// <summary>
        /// States if the profile has a D to B2 tag (Device->PCS: saturation)
        /// </summary>
        public bool HasDToB2 { get; private set; }
        /// <summary>
        /// States if the profile has a D to B3 tag (Device->PCS: absolute colorimetric)
        /// </summary>
        public bool HasDToB3 { get; private set; }

        #endregion

        private ICCProfile profile;
        private Whitepoint wp;
        private LocalizedString[] des;
        private LocalizedString[] cprs;
        private double[,] ma;
        private double[,] ma1;

        #endregion

        #region Init

        /// <summary>
        /// Reads an ICC profile from disk
        /// </summary>
        /// <param name="path">Path to the icc file</param>
        public ICC(string path)
        {
            ProfileName = Path.GetFileNameWithoutExtension(path);
            profile = new ICCProfile(path, true);

            //mediaWhitePointTag
            double[] wpXYZ = null;
            TagDataEntry wpEn = profile.GetFirstEntry(TagSignature.mediaWhitePointTag);
            if (wpEn != null && wpEn.Signature == TypeSignature.XYZ) { wpXYZ = ((XYZTagDataEntry)wpEn).Data[0].GetArray(); }

            if (wpXYZ != null) { wp = new Whitepoint(wpXYZ); }
            else { wp = new Whitepoint(Header.PCSIlluminant.GetArray()); }
            
            //profileDescriptionTag
            TagDataEntry desc = profile.GetFirstEntry(TagSignature.profileDescriptionTag);
            if (desc.Signature == TypeSignature.multiLocalizedUnicode) { des = ((multiLocalizedUnicodeTagDataEntry)desc).Text; }
            else if (desc.Signature == TypeSignature.text) { des = new LocalizedString[] { new LocalizedString(new CultureInfo("en"), ((textTagDataEntry)desc).Text) }; }

            //copyrightTag
            TagDataEntry cpr = profile.GetFirstEntry(TagSignature.copyrightTag);
            if (cpr.Signature == TypeSignature.multiLocalizedUnicode) { cprs = ((multiLocalizedUnicodeTagDataEntry)cpr).Text; }
            else if (cpr.Signature == TypeSignature.text) { cprs = new LocalizedString[] { new LocalizedString(new CultureInfo("en"), ((textTagDataEntry)cpr).Text) }; }

            //chromaticAdaptationTag (if data has different reference white as PCS)
            s15Fixed16ArrayTagDataEntry cam = (s15Fixed16ArrayTagDataEntry)profile.GetFirstEntry(TagSignature.chromaticAdaptationTag);
            if (cam != null) 
            {
                ma = new double[,] { { cam.Data[0], cam.Data[1], cam.Data[2] }, { cam.Data[3], cam.Data[4], cam.Data[5] }, { cam.Data[6], cam.Data[7], cam.Data[8] } };
                ma1 = MMath.StaticInvertMatrix(ma);
            }

            SetHasVariables();
        }

        private void SetHasVariables()
        {
            for (int i = 0; i < TagTable.Data.Length; i++)
            {
                switch (TagTable.Data[i].Signature)
                {
                    case TagSignature.AToB0Tag:HasAToB0 = true; break;
                    case TagSignature.AToB1Tag:HasAToB1 = true; break;
                    case TagSignature.AToB2Tag:HasAToB2 = true; break;

                    case TagSignature.BToA0Tag:HasBToA0 = true; break;
                    case TagSignature.BToA1Tag:HasBToA1 = true; break;
                    case TagSignature.BToA2Tag:HasBToA2 = true; break;

                    case TagSignature.BToD0Tag:HasBToD0 = true; break;
                    case TagSignature.BToD1Tag:HasBToD1 = true; break;
                    case TagSignature.BToD2Tag:HasBToD2 = true; break;
                    case TagSignature.BToD3Tag:HasBToD3 = true; break;

                    case TagSignature.DToB0Tag:HasDToB0 = true; break;
                    case TagSignature.DToB1Tag:HasDToB1 = true; break;
                    case TagSignature.DToB2Tag:HasDToB2 = true; break;
                    case TagSignature.DToB3Tag:HasDToB3 = true; break;

                    case TagSignature.grayTRCTag:HasGrayTRC = true; break;
                    case TagSignature.redTRCTag:HasRedTRC = true; break;
                    case TagSignature.greenTRCTag:HasGreenTRC = true; break;
                    case TagSignature.blueTRCTag: HasBlueTRC = true; break;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the first entry with the specific signature. Returns null if none is found
        /// </summary>
        /// <param name="TagName">The signature of the entry</param>
        /// <returns>The data of the named entry</returns>
        internal TagDataEntry GetEntry(TagSignature TagName)
        {
            return profile.GetFirstEntry(TagName);
        }

        /// <summary>
        /// Get the all entries with the specific signature
        /// </summary>
        /// <param name="TagName">The signature of the entry</param>
        /// <returns>The array of data of the named entry</returns>
        internal TagDataEntry[] GetAllEntries(TagSignature TagName)
        {
            return profile.GetAllEntries(TagName);
        }

        #endregion
        
        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            ICC c = obj as ICC;
            if ((Object)c == null) { return false; }

            return Header.ProfileID == c.Header.ProfileID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ICC a, ICC b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.Header.ProfileID == b.Header.ProfileID;
        }

        public static bool operator !=(ICC a, ICC b)
        {
            return !(a == b);
        }

        #endregion
    }
}