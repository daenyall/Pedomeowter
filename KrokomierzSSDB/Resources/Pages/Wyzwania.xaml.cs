using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrokomierzSSDB.Resources.Databases;


namespace KrokomierzSSDB
{
    public partial class Wyzwania : ContentPage
    {
        private readonly LocalDbService _dbService;
        private List<Wyzwanie> _wyzwania;

        public Wyzwania()
        {
            InitializeComponent();
            _dbService = new LocalDbService();
            InitializeChallenges();
        }

        private async void InitializeChallenges()
        {
            
            _wyzwania = new List<Wyzwanie>
            {
                new Wyzwanie { Nazwa = "Przejd� 1000 krok�w", Cel = 1000},
                new Wyzwanie { Nazwa = "Przejd� 5000 krok�w", Cel = 5000 },
                new Wyzwanie { Nazwa = "Przejd� 10000 krok�w", Cel = 10000 },
                new Wyzwanie { Nazwa = "Przejd� 50000 krok�w", Cel = 50000 },
                new Wyzwanie { Nazwa = "Przejd� 100000 krok�w", Cel = 100000 }
            };

            
            int totalSteps = await _dbService.GetTotalStepsAsync();

           
            foreach (var wyzwanie in _wyzwania)
            {
                wyzwanie.CzyUkonczone = totalSteps >= wyzwanie.Cel;
                wyzwanie.Postep = $"{totalSteps}/{wyzwanie.Cel} krok�w";
                if (totalSteps >= wyzwanie.Cel)
                {
                    wyzwanie.CzyUkonczone = true;
                }
              

                
                ChallengesListView.ItemsSource = _wyzwania;
        }
    }

    public class Wyzwanie
    {
        public string Nazwa { get; set; }
        public int Cel { get; set; }
        public bool CzyUkonczone { get; set; }
        public string Postep { get; set; }
        }
}}
