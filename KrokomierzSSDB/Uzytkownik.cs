using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrokomierzSSDB
{
    [Table("uzytkownik")]
    public class Uzytkownik
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("gwarantownyLegendarny")]
        public int gwarantowanyLegendarny { get; set; }
        [Column("gwarantownyEpicki")]
        public int gwarantowanyEpicki { get; set; }
    }
}
