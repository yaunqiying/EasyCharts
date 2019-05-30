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
    /// Represents a YCbCr colorspace with all the data
    /// </summary>
    public abstract class YCbCrColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public abstract YCbCrSpaceName Name { get; }
        /// <summary>
        /// The reference white
        /// </summary>
        public abstract Whitepoint ReferenceWhite { get; }

        internal abstract double KR { get; }
        internal abstract double KB { get; }
        internal double KG { get { return 1 - KB - KR; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal abstract double[] Cr { get; }
        /// <summary>
        /// Green primary
        /// </summary>
        internal abstract double[] Cg { get; }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal abstract double[] Cb { get; }


        /// <summary>
        /// Get a colorspace from the name
        /// </summary>
        /// <param name="name">The name of the colorspace</param>
        /// <returns>The named colorspace</returns>
        public static YCbCrColorspace GetColorspace(YCbCrSpaceName name)
        {
            switch (name)
            {
                case YCbCrSpaceName.ITU_R_BT601_625:
                    return new ITU_R_BT601_625();
                case YCbCrSpaceName.ITU_R_BT601_525:
                    return new ITU_R_BT601_525();
                case YCbCrSpaceName.ITU_R_BT709_1125:
                    return new ITU_R_BT709_1125();
                case YCbCrSpaceName.ITU_R_BT709_1250:
                    return new ITU_R_BT709_1250();

                default:
                    throw new Exception("Colorspace not found");
            }
        }

        #region Standard Overrides

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            YCbCrColorspace c = obj as YCbCrColorspace;
            if ((Object)c == null) { return false; }

            return Name == c.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(YCbCrColorspace a, YCbCrColorspace b)
        {
            if (Object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            return a.Name == b.Name;
        }

        public static bool operator !=(YCbCrColorspace a, YCbCrColorspace b)
        {
            return !(a == b);
        }

        #endregion
    }

    /// <summary>
    /// The ITU-R BT.601 SD TV Rec. 625 YCbCr colorspace
    /// </summary>
    public sealed class ITU_R_BT601_625 : YCbCrColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override YCbCrSpaceName Name { get { return YCbCrSpaceName.ITU_R_BT601_625; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }

        internal override double KR { get { return 0.299; } }
        internal override double KB { get { return 0.114; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.29, 0.6 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ITU_R_BT601 colorspace
        /// </summary>
        public ITU_R_BT601_625()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The ITU-R BT.601 SD TV Rec. 525 YCbCr colorspace
    /// </summary>
    public sealed class ITU_R_BT601_525 : YCbCrColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override YCbCrSpaceName Name { get { return YCbCrSpaceName.ITU_R_BT601_525; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }

        internal override double KR { get { return 0.299; } }
        internal override double KB { get { return 0.114; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.63, 0.34 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.31, 0.595 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.155, 0.07 }; } }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ITU_R_BT601 colorspace
        /// </summary>
        public ITU_R_BT601_525()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The ITU-R BT.709 HD TV Rec. 1125 YCbCr colorspace
    /// </summary>
    public sealed class ITU_R_BT709_1125 : YCbCrColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override YCbCrSpaceName Name { get { return YCbCrSpaceName.ITU_R_BT709_1125; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }

        internal override double KR { get { return 0.2126; } }
        internal override double KB { get { return 0.0722; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.3, 0.6 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ITU_R_BT709 colorspace
        /// </summary>
        public ITU_R_BT709_1125()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }

    /// <summary>
    /// The ITU-R BT.709 HD TV Rec. 1250 YCbCr colorspace
    /// </summary>
    public sealed class ITU_R_BT709_1250 : YCbCrColorspace
    {
        /// <summary>
        /// The name of this colorspace
        /// </summary>
        public override YCbCrSpaceName Name { get { return YCbCrSpaceName.ITU_R_BT709_1250; } }
        /// <summary>
        /// The reference white
        /// </summary>
        public override Whitepoint ReferenceWhite { get { return wp; } }

        internal override double KR { get { return 0.2126; } }
        internal override double KB { get { return 0.0722; } }

        /// <summary>
        /// Red primary
        /// </summary>
        internal override double[] Cr { get { return new double[] { 0.64, 0.33 }; } }
        /// <summary>
        /// Green primary
        /// </summary>
        internal override double[] Cg { get { return new double[] { 0.3, 0.6 }; } }
        /// <summary>
        /// Blue primary
        /// </summary>
        internal override double[] Cb { get { return new double[] { 0.15, 0.06 }; } }

        private Whitepoint wp;

        /// <summary>
        /// Creates an instance of the ITU_R_BT709 colorspace
        /// </summary>
        public ITU_R_BT709_1250()
        {
            wp = new Whitepoint(WhitepointName.D65);
        }
    }
}
