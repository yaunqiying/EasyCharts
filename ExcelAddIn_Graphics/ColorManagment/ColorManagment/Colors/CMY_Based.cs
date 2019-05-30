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
    /// CMY color
    /// </summary>
    public class ColorCMY : Color
    {
        /// <summary>
        /// Cyan value (0.0 to 100.0)
        /// </summary>
        public double C
        {
            get { return (ColorValues[0] > Cmax) ? Cmax : ((ColorValues[0] < Cmin) ? Cmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }
        /// <summary>
        /// Magenta value (0.0 to 100.0)
        /// </summary>
        public double M
        {
            get { return (ColorValues[1] > Mmax) ? Mmax : ((ColorValues[1] < Mmin) ? Mmin : ColorValues[1]); }
            set { ColorValues[1] = value; }
        }
        /// <summary>
        /// Yellow value (0.0 to 100.0)
        /// </summary>
        public double Y
        {
            get { return (ColorValues[2] > Ymax) ? Ymax : ((ColorValues[2] < Ymin) ? Ymin : ColorValues[2]); }
            set { ColorValues[2] = value; }
        }
        
        /// <summary>
        /// Minimum value for C
        /// </summary>
        public const double Cmin = 0;
        /// <summary>
        /// Minimum value for M
        /// </summary>
        public const double Mmin = 0;
        /// <summary>
        /// Minimum value for Y
        /// </summary>
        public const double Ymin = 0;
        /// <summary>
        /// Maximum value for C
        /// </summary>
        public const double Cmax = 100;
        /// <summary>
        /// Maximum value for M
        /// </summary>
        public const double Mmax = 100;
        /// <summary>
        /// Maximum value for Y
        /// </summary>
        public const double Ymax = 100;

        /// <summary>
        /// The colormodel of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CMY; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 3; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CMY color
        /// </summary>
        /// <param name="profile">The icc profile</param>
        public ColorCMY(ICC profile)
            : this(profile, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CMY color
        /// </summary>
        /// <param name="profile">The icc profile</param>
        /// <param name="C">Cyan value (0.0 - 100.0)</param>
        /// <param name="M">Magenta value (0.0 - 100.0)</param>
        /// <param name="Y">Yellow value (0.0 - 100.0)</param>
        public ColorCMY(ICC profile, double C, double M, double Y)
            : base(profile)
        {
            this.C = C;
            this.M = M;
            this.Y = Y;
        }

        /// <summary>
        /// Creates a new instance of a CMY color
        /// </summary>
        /// <param name="C">Cyan value (0.0 - 100.0)</param>
        /// <param name="M">Magenta value (0.0 - 100.0)</param>
        /// <param name="Y">Yellow value (0.0 - 100.0)</param>
        protected ColorCMY(double C, double M, double Y)
            : base()
        {
            this.C = C;
            this.M = M;
            this.Y = Y;
        }

        #endregion
    }

    /// <summary>
    /// CMYK color
    /// </summary>
    public sealed class ColorCMYK : ColorCMY
    {
        /// <summary>
        /// Key (Black) value (0.0 to 100.0)
        /// </summary>
        public double K
        {
            get { return (ColorValues[3] > Kmax) ? Kmax : ((ColorValues[3] < Kmin) ? Kmin : ColorValues[3]); }
            set { ColorValues[3] = value; }
        }

        /// <summary>
        /// Minimum value for K
        /// </summary>
        public const double Kmin = 0;
        /// <summary>
        /// Maximum value for K
        /// </summary>
        public const double Kmax = 100;

        /// <summary>
        /// The colormodel of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.CMYK; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 4; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0], ColorValues[1], ColorValues[2], ColorValues[3] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a CMYK color
        /// </summary>
        /// <param name="profile">The icc profile</param>
        public ColorCMYK(ICC profile)
            : this(profile, 0, 0, 0, 0)
        { }

        /// <summary>
        /// Creates a new instance of a CMYK color
        /// </summary>
        /// <param name="profile">The icc profile</param>
        /// <param name="C">Cyan value (0.0 - 100.0)</param>
        /// <param name="M">Magenta value (0.0 - 100.0)</param>
        /// <param name="Y">Yellow value (0.0 - 100.0)</param>
        /// <param name="K">Key (Black) value (0.0 - 100.0)</param>
        public ColorCMYK(ICC profile, double C, double M, double Y, double K)
            : base(profile, C, M, Y)
        {
            this.K = K;
        }

        #endregion
    }
}
