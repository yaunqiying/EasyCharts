using System;
using System.IO;
using System.Linq;

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
    /// Stores all values from an ICC profile
    /// </summary>
    public sealed class ICCProfile
    {
        /// <summary>
        /// The header of this profile
        /// </summary>
        public ICCHeader Header { get; private set; }
        /// <summary>
        /// The tag table of this profile
        /// </summary>
        public ICCTagTable TagTable { get; private set; }
        /// <summary>
        /// The tag data of this profile
        /// </summary>
        public TagDataEntry[] TagData { get; private set; }

        internal static byte[] DataBytes;
        private int HeaderEnd { get { return 127; } }
        private static bool ile = BitConverter.IsLittleEndian;
        
        /// <summary>
        /// Creates a new ICC profile
        /// </summary>
        /// <param name="Path">The path to the ICC file</param>
        /// <param name="Read">States if the profile should be read immediately or later</param>
        public ICCProfile(string Path, bool Read)
        {
            DataBytes = File.ReadAllBytes(Path);
            if (Read) ReadAll();
        }

        /// <summary>
        /// Reads all information stored in an ICC profile
        /// </summary>
        public void ReadAll()
        {
            ReadHeader();
            ReadTagTable();
            ReadData();
        }

        private void ReadHeader()
        {
            try { Header = new ICCHeader((byte[])DataBytes.Clone()); }
            catch (Exception ex) { throw new CorruptProfileException("Profile is corrupt", ex); }
        }

        private void ReadTagTable()
        {
            try { TagTable = new ICCTagTable((byte[])DataBytes.Clone()); }
            catch (Exception ex) { throw new CorruptProfileException("Profile is corrupt", ex); }
        }

        private void ReadData()
        {
            try
            {
                TagData = new TagDataEntry[TagTable.TagCount];
                for (int j = 0; j < TagTable.TagCount; j++) { TagData[j] = TagDataEntry.CreateEntry(TagTable.Data[j], Header); }
            }
            catch (Exception ex) { throw new CorruptProfileException("Profile is corrupt", ex); }
        }

        /// <summary>
        /// Get the first entry with the specific signature. Returns null if none is found
        /// </summary>
        /// <param name="TagName">The signature of the entry</param>
        /// <returns>The data of the named entry</returns>
        public TagDataEntry GetFirstEntry(TagSignature TagName)
        {
            TagTableEntry[] Entries = TagTable.Data.Where(t => t.Signature == TagName).ToArray();
            if (Entries.Length != 0) { return TagData[Entries[0].Index]; }
            else { return null; }
        }

        /// <summary>
        /// Get the all entries with the specific signature
        /// </summary>
        /// <param name="TagName">The signature of the entry</param>
        /// <returns>The array of data of the named entry</returns>
        public TagDataEntry[] GetAllEntries(TagSignature TagName)
        {
            TagTableEntry[] Entries = TagTable.Data.Where(t => t.Signature == TagName).ToArray();
            if (Entries.Length != 0) 
            {
                TagDataEntry[] output = new TagDataEntry[Entries.Length];
                for (int i = 0; i < Entries.Length; i++ ) { output[i] = TagData[Entries[i].Index]; }
                return output;
            }
            else { return null; }
        }
    }
}
