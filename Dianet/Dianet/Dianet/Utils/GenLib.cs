using Dianet.DB.Entities;
using Dianet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Utils
{
    public class GenLib
    {
        public async static void FullServiceLoadAndStore()
        {
            try
            {
                ModelService<Meal> servMeal = await ServiceConnector.GetServiceData<ModelService<Meal>>("/meal/getall");
                servMeal.InsertAllToDB();
            }
            catch (Exception ex){

            }

            //Να προστεθούν όσες υπηρεσίες χρειάζεται 

        }
    }
}
