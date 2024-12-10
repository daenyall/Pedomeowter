using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrokomierzSSDB
{
    [Table("Historia")]
    public class DaneDB
    {
        [Column("cel Kroki")]
        public int celKroki { get; set; }
    }
}
