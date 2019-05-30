using System;
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
    /// The data of an entry
    /// </summary>
    public class TagDataEntry
    {
        public virtual TypeSignature Signature { get { return TypeSignature.Unknown; } }
        protected static bool ile = BitConverter.IsLittleEndian;
        protected static int size;

        public static TagDataEntry CreateEntry(TagTableEntry entry, ICCHeader header)
        {
            size = (int)entry.DataSize;
            //Tag signature (byte position 0 to 3) (4 to 7 are zero)
            TypeSignature t = (TypeSignature)Helper.GetUInt32((int)entry.Offset);
            return GetEntry(t, (int)entry.Offset + 8, header);
        }
        
        private static TagDataEntry GetEntry(TypeSignature type, int idx, ICCHeader header)
        {
            switch (type)
            {
                case TypeSignature.chromaticity:
                    return new chromaticityTagDataEntry(idx);
                case TypeSignature.colorantOrder:
                    return new colorantOrderTagDataEntry(idx);
                case TypeSignature.colorantTable:
                    return new colorantTableTagDataEntry(idx);
                case TypeSignature.curve:
                    return new curveTagDataEntry(idx);
                case TypeSignature.data:
                    return new dataTagDataEntry(idx);
                case TypeSignature.dateTime:
                    return new dateTimeTagDataEntry(idx);
                case TypeSignature.lut16:
                    return new lut16TagDataEntry(idx);
                case TypeSignature.lut8:
                    return new lut8TagDataEntry(idx);
                case TypeSignature.lutAToB:
                    return new lutAToBTagDataEntry(idx);
                case TypeSignature.lutBToA:
                    return new lutBToATagDataEntry(idx);
                case TypeSignature.measurement:
                    return new measurementTagDataEntry(idx);
                case TypeSignature.multiLocalizedUnicode:
                    return new multiLocalizedUnicodeTagDataEntry(idx, false);
                case TypeSignature.multiProcessElements:
                    return new multiProcessElementsTagDataEntry(idx);
                case TypeSignature.namedColor2:
                    return new namedColor2TagDataEntry(idx);
                case TypeSignature.parametricCurve:
                    return new parametricCurveTagDataEntry(idx);
                case TypeSignature.profileSequenceDesc:
                    return new profileSequenceDescTagDataEntry(idx, header);
                case TypeSignature.profileSequenceIdentifier:
                    return new profileSequenceIdentifierTagDataEntry(idx);
                case TypeSignature.responseCurveSet16:
                    return new responseCurveSet16TagDataEntry(idx);
                case TypeSignature.s15Fixed16Array:
                    return new s15Fixed16ArrayTagDataEntry(idx);
                case TypeSignature.signature:
                    return new signatureTagDataEntry(idx);
                case TypeSignature.text:
                    return new textTagDataEntry(idx);
                case TypeSignature.u16Fixed16Array:
                    return new u16Fixed16ArrayTagDataEntry(idx);
                case TypeSignature.uInt16Array:
                    return new uInt16ArrayTagDataEntry(idx);
                case TypeSignature.uInt32Array:
                    return new uInt32ArrayTagDataEntry(idx);
                case TypeSignature.uInt64Array:
                    return new uInt64ArrayTagDataEntry(idx);
                case TypeSignature.uInt8Array:
                    return new uInt8ArrayTagDataEntry(idx);
                case TypeSignature.viewingConditions:
                    return new viewingConditionsTagDataEntry(idx);
                case TypeSignature.XYZ:
                    return new XYZTagDataEntry(idx);

                default:
                    return new TagDataEntry();
            }
        }
    }
    
    /// <summary>
    /// The chromaticity tag type provides basic chromaticity data and type of phosphors or colorants of a monitor to applications and utilities.
    /// </summary>
    public sealed class chromaticityTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.chromaticity; } }
        public ushort ChannelCount { get; private set; }
        public ColorantEncoding ColorantType { get; private set; }
        public double[][] ChannelValues { get; private set; }

        internal chromaticityTagDataEntry(int idx)
        {
            //Number of device channels (2 bytes)            
            ChannelCount = Helper.GetUInt16(idx);
            //Encoded value of phosphor or colorant type (2 bytes)            
            ColorantType = (ColorantEncoding)Helper.GetUInt16(idx + 2);
            if (ColorantType != ColorantEncoding.Unknown && ChannelCount != 3) { throw new CorruptProfileException("chromaticityTagDataEntry"); }
            else if (ColorantType != ColorantEncoding.Unknown) { ChannelValues = SetValues(ColorantType); }
            else
            {
                //CIE xy coordinate values of channel n
                ChannelValues = new double[ChannelCount][];
                int c = 0; int end = (idx + 4) + 8 * ChannelCount;
                for (int i = idx + 4; i < end; i += 8)
                {
                    ChannelValues[c] = new double[2];
                    //x value (4 bytes)
                    ChannelValues[c][0] = Helper.GetU16Fixed16Number(i);
                    //y value (4 bytes)
                    ChannelValues[c][1] = Helper.GetU16Fixed16Number(i + 4);
                    c++;
                }
            }
        }

        private static double[][] SetValues(ColorantEncoding type)
        {
            double[][] tmp = new double[3][];
            switch (type)
            {
                case ColorantEncoding.EBU_Tech_3213_E:
                    tmp[0] = new double[2] { 0.640, 0.330 };
                    tmp[1] = new double[2] { 0.290, 0.600 };
                    tmp[2] = new double[2] { 0.150, 0.060 };
                    break;
                case ColorantEncoding.ITU_R_BT_709_2:
                    tmp[0] = new double[2] { 0.640, 0.330 };
                    tmp[1] = new double[2] { 0.300, 0.600 };
                    tmp[2] = new double[2] { 0.150, 0.060 };
                    break;
                case ColorantEncoding.P22:
                    tmp[0] = new double[2] { 0.625, 0.340 };
                    tmp[1] = new double[2] { 0.280, 0.605 };
                    tmp[2] = new double[2] { 0.155, 0.070 };
                    break;
                case ColorantEncoding.SMPTE_RP145:
                    tmp[0] = new double[2] { 0.630, 0.340 };
                    tmp[1] = new double[2] { 0.310, 0.595 };
                    tmp[2] = new double[2] { 0.155, 0.070 };
                    break;
            }
            return tmp;
        }
    }

    /// <summary>
    /// This tag specifies the laydown order in which colorants will be printed on an n-colorant device.
    /// </summary>
    public sealed class colorantOrderTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.colorantOrder; } }
        public uint ColorantCount { get; private set; }
        public byte[] ColorantNumber { get; private set; }

        internal colorantOrderTagDataEntry(int idx)
        {
            //Count of colorants (4 bytes)            
            ColorantCount = Helper.GetUInt32(idx);
            //Number of the colorant to be printed first
            ColorantNumber = new byte[ColorantCount]; int c = 0;
            for (int i = idx + 4; i < idx + 4 + ColorantCount; i++) { ColorantNumber[c] = ICCProfile.DataBytes[i]; c++; }
        }
    }

    /// <summary>
    /// The purpose of this tag is to identify the colorants used in the profile by a unique name and set of PCSXYZ or PCSLAB values to give the colorant an unambiguous value.
    /// </summary>
    public sealed class colorantTableTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.colorantTable; } }
        public uint ColorantCount { get; private set; }
        public ColorantDataEntry[] ColorantData { get; private set; }

        internal colorantTableTagDataEntry(int idx)
        {
            //Count of colorants (4 bytes)            
            ColorantCount = Helper.GetUInt32(idx);
            //Number of the colorant to be printed first
            ColorantData = new ColorantDataEntry[ColorantCount];
            int c = 0; int end = (idx + 4) + (int)ColorantCount * 38;
            for (int i = idx + 4; i < end; i++)
            {
                //Colorant name (32 bytes)
                string name = Helper.GetASCIIString(i, 32);
                //PCS values (6 bytes (2 bytes each))
                ushort pcs1 = Helper.GetUInt16(i + 32);
                ushort pcs2 = Helper.GetUInt16(i + 34);
                ushort pcs3 = Helper.GetUInt16(i + 36);
                ColorantData[c] = new ColorantDataEntry(name, new ushort[] { pcs1, pcs2, pcs3 });
                c++;
            }
        }
    }

    /// <summary>
    /// The type contains a one-dimensional table of double values.
    /// </summary>
    public sealed class curveTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.curve; } }
        public uint CurvePointCount { get; private set; }
        public double[] CurveData { get; private set; }
        internal int end;
        
        internal curveTagDataEntry(int idx)
        {
            //Count of colorants (4 bytes)            
            CurvePointCount = Helper.GetUInt32(idx);
            //Curve values (2 bytes)
            CurveData = new double[CurvePointCount];
            if (CurvePointCount == 1) { CurveData[0] = Helper.GetU8Fixed8Number(idx + 4); end = idx + 6; }
            else
            {
                int c = 0; end = (idx + 4) + (int)CurvePointCount * 2;
                for (int i = idx + 4; i < end; i += 2)
                {
                    CurveData[c] = Helper.GetUInt16(i) / 65535d;
                    c++;
                }
            }
        }

        public double GetValue(double number)
        {
            double t = number * (CurveData.Length - 1);
            if (t % 1 != 0)
            {
                int i = (int)Math.Floor(t);
                return CurveData[i] + ((CurveData[i + 1] - CurveData[i]) * (t % 1));
            }
            else { return CurveData[(int)t]; }
        }

        public double GetValueInverted(double number)
        {
            int i = 0;
            while (i < CurveData.Length && number > CurveData[i]) { i++; }
            if (CurveData[i] == number) { return i / CurveData.Length; }
            else if (i > 0) { return (i - 1 + ((CurveData[i] - number) / (CurveData[i] - CurveData[i - 1]))) / CurveData.Length; }
            else { return 0; }
        }
    }

    /// <summary>
    /// The dataType is a simple data structure that contains either 7-bit ASCII or binary data, i.e. textType data or transparent bytes.
    /// </summary>
    public sealed class dataTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.data; } }
        public byte[] Data { get; private set; }
        public bool IsASCII { get; private set; }

        internal dataTagDataEntry(int idx)
        {
            //Data flag (4 bytes) (0 = ASCII - 1 = binary)            
            uint f = Helper.GetUInt32(idx);
            IsASCII = f == 0 ? true : false;
            //Data
            Data = new byte[size - 12];
            for (int i = 0; i < size - 12; i++) { Data[i] = ICCProfile.DataBytes[idx + 4 + i]; }
        }
    }

    /// <summary>
    /// This type is a representation of the time and date.
    /// </summary>
    public sealed class dateTimeTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.dateTime; } }
        public DateTime DateAndTime { get; private set; }

        internal dateTimeTagDataEntry(int idx)
        {
            DateAndTime = Helper.GetDateTime(idx);
        }
    }

    /// <summary>
    /// This structure represents a colour transform using tables with 16-bit precision.
    /// </summary>
    public sealed class lut16TagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.lut16; } }
        public byte InputChannelCount { get; private set; }
        public byte OutputChannelCount { get; private set; }
        public byte CLUTGridPointCount { get; private set; }
        public ushort InputTableEntryCount { get; private set; }
        public ushort OutputTableEntryCount { get; private set; }

        public double[,] Matrix { get; private set; }
        public LUT16[] InputValues { get; private set; }
        public CLUT16 CLUTValues { get; private set; }
        public LUT16[] OutputValues { get; private set; }

        internal lut16TagDataEntry(int idx)
        {
            //Channel counts (1 byte each)
            InputChannelCount = ICCProfile.DataBytes[idx];
            OutputChannelCount = ICCProfile.DataBytes[idx + 1];
            CLUTGridPointCount = ICCProfile.DataBytes[idx + 2];
            //1 byte reserved
            //Matrix (4 bytes each)
            Matrix = Helper.GetMatrix(3, 3,  idx + 4, false);
            //Number of input table entries
            InputTableEntryCount = Helper.GetUInt16(idx + 40);
            //Number of output table entries
            OutputTableEntryCount = Helper.GetUInt16(idx + 42);
            //Input
            InputValues = new LUT16[InputChannelCount]; int c = 0;
            for (int i = 0; i < InputChannelCount; i++) { InputValues[i] = new LUT16(idx + 44 + c, InputTableEntryCount); c += InputTableEntryCount * 2; }
            //CLUT
            byte[] tmp = new byte[16];
            for (int i = 0; i < 16; i++) { tmp[i] = CLUTGridPointCount; }
            CLUTValues = new CLUT16(idx + 44 + c, InputChannelCount, OutputChannelCount, tmp);
            //Output
            OutputValues = new LUT16[OutputChannelCount]; c = 0;
            for (int i = 0; i < OutputChannelCount; i++) { OutputValues[i] = new LUT16(CLUTValues.end + c, OutputTableEntryCount); c += OutputTableEntryCount * 2; }
        }

        public double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match LUT channel count"); }

            //Matrix
            double[] m = Helper.MultiplyMatrix3x3(Matrix, inColor);
            //Input LUT
            double[] ilut = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++) { ilut[i] = InputValues[i].GetNumber(m[i]); }
            //CLUT
            double[] c = CLUTValues.GetValue(ilut);
            //Output LUT
            double[] olut = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++) { olut[i] = OutputValues[i].GetNumber(c[i]); }

            return olut;
        }
    }

    /// <summary>
    /// This structure represents a colour transform using tables with 8-bit precision.
    /// </summary>
    public sealed class lut8TagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.lut8; } }
        public byte InputChannelCount { get; private set; }
        public byte OutputChannelCount { get; private set; }
        public byte CLUTGridPointCount { get; private set; }

        public double[,] Matrix { get; private set; }
        public LUT8[] InputValues { get; private set; }
        public CLUT8 CLUTValues { get; private set; }
        public LUT8[] OutputValues { get; private set; }

        internal lut8TagDataEntry(int idx)
        {
            //Channel counts (1 byte each)
            InputChannelCount = ICCProfile.DataBytes[idx];
            OutputChannelCount = ICCProfile.DataBytes[idx + 1];
            CLUTGridPointCount = ICCProfile.DataBytes[idx + 2];
            //1 byte reserved
            //Matrix (4 bytes each)
            Matrix = Helper.GetMatrix(3, 3,  idx + 4, false);
            //Input
            InputValues = new LUT8[InputChannelCount]; int c = 0;
            for (int i = 0; i < InputChannelCount; i++) { InputValues[i] = new LUT8(idx + 40 + c); c += 256; }
            //CLUT
            byte[] tmp = new byte[16];
            for (int i = 0; i < 16; i++) { tmp[i] = CLUTGridPointCount; }
            CLUTValues = new CLUT8(idx + 40 + (256 * InputChannelCount), InputChannelCount, OutputChannelCount, tmp);
            //Output
            OutputValues = new LUT8[OutputChannelCount]; c = 0;
            for (int i = 0; i < OutputChannelCount; i++) { OutputValues[i] = new LUT8(CLUTValues.end + c); c += 256; }
        }

        public double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match LUT channel count"); }

            //Matrix
            double[] m = Helper.MultiplyMatrix3x3(Matrix, inColor);
            //Input LUT
            double[] ilut = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++) { ilut[i] = InputValues[i].GetNumber(inColor[i]); }
            //CLUT
            double[] c = CLUTValues.GetValue(ilut);
            //Output LUT
            double[] olut = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++) { olut[i] = OutputValues[i].GetNumber(c[i]); }

            return olut;
        }
    }

    /// <summary>
    /// This structure represents a colour transform.
    /// </summary>
    public sealed class lutAToBTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.lutAToB; } }
        public byte InputChannelCount { get; private set; }
        public byte OutputChannelCount { get; private set; }
        public double[,] Matrix3x3 { get; private set; }
        public double[] Matrix3x1 { get; private set; }
        public CLUT CLUTValues { get; private set; }
        public TagDataEntry[] CurveB { get { return curveB; } }
        public TagDataEntry[] CurveM { get { return curveM ; } }
        public TagDataEntry[] CurveA { get { return curveA; } }

        private int BCurveOffset;
        private int MatrixOffset;
        private int MCurveOffset;
        private int CLUTOffset;
        private int ACurveOffset;
        private TagDataEntry[] curveB;
        private TagDataEntry[] curveM;
        private TagDataEntry[] curveA;

        internal lutAToBTagDataEntry(int idx)
        {
            //Number of Input Channels (1 byte)
            InputChannelCount = ICCProfile.DataBytes[idx];
            //Number of Output Channels (1 byte)
            OutputChannelCount = ICCProfile.DataBytes[idx + 1];
            //Reserved for padding (2 bytes)
            //Offset to first "B" curve (4 bytes)
            BCurveOffset = (int)Helper.GetUInt32(idx + 4);
            //Offset to matrix (4 bytes)
            MatrixOffset = (int)Helper.GetUInt32(idx + 8);
            //Offset to first "M" curve (4 bytes)
            MCurveOffset = (int)Helper.GetUInt32(idx + 12);
            //Offset to CLUT (4 bytes)
            CLUTOffset = (int)Helper.GetUInt32(idx + 16);
            //Offset to first "A" curve (4 bytes)
            ACurveOffset = (int)Helper.GetUInt32(idx + 20);

            //Curves
            if (BCurveOffset != 0) { GetCurve(ACurveOffset, ref curveB,  idx, InputChannelCount); }
            if (MCurveOffset != 0) { GetCurve(ACurveOffset, ref curveM,  idx, InputChannelCount); }
            if (CLUTOffset != 0) { CLUTValues = CLUT.GetCLUT(idx - 8 + CLUTOffset, false, InputChannelCount, OutputChannelCount); }
            if (ACurveOffset != 0) { GetCurve(ACurveOffset, ref curveA,  idx, InputChannelCount); }
            
            //Matrix
            if (MatrixOffset != 0)
            {
                int i = MatrixOffset + idx - 8;
                //Matrix 3x3 (4 bytes each)
                Matrix3x3 = Helper.GetMatrix(3, 3,  i, false);
                //Matrix 3x1 (4 bytes each)
                Matrix3x1 = Helper.GetMatrix(3,  i + 36, false);
            }
        }

        private static void GetCurve(int Offset, ref TagDataEntry[] data, int idx, int InputChannelCount)
        {
            //Signature (4 bytes + 4 bytes reserved)
            TypeSignature t = (TypeSignature)Helper.GetUInt32(Offset);
            if (t != TypeSignature.curve && t != TypeSignature.parametricCurve) { throw new CorruptProfileException("lutAToBTagDataEntry"); }
            //Curve
            data = new TagDataEntry[InputChannelCount];
            int end = idx + Offset;
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (t == TypeSignature.curve)
                {
                    data[i] = new curveTagDataEntry(end);
                    end = ((curveTagDataEntry)data[i]).end;
                }
                else if (t == TypeSignature.parametricCurve)
                {
                    data[i] = new parametricCurveTagDataEntry(idx);
                    end = ((parametricCurveTagDataEntry)data[i]).end;
                }
                end += (end % 4) + 8;
            }
        }

        public double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match LUT channel count"); }

            if (ACurveOffset != 0 && CLUTOffset != 0 && MCurveOffset != 0 && MatrixOffset != 0 && BCurveOffset != 0) { return GetValue_A_CLUT_M_Matrix_B(inColor); }
            else if (ACurveOffset != 0 && CLUTOffset != 0 && BCurveOffset != 0) { return GetValue_A_CLUT_B(inColor); }
            else if (MCurveOffset != 0 && MatrixOffset != 0 && BCurveOffset != 0) { return GetValue_M_Matrix_B(inColor); }
            else if (BCurveOffset != 0) { return GetValue_B(inColor); }
            else { throw new CorruptProfileException("Invalid set of values in lutAtoB"); }
        }

        private double[] GetValue_B(double[] inColor)
        {
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveB[i]).GetValue(inColor[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of BCurves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_M_Matrix_B(double[] inColor)
        {
            //M Curves
            double[] m = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveM[i].Signature == TypeSignature.curve) { m[i] = ((curveTagDataEntry)CurveM[i]).GetValue(inColor[i]); }
                else if (CurveM[i].Signature == TypeSignature.parametricCurve) { m[i] = ((parametricCurveTagDataEntry)CurveM[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of M-Curves is not correct"); }
            }
            
            //Matrix
            double[] t = Helper.MultiplyMatrix3x3(Matrix3x3, m);

            //B Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveB[i]).GetValue(t[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(t[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_A_CLUT_B(double[] inColor)
        {
            //A Curves
            double[] a = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveA[i].Signature == TypeSignature.curve) { a[i] = ((curveTagDataEntry)CurveA[i]).GetValue(inColor[i]); }
                else if (CurveA[i].Signature == TypeSignature.parametricCurve) { a[i] = ((parametricCurveTagDataEntry)CurveA[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of A-Curves is not correct"); }
            }

            //CLUT
            double[] c = CLUTValues.GetValue(a);

            //B Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveB[i]).GetValue(c[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(c[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_A_CLUT_M_Matrix_B(double[] inColor)
        {
            //A Curves
            double[] a = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveA[i].Signature == TypeSignature.curve) { a[i] = ((curveTagDataEntry)CurveA[i]).GetValue(inColor[i]); }
                else if (CurveA[i].Signature == TypeSignature.parametricCurve) { a[i] = ((parametricCurveTagDataEntry)CurveA[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of A-Curves is not correct"); }
            }

            //CLUT
            double[] c = CLUTValues.GetValue(a);

            //M Curves
            double[] m = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveM[i].Signature == TypeSignature.curve) { m[i] = ((curveTagDataEntry)CurveM[i]).GetValue(c[i]); }
                else if (CurveM[i].Signature == TypeSignature.parametricCurve) { m[i] = ((parametricCurveTagDataEntry)CurveM[i]).Curve.Function(c[i]); }
                else { throw new CorruptProfileException("Curvetype of M-Curves is not correct"); }
            }

            //Matrix
            double[] t = Helper.MultiplyMatrix3x3(Matrix3x3, m);

            //B Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveB[i]).GetValue(t[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(t[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }
            return output;
        }
    }

    /// <summary>
    /// This structure represents a colour transform.
    /// </summary>
    public sealed class lutBToATagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.lutAToB; } }
        public byte InputChannelCount { get; private set; }
        public byte OutputChannelCount { get; private set; }
        public double[,] Matrix3x3 { get; private set; }
        public double[] Matrix3x1 { get; private set; }
        public CLUT CLUTValues { get; private set; }
        public TagDataEntry[] CurveB { get { return curveB; } }
        public TagDataEntry[] CurveM { get { return curveM ; } }
        public TagDataEntry[] CurveA { get { return curveA; } }

        private int BCurveOffset;
        private int MatrixOffset;
        private int MCurveOffset;
        private int CLUTOffset;
        private int ACurveOffset;
        private TagDataEntry[] curveB;
        private TagDataEntry[] curveM;
        private TagDataEntry[] curveA;

        internal lutBToATagDataEntry(int idx)
        {
            //Number of Input Channels (1 byte)
            InputChannelCount = ICCProfile.DataBytes[idx];
            //Number of Output Channels (1 byte)
            OutputChannelCount = ICCProfile.DataBytes[idx + 1];
            //Reserved for padding (2 bytes)
            //Offset to first "B" curve (4 bytes)
            BCurveOffset = (int)Helper.GetUInt32(idx + 4);
            //Offset to matrix (4 bytes)
            MatrixOffset = (int)Helper.GetUInt32(idx + 8);
            //Offset to first "M" curve (4 bytes)
            MCurveOffset = (int)Helper.GetUInt32(idx + 12);
            //Offset to CLUT (4 bytes)
            CLUTOffset = (int)Helper.GetUInt32(idx + 16);
            //Offset to first "A" curve (4 bytes)
            ACurveOffset = (int)Helper.GetUInt32(idx + 20);

            //Curves
            if (BCurveOffset != 0) { GetCurve(ACurveOffset, ref curveB,  idx, InputChannelCount); }
            if (MCurveOffset != 0) { GetCurve(ACurveOffset, ref curveM,  idx, InputChannelCount); }
            if (CLUTOffset != 0) { CLUTValues = CLUT.GetCLUT(idx - 8 + CLUTOffset, false, InputChannelCount, OutputChannelCount); }
            if (ACurveOffset != 0) { GetCurve(ACurveOffset, ref curveA,  idx, InputChannelCount); }

            //Matrix
            if (MatrixOffset != 0)
            {
                int i = MatrixOffset + idx - 8;
                //Matrix 3x3 (4 bytes each)
                Matrix3x3 = Helper.GetMatrix(3, 3,  i, false);
                //Matrix 3x1 (4 bytes each)
                Matrix3x1 = Helper.GetMatrix(3,  i + 36, false);
            }
        }

        private static void GetCurve(int Offset, ref TagDataEntry[] data, int idx, int InputChannelCount)
        {
            //Signature (4 bytes + 4 bytes reserved)
            TypeSignature t = (TypeSignature)Helper.GetUInt32(Offset);
            if (t != TypeSignature.curve && t != TypeSignature.parametricCurve) { throw new CorruptProfileException("lutAToBTagDataEntry"); }
            //Curve
            data = new TagDataEntry[InputChannelCount];
            int end = idx + Offset;
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (t == TypeSignature.curve)
                {
                    data[i] = new curveTagDataEntry(end);
                    end = ((curveTagDataEntry)data[i]).end;
                }
                else if (t == TypeSignature.parametricCurve)
                {
                    data[i] = new parametricCurveTagDataEntry(idx);
                    end = ((parametricCurveTagDataEntry)data[i]).end;
                }
                end += (end % 4) + 8;
            }
        }

        public double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match LUT channel count"); }

            if (ACurveOffset != 0 && CLUTOffset != 0 && MCurveOffset != 0 && MatrixOffset != 0 && BCurveOffset != 0) { return GetValue_B_Matrix_M_CLUT_A(inColor); }
            else if (ACurveOffset != 0 && CLUTOffset != 0 && BCurveOffset != 0) { return GetValue_B_CLUT_A(inColor); }
            else if (MCurveOffset != 0 && MatrixOffset != 0 && BCurveOffset != 0) { return GetValue_B_Matrix_M(inColor); }
            else if (BCurveOffset != 0) { return GetValue_B(inColor); }
            else { throw new CorruptProfileException("Invalid set of values in lutAtoB"); }
        }

        private double[] GetValue_B(double[] inColor)
        {
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveB[i]).GetValue(inColor[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_B_Matrix_M(double[] inColor)
        {
            //B Curves
            double[] b = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { b[i] = ((curveTagDataEntry)CurveB[i]).GetValue(inColor[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { b[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }

            //Matrix
            double[] t = Helper.MultiplyMatrix3x3(Matrix3x3, b);

            //M Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveM[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveM[i]).GetValue(t[i]); }
                else if (CurveM[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveM[i]).Curve.Function(t[i]); }
                else { throw new CorruptProfileException("Curvetype of M-Curves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_B_CLUT_A(double[] inColor)
        {
            //B Curves
            double[] b = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { b[i] = ((curveTagDataEntry)CurveB[i]).GetValue(inColor[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { b[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }

            //CLUT
            double[] c = CLUTValues.GetValue(b);

            //A Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveA[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveA[i]).GetValue(c[i]); }
                else if (CurveA[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveA[i]).Curve.Function(c[i]); }
                else { throw new CorruptProfileException("Curvetype of A-Curves is not correct"); }
            }
            return output;
        }

        private double[] GetValue_B_Matrix_M_CLUT_A(double[] inColor)
        {
            //B Curves
            double[] b = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveB[i].Signature == TypeSignature.curve) { b[i] = ((curveTagDataEntry)CurveB[i]).GetValue(inColor[i]); }
                else if (CurveB[i].Signature == TypeSignature.parametricCurve) { b[i] = ((parametricCurveTagDataEntry)CurveB[i]).Curve.Function(inColor[i]); }
                else { throw new CorruptProfileException("Curvetype of B-Curves is not correct"); }
            }

            //Matrix
            double[] t = Helper.MultiplyMatrix3x3(Matrix3x3, b);

            //M Curves
            double[] m = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++)
            {
                if (CurveM[i].Signature == TypeSignature.curve) { m[i] = ((curveTagDataEntry)CurveM[i]).GetValue(t[i]); }
                else if (CurveM[i].Signature == TypeSignature.parametricCurve) { m[i] = ((parametricCurveTagDataEntry)CurveM[i]).Curve.Function(t[i]); }
                else { throw new CorruptProfileException("Curvetype of M-Curves is not correct"); }
            }

            //CLUT
            double[] c = CLUTValues.GetValue(m);

            //A Curves
            double[] output = new double[OutputChannelCount];
            for (int i = 0; i < OutputChannelCount; i++)
            {
                if (CurveA[i].Signature == TypeSignature.curve) { output[i] = ((curveTagDataEntry)CurveA[i]).GetValue(c[i]); }
                else if (CurveA[i].Signature == TypeSignature.parametricCurve) { output[i] = ((parametricCurveTagDataEntry)CurveA[i]).Curve.Function(c[i]); }
                else { throw new CorruptProfileException("Curvetype of A-Curves is not correct"); }
            }
            return output;
        }
    }

    /// <summary>
    /// The measurementType information refers only to the internal profile data and is meant to provide profile makers an alternative to the default measurement specifications.
    /// </summary>
    public sealed class measurementTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.measurement; } }
        public StandardObserver Observer { get; private set; }
        public XYZnumber XYZBacking { get; private set; }
        public MeasurementGeometry Geometry { get; private set; }
        public double Flare { get; private set; }
        public StandardIlluminant Illuminant { get; private set; }

        internal measurementTagDataEntry(int idx)
        {
            //Standard observer (4 bytes)            
            Observer = (StandardObserver)Helper.GetUInt32(idx);
            //nCIEXYZ tristimulus values for measurement backing
            XYZBacking = new XYZnumber(idx + 4);
            //Measurement geometry (4 bytes)
            Geometry = (MeasurementGeometry)Helper.GetUInt32(idx + 16);
            //Measurement flare (4 bytes)
            Flare = Helper.GetU16Fixed16Number(idx + 20);
            //Standard illuminant (4 bytes)
            Illuminant = (StandardIlluminant)Helper.GetUInt32(idx + 24);
        }
    }

    /// <summary>
    /// This tag structure contains a set of records each referencing a multilingual string associated with a profile.
    /// </summary>
    public sealed class multiLocalizedUnicodeTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.multiLocalizedUnicode; } }
        public int RecordCount { get; private set; }
        public LocalizedString[] Text { get; private set; }

        private int RecordSize;
        private string[] LanguageCode;  //ISO 639-1
        private string[] CountryCode;   //ISO 3166-1
        private int[] StringLength;
        private int[] StringOffset;
        internal int end;

        internal multiLocalizedUnicodeTagDataEntry(int idx, bool IsPlaceholder)
        {
            if (!IsPlaceholder)
            {
                //Number of records (4 bytes)
                RecordCount = (int)Helper.GetUInt32(idx);
                //Record size (has to be 12 as for V4.3) (4 bytes)
                RecordSize = (int)Helper.GetUInt32(idx + 4);
                //Records
                LanguageCode = new string[RecordCount];
                CountryCode = new string[RecordCount];
                StringLength = new int[RecordCount];
                StringOffset = new int[RecordCount];
                int end = idx + 8 + RecordCount * RecordSize; int c = 0;
                for (int i = idx + 8; i < end; i += RecordSize)
                {
                    //Language Code (2 bytes)
                    LanguageCode[c] = Helper.GetASCIIString(i, 2);
                    //Country Code (2 bytes)
                    CountryCode[c] = Helper.GetASCIIString(i + 2, 2);
                    //Record string length (4 bytes)
                    StringLength[c] = (int)Helper.GetUInt32(i + 4);
                    //Record offset (4 bytes)
                    StringOffset[c] = (int)Helper.GetUInt32(i + 8);
                    c++;
                }
                //The strings
                Text = new LocalizedString[RecordCount];
                for (int i = 0; i < RecordCount; i++) { Text[i] = new LocalizedString(LanguageCode[i], CountryCode[i],  idx + StringOffset[i] - 8, StringLength[i]); }
                this.end = end + StringLength.Sum();
            }
            else { this.end = idx; }
        }
    }

    /// <summary>
    /// This structure represents a colour transform, containing a sequence of processing elements.
    /// </summary>
    public sealed class multiProcessElementsTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.multiProcessElements; } }
        public ushort InputChannelCount { get; private set; }
        public ushort OutputChannelCount { get; private set; }
        public int ProcessingElementCount { get; private set; }
        internal PositionNumber[] PositionTable { get; private set; }
        public MultiProcessElement[] Data { get; private set; }

        internal multiProcessElementsTagDataEntry(int idx)
        {
            //Number of input channels (2 bytes)            
            InputChannelCount = Helper.GetUInt16(idx);
            //Number of output channels (2 bytes)            
            OutputChannelCount = Helper.GetUInt16(idx + 2);
            //Number of processing elements (4 bytes)            
            ProcessingElementCount = (int)Helper.GetUInt32(idx + 4);
            //Process element positions table (8 bytes each)
            PositionTable = new PositionNumber[ProcessingElementCount];
            int end = idx + 8 + 8 * ProcessingElementCount; int c = 0;
            for (int i = idx + 8; i < end; i += 8) { PositionTable[c] = new PositionNumber(i); c++; }
            //Data
            Data = new MultiProcessElement[ProcessingElementCount];
            for (int i = 0; i < ProcessingElementCount; i++) { Data[i] = MultiProcessElement.CreateElement(PositionTable[i].Offset); }
        }

        public double[] GetValue(double[] inColor)
        {
            double[] output = inColor;
            for (int i = 0; i < ProcessingElementCount; i++) { output = Data[i].GetValue(output); }
            return output;
        }
    }

    /// <summary>
    /// The namedColor2Type is a count value and array of structures that provide colour coordinates for colour names.
    /// </summary>
    public sealed class namedColor2TagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.namedColor2; } }
        public byte[] VendorFlag { get; private set; }
        public int NamedColorCount { get; private set; }
        public int DeviceCoordinateCount { get; private set; }
        public string Prefix { get; private set; }
        public string Suffix { get; private set; }
        public NamedColor[] Colors { get; private set; }

        internal namedColor2TagDataEntry(int idx)
        {
            //Vendor flag (4 bytes)
            VendorFlag = new byte[] { ICCProfile.DataBytes[idx], ICCProfile.DataBytes[idx + 1], ICCProfile.DataBytes[idx + 2], ICCProfile.DataBytes[idx + 3] };
            //Count of named colours (4 bytes)
            NamedColorCount = (int)Helper.GetUInt32(idx + 4);
            //Number of device coordinates (4 bytes)            
            DeviceCoordinateCount = (int)Helper.GetUInt32(idx + 8);
            //Prefix for each colour name
            Prefix = Helper.GetASCIIString(idx + 12, 32);
            //Suffix for each colour name
            Prefix = Helper.GetASCIIString(idx + 44, 32);
            //Colors
            Colors = new NamedColor[NamedColorCount];
            int p = 38 + 2 * DeviceCoordinateCount;
            int end = idx + 76 + NamedColorCount * p;  int c = 0;
            for (int i = 0; i < end; i += p) { Colors[c] = new NamedColor(i, DeviceCoordinateCount); c++; }
        }
    }

    /// <summary>
    /// The parametricCurveType describes a one-dimensional curve by specifying one of a predefined set of functions using the parameters.
    /// </summary>
    public sealed class parametricCurveTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.parametricCurve; } }
        public ParametricCurve Curve { get; private set; }

        private ushort FunctionType;
        internal int end;

        internal parametricCurveTagDataEntry(int idx)
        {
            //Encoded value of the function type (2 bytes)            
            FunctionType = Helper.GetUInt16(idx);
            //2 bytes reserved
            Curve = new ParametricCurve(FunctionType,  idx + 4);
            end = Curve.end + 8;
        }
    }

    /// <summary>
    /// This type is an array of structures, each of which contains information from the header fields and tags from the original profiles which were combined to create the final profile.
    /// </summary>
    public sealed class profileSequenceDescTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.profileSequenceDesc; } }
        public int DescriptionCount { get; private set; }
        public ProfileDescription[] Descriptions { get; private set; }
        
        internal profileSequenceDescTagDataEntry(int idx, ICCHeader header)
        {
            //Count value specifying number of description structures in the array (4 bytes)
            DescriptionCount = (int)Helper.GetUInt32(idx);
            //Profile description structures
            Descriptions = new ProfileDescription[DescriptionCount];
            int end = idx + 4;
            for (int i = 0; i < DescriptionCount; i++) { Descriptions[i] = new ProfileDescription(end, header); end = Descriptions[i].end; }
        }
    }

    /// <summary>
    /// This type is an array of structures, each of which contains information for identification of a profile used in a sequence.
    /// </summary>
    public sealed class profileSequenceIdentifierTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.profileSequenceIdentifier; } }
        public int NumberCount { get; private set; }
        internal PositionNumber[] PositionTable { get; private set; }
        public string ProfileID { get; private set; }
        public multiLocalizedUnicodeTagDataEntry ProfileDescription { get; private set; }

        internal profileSequenceIdentifierTagDataEntry(int idx)
        {
            //Count, specifying number of structures in the array (4 bytes)
            NumberCount = (int)Helper.GetUInt32(idx);
            //Positions table for profile identifiers
            PositionTable = new PositionNumber[NumberCount];
            int end = idx + 4 + 8 * NumberCount; int c = 0;
            for (int i = idx + 4; i < end; i += 8) { PositionTable[c] = new PositionNumber(i); c++; }
            //Profile ID (16 bytes)
            ProfileID = Helper.GetProfileID(end);
            //Profile description
            ProfileDescription = new multiLocalizedUnicodeTagDataEntry(end + 16, false);
        }
    }

    /// <summary>
    /// The purpose of this tag type is to provide a mechanism to relate physical colorant amounts with the normalized device codes produced by lut8Type, lut16Type, lutAToBType, lutBToAType or multiProcessElementsType tags so that corrections can be made for variation in the device without having to produce a new profile.
    /// </summary>
    public sealed class responseCurveSet16TagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.responseCurveSet16; } }
        public ushort ChannelCount { get; private set; }
        public ushort MeasurmentTypesCount { get; private set; }
        public ResponseCurve[] Curves { get; private set; }
        private int[] Offset;

        internal responseCurveSet16TagDataEntry(int idx)
        {
            //Number of channels (2 bytes)
            ChannelCount = Helper.GetUInt16(idx);
            //Count of measurement types (2 bytes)
            MeasurmentTypesCount = Helper.GetUInt16(idx + 2);
            //Offsets (4 bytes each)
            Offset = new int[MeasurmentTypesCount];
            int end = idx + 4 + 4 * MeasurmentTypesCount; int c = 0;
            for (int i = idx + 4; i < end; i += 4) { Offset[c] = (int)Helper.GetUInt32(i); c++; }
            //Response curve structures
            Curves = new ResponseCurve[MeasurmentTypesCount];
            for (int i = 0; i < MeasurmentTypesCount; i++) { Curves[i] = new ResponseCurve(Offset[i] + idx - 8, ChannelCount); }
        }
    }

    /// <summary>
    /// This type represents an array of doubles (from 32bit fixed point values).
    /// </summary>
    public sealed class s15Fixed16ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.s15Fixed16Array; } }
        public double[] Data { get; private set; }

        internal s15Fixed16ArrayTagDataEntry(int idx)
        {
            Data = new double[(size - 8) / 4]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 4) { Data[c] = Helper.GetS15Fixed16Number(i) / 256d; c++; }
        }
    }

    /// <summary>
    /// Typically this type is used for registered tags that can be displayed on many development systems as a sequence of four characters.
    /// </summary>
    public sealed class signatureTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.signature; } }
        public string SignatureData { get; private set; }

        internal signatureTagDataEntry(int idx)
        {
            SignatureData = Helper.GetASCIIString(idx, 4);
        }
    }

    /// <summary>
    /// The textType is a simple text structure that contains a text string.
    /// </summary>
    public sealed class textTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.text; } }
        public string Text { get; private set; }

        internal textTagDataEntry(int idx)
        {
            Text = Helper.GetASCIIString(idx, size - 8);
        }
    }

    /// <summary>
    /// This type represents an array of doubles (from 32bit values).
    /// </summary>
    public sealed class u16Fixed16ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.u16Fixed16Array; } }
        public double[] Data { get; private set; }

        internal u16Fixed16ArrayTagDataEntry(int idx)
        {
            Data = new double[(size - 8) / 4]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 4) { Data[c] = Helper.GetU16Fixed16Number(i); c++; }
        }
    }

    /// <summary>
    /// This type represents an array of unsigned shorts.
    /// </summary>
    public sealed class uInt16ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.uInt16Array; } }
        public ushort[] Data { get; private set; }

        internal uInt16ArrayTagDataEntry(int idx)
        {
            Data = new ushort[(size - 8) / 2]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 2) { Data[c] = Helper.GetUInt16(i); c++; }
        }
    }

    /// <summary>
    /// This type represents an array of unsigned 32bit integers.
    /// </summary>
    public sealed class uInt32ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.uInt32Array; } }
        public uint[] Data { get; private set; }

        internal uInt32ArrayTagDataEntry(int idx)
        {
            Data = new uint[(size - 8) / 4]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 4) { Data[c] = Helper.GetUInt32(i); c++; }
        }
    }

    /// <summary>
    /// This type represents an array of unsigned 64bit integers.
    /// </summary>
    public sealed class uInt64ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.uInt64Array; } }
        public UInt64[] Data { get; private set; }

        internal uInt64ArrayTagDataEntry(int idx)
        {
            Data = new UInt64[(size - 8) / 8]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 8) { Data[c] = Helper.GetUInt64(i); c++; }
        }
    }

    /// <summary>
    /// This type represents an array of bytes.
    /// </summary>
    public sealed class uInt8ArrayTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.uInt8Array; } }
        public byte[] Data { get; private set; }

        internal uInt8ArrayTagDataEntry(int idx)
        {
            Data = new byte[size - 8]; int c = 0;
            for (int i = idx; i < size - 8; i++) { Data[c] = ICCProfile.DataBytes[i]; c++; }
        }
    }

    /// <summary>
    /// This type represents a set of viewing condition parameters.
    /// </summary>
    public sealed class viewingConditionsTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.viewingConditions; } }
        public XYZnumber IlluminantXYZ { get; private set; }
        public XYZnumber SurroundXYZ { get; private set; }
        public StandardIlluminant Illuminant { get; private set; }

        internal viewingConditionsTagDataEntry(int idx)
        {
            //Un-normalized CIEXYZ values for illuminant (12 bytes)
            IlluminantXYZ = new XYZnumber(idx);
            //Un-normalized CIEXYZ values for surround (12 bytes)
            IlluminantXYZ = new XYZnumber(idx + 12);
            //Standard illuminant (4 bytes)
            Illuminant = (StandardIlluminant)Helper.GetUInt32(idx + 24);
        }
    }

    /// <summary>
    /// The XYZType contains an array of XYZ values.
    /// </summary>
    public sealed class XYZTagDataEntry : TagDataEntry
    {
        public override TypeSignature Signature { get { return TypeSignature.XYZ; } }
        public XYZnumber[] Data { get; private set; }

        internal XYZTagDataEntry(int idx)
        {
            Data = new XYZnumber[(size - 8) / 12]; int c = 0;
            for (int i = idx; i < idx + size - 8; i += 12) { Data[c] = new XYZnumber(i); c++; }
        }
    }
}
