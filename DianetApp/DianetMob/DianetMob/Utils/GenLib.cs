using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using SQLite;
using System;
using System.Collections.Generic;

namespace DianetMob.Utils
{
    public class GenLib
    {

        public async static void FullSynch()
        {
            try
            {
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                UserSettings usersettings = StorageManager.GetConnectionInfo().UserSettings;

                FullServiceLoadAndStore(loginUser,usersettings);
                FullServiceSend(loginUser, usersettings);

                StorageManager.GetConnectionInfo().UserSettings.LastSyncDate = DateTime.UtcNow;
                StorageManager.UpdateData<UserSettings>(StorageManager.GetConnectionInfo().UserSettings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static void FullServiceSend(User user, UserSettings usersettings)
        {
            //TODO: SYncdate and save serverid
            SQLiteConnection conn = StorageManager.GetConnection();
            string iduser = user.IDUser.ToString();
            
            string lastUpdateDate = usersettings.LastSyncDate.ToString("yyyyMMdd");

            /**alert**/
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<Alert> srvNewUser = null;
            foreach (Alert alt in alts)
            {
                srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alt);
                alt.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<Alert>(alt);
            }
            /**exercise**/
            IEnumerable<Exercise> exes = conn.Query<Exercise>("SELECT * FROM Exercise WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<Exercise> srvExercise = null;
            foreach (Exercise exe in exes)
            {
                srvExercise = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exe);
                exe.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<Exercise>(exe);
            }

            /**plan**/
            IEnumerable<Plan> plns = conn.Query<Plan>("SELECT * FROM Plan WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<Plan> srvPlan = null;
            foreach (Plan pln in plns)
            {
                srvPlan = await ServiceConnector.InsertServiceData<ModelService<Plan>>("/plan/save", pln);
                pln.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<Plan>(pln);
            }

            /**subscription**/
            IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<Subscription> srvSubscription = null;
            foreach (Subscription sub in subs)
            {
                srvSubscription = await ServiceConnector.InsertServiceData<ModelService<Subscription>>("/subscription/save", sub);
                sub.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<Subscription>(sub);
            }

            /**userfood**/
            IEnumerable<UserFood> ufoods = conn.Query<UserFood>("SELECT * FROM Userfood WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<UserFood> srvUserfood = null;
            foreach (UserFood ufood in ufoods)
            {
                srvUserfood = await ServiceConnector.InsertServiceData<ModelService<UserFood>>("/userfood/save", ufood);
                ufood.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<UserFood>(ufood);
            }

            /**usermeal**/
            IEnumerable<UserMeal> umeals = conn.Query<UserMeal>("SELECT * FROM Usermeal WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<UserMeal> srvUserMeal = null;
            foreach (UserMeal umeal in umeals)
            {
                srvUserMeal = await ServiceConnector.InsertServiceData<ModelService<UserMeal>>("/usermeal/save", umeal);
                umeal.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<UserMeal>(umeal);
            }

            /**weight**/
            IEnumerable<Weight> wgts = conn.Query<Weight>("SELECT * FROM Weight WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
            ModelService<Weight> srvWeight = null;
            foreach (Weight wgt in wgts)
            {
                srvWeight = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", wgt);
                wgt.IDServer = srvNewUser.ID;
                StorageManager.UpdateData<Weight>(wgt);
            }
        }





        public async static void FullServiceLoadAndStore(User user, UserSettings usersettings)
        {

            string gencall = "/accesstoken=" + user.AccessToken + "/upddate=" + usersettings.LastSyncDate.ToString("yyyyMMdd");
            string usercall = gencall + "/iduser=" + user.IDUser.ToString();

            //TODO: service call check services

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
         
        }
    }
}
