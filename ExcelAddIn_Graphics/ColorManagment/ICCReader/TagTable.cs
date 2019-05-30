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
    /// A table with all tags that are stored in the icc
    /// </summary>
    public sealed class ICCTagTable
    {
        /// <summary>
        /// Number of tags
        /// </summary>
        public uint TagCount { get; private set; }
        /// <summary>
        /// An array storing all tags
        /// </summary>
        public TagTableEntry[] Data { get; private set; }
        private static bool ile = BitConverter.IsLittleEndian;

        internal ICCTagTable(byte[] arr)
        {
            int idx = 128;
            //Tag count (byte position 0 to 3)
            TagCount = Helper.GetUInt32(idx);
            Data = new TagTableEntry[TagCount];

            int c = 0;
            int length = (idx + 4) + (12 * (int)TagCount);
            for (int j = idx + 4; j < length; j += 12)
            {
                //Tag signature (byte position 4 to 7 and repeating)
                uint sig = Helper.GetUInt32(j);
                //Offset to beginning of tag data element (byte position 8 to 11 and repeating)
                uint off = Helper.GetUInt32(j + 4);
                //Tag data element size (byte position 12 to 15 and repeating)
                uint size = Helper.GetUInt32(j + 8);

                Data[c] = new TagTableEntry((TagSignature)sig, off, size, c);
                c++;
            }
        }
    }
}
