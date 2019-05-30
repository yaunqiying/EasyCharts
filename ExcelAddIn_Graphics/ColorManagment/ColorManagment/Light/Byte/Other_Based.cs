
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

namespace ColorManagment.Light
{
    /// <summary>
    /// Gray Color
    /// </summary>
    public sealed class BColorGray : BColor
    {
        /// <summary>
        /// Gray: 0 to 255
        /// </summary>
        public byte G
        {
            get { return ColorValues[0]; }
            set { ColorValues[0] = value; }
        }

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
        public override byte[] ColorArray { get { return new byte[] { ColorValues[0] }; } }
        /// <summary>
        /// All color components in a double array
        /// </summary>
        public override double[] DoubleColorArray { get { return new double[] { ColorValues[0] / 255d }; } }

        #region Constructor

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        public BColorGray()
            : this(ColorConverter.ReferenceWhite.Name, 0)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="G">The value of the gray (0 - 255)</param>
        public BColorGray(byte G)
            : this(ColorConverter.ReferenceWhite.Name, G)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorGray(WhitepointName ReferenceWhite)
            : this(ReferenceWhite, 0)
        { }

        /// <summary>
        /// Creates a new instance of a gray Color
        /// </summary>
        /// <param name="G">The value of the gray (0 - 255)</param>
        /// <param name="ReferenceWhite">The reference white</param>
        public BColorGray(WhitepointName ReferenceWhite, byte G)
            : base()
        {
            this.G = G;
            this.wp = ReferenceWhite;
        }
        
        #endregion
    }
}
