namespace SensorFusionLocationTracking
{
	public partial class MainPage : ContentPage
	{
		int count = 0;

		private AccelerometerData AccData;

		public MainPage()
		{
			InitializeComponent();

			EnableAccelerometer();
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

		public void ToggleAccelerometer()
		{
			if (Accelerometer.Default.IsSupported)
			{
				if (!Accelerometer.Default.IsMonitoring)
				{
					// Turn on accelerometer
					Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
					Accelerometer.Default.Start(SensorSpeed.UI);
				}
				else
				{
					// Turn off accelerometer
					Accelerometer.Default.Stop();
					Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
				}
			}
		}

		private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
		{
			AccData = e.Reading;

			ACCx.Text = "X: " + AccData.Acceleration.X.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
			ACCy.Text = "Y: " + AccData.Acceleration.Y.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
			ACCz.Text = "Z: " + AccData.Acceleration.Z.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}