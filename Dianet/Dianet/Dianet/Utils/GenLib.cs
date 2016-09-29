using Dianet.DB;
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
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                Settings settings = StorageManager.GetConnectionInfo().Settings;
                string gencall = "/accesstoken = " + loginUser.AccessToken + " / upddate = " + settings.LastSyncDate.ToString("yyyyMMdd");
                string usercall = gencall + "/userid=" + loginUser.IdUser.ToString();

                //general calls
                ModelService<Meal> servMeal = await ServiceConnector.GetServiceData<ModelService<Meal>>("/meal/getall" + gencall);
                servMeal.InsertAllToDB();

                ModelService<MealUnit> servMealUnit = await ServiceConnector.GetServiceData<ModelService<MealUnit>>("/mealunit/getall" + gencall);
                servMealUnit.InsertAllToDB();

                ModelService<Package> servPackage = await ServiceConnector.GetServiceData<ModelService<Package>>("/package/getall" + gencall);
                servPackage.InsertAllToDB();

                ModelService<Settings> servSettings = await ServiceConnector.GetServiceData<ModelService<Settings>>("/settings/getall" + gencall);
                servSettings.InsertAllToDB();

                //User calls
                ModelService<Alert> servAlert = await ServiceConnector.GetServiceData<ModelService<Alert>>("/alert/getall" + usercall);
                servAlert.InsertAllToDB();

                ModelService<Exercise> servExercise = await ServiceConnector.GetServiceData<ModelService<Exercise>>("/exercise/getall" + usercall);
                servExercise.InsertAllToDB();

                ModelService<Plan> servPlan = await ServiceConnector.GetServiceData<ModelService<Plan>>("/plan/getall" + usercall);
                servPlan.InsertAllToDB();

                ModelService<Subscription> servSubscription = await ServiceConnector.GetServiceData<ModelService<Subscription>>("/subscription/getall" + usercall);
                servSubscription.InsertAllToDB();

                ModelService<UserFood> servUserFood = await ServiceConnector.GetServiceData<ModelService<UserFood>>("/userfood/getall" + usercall);
                servUserFood.InsertAllToDB();

                ModelService<UserMeal> servUserMeal = await ServiceConnector.GetServiceData<ModelService<UserMeal>>("/usermeal/getall" + usercall);
                servUserMeal.InsertAllToDB();

                ModelService<Weight> servWeight = await ServiceConnector.GetServiceData<ModelService<Weight>>("/weight/getall" + usercall);
                servWeight.InsertAllToDB();

                StorageManager.GetConnectionInfo().Settings.LastSyncDate = DateTime.UtcNow;
                StorageManager.UpdateData<Settings>(StorageManager.GetConnectionInfo().Settings);
            }
            catch (Exception ex)
            {

            }

            //Να προστεθούν όσες υπηρεσίες χρειάζεται 

        }
    }
}
