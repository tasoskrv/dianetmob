using DianetMob.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.Model
{
    public class Points
    {
        private double breakfast;
        private double lunch;
        private double dinner;
        private double snack;
        private double exercise;

        public double Budget
        {
            get
            {
                return PointSystem.DayPointCalculate();
            } 
        }

        public double Food
        {
            get
            {
                return Breakfast + Lunch + Dinner + Snack;
            }
        }

        public double Exercise
        {
            get
            {
                return PointSystem.PointExerciseCalculate(exercise);
            }
            set
            {
                exercise = value;
            }
        }

        public double Net
        {
            get
            {
                return Food - Exercise;
            }
        }

        public double Under
        {
            get
            {
                return Budget - Net;
            }
        }

        //βαζεις cals επιστεφει points
        public double Breakfast
        {
            get
            {
                return PointSystem.PointCalculate(breakfast);
            }
            set
            {
                breakfast = value;
            }
        }

        public double Lunch
        {
            get
            {
                return PointSystem.PointCalculate(lunch);
            }
            set {
                lunch = value;
            }
        }

        public double Dinner
        {
            get
            {
                return PointSystem.PointCalculate(dinner);
            }
            set
            {
                dinner = value;
            }
        }

        public double Snack
        {
            get
            {
                return PointSystem.PointCalculate(snack);
            }
            set
            {
                snack = value;
            }
        }
    }
}
