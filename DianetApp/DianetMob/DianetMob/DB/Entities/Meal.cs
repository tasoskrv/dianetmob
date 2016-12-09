using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class Meal : Model
    {
        [PrimaryKey]
        public int IDMeal { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(800)]
        public string Description { get; set; }

        public int IDLang { get; set; }

        [MaxLength(20)]
        public string Fertility { get; set; }

        public string Identifier { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
