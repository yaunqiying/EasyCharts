using System;

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
    /// Conversion Matrix and XYZ positions of whitepoints
    /// </summary>
    public sealed class Whitepoint
    {
        /// <summary>
        /// The name of this whitepoint
        /// </summary>
        public WhitepointName Name { get; private set; }

        /// <summary>
        /// X-value of this whitepoint
        /// </summary>
        public double X { get { return DefVal[0]; } }
        /// <summary>
        /// Y-value of this whitepoint
        /// </summary>
        public double Y { get { return DefVal[1]; } }
        /// <summary>
        /// Z-value of this whitepoint
        /// </summary>
        public double Z { get { return DefVal[2]; } }
        /// <summary>
        /// X, Y and Z value in an array
        /// </summary>
        public double[] ValueArray { get { return new double[] { DefVal[0], DefVal[1], DefVal[2] }; } }

        /// <summary>
        /// x-chromaticity coordinate
        /// </summary>
        public double Cx { get { return DefCh[0]; } }
        /// <summary>
        /// y-chromaticity coordinate
        /// </summary>
        public double Cy { get { return DefCh[1]; } }
        /// <summary>
        /// Chromaticity coordinates
        /// </summary>
        public double[] ChromaticityArray { get { return new double[] { DefCh[0], DefCh[1] }; } }

        private double[] DefCh;
        private double[] DefVal;

        /// <summary>
        /// Creates a new instance of a whitepoint
        /// </summary>
        /// <param name="Name">The name of the whitepoint</param>
        public Whitepoint(WhitepointName Name)
        {
            this.Name = Name;
            DefVal = GetXYZValues(Name);
            DefCh = GetChromaticityValues(Name);
            if (DefVal == null || DefCh == null) this.Name = WhitepointName.Custom;
        }

        /// <summary>
        /// Creates a new instance of a whitepoint
        /// </summary>
        /// <param name="XYZvalues">The XYZ values of the whitepoint</param>
        public Whitepoint(double[] XYZvalues)
        {
            if (XYZvalues[0] == 0.9642 && XYZvalues[1] == 1 && XYZvalues[2] == 0.8249) { this.Name = WhitepointName.D50; }
            else
            {
                var e = Enum.GetValues(typeof(WhitepointName));
                foreach (int i in e)
                {
                    if (GetXYZValues((WhitepointName)i) == XYZvalues) { this.Name = (WhitepointName)i; break; }
                    else { this.Name = WhitepointName.Custom; }
                }
            }

            if (Name != WhitepointName.Custom)
            {
                DefVal = GetXYZValues(Name);
                DefCh = GetChromaticityValues(Name);
            }
            else
            {
                DefVal = XYZvalues;
                DefCh = new double[2];
            }
        }

        /// <summary>
        /// Copies this whitepoint to a new whitepoint with the same values
        /// </summary>
        /// <returns>A copy of this whitepoint</returns>
        public Whitepoint Copy()
        {
            Whitepoint p = new Whitepoint(this.Name);
            p.DefCh = (double[])this.DefCh.Clone();
            p.DefVal = (double[])this.DefVal.Clone();
            return p;
        }

        /// <summary>
        /// The position of a whitepoint in XYZ values
        /// </summary>
        /// <param name="whitepoint">The name of the whitepoint</param>
        /// <returns>The XYZ values from the whitepoint</returns>
        public static double[] GetXYZValues(WhitepointName whitepoint)
        {
            switch (whitepoint)
            {
                case WhitepointName.A:
                    return XYZ_A;
                case WhitepointName.B:
                    return XYZ_B;
                case WhitepointName.C:
                    return XYZ_C;
                case WhitepointName.D50:
                    return XYZ_D50;
                case WhitepointName.D55:
                    return XYZ_D55;
                case WhitepointName.D65:
                    return XYZ_D65;
                case WhitepointName.D75:
                    return XYZ_D75;
                case WhitepointName.E:
                    return XYZ_E;
                case WhitepointName.F2:
                    return XYZ_F2;
                case WhitepointName.F7:
                    return XYZ_F7;
                case WhitepointName.F11:
                    return XYZ_F11;

                default:
                    return null;
            }
        }

        /// <summary>
        /// The position of a whitepoint in chromaticity coordinates
        /// </summary>
        /// <param name="whitepoint">The name of the whitepoint</param>
        /// <returns>The chromaticity coordinates from the whitepoint</returns>
        public static double[] GetChromaticityValues(WhitepointName whitepoint)
        {
            switch (whitepoint)
            {
                case WhitepointName.A:
                    return Ch_A;
                case WhitepointName.B:
                    return Ch_B;
                case WhitepointName.C:
                    return Ch_C;
                case WhitepointName.D50:
                    return Ch_D50;
                case WhitepointName.D55:
                    return Ch_D55;
                case WhitepointName.D65:
                    return Ch_D65;
                case WhitepointName.D75:
                    return Ch_D75;
                case WhitepointName.E:
                    return Ch_E;
                case WhitepointName.F2:
                    return Ch_F2;
                case WhitepointName.F7:
                    return Ch_F7;
                case WhitepointName.F11:
                    return Ch_F11;

                default:
                    return null;
            }
        }

        #region Values

        //Chromaticity
        public static readonly double[] Ch_A = { 0.44757, 0.40745 };
        public static readonly double[] Ch_B = { 0.34842, 0.35161 };
        public static readonly double[] Ch_C = { 0.31006, 0.31616 };
        public static readonly double[] Ch_D50 = { 0.34567, 0.3585 };
        public static readonly double[] Ch_D55 = { 0.33242, 0.34743 };
        public static readonly double[] Ch_D65 = { 0.31271, 0.32902 };
        public static readonly double[] Ch_D75 = { 0.29902, 0.31485 };
        public static readonly double[] Ch_E = { 0.33333, 0.33333 };
        public static readonly double[] Ch_F2 = { 0.37208, 0.37529 };
        public static readonly double[] Ch_F7 = { 0.31292, 0.32933 };
        public static readonly double[] Ch_F11 = { 0.38052, 0.37713 };

        //XYZ
        public static readonly double[] XYZ_A = { 1.0985, 1, 0.35585 };
        public static readonly double[] XYZ_B = { 0.99072, 1, 0.85223 };
        public static readonly double[] XYZ_C = { 0.98074, 1, 1.18232 };
        public static readonly double[] XYZ_D50 = { 0.96422, 1, 0.82521 };
        public static readonly double[] XYZ_D55 = { 0.95682, 1, 0.92149 };
        public static readonly double[] XYZ_D65 = { 0.95047, 1, 1.08883 };
        public static readonly double[] XYZ_D75 = { 0.94972, 1, 1.22638 };
        public static readonly double[] XYZ_E = { 1, 1, 1 };
        public static readonly double[] XYZ_F2 = { 0.99186, 1, 0.67393 };
        public static readonly double[] XYZ_F7 = { 0.95041, 1, 1.08747 };
        public static readonly double[] XYZ_F11 = { 1.00962, 1, 0.6435 };
        
        #endregion

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            Whitepoint c = obj as Whitepoint;
            if ((Object)c == null) { return false; }

            return Name == c.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Whitepoint a, Whitepoint b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.Name == b.Name;
        }

        public static bool operator !=(Whitepoint a, Whitepoint b)
        {
            return !(a == b);
        }

        #endregion
    }
}