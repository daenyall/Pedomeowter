
namespace KrokomierzSSDB
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DateTime timeStarted;
        bool isTimerEnabled = false;
        bool _isCheckingLocation;
        CancellationTokenSource _cancelTokenSource;
        Location oldLocation;
        Location location;
        int height = 180;
        double stepLength;
        double Distance = 0;
        double steps;
        double avgSpeed;


        public MainPage()
        {
            stepLength = height * 0.36;
            InitializeComponent();
        }

        private void startToMeasure(object sender, EventArgs e)
        {

            if (!isTimerEnabled)
            {
                timeStarted = DateTime.Now;
                isTimerEnabled = true;
                Thread thread = new Thread(timer);
                thread.Start();
            }
            else
            {
                isTimerEnabled = false;
            }

        }


        async Task getCurrentLocation()
        {

            if(location != null) { 
            oldLocation = location;
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    _isCheckingLocation = false;
                }
            }
            else
            {
                try
                {
                    _isCheckingLocation = true;

                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                    _cancelTokenSource = new CancellationTokenSource();

                    location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                    oldLocation = location;

                    if (location != null)
                        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    _isCheckingLocation = false;
                }
            }

        }

        double calculateDistance() {
            return Location.CalculateDistance(oldLocation, location, DistanceUnits.Kilometers);
        }

        double calculateSteps() {
            return Distance / stepLength;
        }

        double calculateAvgSpeed(TimeSpan timePassed) {
            return Distance / timePassed.TotalHours;
        }

        void timer() { 
            if(!isTimerEnabled)
            {
                return;
            }

            DateTime time = DateTime.Now;
            TimeSpan timePassed = time - timeStarted;
            Dispatcher.Dispatch(new Action(() => {
                timeLabel.Text = timePassed.ToString();
                getCurrentLocation();
                Distance = calculateDistance();
                distanceLabel.Text = Distance.ToString();
                steps = calculateSteps();
                stepsLabel.Text = steps.ToString();
                avgSpeed = calculateAvgSpeed(timePassed);
                averageSpeedLabel.Text = avgSpeed.ToString();
                //Nie wiem na jakiej zasadzie chcesz liczyc kalorie dlatego tego juz nie dodaje okok
                
            }
                
            )) ;

            Thread.Sleep(1000);
            
            timer();
        }
    }

    //TES


}
