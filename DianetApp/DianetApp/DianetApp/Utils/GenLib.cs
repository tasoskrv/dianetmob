using DianetApp.DB;
using DianetApp.DB.Entities;
using DianetApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetApp.Utils
{
    public class GenLib
    {
        public async static void FullServiceLoadAndStore()
        {
            try
            {
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                Settings settings = StorageManager.GetConnectionInfo().Settings;
                string gencall = "/accesstoken=" + loginUser.AccessToken + "/upddate=" + settings.LastSyncDate.ToString("yyyyMMdd");
                string usercall = gencall + "/iduser=" + loginUser.IDUser.ToString();

                //general calls - add required services
                ModelService<Unit> servUnit = await ServiceConnector.GetServiceData<ModelService<Unit>>("/unit/getall" + gencall);
                servUnit.SaveAllToDB();

                ModelService<Meal> servMeal = await ServiceConnector.GetServiceData<ModelService<Meal>>("/meal/getall" + gencall);
                servMeal.SaveAllToDB();

                ModelService<MealUnit> servMealUnit = await ServiceConnector.GetServiceData<ModelService<MealUnit>>("/mealunit/getall" + gencall);
                servMealUnit.SaveAllToDB();

                ModelService<Package> servPackage = await ServiceConnector.GetServiceData<ModelService<Package>>("/package/getall" + gencall);
                servPackage.SaveAllToDB();

                ModelService<Settings> servSettings = await ServiceConnector.GetServiceData<ModelService<Settings>>("/settings/getall" + gencall);
                servSettings.SaveAllToDB();

                //User calls
                ModelService<Alert> servAlert = await ServiceConnector.GetServiceData<ModelService<Alert>>("/alert/getall" + usercall);
                servAlert.SaveAllToDB();

                ModelService<Exercise> servExercise = await ServiceConnector.GetServiceData<ModelService<Exercise>>("/exercise/getall" + usercall);
                servExercise.SaveAllToDB();

                ModelService<Plan> servPlan = await ServiceConnector.GetServiceData<ModelService<Plan>>("/plan/getall" + usercall);
                servPlan.SaveAllToDB();

                ModelService<Subscription> servSubscription = await ServiceConnector.GetServiceData<ModelService<Subscription>>("/subscription/getall" + usercall);
                servSubscription.SaveAllToDB();

                ModelService<UserFood> servUserFood = await ServiceConnector.GetServiceData<ModelService<UserFood>>("/userfood/getall" + usercall);
                servUserFood.SaveAllToDB();

                ModelService<UserMeal> servUserMeal = await ServiceConnector.GetServiceData<ModelService<UserMeal>>("/usermeal/getall" + usercall);
                servUserMeal.SaveAllToDB();

                ModelService<Weight> servWeight = await ServiceConnector.GetServiceData<ModelService<Weight>>("/weight/getall" + usercall);
                servWeight.SaveAllToDB();

                StorageManager.GetConnectionInfo().Settings.LastSyncDate = DateTime.UtcNow;
                StorageManager.UpdateData<Settings>(StorageManager.GetConnectionInfo().Settings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); //ex.Message
            }            
        }
    }
}
