namespace SensorFusionLocationTracking
{
	public partial class MainPage : ContentPage
	{
		private System.Numerics.Vector3 AccData;
		private double NorthHeading;

		public MainPage()
		{
			InitializeComponent();

			EnableAccelerometer();
			EnableCompass();
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
		private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
		{
			NorthHeading = e.Reading.HeadingMagneticNorth;

			CompHeading.Text = "North-Heading: " + NorthHeading.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "°";
		}
		private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
		{
			AccData = e.Reading.Acceleration;

			ACCx.Text = "X: " + AccData.X.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
			ACCy.Text = "Y: " + AccData.Y.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
			ACCz.Text = "Z: " + AccData.Z.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}