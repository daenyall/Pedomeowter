using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KrokomierzSSDB
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<HistoriaDB>(); // Tworzymy tabelę Historia
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
            // Obliczamy zakres dat w kodzie C#
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1);

            // Szukamy wpisu w podanym zakresie
            var existing = await _connection.Table<HistoriaDB>()
                                             .Where(x => x.data >= startOfDay && x.data < endOfDay)
                                             .FirstOrDefaultAsync();

            if (existing != null)
            {
                // Jeśli istnieje, aktualizujemy dane
                existing.kroki += kroki;
                await Update(existing);
            }
            else
            {
                // Jeśli nie istnieje, tworzymy nowy wpis
                var newEntry = new HistoriaDB { kroki = kroki, data = date };
                await Create(newEntry);
            }
        }

        public async Task<HistoriaDB> GetByDate(DateTime date)
        {
            return await _connection.Table<HistoriaDB>()
                .FirstOrDefaultAsync(x => x.data >= date.Date && x.data < date.Date.AddDays(1));
        }
    }
}
