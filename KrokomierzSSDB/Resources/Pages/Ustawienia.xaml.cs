using KrokomierzSSDB.Resources.Databases;
using Microsoft.Maui.Controls;
using SQLite;
using System;

namespace KrokomierzSSDB
{
    public partial class Ustawienia : ContentPage
    {
        private readonly SQLiteAsyncConnection _connection;

        public Ustawienia(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            InitializeComponent();
        }

        private async void UpdateChallengeSteps(object sender, EventArgs e)
        {
            int steps;
            if (int.TryParse(stepsEntry.Text, out steps))
            {
                var existingRecord = await _connection.Table<DaneDB>().FirstOrDefaultAsync();

                if (existingRecord != null)
                {
                    existingRecord.celKroki = steps;
                    await _connection.UpdateAsync(existingRecord);
                }
                else
                {
                    var newRecord = new DaneDB { celKroki = steps };
                    await _connection.InsertAsync(newRecord);
                }

                
                MessagingCenter.Send(this, "UpdateChallengeSteps", steps);
            }
        }
    }
}
