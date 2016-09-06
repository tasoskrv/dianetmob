using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dianet.DB.Entities;
using Dianet.DB;

namespace Dianet.Service
{
    class MealService: ModelService
    {
        public Meal[] data { get; set;}

        public void InsertMeals()
        {
            for (var i = 0; i < totalRows; i++)
            {
                StorageManager.InsertData<Meal>(data[i]);
            }

        }
    }

}
