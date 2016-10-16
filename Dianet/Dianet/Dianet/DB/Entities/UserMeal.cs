using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.DB.Entities
{
    public class UserMeal: Model
    {
        [Indexed(Name = "IDUserMeal_PK", Order = 1)]
        public int IDUserMeal { get; set; }

        [Indexed(Name = "IDUserMeal_IDUser_PK", Order = 2), ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [ForeignKey(typeof(MealUnit))]
        public int IDMealUnit { get; set; }

        public int IDCategory { get; set; }
        
        public double Qty { get; set; }

        public DateTime MealDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
