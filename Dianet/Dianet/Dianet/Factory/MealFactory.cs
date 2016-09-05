using Dianet.DB;
using Dianet.DB.Entities;
using Dianet.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Factory
{

    public static class MealFactory
    {
       
         static void InsertMeal(MealService serv)
        {
            SQLiteConnection conn = StorageManager.GetConnection();
            for (var i = 0; i < serv.totalRows; i++)
            {
                StorageManager.InsertMealData(conn, serv.data[i]);
            }
                
        }
    }
}
