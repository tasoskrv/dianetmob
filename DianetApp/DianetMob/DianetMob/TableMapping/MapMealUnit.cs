using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.TableMapping
{
    public class MapMealUnit
    {
            public string UName { get; set; }
            public int IdMealUnit { get; set; }
            public double Calories { get; set; }

            public MapMealUnit()
            {
                UName = "";
                IdMealUnit = 0;
                Calories = 0;
            }

        
    }
}
