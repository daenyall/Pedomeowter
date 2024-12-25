using SQLite;

namespace KrokomierzSSDB.Resources.Databases
{
    public class DaneDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int celKroki { get; set; }
        public DateTime Data { get; set; } = DateTime.Now.Date;
        public int Currency { get; set; }
        public int Pulls { get; set; }
    }
}