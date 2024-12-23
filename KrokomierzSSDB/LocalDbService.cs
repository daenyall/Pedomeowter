using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KrokomierzSSDB
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        private const int DefaultStepGoal = 5000; // Domyślny cel kroków

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<HistoriaDB>();
            _connection.CreateTableAsync<DaneDB>();
        }

        public async Task<List<HistoriaDB>> GetHistorias()
        {
            return await _connection.Table<HistoriaDB>().ToListAsync();
        }

        public async Task<HistoriaDB> GetById(int id)
        {
            return await _connection.Table<HistoriaDB>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(HistoriaDB historiaDB)
        {
            await _connection.InsertAsync(historiaDB);
        }

        public async Task Update(HistoriaDB historiaDB)
        {
            await _connection.UpdateAsync(historiaDB);
        }

        public async Task Delete(HistoriaDB historiaDB)
        {
            await _connection.DeleteAsync(historiaDB);
        }

        public async Task AddOrUpdateDailySteps(int kroki, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1);

            var existing = await _connection.Table<HistoriaDB>()
                                             .Where(x => x.data >= startOfDay && x.data < endOfDay)
                                             .FirstOrDefaultAsync();

            if (existing != null)
            {
                existing.kroki += kroki;
                await Update(existing);
            }
            else
            {
                var newEntry = new HistoriaDB { kroki = kroki, data = date };
                await Create(newEntry);
            }
        }

   

        public async Task UpdateChallengeSteps(int przekazaneKroki)
        {
            var doesExist = await _connection.Table<DaneDB>().FirstOrDefaultAsync();

            if (doesExist != null)
            {
                doesExist.celKroki = przekazaneKroki;
                await Update(doesExist);
            }
            else
            {
                var daneEntry = new DaneDB { celKroki = przekazaneKroki };
                await Create(daneEntry);
            }
        }

        private async Task Create(DaneDB daneDB)
        {
            await _connection.InsertAsync(daneDB);
        }

        private async Task Update(DaneDB daneDB)
        {
            await _connection.UpdateAsync(daneDB);
        }

        public async Task<int> GetChallengeSteps()
        {
            var existingRecord = await _connection.Table<DaneDB>().FirstOrDefaultAsync();
            return existingRecord?.celKroki ?? DefaultStepGoal;
        }
        public async Task<int> GetTotalStepsAsync()
        {
            var allRecords = await _connection.Table<HistoriaDB>().ToListAsync();

            // Sumowanie wartości 'kroki'
            int totalSteps = allRecords.Sum(record => record.kroki);

            return totalSteps;
        }

        public async Task<int> GetCurrency()
        {
            var existingRecord = await _connection.Table<DaneDB>().FirstOrDefaultAsync();
            return existingRecord?.Currency ?? 0;
        }



    }
}
