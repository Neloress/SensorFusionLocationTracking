using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorFusionLocationTracking
{
	internal class Vector
	{
		internal double X;
		internal double Y;
		internal double Z;
		internal double W;
		internal Vector(double x, double y, double z, double w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		internal string ToString()
		{
			return X.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + Y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "|" + Z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
		}
		internal Vector Normalize()
		{
			double l = this.Length();
			return new Vector(X/l, Y/l, Z/l, W);
		}
		internal double Length()
		{
			return Math.Sqrt(X*X+Y*Y+Z*Z);
		}
		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z, 1);
		}
		public static Vector operator *(Vector a, double b)
		{
			return new Vector(a.X * b, a.Y * b, a.Z * b, a.W);
		}
		public static double operator *(Vector a, Vector b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}
	}
}
