using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using KrokomierzSSDB.Resources.Databases;

namespace KrokomierzSSDB
{
    public partial class Statystyki : ContentPage
    {
        private readonly LocalDbService _dbService;

        // Lista, kt�ra b�dzie trzyma�a dane do wykresu
        public List<StepsData> StepsData { get; set; }

        public Statystyki(LocalDbService dbService)
        {
            _dbService = dbService;
            InitializeComponent();
            LoadData(); // Za�aduj dane z bazy przy inicjalizacji
        }

        private async void LoadData()
        {
            // Pobierz dane z bazy danych
            var historyData = await _dbService.GetHistorias();

            // Zmie� dane na format odpowiedni do wykresu
            StepsData = historyData.Select(x => new StepsData
            {
                Date = x.data.ToString("dd-MM-yyyy"), // Przekszta�cenie daty na string
                Steps = x.kroki
            }).ToList();

            // Ustawienie �r�d�a danych wykresu
            BindingContext = this;
        }
    }

    // Klasa pomocnicza do przechowywania danych wykresu
    public class StepsData
    {
        public string Date { get; set; } // Data w formacie string
        public int Steps { get; set; }   // Ilo�� krok�w
    }
}
