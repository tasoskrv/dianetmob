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
        [PrimaryKey, AutoIncrement]
        public int IDUserMeal { get; set; }

        [ForeignKey(typeof(User))]
        public int IDUser { get; set; }

        [ForeignKey(typeof(MealUnit))]
        public int IDMealUnit { get; set; }

        public int IDServer { get; set; }

        public int IDCategory { get; set; }
        
        public double Qty { get; set; }

        public DateTime MealDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public UserMeal()
        {
            IDServer = -1;
        }

    }
}
