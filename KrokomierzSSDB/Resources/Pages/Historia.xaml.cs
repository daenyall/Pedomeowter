using KrokomierzSSDB.Resources.Databases;

namespace KrokomierzSSDB
{
    public partial class Historia : ContentPage
    {
        private readonly LocalDbService _dbService;

        public Historia(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            LoadDataAsync().ConfigureAwait(false); // £adujemy dane przy starcie strony
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var historiaList = await _dbService.GetHistorias();
                // Ustawiamy ListView na dane z bazy
                HistoriaListView.ItemsSource = historiaList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("B³¹d", $"Nie uda³o siê za³adowaæ danych: {ex.Message}", "OK");
            }
        }
    }
}
