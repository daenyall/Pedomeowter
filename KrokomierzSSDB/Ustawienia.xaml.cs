using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Syncfusion.Maui.Charts;



namespace KrokomierzSSDB
{

    public partial class Ustawienia : ContentPage
    {
        private readonly LocalDbService _dbService;
        public Ustawienia(LocalDbService dbService)
        {
            _dbService = dbService;
            InitializeComponent();
        }

        private void UpdateChallengeSteps(object sender, EventArgs e)
        {
            
            _dbService.UpdateChallengeSteps(int.Parse(stepsEntry.Text));
        }
    }
}