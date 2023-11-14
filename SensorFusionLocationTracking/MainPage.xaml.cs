using System.Runtime.CompilerServices;

namespace SensorFusionLocationTracking
{
	public enum SensorType
	{
		Gyro, Acc
	}
	public partial class MainPage : ContentPage
	{
		private System.Numerics.Vector3 AccData;
		private System.Numerics.Vector3 GyroData;
		private double NorthHeading;
		private Location LocationData;

		private Matrix Orientation = new Matrix();

		private List<Tuple<Vector, long, SensorType>> Log = new List<Tuple<Vector, long, SensorType>>();

		private Vector GravitiyOrientation = new Vector(0, 0, 0, 0);
		private bool GravityOrientationSet = false;
		private bool StartTimeSet = false;
		private long StartTime = -1;

		public MainPage()
		{
			InitializeComponent();

			EnableAccelerometer();
			EnableGyroscope();
			EnableCompass();

			//Vector t = new Vector(0, 1, 0, 0);
			Matrix test = Matrix.GetRotation(new Vector(0,0,0,1));

			//Vector m = test * t;

			//GetGPS(10);
		}
		private void SetOrientation()
		{
			Matrix m = new Matrix();

			long lastTimeAcc = -1;
			long lastTimeGyro = -1;

			for (int i = 0; i < Log.Count; i++)
			{
				Tuple<Vector, long, SensorType> element = Log[i];
				switch (element.Item3)
				{
					case SensorType.Acc:
						long difTimeAcc = element.Item2 - lastTimeAcc;
						if (lastTimeAcc == -1)
							difTimeAcc = element.Item2 - StartTime;


						lastTimeAcc = element.Item2;
						break;
					case SensorType.Gyro:
						long difTimeGyro = element.Item2 - lastTimeGyro;
						if (lastTimeGyro == -1)
							difTimeGyro = element.Item2 - StartTime;

						//anteilig verwenden für acc
						if (difTimeGyro != 0 && element.Item1.Length()>0.1)
						{
							Matrix gyroM = Matrix.GetRotation(element.Item1 * (1.0 / (difTimeGyro / 1000.0)));

							m = gyroM * m;
						}

						lastTimeGyro = element.Item2;
						break;
				}
			}

			Orientation = m;
		}
		private long GetTime()
		{
			return DateTimeOffset.Now.ToUnixTimeMilliseconds();
		}
		private void EnableAccelerometer()
		{
			if (Accelerometer.Default.IsSupported)
			{
				Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
				Accelerometer.Default.Start(SensorSpeed.UI);
			}
			else
			{
				throw new Exception("Accelorometer not available");
			}
		}
		private void EnableGyroscope()
		{
			if (Gyroscope.Default.IsSupported)
			{
				Gyroscope.Default.ReadingChanged += Gyroscope_ReadingChanged;
				Gyroscope.Default.Start(SensorSpeed.UI);
			}
			else
			{
				throw new Exception("Gyroscope not available");
			}
		}

		private void EnableCompass()
		{
			if (Compass.Default.IsSupported)
			{
				Compass.Default.ReadingChanged += Compass_ReadingChanged;
				Compass.Default.Start(SensorSpeed.UI);
			}
			else
			{
				throw new Exception("Compass not available");
			}
		}

		private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
		{
			long time = GetTime();

			if (!StartTimeSet)
			{
				StartTimeSet = true;
				StartTime = time;
			}

			AccData = e.Reading.Acceleration;

			Log.Add(new Tuple<Vector, long, SensorType>(new Vector(AccData.X, AccData.Y, AccData.Z, 0), time, SensorType.Acc));

			ACCx.Text = "X: " + AccData.X.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			ACCy.Text = "Y: " + AccData.Y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			ACCz.Text = "Z: " + AccData.Z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
		}
		private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
		{
			long time = GetTime();

			if (!StartTimeSet)
			{
				StartTimeSet = true;
				StartTime = time;
			}

			GyroData = e.Reading.AngularVelocity;

			Log.Add(new Tuple<Vector, long, SensorType>(new Vector(GyroData.X, GyroData.Y, GyroData.Z, 0), time, SensorType.Gyro));

			GyroX.Text = "X: " + GyroData.X.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			GyroY.Text = "Y: " + GyroData.Y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			GyroZ.Text = "Z: " + GyroData.Z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);


			//test
			SetOrientation();
			Info1.Text = Orientation.ToString();

			Vector xAxis = (Orientation*(new Vector(1,0,0,0))).Normalize();
			Vector yAxis = (Orientation * (new Vector(0, 1, 0, 0))).Normalize();
			Vector zAxis = (Orientation * (new Vector(0, 0, 1, 0))).Normalize();

			Info2.Text = xAxis.ToString() + "\n" + yAxis.ToString()+ "\n" + zAxis.ToString();

			Info3.Text = (xAxis * yAxis) + "    " + (xAxis * zAxis);

		}
		private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
		{
			NorthHeading = e.Reading.HeadingMagneticNorth;

			CompHeading.Text = "North-Heading: " + NorthHeading.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "°";
		}
		private async void OnResetClick(object sender, EventArgs e)
		{
			StartTimeSet = false;
			Log.Clear();
		}
		
		private async void OnGPSclick(object sender, EventArgs e)
		{
			double time = double.Parse(GPStime.Text, System.Globalization.CultureInfo.InvariantCulture);
			GetGPS(time);

			if (LocationData == null)
			{
				GPSlatitude.Text = "NON";
				GPSlongitude.Text = "NON";
				GPSaltitude.Text = "NON";

				GPSacc.Text = "NON";
				GPSVacc.Text = "NON";
			}
			else
			{
				GPSlatitude.Text = "Latitude: " + LocationData.Latitude.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "°";
				GPSlongitude.Text = "Longitude: " + LocationData.Longitude.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "°";
				GPSaltitude.Text = "Altitude: " + LocationData.Altitude == null ? "Non" : LocationData.Altitude.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "m";

				GPSacc.Text = "Accuracy: " + LocationData.Accuracy == null ? "Non" : LocationData.Accuracy.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "m";
				GPSVacc.Text = "VerticalAccuracy: " + LocationData.VerticalAccuracy == null ? "Non" : LocationData.VerticalAccuracy.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "m";
			}
		}
		private async void GetGPS(double timeS)
		{
			LocationData = null;
			try
			{
				GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(timeS));
				Location location = await Geolocation.Default.GetLocationAsync(request);

				LocationData = location;
			}
			catch (Exception ex)
			{
				throw new Exception("No GPS available");
			}
		}
	}
}