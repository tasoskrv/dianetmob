using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DianetMob.Utils
{
    public class GenLib
    {

        public async static void FullSynch()
        {
            try
            {

                var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
                notifier.Notify(new LocalNotification()
                {
                    Title = "Loading Data",
                    Text = "Loading Database from online server",
                    Id = 100,
                    NotifyTime = DateTime.Now,
                });
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                UserSettings usersettings = StorageManager.GetConnectionInfo().UserSettings;

                await FullServiceLoadAndStore(loginUser, usersettings);
                await FullServiceSend(loginUser, usersettings);

                StorageManager.GetConnectionInfo().UserSettings.LastSyncDate = DateTime.UtcNow;
                StorageManager.UpdateData<UserSettings>(StorageManager.GetConnectionInfo().UserSettings);
                notifier.Notify(new LocalNotification()
                {
                    Title = "Finish Loading Data",
                    Text = "Your Database is synchronized",
                    Id = 100,
                    NotifyTime = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static Task FullServiceSend(User user, UserSettings usersettings)
        {
            try
            {
                //TODO: SYncdate and save serverid
                SQLiteConnection conn = StorageManager.GetConnection();
                string iduser = user.IDUser.ToString();

                string lastUpdateDate = usersettings.LastSyncDate.ToString("yyyyMMdd");

                /**alert**/
                IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<Alert> srvNewAlert = null;
                foreach (Alert alt in alts)
                {
                    srvNewAlert = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alt);
                    alt.IDServer = srvNewAlert.ID;
                    StorageManager.UpdateData<Alert>(alt);
                }
                /**exercise**/
                IEnumerable<Exercise> exes = conn.Query<Exercise>("SELECT * FROM Exercise WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<Exercise> srvExercise = null;
                foreach (Exercise exe in exes)
                {
                    srvExercise = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exe);
                    exe.IDServer = srvExercise.ID;
                    StorageManager.UpdateData<Exercise>(exe);
                }

                /**plan**/
                IEnumerable<Plan> plns = conn.Query<Plan>("SELECT * FROM Plan WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<Plan> srvPlan = null;
                foreach (Plan pln in plns)
                {
                    srvPlan = await ServiceConnector.InsertServiceData<ModelService<Plan>>("/plan/save", pln);
                    pln.IDServer = srvPlan.ID;
                    StorageManager.UpdateData<Plan>(pln);
                }

                /**subscription**/
                IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<Subscription> srvSubscription = null;
                foreach (Subscription sub in subs)
                {
                    srvSubscription = await ServiceConnector.InsertServiceData<ModelService<Subscription>>("/subscription/save", sub);
                    sub.IDServer = srvSubscription.ID;
                    StorageManager.UpdateData<Subscription>(sub);
                }

                /**userfood**/
                IEnumerable<UserFood> ufoods = conn.Query<UserFood>("SELECT * FROM Userfood WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<UserFood> srvUserfood = null;
                foreach (UserFood ufood in ufoods)
                {
                    srvUserfood = await ServiceConnector.InsertServiceData<ModelService<UserFood>>("/userfood/save", ufood);
                    ufood.IDServer = srvUserfood.ID;
                    StorageManager.UpdateData<UserFood>(ufood);
                }

                /**usermeal**/
                IEnumerable<UserMeal> umeals = conn.Query<UserMeal>("SELECT * FROM Usermeal WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<UserMeal> srvUserMeal = null;
                foreach (UserMeal umeal in umeals)
                {
                    srvUserMeal = await ServiceConnector.InsertServiceData<ModelService<UserMeal>>("/usermeal/save", umeal);
                    umeal.IDServer = srvUserMeal.ID;
                    StorageManager.UpdateData<UserMeal>(umeal);
                }

                /**weight**/
                IEnumerable<Weight> wgts = conn.Query<Weight>("SELECT * FROM Weight WHERE IDUser=" + iduser + " AND UpdateDate>= " + lastUpdateDate);
                ModelService<Weight> srvWeight = null;
                foreach (Weight wgt in wgts)
                {
                    srvWeight = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", wgt);
                    wgt.IDServer = srvWeight.ID;
                    StorageManager.UpdateData<Weight>(wgt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static Task FullServiceLoadAndStore(User user, UserSettings usersettings)
        {
            try
            {
                string gencall = "/accesstoken=" + user.AccessToken + "/updatedate=" + usersettings.LastSyncDate.ToString("yyyyMMdd");
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
                servAlert.SaveAllToDBWithServerID("IDAlert");

                ModelService<Exercise> servExercise = await ServiceConnector.GetServiceData<ModelService<Exercise>>("/exercise/getall" + usercall);
                servExercise.SaveAllToDBWithServerID("IDExercise");

                ModelService<Plan> servPlan = await ServiceConnector.GetServiceData<ModelService<Plan>>("/plan/getall" + usercall);
                servPlan.SaveAllToDBWithServerID("IDPlan");

                ModelService<Subscription> servSubscription = await ServiceConnector.GetServiceData<ModelService<Subscription>>("/subscription/getall" + usercall);
                servSubscription.SaveAllToDBWithServerID("IDSubscription");

                ModelService<UserMeal> servUserMeal = await ServiceConnector.GetServiceData<ModelService<UserMeal>>("/usermeal/getall" + usercall);
                servUserMeal.SaveAllToDBWithServerID("IDUserMeal");

                ModelService<Weight> servWeight = await ServiceConnector.GetServiceData<ModelService<Weight>>("/weight/getall" + usercall);
                servWeight.SaveAllToDBWithServerID("IDWeight");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GreeklishToGreek(string text)
        {
            string[] greek = new string[] { "α", "ά", "Ά", "Α", "β", "Β", "γ", "Γ", "δ", "Δ", "ε", "έ", "Ε", "Έ", "ζ", "Ζ", "η", "ή", "Η", "θ", "Θ", "ι", "ί", "ϊ", "ΐ", "Ι", "Ί", "κ", "Κ", "λ", "Λ", "μ", "Μ", "ν", "Ν", "ξ", "Ξ", "ο", "ό", "Ο", "Ό", "π", "Π", "ρ", "Ρ", "σ", "ς", "Σ", "τ", "Τ", "υ", "ύ", "Υ", "Γ", "φ", "Φ", "χ", "Χ", "ψ", "Ψ", "ω", "ώ", "Ω", "Ώ", "", "", "", "", "", "", "", "" };
            string[] english = new string[] { "a", "a", "A", "A", "b", "B", "g", "G", "d", "D", "e", "e", "E", "E", "z", "Z", "i", "i", "I", "th", "Th", "i", "i", "i", "i", "I", "I", "k", "K", "l", "L", "m", "M", "n", "N", "x", "X", "o", "o", "O", "O", "p", "P", "r", "R", "s", "s", "S", "t", "T", "u", "u", "Y", "Y", "f", "F", "ch", "Ch", "ps", "Ps", "o", "o", "O", "O", " ", "\"", ",", ".", "(", ")", "!", "*" };
            for (int i = 0; i < greek.Length; i++)
            {
                text = text.Replace(english[i], greek[i]);
            }
            return text;

        }
        public static string NormalizeGreek(string text)
        {
            string[] greek = new string[] { "α", "ά", "Ά", "Α", "β", "Β", "γ", "Γ", "δ", "Δ", "ε", "έ", "Ε", "Έ", "ζ", "Ζ", "η", "ή", "Η", "θ", "Θ", "ι", "ί", "ϊ", "ΐ", "Ι", "Ί", "κ", "Κ", "λ", "Λ", "μ", "Μ", "ν", "Ν", "ξ", "Ξ", "ο", "ό", "Ο", "Ό", "π", "Π", "ρ", "Ρ", "σ", "ς", "Σ", "τ", "Τ", "υ", "ύ", "Υ", "Γ", "φ", "Φ", "χ", "Χ", "ψ", "Ψ", "ω", "ώ", "Ω", "Ώ", " ", "\"", ",", ".", "(", ")", "!", "*" };
            string[] greekNorm = new string[] { "α", "α", "α", "α", "β", "β", "γ", "γ", "δ", "δ", "ε", "ε", "ε", "ε", "ζ", "ζ", "η", "η", "η", "θ", "θ", "ι", "ι", "ι", "ι", "ι", "ι", "κ", "κ", "λ", "λ", "μ", "μ", "ν", "ν", "ξ", "ξ", "ο", "ο", "ο", "ο", "π", "π", "ρ", "ρ", "σ", "ς", "σ", "τ", "τ", "υ", "υ", "γ", "γ", "φ", "φ", "χ", "χ", "ψ", "ψ", "ω", "ω", "ω", "ω", "", "", "", "", "", "", "", "" };
            for (int i = 0; i < greek.Length; i++)
            {
                text = text.Replace(greek[i], greekNorm[i]);
            }
            return text;

        }
    }
}
