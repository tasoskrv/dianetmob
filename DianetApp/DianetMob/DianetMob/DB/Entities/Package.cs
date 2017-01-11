using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Package : Model
    {
        [PrimaryKey]
        public int IDPackage { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }

        public bool IsActive { get; set; }

        public int IDLang { get; set; }

        public string GooglePlay { get; set; }

        public string AppleStore { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
