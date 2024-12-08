using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _connection.CreateTableAsync<HistoriaDB>(); //dodajemy tabele historia
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
    }
}
