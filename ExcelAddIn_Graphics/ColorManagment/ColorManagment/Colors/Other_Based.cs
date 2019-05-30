using System;
using System.Linq;
using ICCReader;
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
    /// X-Component Color (2-15 channels)
    /// </summary>
    public sealed class ColorX : Color
    {
        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ModelName; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return ColorValues.Take(ChannelCount).ToArray(); } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return CC; } }

        /// <summary>
        /// Minimum value for X
        /// </summary>
        public const double Xmin = double.MinValue;
        /// <summary>
        /// Maximum value for X
        /// </summary>
        public const double Xmax = double.MaxValue;

        private ColorModel ModelName;
        private byte CC = 15;

        #region Constructor

        /// <summary>
        /// Creates a new instance of a X-Channel Color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        /// <param name="Channels">The values for each channel (2 to 15)</param>
        public ColorX(ICC profile, params double[] Channels)
            : base(profile)
        {
            SetValues(profile.Header.DataColorspace);
            if (ChannelCount != Channels.Length) { throw new ArgumentException("Profile-space and channel-values do not match (wrong channelcount)"); }
            ColorValues = Channels;
        }

        /// <summary>
        /// Creates a new instance of a X-Channel Color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        public ColorX(ICC profile)
            : base(profile)
        {
            SetValues(profile.Header.DataColorspace);
        }

        private void SetValues(ICCReader.ColorSpaceType type)
        {
            switch (type)
            {
                case ICCReader.ColorSpaceType.Color2: CC = 2; ModelName = ColorModel.Color2; break;
                case ICCReader.ColorSpaceType.Color3: CC = 3; ModelName = ColorModel.Color3; break;
                case ICCReader.ColorSpaceType.Color4: CC = 4; ModelName = ColorModel.Color4; break;
                case ICCReader.ColorSpaceType.Color5: CC = 5; ModelName = ColorModel.Color5; break;
                case ICCReader.ColorSpaceType.Color6: CC = 6; ModelName = ColorModel.Color6; break;
                case ICCReader.ColorSpaceType.Color7: CC = 7; ModelName = ColorModel.Color7; break;
                case ICCReader.ColorSpaceType.Color8: CC = 8; ModelName = ColorModel.Color8; break;
                case ICCReader.ColorSpaceType.Color9: CC = 9; ModelName = ColorModel.Color9; break;
                case ICCReader.ColorSpaceType.Color10: CC = 10; ModelName = ColorModel.Color10; break;
                case ICCReader.ColorSpaceType.Color11: CC = 11; ModelName = ColorModel.Color11; break;
                case ICCReader.ColorSpaceType.Color12: CC = 12; ModelName = ColorModel.Color12; break;
                case ICCReader.ColorSpaceType.Color13: CC = 13; ModelName = ColorModel.Color13; break;
                case ICCReader.ColorSpaceType.Color14: CC = 14; ModelName = ColorModel.Color14; break;
                case ICCReader.ColorSpaceType.Color15: CC = 15; ModelName = ColorModel.Color15; break;

                default: throw new ArgumentException("Wrong colorspace");
            }
        }

        #endregion
    }

    /// <summary>
    /// Gray Color
    /// </summary>
    public sealed class ColorGray : Color
    {
        /// <summary>
        /// Gray: 0.0 to 1.0
        /// </summary>
        public double G
        {
            get { return (ColorValues[0] > Gmax) ? Gmax : ((ColorValues[0] < Gmin) ? Gmin : ColorValues[0]); }
            set { ColorValues[0] = value; }
        }

        /// <summary>
        /// Minimum value for G
        /// </summary>
        public const double Gmin = 0;
        /// <summary>
        /// Maximum value for G
        /// </summary>
        public const double Gmax = 1;

        /// <summary>
        /// The name of this color
        /// </summary>
        public override ColorModel Model { get { return ColorModel.Gray; } }
        /// <summary>
        /// The number of channels this color has
        /// </summary>
        public override byte ChannelCount { get { return 1; } }
        /// <summary>
        /// All color components in an array
        /// </summary>
        public override double[] ColorArray { get { return new double[] { ColorValues[0] }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        public ColorGray()
            : this(ColorConverter.ReferenceWhite, 0)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="G">The value of the gray</param>
        public ColorGray(double G)
            : this(ColorConverter.ReferenceWhite, G)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorGray(Whitepoint ReferenceWhite)
            : this(ReferenceWhite, 0)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="G">The value of the gray</param>
        /// <param name="ReferenceWhite">The reference white</param>
        public ColorGray(Whitepoint ReferenceWhite, double G)
            : base()
        {
            this.G = G;
            this.wp = ReferenceWhite;
        }


        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        /// <param name="G">The value of the gray</param>
        public ColorGray(ICC profile, double G)
            : base(profile)
        {
            this.G = G;
        }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="profile">The ICC profile for this color</param>
        public ColorGray(ICC profile)
            : base(profile)
        { }

        #endregion
    }
}
