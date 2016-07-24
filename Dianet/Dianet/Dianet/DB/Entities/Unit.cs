using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Unit
    {
        [PrimaryKey]
        public int IDUnit { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

    }
}
