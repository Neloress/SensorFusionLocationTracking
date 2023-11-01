namespace SensorFusionLocationTracking
{
	public partial class MainPage : ContentPage
	{
		private System.Numerics.Vector3 AccData;
		private System.Numerics.Vector3 GyroData;
		private double NorthHeading;
		private Location LocationData;

		public MainPage()
		{
			InitializeComponent();

			EnableAccelerometer();
			EnableGyroscope();
			EnableCompass();

			GetGPS(10);
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
			AccData = e.Reading.Acceleration;

			ACCx.Text = "X: " + AccData.X.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
			ACCy.Text = "Y: " + AccData.Y.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
			ACCz.Text = "Z: " + AccData.Z.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
		}
		private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
		{
			GyroData = e.Reading.AngularVelocity;

			GyroX.Text = "X: " + GyroData.X.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
			GyroY.Text = "Y: " + GyroData.Y.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
			GyroZ.Text = "Z: " + GyroData.Z.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
		}
		private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
		{
			NorthHeading = e.Reading.HeadingMagneticNorth;

			CompHeading.Text = "North-Heading: " + NorthHeading.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "°";
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
				GPSlatitude.Text = "Latitude: "+ LocationData.Latitude.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)+ "°";
				GPSlongitude.Text = "Longitude: " + LocationData.Longitude.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "°";
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