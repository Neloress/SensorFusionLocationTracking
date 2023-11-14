using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SensorFusionLocationTracking
{
	internal class Matrix
	{
		private double[,] V = new double[4, 4];
		internal Matrix()
		{
			V[0, 0] = 1;
			V[1, 1] = 1;
			V[2, 2] = 1;
			V[3, 3] = 1;
		}
		internal string ToString()
		{
			string s = V[0, 0].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[1, 0].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[2, 0].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[3, 0].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			s += "\n";
			s+= V[0, 1].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[1, 1].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[2, 1].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[3, 1].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			s += "\n";
			s += V[0, 2].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[1, 2].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[2, 2].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[3, 2].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			s += "\n";
			s += V[0, 3].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[1, 3].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[2, 3].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + V[3, 3].ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

			return s;
		}
		internal static Matrix GetRotation(Vector v)
		{
			//https://www.hindawi.com/journals/js/2018/9684326/

			if (v.Length() == 0)
				return new Matrix();

			return GetRotationAroundAxis(v.Length(),v.Normalize());
		}
		internal static Matrix GetRotationX(double rad)
		{
			Matrix m = new Matrix();
			m.V[1, 1] = Math.Cos(rad);
			m.V[2, 1] = -Math.Sin(rad);
			m.V[1, 2] = Math.Sin(rad);
			m.V[2, 2] = Math.Cos(rad);

			return m;
		}
		internal static Matrix GetRotationY(double rad)
		{
			Matrix m = new Matrix();
			m.V[0, 0] = Math.Cos(rad);
			m.V[2, 0] = Math.Sin(rad);
			m.V[0, 2] = -Math.Sin(rad);
			m.V[2, 2] = Math.Cos(rad);

			return m;
		}
		internal static Matrix GetRotationZ(double rad)
		{
			Matrix m = new Matrix();
			m.V[0, 0] = Math.Cos(rad);
			m.V[1, 0] = -Math.Sin(rad);
			m.V[0, 1] = Math.Sin(rad);
			m.V[1, 1] = Math.Cos(rad);

			return m;
		}
		internal static Matrix GetRotationAroundAxis(double rad, Vector vector)
		{
			Vector v = vector.Normalize();

			Matrix m = new Matrix();

			double cos = Math.Cos(rad);
			double oneMcos = 1.0 - cos;
			double sin = Math.Sin(rad);

			m.V[0, 0] = v.X * v.X * (oneMcos) + cos;
			m.V[1, 0] = v.X * v.Y * (oneMcos) - v.Z * sin;
			m.V[2, 0] = v.X * v.Z * (oneMcos) + v.Y * sin;
			m.V[0, 1] = v.Y * v.X * (oneMcos) + v.Z * sin;
			m.V[1, 1] = v.Y * v.Y * (oneMcos) + cos;
			m.V[2, 1] = v.Y * v.Z * (oneMcos) - v.X * sin;
			m.V[0, 2] = v.Z * v.X * (oneMcos) - v.Y * sin;
			m.V[1, 2] = v.Z * v.Y * (oneMcos) + v.X * sin;
			m.V[2, 2] = v.Z * v.Z * (oneMcos) + cos;

			return m;
		}
		internal static Matrix GetTranslation(Vector vector)
		{
			Matrix m = new Matrix();
			m.V[3, 0] = vector.X;
			m.V[3, 1] = vector.Y;
			m.V[3, 2] = vector.Z;

			return m;
		}


		public static Matrix operator *(Matrix a, Matrix b)
		{
			Matrix m = new Matrix();
			m.V[0, 0] = a.V[0, 0] * b.V[0, 0] + a.V[1, 0] * b.V[0, 1] + a.V[2, 0] * b.V[0, 2] + a.V[3, 0] * b.V[0, 3];
			m.V[1, 0] = a.V[0, 0] * b.V[1, 0] + a.V[1, 0] * b.V[1, 1] + a.V[2, 0] * b.V[1, 2] + a.V[3, 0] * b.V[1, 3];
			m.V[2, 0] = a.V[0, 0] * b.V[2, 0] + a.V[1, 0] * b.V[2, 1] + a.V[2, 0] * b.V[2, 2] + a.V[3, 0] * b.V[2, 3];
			m.V[3, 0] = a.V[0, 0] * b.V[3, 0] + a.V[1, 0] * b.V[3, 1] + a.V[2, 0] * b.V[3, 2] + a.V[3, 0] * b.V[3, 3];

			m.V[0, 1] = a.V[0, 1] * b.V[0, 0] + a.V[1, 1] * b.V[0, 1] + a.V[2, 1] * b.V[0, 2] + a.V[3, 1] * b.V[0, 3];
			m.V[1, 1] = a.V[0, 1] * b.V[1, 0] + a.V[1, 1] * b.V[1, 1] + a.V[2, 1] * b.V[1, 2] + a.V[3, 1] * b.V[1, 3];
			m.V[2, 1] = a.V[0, 1] * b.V[2, 0] + a.V[1, 1] * b.V[2, 1] + a.V[2, 1] * b.V[2, 2] + a.V[3, 1] * b.V[2, 3];
			m.V[3, 1] = a.V[0, 1] * b.V[3, 0] + a.V[1, 1] * b.V[3, 1] + a.V[2, 1] * b.V[3, 2] + a.V[3, 1] * b.V[3, 3];

			m.V[0, 2] = a.V[0, 2] * b.V[0, 0] + a.V[1, 2] * b.V[0, 1] + a.V[2, 2] * b.V[0, 2] + a.V[3, 2] * b.V[0, 3];
			m.V[1, 2] = a.V[0, 2] * b.V[1, 0] + a.V[1, 2] * b.V[1, 1] + a.V[2, 2] * b.V[1, 2] + a.V[3, 2] * b.V[1, 3];
			m.V[2, 2] = a.V[0, 2] * b.V[2, 0] + a.V[1, 2] * b.V[2, 1] + a.V[2, 2] * b.V[2, 2] + a.V[3, 2] * b.V[2, 3];
			m.V[3, 2] = a.V[0, 2] * b.V[3, 0] + a.V[1, 2] * b.V[3, 1] + a.V[2, 2] * b.V[3, 2] + a.V[3, 2] * b.V[3, 3];

			m.V[0, 3] = a.V[0, 3] * b.V[0, 0] + a.V[1, 3] * b.V[0, 1] + a.V[2, 3] * b.V[0, 2] + a.V[3, 3] * b.V[0, 3];
			m.V[1, 3] = a.V[0, 3] * b.V[1, 0] + a.V[1, 3] * b.V[1, 1] + a.V[2, 3] * b.V[1, 2] + a.V[3, 3] * b.V[1, 3];
			m.V[2, 3] = a.V[0, 3] * b.V[2, 0] + a.V[1, 3] * b.V[2, 1] + a.V[2, 3] * b.V[2, 2] + a.V[3, 3] * b.V[2, 3];
			m.V[3, 3] = a.V[0, 3] * b.V[3, 0] + a.V[1, 3] * b.V[3, 1] + a.V[2, 3] * b.V[3, 2] + a.V[3, 3] * b.V[3, 3];

			return m;
		}

		public static Vector operator *(Matrix m, Vector v)
		{
			double x = m.V[0, 0] * v.X + m.V[1, 0] * v.Y + m.V[2, 0] * v.Z + m.V[3, 0] * v.W;
			double y = m.V[0, 1] * v.X + m.V[1, 1] * v.Y + m.V[2, 1] * v.Z + m.V[3, 1] * v.W;
			double z = m.V[0, 2] * v.X + m.V[1, 2] * v.Y + m.V[2, 2] * v.Z + m.V[3, 2] * v.W;
			double w = m.V[0, 3] * v.X + m.V[1, 3] * v.Y + m.V[2, 3] * v.Z + m.V[3, 3] * v.W;
			return new Vector(x, y, z, w);

		}
	}
}
