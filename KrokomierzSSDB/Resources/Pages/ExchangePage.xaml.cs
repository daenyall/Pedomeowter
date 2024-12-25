using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using KrokomierzSSDB.Resources.Databases;

namespace KrokomierzSSDB.Resources.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExchangePage : ContentPage
    {

        private readonly LocalDbService _dbService;
        
        public ExchangePage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }

        private async void onCloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void onExchangeButtonClicked(object sender, EventArgs e)
        {
            int actualCurrency = await _dbService.GetCurrency();
            int insertedAmount = int.Parse(amountLabel.Text);
            int newCurrency;

            if (insertedAmount%160 == 0)
            {
                newCurrency = actualCurrency - insertedAmount;
            }
            else
            {
                newCurrency = actualCurrency - insertedAmount + (insertedAmount%160);
            }
            
            await _dbService.SetCurrencyAsync(newCurrency); //setting new amount of currency


            int amountOfPulls = insertedAmount/160;
            await _dbService.SetPullsAsync(amountOfPulls);


            await Navigation.PopModalAsync();
            //add amount of pulls to db and delete certain amount of currency + close the window
        }

        private void updateInformationLabel(int amount)
        {
            informationLabel.Text = "Consume " + amount + " of currency";
        }
        private void minusButton(object sender, EventArgs e)
        {
            int actualAmount = int.Parse(amountLabel.Text);
                actualAmount -= 10;

            if (actualAmount < 0)
            {
                amountLabel.Text = "0";
            }
            else
            {
                amountLabel.Text = actualAmount.ToString();
            }

            updateInformationLabel(actualAmount);
        }

        private void plusButton(object sender, EventArgs e)
        {
            int actualAmount = int.Parse(amountLabel.Text);
            actualAmount += 10;
            amountLabel.Text = actualAmount.ToString();

            updateInformationLabel(actualAmount);
        }
        private void minusHundredButton(object sender, EventArgs e)
        {
            int actualAmount = int.Parse(amountLabel.Text);
            actualAmount -= 100;
            if(actualAmount < 0)
            {
                amountLabel.Text = "0";
            }
            else
            {
                amountLabel.Text = actualAmount.ToString();
            }

            updateInformationLabel(actualAmount);

        }

        private void plusHundredButton(object sender, EventArgs e)
        {
            int actualAmount = int.Parse(amountLabel.Text);
            actualAmount += 100;
            amountLabel.Text = actualAmount.ToString();

            updateInformationLabel(actualAmount);
        }

        private async void maxButton(object sender, EventArgs e)
        {
            int currencyAvailable = await _dbService.GetCurrency();
            amountLabel.Text = currencyAvailable.ToString();

            updateInformationLabel(currencyAvailable);
        }


    }
}