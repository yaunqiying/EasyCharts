using System;
using System.Collections;
using System.Linq;
using System.Globalization;
using System.Text;

/*  This library reads version 4.3 and compatible ICC-profiles as specified
    by the International Color Consortium.
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
    #region Numbers

    public sealed class VersionNumber
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int BugFix { get; private set; }
        private static bool ile = BitConverter.IsLittleEndian;

        public VersionNumber(int Major, int Minor, int Bugfix)
        {
            this.Major = Major;
            this.Minor = Minor;
            this.BugFix = BugFix;
        }

        internal VersionNumber(byte Major, byte Minor)
        {
            this.Major = (int)Major;
            BitArray a = new BitArray(new byte[] { Minor });
            bool[] tmp = new bool[] { false, false, false, false, a[0], a[1], a[2], a[3] };
            BitArray b = new BitArray(tmp);
            this.Minor = GetByte(b);
            tmp = new bool[] { false, false, false, false, a[4], a[5], a[6], a[7] };
            b = new BitArray(tmp);
            this.BugFix = GetByte(b);
        }

        private byte GetByte(BitArray input)
        {
            int len = input.Length;
            int output = 0;
            for (int i = 0; i < len; i++)
            {
                if (input.Get(i))
                {
                    if (!ile) { output += (1 << (len - 1 - i)); }
                    else { output += (1 << i); }
                }
            }
            return (byte)output;
        }
    }

    public sealed class PositionNumber
    {
        public int Offset { get; private set; }
        public int Size { get; private set; }
        private static bool ile = BitConverter.IsLittleEndian;

        internal PositionNumber(int idx)
        {
            this.Offset = (int)Helper.GetUInt32(idx);
            this.Size = (int)Helper.GetUInt32(idx + 4);
        }
    }

    public sealed class Response16Number
    {
        public ushort DeviceCode { get; private set; }
        public double MeasurmentValue { get; private set; }
        private static bool ile = BitConverter.IsLittleEndian;

        internal Response16Number(int idx)
        {
            DeviceCode = Helper.GetUInt16(idx);
            MeasurmentValue = Helper.GetS15Fixed16Number(idx + 4);
        }
    }

    public sealed class XYZnumber
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        internal XYZnumber(int idx)
        {
            this.X = Helper.GetS15Fixed16Number(idx);
            this.Y = Helper.GetS15Fixed16Number(idx + 4);
            this.Z = Helper.GetS15Fixed16Number(idx + 8);
            this.X /= (this.X > 2) ? this.X / 256d : 1d;
            this.Y /= (this.Y > 2) ? this.Y / 256d : 1d;
            this.Z /= (this.Z > 2) ? this.Z / 256d : 1d;
        }

        public double[] GetArray()
        {
            return new double[] { X, Y, Z };
        }
    }

    #endregion

    #region Various

    public sealed class ProfileFlag
    {
        public byte[] Data { get; private set; }

        public bool IsEmbedded { get; private set; }
        public bool IsIndependent { get; private set; }

        internal ProfileFlag(int idx)
        {
            this.Data = new byte[] { ICCProfile.DataBytes[idx], ICCProfile.DataBytes[idx + 1] };
            BitArray a = new BitArray(new byte[] { ICCProfile.DataBytes[idx + 2], ICCProfile.DataBytes[idx + 3] });
            IsEmbedded = a[0];
            IsIndependent = !a[1];
        }
    }

    public sealed class DeviceAttribute
    {
        public DeviceAttributeName Attribute1 { get { return att1 ? DeviceAttributeName.Transparency : DeviceAttributeName.Reflective; } }
        public DeviceAttributeName Attribute2 { get { return att2 ? DeviceAttributeName.Matte : DeviceAttributeName.Glossy; } }
        public DeviceAttributeName MediaPolarity { get { return att3 ? DeviceAttributeName.Negative : DeviceAttributeName.Positive; } }
        public DeviceAttributeName Media { get { return att4 ? DeviceAttributeName.BlackWhite : DeviceAttributeName.Color; } }
        public byte[] VendorData { get; private set; }

        private bool att1;
        private bool att2;
        private bool att3;
        private bool att4;

        internal DeviceAttribute(int idx)
        {
            BitArray a = new BitArray(new byte[] { ICCProfile.DataBytes[idx] });
            att1 = a[0];
            att2 = a[1];
            att3 = a[2];
            att4 = a[3];
            VendorData = new byte[] { ICCProfile.DataBytes[idx + 4], ICCProfile.DataBytes[idx + 5], ICCProfile.DataBytes[idx + 6], ICCProfile.DataBytes[idx + 7] };
        }

    }

    public sealed class TagTableEntry
    {
        public TagSignature Signature { get; private set; }
        internal uint Offset { get; private set; }
        internal uint DataSize { get; private set; }
        internal int Index { get; private set; } 

        internal TagTableEntry(TagSignature Signature, uint Offset, uint DataSize, int Index)
        {
            this.Signature = Signature;
            this.Offset = Offset;
            this.DataSize = DataSize;
            this.Index = Index;
        }
    }

    public sealed class ColorantDataEntry
    {
        public string Name { get; private set; }
        public ushort[] PCSValue { get; private set; }

        internal ColorantDataEntry(string Name, ushort[] PCSValue)
        {
            this.Name = Name;
            this.PCSValue = PCSValue;
        }
    }

    /// <summary>
    /// A color with a name
    /// </summary>
    public sealed class NamedColor
    {
        public string Name { get; private set; }
        public ushort[] PCScoordinates { get; private set; }
        public ushort[] DeviceCoordinates { get; private set; }

        internal NamedColor(int idx, int DeviceCoordinateCount)
        {
            bool ile = BitConverter.IsLittleEndian;
            //Root name (32 bytes)
            Name = Helper.GetASCIIString(idx, 32);
            //PCS coordinates (6 bytes) (2 bytes each)
            PCScoordinates = new ushort[3];
            PCScoordinates[0] = Helper.GetUInt16(idx + 32);
            PCScoordinates[1] = Helper.GetUInt16(idx + 34);
            PCScoordinates[2] = Helper.GetUInt16(idx + 36);
            //Device coordinates (2 bytes each)
            if (DeviceCoordinateCount > 0)
            {
                DeviceCoordinates = new ushort[2 * DeviceCoordinateCount];
                int end = (idx + 38) + 2 * DeviceCoordinateCount; int c = 0;
                for (int i = idx + 38; i < end; i++) { DeviceCoordinates[c] = Helper.GetUInt16(i); c++; }
            }
        }
    }

    public sealed class LocalizedString
    {
        public CultureInfo Locale { get; private set; }
        public string Text { get; private set; }

        public LocalizedString(CultureInfo Locale, string Text)
        {
            this.Locale = Locale;
            this.Text = Text;
        }

        internal LocalizedString(string Language, string Country, int idx, int length)
        {            
            Locale = new CultureInfo(Language + "-" + Country);
            Text = Helper.GetUnicodeString(idx, length);
        }
    }

    public sealed class ProfileDescription
    {
        public uint DeviceManufacturer { get; private set; }
        public uint DeviceModel { get; private set; }
        public DeviceAttribute DeviceAttributes { get; private set; }
        public TagSignature TechnologyInformation { get; private set; }
        public multiLocalizedUnicodeTagDataEntry DeviceManufacturerInfo { get; private set; }
        public multiLocalizedUnicodeTagDataEntry DeviceModelInfo { get; private set; }

        internal int end;

        internal ProfileDescription(int idx, ICCHeader header)
        {
            //Device manufacturer signature (4 bytes)
            DeviceManufacturer = Helper.GetUInt32(idx);
            //Device model signature (4 bytes)
            DeviceModel = Helper.GetUInt32(idx + 4);
            //Device attributes (8 bytes)
            DeviceAttributes = new DeviceAttribute(idx + 8);
            //Device technology information (4 bytes)
            TechnologyInformation = (TagSignature)Helper.GetUInt32(idx + 16);
            //Displayable description of device manufacturer (profile's deviceMfgDescTag)
            DeviceManufacturerInfo = new multiLocalizedUnicodeTagDataEntry(idx + 20, header.DeviceManufacturer == 0 ? true : false);
            //Displayable description of device model (profile's deviceModelDescTag)
            DeviceModelInfo = new multiLocalizedUnicodeTagDataEntry(DeviceManufacturerInfo.end, header.DeviceModel == 0 ? true : false);
            end = DeviceModelInfo.end;
        }
    }

    #endregion

    #region Curves

    public sealed class ResponseCurve
    {
        public CurveMeasurementEncodings CurveType { get; private set; }
        public int[] MeasurmentCounts { get; private set; }
        public XYZnumber[] XYZvalues { get; private set; }
        public Response16Number[] ResponseArrays { get; private set; }

        internal ResponseCurve(int idx, int ChannelCount)
        {
            //Measurement unit signature (4 bytes)
            CurveType = (CurveMeasurementEncodings)Helper.GetUInt32(idx);
            //Counts of measurements in response arrays
            MeasurmentCounts = new int[ChannelCount];
            int end = idx + 4 + 4 * ChannelCount; int c = 0;
            for (int i = idx + 4; i < end; i += 4) { MeasurmentCounts[c] = (int)Helper.GetUInt32(i); c++; }
            //PCSXYZ values
            XYZvalues = new XYZnumber[ChannelCount];
            int start = end; end += 12 * ChannelCount; c = 0;
            for (int i = start; i < end; i += 12) { XYZvalues[c] = new XYZnumber(i); c++; }
            //Response arrays
            int p = MeasurmentCounts.Sum();
            ResponseArrays = new Response16Number[p];
            start = end; end += 8 * p; c = 0;
            for (int i = start; i < end; i += 8) { ResponseArrays[c] = new Response16Number(i); c++; }
        }
    }

    public sealed class ParametricCurve
    {
        internal int end;
        private ushort type;
        private double g;
        private double a;
        private double b;
        private double c;
        private double d;
        private double e;
        private double f;

        public ParametricCurve(ushort type, int idx)
        {
            this.type = type;
            //Parameter (4 bytes each)
            g = Helper.GetS15Fixed16Number(idx);
            end = idx + 4;
            if (type == 1 || type == 2 || type == 3 || type == 4)
            {
                a = Helper.GetS15Fixed16Number(idx + 4);
                b = Helper.GetS15Fixed16Number(idx + 8);
                end = idx + 12;
            }
            if (type == 2 || type == 3 || type == 4) { c = Helper.GetS15Fixed16Number(idx + 12); end = idx + 16; }
            if (type == 3 || type == 4) { d = Helper.GetS15Fixed16Number(idx + 16); end = idx + 20; }
            if (type == 4)
            {
                e = Helper.GetS15Fixed16Number(idx + 20);
                f = Helper.GetS15Fixed16Number(idx + 24);
                end = idx + 28;
            }
        }

        public double Function(double X)
        {
            switch (type)
            {
                case 0:
                    return Math.Pow(X, g);
                case 1:
                    return (X >= -b / a) ? Math.Pow(a * X + b, g) : 0;
                case 2:
                    return (X >= -b / a) ? Math.Pow(a * X + b, g) + c : c;
                case 3:
                    return (X >= d) ? Math.Pow(a * X + b, g) : c * X;
                case 4:
                    return (X >= d) ? Math.Pow(a * X + b, g) + c : c * X + f;

                default:
                    throw new CorruptProfileException("ParametricCurve");
            }
        }

        public double InverseFunction(double X)
        {
            switch (type)
            {
                case 0:
                    return Math.Pow(X, 1 / g);
                case 1:
                    return (X >= -b / a) ? (Math.Pow(a, 1 / g) - b) / X : 0;
                case 2:
                    return (X >= -b / a) ? (Math.Pow(X - c, 1 / g) - b) / a : c;
                case 3:
                    return (X >= d) ? (Math.Pow(a, 1 / g) - b) / X : X / c;
                case 4:
                    return (X >= d) ? (Math.Pow(X - c, 1 / g) - b) / a : (X - f) / c;

                default:
                    throw new CorruptProfileException("ParametricCurve");
            }            
        }
    }

    /// <summary>
    /// A one dimensional curve
    /// </summary>
    public sealed class OneDimensionalCurve
    {
        /// <summary>
        /// Number of curve segments
        /// </summary>
        public ushort SegmentCount { get; private set; }
        /// <summary>
        /// Breakpoints separate two curve segments
        /// </summary>
        public double[] BreakPoints { get; private set; }
        /// <summary>
        /// An array of curve segments
        /// </summary>
        public CurveSegment[] Segments { get; private set; }
        internal int end;

        internal OneDimensionalCurve(int idx)
        {
            //Number of segments (2 bytes) (plus 2 bytes reserved)
            SegmentCount = Helper.GetUInt16(idx);
            //Break points (4 bytes each)
            BreakPoints = new double[SegmentCount - 1];
            int iend = idx + 4 + SegmentCount * 4; int c = 0;
            for (int i = idx + 4; i < iend; i += 4) { BreakPoints[c] = Helper.GetFloat32(i); c++; }
            //Segments
            Segments = new CurveSegment[SegmentCount];
            int start = iend; iend += 1; c = 0;
            for (int i = start; i < SegmentCount; i++) { Segments[i] = CurveSegment.GetCurve(iend); iend = Segments[i].end; c++; }
            end += iend;
        }

        public double GetValue(double input)
        {
            int idx = -1;
            if (Segments.Length != 1)
            {
                for (int i = 0; i < BreakPoints.Length; i++) { if (input <= BreakPoints[i]) { idx = i; break; } }
                if (idx == -1) { idx = Segments.Length - 1; }
            }
            else { idx = 0; }

            return Segments[idx].GetValue(input);
        }
    }

    /// <summary>
    /// A segment of a curve
    /// </summary>
    public abstract class CurveSegment
    {
        /// <summary>
        /// The signature of this segment
        /// </summary>
        public abstract CurveSegmentSignature Signature { get; }
        internal int end;

        internal static CurveSegment GetCurve(int idx)
        {
            //Tag signature (4 bytes) (plus 4 bytes reserved)
            CurveSegmentSignature t = (CurveSegmentSignature)Helper.GetUInt32(idx);
            if (t == CurveSegmentSignature.FormulaCurve) { return new FormulaCurveElement(idx + 8); }
            else if (t == CurveSegmentSignature.SampledCurve) { return new SampledCurveElement(idx + 8); }
            else { throw new CorruptProfileException("CurveSegment"); }
        }

        /// <summary>
        /// Interpolates a given value with the methods of this curve segment
        /// </summary>
        /// <param name="X">The value which will get interpolated</param>
        /// <returns>The interpolated value</returns>
        public abstract double GetValue(double X);
    }

    /// <summary>
    /// A formula based curve segment
    /// </summary>
    public sealed class FormulaCurveElement : CurveSegment
    {
        /// <summary>
        /// The signature of this segment
        /// </summary>
        public override CurveSegmentSignature Signature { get { return CurveSegmentSignature.FormulaCurve; } }

        private ushort formula;
        private double gamma;
        private double a;
        private double b;
        private double c;
        private double d;
        private double e;

        internal FormulaCurveElement(int idx)
        {
            //Encoded value of the function type (2 bytes) (plus 2 bytes reserved)
            formula = Helper.GetUInt16(idx);
            if (formula != 1 && formula != 2 && formula != 3) { throw new CorruptProfileException("FormulaCurveElement"); }
            //Parameters (4 bytes each)
            if (formula == 0 || formula == 1) { gamma = Helper.GetFloat32(idx + 4); }
            if (formula == 0 || formula == 1 || formula == 2)
            {
                a = Helper.GetFloat32(idx + 8);
                b = Helper.GetFloat32(idx + 12);
                c = Helper.GetFloat32(idx + 16);
                end = idx + 20;
            }
            if (formula == 1 || formula == 2) { d = Helper.GetFloat32(idx + 20); end = idx + 24; }
            if (formula == 2) { e = Helper.GetFloat32(idx + 24); end = idx + 24; }
        }

        /// <summary>
        /// Interpolates a given value with the methods of this curve segment
        /// </summary>
        /// <param name="X">The value which will get interpolated</param>
        /// <returns>The interpolated value</returns>
        public override double GetValue(double X)
        {
            if (formula == 0) { return Math.Pow(a * X + b, gamma) + c; }
            else if (formula == 1) { return a * Math.Log10(b * Math.Pow(X, gamma) + c) + d; }
            else if (formula == 2) { return a * Math.Pow(b, c * X + d) + e; }
            else { return -1; }
        }
    }

    /// <summary>
    /// A sampled curve segment
    /// </summary>
    public sealed class SampledCurveElement : CurveSegment
    {
        /// <summary>
        /// The signature of this segment
        /// </summary>
        public override CurveSegmentSignature Signature { get { return CurveSegmentSignature.SampledCurve; } }

        /// <summary>
        /// Number of entries
        /// </summary>
        public int EntryCount { get; private set; }
        /// <summary>
        /// The curve entries
        /// </summary>
        public double[] CurveEntries { get; private set; }

        internal SampledCurveElement(int idx)
        {
            //The number of entries (4 bytes)
            EntryCount = (int)Helper.GetUInt32(idx);
            //Curve entries (4 bytes each)
            CurveEntries = new double[EntryCount];
            end = idx + 4 + 4 * EntryCount; int c = 0;
            for (int i = idx + 4; i < end; i += 4) { CurveEntries[c] = Helper.GetFloat32(i); c++; }
        }

        /// <summary>
        /// Interpolates a given value with the methods of this curve segment
        /// </summary>
        /// <param name="X">The value which will get interpolated</param>
        /// <returns>The interpolated value</returns>
        public override double GetValue(double X)
        {
            double t = X * (CurveEntries.Length - 1);
            if (t % 1 != 0)
            {
                int i = (int)Math.Floor(t);
                return CurveEntries[i] + ((CurveEntries[i + 1] - CurveEntries[i]) * (t % 1));
            }
            else { return CurveEntries[(int)t]; }
        }
    }

    #endregion

    #region Tables

    public abstract class CLUT
    {
        public byte[] GridPointCount { get; protected set; }
        public int InputChannelCount { get; protected set; }
        public int OutputChannelCount { get; protected set; }

        internal int end;

        internal static CLUT GetCLUT(int idx, bool IsFloat, int InputChannels, int OutputChannels)
        {
            //Number of grid points in each dimension
            byte[] gpc = new byte[16];
            for (int i = 0; i < 16; i++) { gpc[i] = ICCProfile.DataBytes[idx + i]; }
            //Precision of data elements
            if (!IsFloat)
            {
                byte p = ICCProfile.DataBytes[idx + 16];
                if (p == 1) { return new CLUT8(idx + 20, InputChannels, OutputChannels, gpc); }
                else if (p == 2) { return new CLUT16(idx + 20, InputChannels, OutputChannels, gpc); }
                else { throw new CorruptProfileException("CLUT"); }
            }
            else { return new CLUTf32(idx + 16, InputChannels, OutputChannels, gpc); }
        }

        public abstract double[] GetValue(params double[] p);
        public abstract ushort[] GetValue(params ushort[] p);
        public abstract byte[] GetValue(params byte[] p);
    }

    public sealed class CLUTf32 : CLUT
    {
        public double[][] Values { get; private set; }

        public CLUTf32(int idx, int InputChannelCount, int OutputChannelCount, byte[] GridPointCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            this.GridPointCount = GridPointCount;
            //Points
            int l = 0; int k = 0;
            for (int i = 0; i < InputChannelCount; i++) { l += (int)Math.Pow(GridPointCount[i], InputChannelCount) / InputChannelCount; }
            Values = new double[l][];
            for (int i = 0; i < l; i++)
            {
                Values[i] = new double[OutputChannelCount];
                for (int o = 0; o < OutputChannelCount; o++) { Values[i][o] = Helper.GetFloat32(idx + k); k += 4; }
            }
            this.end = idx + l * OutputChannelCount * 4;
        }

        public override double[] GetValue(params double[] p)
        {
            if (p.Length != InputChannelCount) { throw new ArgumentException("Inputcount does not match channelcount"); }
            double[] pd = new double[p.Length];
            for (int i = 0; i < p.Length; i++) { pd[i] = p[i] * GridPointCount[i]; }
            double idx = pd[InputChannelCount - 1]; int c = 1;
            for (int i = InputChannelCount - 2; i >= 0; i--) { idx += Math.Round(pd[i]) * (int)Math.Pow(GridPointCount[i], c); c++; }
            return Values[(int)idx];
        }

        public override byte[] GetValue(params byte[] p)
        {
            double[] tmp = GetValue(p.Select(t => t / 255d).ToArray());
            byte[] o = new byte[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = (byte)(tmp[i] * 255); }
            return o;
        }

        public override ushort[] GetValue(params ushort[] p)
        {
            double[] tmp = GetValue(p.Select(t => t / 65535d).ToArray());
            ushort[] o = new ushort[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = (ushort)(tmp[i] * 65535d); }
            return o;
        }
    }

    public sealed class CLUT16 : CLUT
    {
        public ushort[][] Values { get; private set; }

        public CLUT16(int idx, int InputChannelCount, int OutputChannelCount, byte[] GridPointCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            this.GridPointCount = GridPointCount;
            //Points
            int l = 0; int k = 0;
            for (int i = 0; i < InputChannelCount; i++) { l += (int)Math.Pow(GridPointCount[i], InputChannelCount) / InputChannelCount; }
            Values = new ushort[l][];
            for (int i = 0; i < l; i++)
            {
                Values[i] = new ushort[OutputChannelCount];
                for (int o = 0; o < OutputChannelCount; o++) { Values[i][o] = Helper.GetUInt16(idx + k); k += 2; }
            }
            this.end = idx + l * OutputChannelCount * 2;
        }

        public override ushort[] GetValue(params ushort[] p)
        {
            if (p.Length != InputChannelCount) { throw new ArgumentException("Inputcount does not match channelcount"); }
            double[] pd = new double[p.Length];
            for (int i = 0; i < p.Length; i++) { pd[i] = (p[i] != 0) ? (p[i] / 65535d) * GridPointCount[i] - 1 : 0; }
            double idx = pd[InputChannelCount - 1]; int c = 1;
            for (int i = InputChannelCount - 2; i >= 0; i--) { idx += Math.Round(pd[i]) * (int)Math.Pow(GridPointCount[i], c); c++; }
            return Values[(int)idx];
        }
        
        public override byte[] GetValue(params byte[] p)
        {
            ushort[] tmp = GetValue(p.Select(t => (ushort)(t * 257)).ToArray());
            byte[] o = new byte[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = (byte)(tmp[i] / 257d); }
            return o;
        }

        public override double[] GetValue(params double[] p)
        {
            ushort[] tmp = GetValue(p.Select(t => (ushort)(t * 65535)).ToArray());
            double[] o = new double[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = tmp[i] / 65535d; }
            return o;
        }
    }

    public sealed class CLUT8 : CLUT
    {
        public byte[][] Values { get; private set; }
        
        public CLUT8(int idx, int InputChannelCount, int OutputChannelCount, byte[] GridPointCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            this.GridPointCount = GridPointCount;
            //Points
            int l = 0;
            for (int i = 0; i < InputChannelCount; i++) { l += (int)Math.Pow(GridPointCount[i], InputChannelCount) / InputChannelCount; }
            Values = new byte[l][];
            for (int i = 0; i < l; i++)
            {
                Values[i] = new byte[OutputChannelCount];
                for (int o = 0; o < OutputChannelCount; o++) { Values[i][o] = ICCProfile.DataBytes[idx + l]; }
            }
            this.end = idx + l * OutputChannelCount;
        }

        public override byte[] GetValue(params byte[] p)
        {
            if (p.Length != InputChannelCount) { throw new ArgumentException("Inputcount does not match channelcount"); }
            double[] pd = new double[p.Length];
            for (int i = 0; i < p.Length; i++) { pd[i] = (p[i] != 0) ? (p[i] / 255d) * GridPointCount[i] : 0; }
            double idx = pd[InputChannelCount - 1]; int c = 1;
            for (int i = InputChannelCount - 2; i >= 0; i--) { idx += Math.Round(pd[i]) * (int)Math.Pow(GridPointCount[i], c); c++; }
            return Values[(int)idx];
        }

        public override ushort[] GetValue(params ushort[] p)
        {
            ushort[] tmp = GetValue(p.Select(t => (ushort)(t / 257d)).ToArray());
            ushort[] o = new ushort[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = (ushort)(tmp[i] * 257d); }
            return o;
        }

        public override double[] GetValue(params double[] p)
        {
            byte[] tmp = GetValue(p.Select(t => (byte)(t * 255)).ToArray());
            double[] o = new double[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) { o[i] = tmp[i] / 255d; }
            return o;
        }
    }

    public sealed class LUT16
    {
        public ushort[] Values { get; private set; }

        public LUT16(int idx, int ValueCount)
        {
            Values = new ushort[ValueCount]; int c = 0;
            for (int i = 0; i < ValueCount; i++) { Values[i] = Helper.GetUInt16(idx + c); c += 2; }
        }

        public double GetNumber(ushort number)
        {
            double t = (number / 65535d) * (Values.Length - 1);
            t = (t > Values.Length - 1) ? Values.Length - 1 : (t < 0) ? 0 : t;
            if (t % 1 != 0)
            {
                int i = (int)Math.Floor(t);
                if (i + 1 < Values.Length) return (Values[i] + ((Values[i + 1] - Values[i]) * (t % 1))) / (double)ushort.MaxValue;
                else return Values[i] / (double)ushort.MaxValue;
            }
            else { return Values[(int)t] / (double)ushort.MaxValue; }
        }

        public double GetNumber(double number)
        {
            number *= (Values.Length - 1);
            number = (number > Values.Length - 1) ? Values.Length - 1 : (number < 0) ? 0 : number;
            if (number % 1 != 0)
            {
                int i = (int)Math.Floor(number);
                if (i + 1 < Values.Length) return (Values[i] + ((Values[i + 1] - Values[i]) * (number % 1))) / (double)ushort.MaxValue;
                else return Values[i] / (double)ushort.MaxValue;
            }
            else { return Values[(int)number] / (double)ushort.MaxValue; }
        }
    }

    public sealed class LUT8
    {
        public byte[] Values { get; private set; }

        public LUT8(int idx)
        {
            Values = new byte[256];
            for (int i = 0; i < 256; i++) { Values[i] = ICCProfile.DataBytes[idx + i]; }
        }

        public double GetNumber(byte number)
        {
            double t = (number / 255d) * (Values.Length - 1);
            t = (t > Values.Length - 1) ? Values.Length - 1 : (t < 0) ? 0 : t;
            if (t % 1 != 0)
            {
                int i = (int)Math.Floor(t);
                if (i + 1 < Values.Length) return (Values[i] + ((Values[i + 1] - Values[i]) * (t % 1))) / (double)byte.MaxValue;
                else return Values[i] / (double)byte.MaxValue;
            }
            else { return Values[(int)t] / (double)byte.MaxValue; }
        }

        public double GetNumber(double number)
        {
            number *= (Values.Length - 1);
            number = (number > Values.Length - 1) ? Values.Length - 1 : (number < 0) ? 0 : number;
            if (number % 1 != 0)
            {
                int i = (int)Math.Floor(number);
                if (i + 1 < Values.Length) return Values[i] + ((Values[i + 1] - Values[i]) * (number % 1)) / (double)byte.MaxValue;
                else return Values[i] / (double)byte.MaxValue;
            }
            else { return Values[(int)number] / (double)byte.MaxValue; }
        }
    }

    #endregion

    #region Exception and Helper

    public sealed class CorruptProfileException : Exception
    {
        public CorruptProfileException() { }

        public CorruptProfileException(string message) : base(message) { }

        public CorruptProfileException(string message, Exception innerException) : base(message, innerException) { }
    }

    internal static class Helper
    {
        internal static bool[] IsReverse = new bool[10];

        private static void DoReverse(int idx, int length)
        {
            if (BitConverter.IsLittleEndian && !IsReverse[idx])
            {
                Array.Reverse(ICCProfile.DataBytes, idx, length);
                IsReverse[idx] = true;
            }
        }

        internal static DateTime GetDateTime(int idx)
        {
            int Year = Helper.GetUInt16(idx);
            int Month = Helper.GetUInt16(idx + 2);
            int Day = Helper.GetUInt16(idx + 4);
            int Hour = Helper.GetUInt16(idx + 6);
            int Minute = Helper.GetUInt16(idx + 8);
            int Second = Helper.GetUInt16(idx + 10);

            return new DateTime(Year, Month, Day, Hour, Minute, Second);
        }
        
        internal static double GetS15Fixed16Number(int idx)
        {
            short tmp = BitConverter.ToInt16(ICCProfile.DataBytes, idx);
            return (Math.Abs(tmp) + (Helper.GetUInt16(idx + 2) / 65536d)) * (Math.Sign(tmp) == 0 ? 1 : Math.Sign(tmp));
        }

        internal static double GetU16Fixed16Number(int idx)
        {
            return Helper.GetUInt16(idx) + (Helper.GetUInt16(idx + 2) / 65536d);
        }

        internal static double GetU1Fixed15Number(int idx)
        {
            BitArray a = new BitArray(new byte[] { ICCProfile.DataBytes[idx], ICCProfile.DataBytes[idx + 1] });
            double n = a[0] ? 1 : 0; a[0] = false;
            byte[] tmp = new byte[2];
            a.CopyTo(tmp, 0);
            Array.Reverse(tmp, 0, 2);
            return n + (BitConverter.ToUInt16(tmp, 0) / 32768d);
        }

        internal static double GetU8Fixed8Number(int idx)
        {
            return ICCProfile.DataBytes[idx] + (ICCProfile.DataBytes[idx + 1] / 256d);
        }

        internal static uint GetUInt32(int idx)
        {
            DoReverse(idx, 4);
            return BitConverter.ToUInt32(ICCProfile.DataBytes, idx);
        }

        internal static ushort GetUInt16(int idx)
        {
            DoReverse(idx, 2);
            return BitConverter.ToUInt16(ICCProfile.DataBytes, idx);
        }

        internal static UInt64 GetUInt64(int idx)
        {
            DoReverse(idx, 8);
            return BitConverter.ToUInt64(ICCProfile.DataBytes, idx);
        }

        internal static double GetFloat32(int idx)
        {
            DoReverse(idx, 4);
            return BitConverter.ToSingle(ICCProfile.DataBytes, idx);
        }

        internal static string GetASCIIString(int idx, int length)
        {
            return Encoding.ASCII.GetString(ICCProfile.DataBytes, idx, length);
        }

        internal static string GetUnicodeString(int idx, int length)
        {
            return Encoding.BigEndianUnicode.GetString(ICCProfile.DataBytes, idx, length);
        }

        internal static string GetProfileID(int idx)
        {
            return BitConverter.ToString(ICCProfile.DataBytes, idx, 16);
        }

        internal static double[,] GetMatrix(int Xlength, int Ylength, int idx, bool IsFloat)
        {
            //Matrix x,y (4 bytes each)
            double[,] Matrix = new double[Xlength, Ylength]; int c = 0;
            for (int y = 0; y < Ylength; y++)
            {
                for (int x = 0; x < Xlength; x++)
                {
                    if (IsFloat) { Matrix[x, y] = Helper.GetFloat32(idx + c); }
                    else { Matrix[x, y] = Helper.GetS15Fixed16Number(idx + c) / 256d; }
                    c += 4;
                }
            }
            return Matrix;
        }

        internal static double[] GetMatrix(int Ylength, int idx, bool IsFloat)
        {
            //Matrix 1, y (4 bytes each)
            double[] Matrix = new double[Ylength]; int c = 0;
            for (int i = 0; i < Ylength; i++)
            {
                if (IsFloat) { Matrix[i] = Helper.GetFloat32(idx + c); }
                else { Matrix[i] = Helper.GetS15Fixed16Number(idx + c) / 256d; }
                c += 4;
            }
            return Matrix;
        }

        internal static double[] MultiplyMatrix3x3(double[,] a, double[] b)
        {
            double[] c = new double[3];
            c[0] = b[0] * a[0, 0] + b[1] * a[0, 1] + b[2] * a[0, 2];
            c[1] = b[0] * a[1, 0] + b[1] * a[1, 1] + b[2] * a[1, 2];
            c[2] = b[0] * a[2, 0] + b[1] * a[2, 1] + b[2] * a[2, 2];
            return c;
        }
        
        internal static double[,] MultiplyMatrix(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) { throw new ArgumentException("Cannot multiply: Size of matrices do not match"); }

            double[,] output = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < output.GetLength(0); i++)
            {
                for (int j = 0; j < output.GetLength(1); j++)
                {
                    for (int k = 0; k < a.GetLength(1); k++) { output[i, j] += a[i, k] * b[k, j]; }
                }
            }
            return output;
        }

        internal static double[] MultiplyMatrix(double[,] a, double[] b)
        {
            if (a.GetLength(1) != b.Length) { throw new ArgumentException("Cannot multiply: Size of matrices do not match"); }

            double[] output = new double[a.GetLength(0)];
            for (int i = 0; i < output.GetLength(0); i++)
            {
                for (int k = 0; k < a.GetLength(1); k++) { output[i] += a[i, k] * b[k]; }
            }
            return output;
        }

        internal static double[] AddMatrix(double[] a, double[] b)
        {
            if (a.Length != b.Length) { throw new ArgumentException("Cannot multiply: Size of matrices do not match"); }

            double[] output = new double[a.Length];
            for (int i = 0; i < a.Length; i++) { output[i] = a[i] + b[i]; }
            return output;
        }
    }

    #endregion
}
