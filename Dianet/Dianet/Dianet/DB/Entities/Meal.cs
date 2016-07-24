using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class Meal
    {
        [PrimaryKey]
        public int IDMeal { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(800)]
        public string Description { get; set; }

        public int IDLang { get; set; }

        [MaxLength(10)]
        public string Fertility { get; set; } 

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
