using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Package
    {
        [PrimaryKey]
        public int IDPackage { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }

        public bool IsActive { get; set; }
    }
}
