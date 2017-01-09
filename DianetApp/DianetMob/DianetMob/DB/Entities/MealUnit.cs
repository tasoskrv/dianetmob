using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.DB.Entities
{
    public class MealUnit : Model
    {
        [PrimaryKey, AutoIncrement]
        public int IDMealUnit { get; set; }

        [ForeignKey(typeof(Unit))]
        public int IDUnit { get; set; }

        [ForeignKey(typeof(Meal))]
        public int IDMeal { get; set; }

        public int IDUser { get; set; }

        [Ignore]
        public int IDServerMeal { get; set; }

        public int IDServer { get; set; }

        public double Calories { get; set; }

        public double Protein { get; set; }

        public double Carb { get; set; }

        public double Fat { get; set; }

        public double SatFat { get; set; }

        public double UnSatFat { get; set; }

        public double Cholesterol { get; set; }

        public double Sugar { get; set; }

        public double Natrium { get; set; }

        public double Potassium { get; set; }

        public double Fiber { get; set; }

        public int Deleted { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public MealUnit()
        {
            IDServer = 0;
            IDUser = 0;
            Deleted = 0;
        }

        public override string ToString()
        {
            string str = "";

            str += "&idserver=" + IDServer.ToString();
            if (IDUnit != -1)
                str += "&idunit=" + IDUnit.ToString();
            if (IDServerMeal != -1)
                str += "&idmeal=" + IDServerMeal.ToString();
            if (IDUser != 0)
                str += "&iduser=" + IDUser.ToString();
            if (Calories != -1)
                str += "&calories=" + Calories.ToString();
            if (Protein != -1)
                str += "&protein=" + Protein.ToString();
            if (Carb != -1)
                str += "&carb=" + Carb.ToString();
            if (Fat != -1)
                str += "&fat=" + Fat.ToString();
            if (SatFat != -1)
                str += "&satfat=" + SatFat.ToString();
            if (UnSatFat != -1)
                str += "&unsatfat=" + UnSatFat.ToString();
            if (Cholesterol != -1)
                str += "&cholesterol=" + Cholesterol.ToString();
            if (Sugar != -1)
                str += "&sugar=" + Sugar.ToString();
            if (Natrium != -1)
                str += "&natrium=" + Natrium.ToString();
            if (Potassium != -1)
                str += "&potassium=" + Potassium.ToString();
            if (Fiber != -1)
                str += "&fiber=" + Fiber.ToString();
            if (Deleted != -1)
                str += "&deleted=" + Deleted.ToString();
            if (InsertDate != null)
                str += "&insertdate=" + InsertDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (UpdateDate != null)
                str += "&updatedate=" + UpdateDate.ToString("yyyy-MM-dd HH:mm:ss");

            return str.Substring(1);
        }


    }
}
