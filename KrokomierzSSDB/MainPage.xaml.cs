
using Microsoft.Maui;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Diagnostics;

namespace KrokomierzSSDB
{
    public partial class MainPage : ContentPage
    {

        private Stopwatch stopwatch = new Stopwatch();
        private IDispatcherTimer timert;
        private int stepsCount = 0;
        private double distance = 0.0;
        private double caloriesBurned = 0.0;
        //private double speed = 0.0;
        private bool isTracking = false;
        private bool isPaused = false; // Dodana flaga do �ledzenia pauzy
        //private IDispatcherTimer locationUpdateTimer;

        private const double StepThreshold = 1.2;
        private const int StepCooldown = 300;
        private const double StepLength = 0.78;
        private const double CaloriesPerMeter = 0.05;
        //private bool _isWaiting = false;

        private DateTime _lastStepTime = DateTime.MinValue;
        private DateTime _startTime = DateTime.MinValue;
        private DateTime _lastUpdateTime = DateTime.MinValue;


        //Moje zmienne ogolem nie oszusta


        int count = 0;
        //DateTime timeStarted;
        bool isTimerEnabled = false;
        bool _isCheckingLocation;
        //CancellationTokenSource _cancelTokenSource;
       // Location oldLocation;
        //Location location;
        int height = 180;
        double stepLength;
        double Distance = 0;
        double steps;
        double avgSpeed;






        private readonly LocalDbService _dbService;

        public MainPage(LocalDbService dbService)
        {
            stepLength = height * 0.36;
            InitializeComponent();
            // Inicjalizacja timera
            timert = Dispatcher.CreateTimer();
            timert.Interval = TimeSpan.FromMilliseconds(100);
            timert.Tick += OnTimerTick;

            RequestPermissions();

            _dbService = dbService;
        }

        private void startToMeasure(object sender, EventArgs e)
        {


            if (stopwatch.IsRunning && !isPaused)
            {
                // Zatrzymanie stopera i zmiana statusu na pauzowany
                stopwatch.Stop();
                timert.Stop();
                isPaused = true;
                
            }
            else if (isPaused)
            {
                // Wznawianie liczenia po pauzie
                stopwatch.Start();
                timert.Start();
                isPaused = false;
                
            }
            else
            {
                // Rozpocz�cie nowego treningu
                stopwatch.Restart();
                timert.Start();
                
                isTracking = true;

                _startTime = DateTime.Now;
                

                
                
            }

        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                timeLabel.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                UpdateDistance();
                UpdateCalories();
                UpdateAveragePace();
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
                DisplayAlert("Brak wsparcia", "Tw�j telefon nie wspiera akcelerometru", "OK");
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
                    double paceInMinutesPerKm = (elapsedTime.TotalMinutes / (distance / 1000));

                    int minutes = (int)paceInMinutesPerKm;
                    int seconds = (int)((paceInMinutesPerKm - minutes) * 60);

                    averageSpeedLabel.Text = $"{minutes:D2}:{seconds:D2}";
                }
            }
        }


        private void RequestPermissions()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Permissions.RequestAsync<Permissions.Sensors>();
                StartStepCounter();
            }
        }


           
    }
}
