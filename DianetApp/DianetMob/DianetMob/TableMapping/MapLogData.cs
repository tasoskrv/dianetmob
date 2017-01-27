﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.TableMapping
{
    public class MapLogData
    {
        public int IDUserMeal { get; set; }
        public string MealName { get; set; }
        public int IDCategory { get; set; }
        public double Calories { get; set; }

        public DateTime MealDate { get; set; }
    }
}
