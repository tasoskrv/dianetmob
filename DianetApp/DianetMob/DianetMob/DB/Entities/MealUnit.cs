﻿using SQLite;
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
            Deleted = 0;
        }

        

    }
}
