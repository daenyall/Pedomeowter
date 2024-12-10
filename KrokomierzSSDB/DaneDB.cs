using SQLite;

namespace KrokomierzSSDB
{
    public class DaneDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int celKroki { get; set; }
        public DateTime Data { get; set; } = DateTime.Now.Date;
    }
}