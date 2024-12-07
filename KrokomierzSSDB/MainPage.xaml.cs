namespace KrokomierzSSDB
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DateTime timeStarted;
        bool isTimerEnabled = false;


        public MainPage()
        {
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

        void timer() { 
            if(!isTimerEnabled)
            {
                return;
            }

            DateTime time = DateTime.Now;
            TimeSpan timePassed = time - timeStarted;
            Dispatcher.Dispatch(new Action(() => {
                timeLabel.Text = timePassed.ToString();
            }
                
            )) ;

            Thread.Sleep(10);
            timer();
        }
    }

}
