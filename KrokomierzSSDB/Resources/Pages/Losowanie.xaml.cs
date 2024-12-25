namespace KrokomierzSSDB;

public partial class Losowanie : ContentPage
{
    private int gwarantowanyLegendarny = 80;
    private int gwarantowanyEpicki = 10;
    private int szansa = 1;
    private string rzadkosc;
    Random random = new Random();
    public Losowanie()
    {
        InitializeComponent();
    }

    private void Button_1X_Clicked(object sender, EventArgs e)
    {
        gwarantowanyLegendarny--;
        gwarantowanyEpicki--;
        Random rnd = new Random();
        int randomLegendarny = rnd.Next(1, gwarantowanyLegendarny + 1);
        int randomEpicki = rnd.Next(1, gwarantowanyEpicki + 1); 
        TestLiczby.Text = "Ile do legendarnego" + gwarantowanyLegendarny.ToString() + " Ile do epickiego" + gwarantowanyEpicki.ToString();
        if (randomLegendarny == gwarantowanyLegendarny)
        {
            TestRzadkosc.Text = "Legendarny" + randomLegendarny.ToString() + " "+  randomEpicki.ToString();
            gwarantowanyLegendarny = 80;
        }
        else if (randomLegendarny != gwarantowanyLegendarny && randomEpicki != gwarantowanyEpicki)
        {
            TestRzadkosc.Text = "Zwyk³y" + randomLegendarny.ToString() + " "+ randomEpicki.ToString();
        }
        else if (randomLegendarny != gwarantowanyLegendarny && randomEpicki == gwarantowanyEpicki)
        {
            TestRzadkosc.Text = "Epicki" + randomLegendarny.ToString() +" "+ randomEpicki.ToString();
            gwarantowanyEpicki = 10;
        }
    }
}