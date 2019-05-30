using System;

/*  This library reads version 4.3 and compatible ICC-profiles as specified by the International Color Consortium.
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

namespace ICCReader
{
    /// <summary>
    /// The header of an ICC profile
    /// </summary>
    public sealed class ICCHeader
    {
        /// <summary>
        /// Size of profile in bytes
        /// </summary>
        public uint ProfileSize { get; private set; }
        /// <summary>
        /// Preferred CMM (Color Management Module) type
        /// </summary>
        public string CMMType { get; private set; }
        /// <summary>
        /// Version number of profile
        /// </summary>
        public VersionNumber ProfileVersionNumber { get; private set; }
        /// <summary>
        /// Type of profile
        /// </summary>
        public ProfileClassName ProfileClass { get; private set; }
        /// <summary>
        /// Colorspace of data
        /// </summary>
        public ColorSpaceType DataColorspace { get; private set; }
        /// <summary>
        /// Profile Connection Space
        /// </summary>
        public ColorSpaceType PCS { get; private set; }
        /// <summary>
        /// Date and time this profile was first created
        /// </summary>
        public DateTime CreationDate { get; private set; }
        /// <summary>
        /// Has to be "acsp"
        /// </summary>
        public string ProfileFileSignature { get; private set; }
        /// <summary>
        /// Primary platform this profile was created for
        /// </summary>
        public PrimaryPlatformName PrimaryPlatformSignature { get; private set; }
        /// <summary>
        /// Profile flags to indicate various options for the CMM such as distributed processing and caching options
        /// </summary>
        public ProfileFlag ProfileFlags { get; private set; }
        /// <summary>
        /// Device manufacturer of the device for which this profile is created
        /// </summary>
        public TagSignature DeviceManufacturer { get; private set; }
        /// <summary>
        /// Device model of the device for which this profile is created
        /// </summary>
        public TagSignature DeviceModel { get; private set; }
        /// <summary>
        /// Device attributes unique to the particular device setup such as media type
        /// </summary>
        public DeviceAttribute DeviceAttributes { get; private set; }
        /// <summary>
        /// Rendering Intent
        /// </summary>
        public RenderingIntentName RenderingIntent { get; private set; }
        /// <summary>
        /// The normalized XYZ values of the illuminant of the PCS
        /// </summary>
        public XYZnumber PCSIlluminant { get; private set; }
        /// <summary>
        /// Profile creator signature
        /// </summary>
        public uint ProfileCreatorSignature { get; private set; }
        /// <summary>
        /// Profile ID
        /// </summary>
        public string ProfileID { get; private set; }
        
        internal ICCHeader(byte[] arr)
        {
            bool ile = BitConverter.IsLittleEndian;

            //Profile size field (bytes 0 to 3)
            ProfileSize = Helper.GetUInt32(0);
            Helper.IsReverse = new bool[ProfileSize];
            //Preferred CMM type field (bytes 4 to 7)
            CMMType = Helper.GetASCIIString(4, 4);
            //Profile version field (bytes 8 to 11) (10 and 11 are not used)
            ProfileVersionNumber = new VersionNumber(ICCProfile.DataBytes[8], ICCProfile.DataBytes[9]);
            //Profile/device class field (bytes 12 to 15)
            ProfileClass = (ProfileClassName)Helper.GetUInt32(12);
            //Data colour space field (bytes 16 to 20)
            DataColorspace = (ColorSpaceType)Helper.GetUInt32(16);
            //PCS field (bytes 20 to 23)
            PCS = (ColorSpaceType)Helper.GetUInt32(20);
            //Date and time field (bytes 24 to 35)
            CreationDate = Helper.GetDateTime(24);
            //Profile file signature field (bytes 36 to 39)
            ProfileFileSignature = Helper.GetASCIIString(36, 4);
            //Primary platform field (bytes 40 to 43)
            PrimaryPlatformSignature = (PrimaryPlatformName)Helper.GetUInt32(40);
            //Profile flags field (bytes 44 to 47)
            ProfileFlags = new ProfileFlag(44);
            //Device manufacturer field (bytes 48 to 51)
            DeviceManufacturer = (TagSignature)Helper.GetUInt32(48);
            //Device model field (bytes 52 to 55)
            DeviceModel = (TagSignature)Helper.GetUInt32(52);
            //Device attributes field (bytes 56 to 63)
            DeviceAttributes = new DeviceAttribute(56);
            //Rendering intent field (bytes 64 to 67) (66 and 67 are zero)
            RenderingIntent = (RenderingIntentName)Helper.GetUInt16(64);
            //PCS illuminant field (Bytes 68 to 79)
            PCSIlluminant = new XYZnumber(68);
            //Profile creator field (bytes 80 to 83)
            ProfileCreatorSignature = Helper.GetUInt32(64); ;
            //Profile ID field (bytes 84 to 99)
            ProfileID = Helper.GetProfileID(84);
            //Reserved field (bytes 100 to 127)
        }
    }
}
