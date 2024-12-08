using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KrokomierzSSDB
{
    public partial class MainPage : ContentPage
    {
        private Stopwatch stopwatch = new Stopwatch();
        private IDispatcherTimer timer;
        private int stepsCount = 0;
        private double distance = 0.0;
        private double caloriesBurned = 0.0;
        private bool isTracking = false;
        private bool isPaused = false;
        private const double StepThreshold = 1.2;
        private const int StepCooldown = 300; // 300 ms cooldown between steps
        private const double StepLength = 0.78; // Length of each step in meters
        private const double CaloriesPerMeter = 0.05; // Calories per meter

        private DateTime _lastStepTime = DateTime.MinValue;
        private DateTime _startTime = DateTime.MinValue;
        private DateTime _lastUpdateTime = DateTime.MinValue;

        private readonly LocalDbService _dbService;
        private double stepLength;
        private int height = 180; // Your height (cm)

        public MainPage(LocalDbService dbService)
        {
            stepLength = height * 0.36; // Step length based on height
            InitializeComponent();
            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += OnTimerTick;
            _dbService = dbService;

            // Request permissions when app starts
            RequestPermissionsAsync().ConfigureAwait(false);
        }

        private async Task RequestPermissionsAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Sensors>();
            if (status != PermissionStatus.Granted)
            {
                DisplayAlert("Permission Denied", "You need to grant permission to use the accelerometer.", "OK");
            }
            else
            {
                StartStepCounter();
            }
        }

        private void StartStepCounter()
        {
            if (Accelerometer.IsSupported && !Accelerometer.IsMonitoring)
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.UI);
            }
            else
            {
                DisplayAlert("Brak wsparcia", "Twój telefon nie wspiera akcelerometru", "OK");
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (isTracking && !isPaused)
            {
                var reading = e.Reading;
                var totalAcceleration = Math.Sqrt(
                    Math.Pow(reading.Acceleration.X, 2) +
                    Math.Pow(reading.Acceleration.Y, 2) +
                    Math.Pow(reading.Acceleration.Z, 2));

                if (totalAcceleration > StepThreshold)
                {
                    if ((DateTime.Now - _lastStepTime).TotalMilliseconds > StepCooldown)
                    {
                        stepsCount++;
                        stepsLabel.Text = stepsCount.ToString();
                        _lastStepTime = DateTime.Now;

                        UpdateDistance();
                        UpdateCalories();
                    }
                }
            }
        }

        private void UpdateDistance()
        {
            distance = stepsCount * StepLength;
            distanceLabel.Text = $"{distance / 1000:F2} km";
        }

        private void UpdateCalories()
        {
            caloriesBurned = distance * CaloriesPerMeter;
            caloriesLabel.Text = $"{caloriesBurned:F2} kcal";
        }

        private void UpdateAveragePace()
        {
            if (distance > 0 && _startTime != DateTime.MinValue)
            {
                if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= 3)
                {
                    _lastUpdateTime = DateTime.Now;
                    TimeSpan elapsedTime = DateTime.Now - _startTime;

                    // Obliczamy prędkość w km/h (dystans w km / czas w godzinach)
                    double speedInKmh = (distance / 1000) / elapsedTime.TotalHours;

                    // Zaokrąglamy do jednej liczby po przecinku
                    averageSpeedLabel.Text = $"{speedInKmh:F1} km/h";
                }
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                timeLabel.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                UpdateAveragePace();
            }
        }

        private void StartStopwatch()
        {
            if (stopwatch.IsRunning && !isPaused)
            {
                stopwatch.Stop();
                timer.Stop();
                Accelerometer.Stop(); // Stop accelerometer when paused
                isPaused = true;
            }
            else if (isPaused)
            {
                stopwatch.Start();
                timer.Start();
                Accelerometer.Start(SensorSpeed.UI); // Start accelerometer when resumed
                isPaused = false;
            }
            else
            {
                stopwatch.Restart();
                timer.Start();
                StartStepCounter();
                isTracking = true;
                _startTime = DateTime.Now;
            }
        }

        // Event handler for the start/stop button
        private void startToMeasure(object sender, EventArgs e)
        {
            StartStopwatch();
        }
    }
}
