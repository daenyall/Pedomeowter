using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrokomierzSSDB.Resources.Databases
{
    [Table("Historia")]
    public class HistoriaDB
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("kroki")]
        public int kroki { get; set; }
        [Column("data")]
        public DateTime data { get; set; }
    }
}
