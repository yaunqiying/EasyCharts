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
    /// An element to process data
    /// </summary>
    public abstract class MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public abstract multiProcessElementSignature Signature { get; }
        /// <summary>
        /// Number of input channels
        /// </summary>
        public ushort InputChannelCount { get; protected set; }
        /// <summary>
        /// Number of output channels
        /// </summary>
        public ushort OutputChannelCount { get; protected set; }
        
        internal static MultiProcessElement CreateElement(int idx)
        {
            //Tag signature (byte position 0 to 3) (4 to 7 are zero)
            multiProcessElementSignature t = (multiProcessElementSignature)Helper.GetUInt32(idx);
            //Number of input channels (2 bytes)
            ushort InputChannelCount = Helper.GetUInt16(idx + 8);
            //Number of output channels (2 bytes)
            ushort OutputChannelCount = Helper.GetUInt16(idx + 10);
            return GetElement(t,  idx + 12, InputChannelCount, OutputChannelCount);
        }

        private static MultiProcessElement GetElement(multiProcessElementSignature type, int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            switch (type)
            {
                case multiProcessElementSignature.CurveSet:
                    return new CurveSetProcessElement(idx, InputChannelCount, OutputChannelCount);
                case multiProcessElementSignature.CLUT:
                    return new CLUTProcessElement(idx, InputChannelCount, OutputChannelCount);
                case multiProcessElementSignature.Matrix:
                    return new MatrixProcessElement(idx, InputChannelCount, OutputChannelCount);
                case multiProcessElementSignature.bACS:
                    return new bACSProcessElement(idx, InputChannelCount, OutputChannelCount);
                case multiProcessElementSignature.eACS:
                    return new eACSProcessElement(idx, InputChannelCount, OutputChannelCount);

                default:
                     throw new CorruptProfileException("MultiProcessElement");
            }
        }
        
        public abstract double[] GetValue(double[] inColor);
    }

    /// <summary>
    /// A matrix element to process data
    /// </summary>
    public sealed class MatrixProcessElement : MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public override multiProcessElementSignature Signature { get { return multiProcessElementSignature.Matrix; } }
        public double[,] MatrixIxO { get; private set; }        
        public double[] MatrixOx1 { get; private set; }

        internal MatrixProcessElement(int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            //Matrix IxO (4 bytes each)
            MatrixIxO = Helper.GetMatrix(InputChannelCount, OutputChannelCount,  idx, true);
            //Matrix Ox1 (4 bytes each)
            MatrixOx1 = Helper.GetMatrix(OutputChannelCount,  idx + (4 * InputChannelCount * OutputChannelCount), true);
        }

        public override double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match element channel count"); }

            return Helper.AddMatrix(Helper.MultiplyMatrix(MatrixIxO, inColor), MatrixOx1);
        }
    }

    /// <summary>
    /// A set of curves to process data
    /// </summary>
    public sealed class CurveSetProcessElement : MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public override multiProcessElementSignature Signature { get { return multiProcessElementSignature.CurveSet; } }
        /// <summary>
        /// An array with one dimensional curves
        /// </summary>
        public OneDimensionalCurve[] Curves { get; private set; }
        internal PositionNumber[] CurvePositions { get; private set; }

        internal CurveSetProcessElement(int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            //Curves
            Curves = new OneDimensionalCurve[InputChannelCount]; int end = idx;
            for (int i = 0; i < InputChannelCount; i++)
            {
                Curves[i] = new OneDimensionalCurve(end);
                end = Curves[i].end + Curves[i].end % 4;
            }
        }

        public override double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match element channel count"); }

            double[] output = new double[InputChannelCount];
            for (int i = 0; i < InputChannelCount; i++) { output[i] = Curves[i].GetValue(inColor[i]); }
            return output;
        }
    }

    /// <summary>
    /// An CLUT (Color Look-Up-Table) element to process data
    /// </summary>
    public sealed class CLUTProcessElement : MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public override multiProcessElementSignature Signature { get { return multiProcessElementSignature.CLUT; } }
        public CLUTf32 CLUTvalues { get; private set; }

        internal CLUTProcessElement(int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            //CLUT
            CLUTvalues = (CLUTf32)CLUT.GetCLUT(idx, true, InputChannelCount, OutputChannelCount);
        }

        public override double[] GetValue(double[] inColor)
        {
            if (inColor.Length != InputChannelCount) { throw new ArgumentException("Input color channel count does not match element channel count"); }

            return CLUTvalues.GetValue(inColor);
        }
    }

    /// <summary>
    /// An empty process element for future expansion
    /// </summary>
    public sealed class bACSProcessElement : MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public override multiProcessElementSignature Signature { get { return multiProcessElementSignature.bACS; } }

        internal bACSProcessElement(int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            //Nothing (reserved for future expansion)
        }

        public override double[] GetValue(double[] inColor)
        {
            return null;
        }
    }

    /// <summary>
    /// An empty process element for future expansion
    /// </summary>
    public sealed class eACSProcessElement : MultiProcessElement
    {
        /// <summary>
        /// The signature of this element
        /// </summary>
        public override multiProcessElementSignature Signature { get { return multiProcessElementSignature.eACS; } }

        internal eACSProcessElement(int idx, ushort InputChannelCount, ushort OutputChannelCount)
        {
            this.InputChannelCount = InputChannelCount;
            this.OutputChannelCount = OutputChannelCount;
            //Nothing (reserved for future expansion)
        }

        public override double[] GetValue(double[] inColor)
        {
            return null;
        }
    }
}
