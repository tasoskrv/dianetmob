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
        private async static void FullServiceSend()
        {
            try
            {
                SQLiteConnection conn = StorageManager.GetConnection();
                string iduser = StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();

                /**alert**/
                IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser);
                Alert alert = new Alert();
                ModelService<Alert> srvNewUser = null;
                foreach (Alert alt in alts)
                {
                    alert.IDUser = alt.IDUser;
                    alert.IDServer = alt.IDServer;
                    alert.AlertTime = alt.AlertTime;
                    alert.Recurrence = alt.Recurrence;
                    alert.Description = alt.Description;
                    alert.InsertDate = alt.InsertDate;
                    alert.UpdateDate = alt.UpdateDate;
                    srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alert);
                }

                /**exercise**/
                IEnumerable<Exercise> exes = conn.Query<Exercise>("SELECT * FROM Exercise WHERE IDUser=" + iduser);
                Exercise exercise = new Exercise();
                ModelService<Exercise> srvExercise = null;
                foreach (Exercise exe in exes)
                {
                    exercise.IDUser = exe.IDUser;
                    exercise.IDServer = exe.IDServer;
                    exercise.Minutes = exe.Minutes;
                    exercise.TrainDate = exe.TrainDate;
                    exercise.InsertDate = exe.InsertDate;
                    exercise.UpdateDate = exe.UpdateDate;
                    srvExercise = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exercise);
                }

                /**plan**/
                IEnumerable<Plan> plns = conn.Query<Plan>("SELECT * FROM Plan WHERE IDUser=" + iduser);
                Plan plan = new Plan();
                ModelService<Plan> srvPlan = null;
                foreach (Plan pln in plns)
                {
                    plan.IDUser = pln.IDUser;
                    plan.IDServer = pln.IDServer;
                    plan.Goal = pln.Goal;
                    plan.GoalDate = pln.GoalDate;
                    plan.InsertDate = pln.InsertDate;
                    plan.UpdateDate = pln.UpdateDate;
                    srvPlan = await ServiceConnector.InsertServiceData<ModelService<Plan>>("/plan/save", plan);
                }

                /**subscription**/
                IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + iduser);
                Subscription subscription = new Subscription();
                ModelService<Subscription> srvSubscription = null;
                foreach (Subscription sub in subs)
                {
                    subscription.IDUser = sub.IDUser;
                    subscription.IDServer = sub.IDServer;
                    subscription.BeginDate = sub.BeginDate;
                    subscription.EndDate = sub.EndDate;
                    subscription.Price = sub.Price;
                    subscription.IsActive = sub.IsActive;
                    subscription.InsertDate = sub.InsertDate;
                    subscription.UpdateDate = sub.UpdateDate;
                    srvSubscription = await ServiceConnector.InsertServiceData<ModelService<Subscription>>("/subscription/save", subscription);
                }

                /**userfood**/
                IEnumerable<UserFood> ufoods = conn.Query<UserFood>("SELECT * FROM Userfood WHERE IDUser=" + iduser);
                UserFood userfood = new UserFood();
                ModelService<UserFood> srvUserfood = null;
                foreach (UserFood ufood in ufoods)
                {
                    userfood.IDUser = ufood.IDUser;
                    userfood.IDServer = ufood.IDServer;
                    userfood.Name = ufood.Name;
                    userfood.Description = ufood.Description;
                    userfood.InsertDate = ufood.InsertDate;
                    userfood.UpdateDate = ufood.UpdateDate;
                    srvUserfood = await ServiceConnector.InsertServiceData<ModelService<UserFood>>("/userfood/save", userfood);
                }

                /**usermeal**/
                IEnumerable<UserMeal> umeals = conn.Query<UserMeal>("SELECT * FROM Usermeal WHERE IDUser=" + iduser);
                UserMeal usermeal = new UserMeal();
                ModelService<UserMeal> srvUserMeal = null;
                foreach (UserMeal umeal in umeals)
                {
                    usermeal.IDUser = umeal.IDUser;
                    usermeal.IDServer = umeal.IDServer;
                    usermeal.IDCategory = umeal.IDCategory;
                    usermeal.IDMealUnit = umeal.IDMealUnit;
                    usermeal.Qty = umeal.Qty;
                    usermeal.MealDate = umeal.MealDate;
                    usermeal.InsertDate = umeal.InsertDate;
                    usermeal.UpdateDate = umeal.UpdateDate;
                    srvUserMeal = await ServiceConnector.InsertServiceData<ModelService<UserMeal>>("/usermeal/save", usermeal);
                }

                /**weight**/
                IEnumerable<Weight> wgts = conn.Query<Weight>("SELECT * FROM Weight WHERE IDUser=" + iduser);
                Weight weight = new Weight();
                ModelService<Weight> srvWeight = null;
                foreach (Weight wgt in wgts)
                {
                    weight.IDUser = wgt.IDUser;
                    weight.IDServer = wgt.IDServer;
                    weight.WValue = wgt.WValue;
                    weight.WeightDate = wgt.WeightDate;
                    weight.InsertDate = wgt.InsertDate;
                    weight.UpdateDate = wgt.UpdateDate;
                    srvWeight = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", weight);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static void FullServiceLoadAndStore()
        {
            try
            {
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                UserSettings settings = StorageManager.GetConnectionInfo().UserSettings;
                string gencall = "/accesstoken=" + loginUser.AccessToken + "/upddate=" + settings.LastSyncDate.ToString("yyyyMMdd");
                string usercall = gencall + "/iduser=" + loginUser.IDUser.ToString();

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

                StorageManager.GetConnectionInfo().UserSettings.LastSyncDate = DateTime.UtcNow;
                StorageManager.UpdateData<UserSettings>(StorageManager.GetConnectionInfo().UserSettings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); //ex.Message
            }            
        }
    }
}
