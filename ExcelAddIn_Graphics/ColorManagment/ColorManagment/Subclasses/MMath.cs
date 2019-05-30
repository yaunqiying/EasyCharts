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
    /// A collection of matrix calculations. Static and nonstatic
    /// </summary>
    internal sealed class MMath
    {
        private double determinant;
        private double[] c = new double[3];
        private double[,] output, tmp3x3 = new double[3, 3];

        public double[,] InvertMatrix(double[,] M)
        {
            tmp3x3 = new double[3, 3];
            determinant = 0;
            for (int i = 0; i < 3; i++) { determinant += (M[0, i] * (M[1, (i + 1) % 3] * M[2, (i + 2) % 3] - M[1, (i + 2) % 3] * M[2, (i + 1) % 3])); }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tmp3x3[i, j] = ((M[(i + 1) % 3, (j + 1) % 3] * M[(i + 2) % 3, (j + 2) % 3]) - (M[(i + 1) % 3, (j + 2) % 3] * M[(i + 2) % 3, (j + 1) % 3])) / determinant;
                }
            }
            return tmp3x3;
        }

        public double[] MultiplyMatrix(double[,] a, double[] b)
        {
            c = new double[3];
            c[0] = b[0] * a[0, 0] + b[1] * a[0, 1] + b[2] * a[0, 2];
            c[1] = b[0] * a[1, 0] + b[1] * a[1, 1] + b[2] * a[1, 2];
            c[2] = b[0] * a[2, 0] + b[1] * a[2, 1] + b[2] * a[2, 2];
            return c;
        }

        public double[,] MultiplyMatrix(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) { throw new ArgumentException("Cannot multiply: Size of matrices do not match"); }

            output = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < output.GetLength(0); i++)
            {
                for (int j = 0; j < output.GetLength(1); j++)
                {
                    for (int k = 0; k < a.GetLength(1); k++) { output[i, j] += a[i, k] * b[k, j]; }
                }
            }
            return output;
        }
        
        public static double[,] StaticInvertMatrix(double[,] M)
        {
            double determinant = 0;
            double[,] tmp3x3 = new double[3, 3];
            for (int i = 0; i < 3; i++) { determinant += (M[0, i] * (M[1, (i + 1) % 3] * M[2, (i + 2) % 3] - M[1, (i + 2) % 3] * M[2, (i + 1) % 3])); }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tmp3x3[i, j] = ((M[(i + 1) % 3, (j + 1) % 3] * M[(i + 2) % 3, (j + 2) % 3]) - (M[(i + 1) % 3, (j + 2) % 3] * M[(i + 2) % 3, (j + 1) % 3])) / determinant;
                }
            }
            return tmp3x3;
        }

        public static double[] StaticMultiplyMatrix(double[,] a, double[] b)
        {
            double[] c = new double[3];
            c[0] = b[0] * a[0, 0] + b[1] * a[0, 1] + b[2] * a[0, 2];
            c[1] = b[0] * a[1, 0] + b[1] * a[1, 1] + b[2] * a[1, 2];
            c[2] = b[0] * a[2, 0] + b[1] * a[2, 1] + b[2] * a[2, 2];
            return c;
        }

        public static double[,] StaticMultiplyMatrix(double[,] a, double[,] b)
        {
            double[,] output = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++) { output[i, j] += a[i, k] * b[k, j]; }
                }
            }
            return output;
        }
    }
}
