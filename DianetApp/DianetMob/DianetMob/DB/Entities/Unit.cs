using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Unit : Model
    {
        [PrimaryKey]
        public int IDUnit { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
