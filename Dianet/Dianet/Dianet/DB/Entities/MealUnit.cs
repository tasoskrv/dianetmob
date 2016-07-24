using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class MealUnit
    {
        [PrimaryKey]
        public int IDMealUnit { get; set; }

        [ForeignKey(typeof(Unit))]
        public int IDUnit { get; set; }

        [ForeignKey(typeof(Meal))]
        public int IDMeal { get; set; }

        public double Calories { get; set; }

        public double Protein { get; set; }

        public double Carb { get; set; }

        public double Fat { get; set; }

    }

}
}
