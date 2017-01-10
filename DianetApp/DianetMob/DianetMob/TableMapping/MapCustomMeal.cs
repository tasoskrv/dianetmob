using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.TableMapping
{
    public class MapCustomMeal
    {
        public int IDMeal { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IDUser { get; set; }
        public int IdMealUnit { get; set; }
        public double Calories { get; set; }
        public double Carb { get; set; }
        public double Fiber { get; set; }
        public double Natrium { get; set; }
        public double Potassium { get; set; }
        public double Fat { get; set; }
        public double SatFat { get; set; }
        public double Sugar { get; set; }
        public double UnSatFat { get; set; }
        public double Cholesterol { get; set; }

        public double Protein { get; set; }
        public int IDServer { get; set; }

    }
}
