using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DianetMob.Utils
{
    public class GenLib
    {
        private static RecurringTask ServiceTask;
        private static RecurringTask NotifTask;
        public static Dictionary<int, Alert> NotifAlerts = new Dictionary<int, Alert>();
        private static RecurringTask nextAlertNotif =null;
        private static Alert selalert = null;

        private static bool isRunning;

        public static void FullSynchA() {
            FullSynch();
        }

        public async static Task FullSynch(bool force=false)
        {            
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    User loginUser = StorageManager.GetConnectionInfo().LoginUser;
                    UserSettings usersettings = StorageManager.GetConnectionInfo().UserSettings;
                    if (usersettings.LastSyncDate.Date < DateTime.UtcNow.Date || force)
                    {
                        if (isRunning)
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "Sync process is already running!", "OK");
                            return;
                        }
                        isRunning = true;
                        var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
                        notifier.Notify(new LocalNotification()
                        {
                            Title = "Loading Data",
                            Text = "Loading Database from online server",
                            Id = 10000,
                            NotifyTime = DateTime.Now,
                        });

                        await FullServiceLoadAndStore(loginUser, usersettings);
                        await FullServiceSend(loginUser, usersettings);
                        
                        usersettings.LastSyncDate = DateTime.UtcNow;
                        StorageManager.UpdateData<UserSettings>(usersettings);
                        notifier.Notify(new LocalNotification()
                        {
                            Title = "Finish Loading Data",
                            Text = "Your Database is synchronized",
                            Id = 10000,
                            NotifyTime = DateTime.Now,
                        });
                        isRunning = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                isRunning = false;
            }
        }

        public static void StartUp()
        {
            if (ServiceTask == null && NotifAlerts.Count==0)
            {
                SQLiteConnection conn = StorageManager.GetConnection();
                string iduser = StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();
                List<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser);

                selalert = null;
                foreach (Alert alt in alts)
                {
                    if (alt.Status == 1)
                    {
                        NotifAlerts.Add(alt.IDAlert, alt);
                        if ((selalert == null) || (alt.GetTimeLeft() < selalert.GetTimeLeft())) {
                            selalert = alt;
                        }
                    }
                }
                if (selalert != null) {
                    nextAlertNotif = new RecurringTask(new Action(() => AlertWake()), selalert.GetTimeLeft());
                    nextAlertNotif.Start();
                }
            }

            var minutes = TimeSpan.FromDays(1);
            if (ServiceTask == null)
            {
                FullSynch();
                ServiceTask = new RecurringTask(new Action(FullSynchA), minutes);
            }
            ServiceTask.Start();

            minutes = TimeSpan.FromHours(6);
            if (NotifTask == null)
            {

                CheckMessages();
                NotifTask = new RecurringTask(new Action(CheckMessages), minutes);
            }
            NotifTask.Start();

        }

        public static void CalcNextAlert() {
            selalert = null;
            if (nextAlertNotif != null)
            {
                nextAlertNotif.Stop();
                nextAlertNotif = null; 
            }
            foreach (var pair in NotifAlerts)
            {
                Alert alt = NotifAlerts[pair.Key];

                if ((selalert == null) || (alt.GetTimeLeft() < selalert.GetTimeLeft()))
                {
                    selalert = alt;
                }
            }

            if (selalert != null)
            {
                nextAlertNotif = new RecurringTask(new Action(() => AlertWake()), selalert.GetTimeLeft());
                nextAlertNotif.Start();
            }
        }

        public static void AlertWake()
        {
            var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
            string atitle = "";
            if (selalert.MealType == 1)
                atitle = "Breakfast";
            else if (selalert.MealType == 2)
                atitle = "Lunch";
            else if (selalert.MealType == 3)
                atitle = "Dinner";
            else if (selalert.MealType == 4)
                atitle = "Snack";


            notifier.Notify(new LocalNotification()
            {
                Title = atitle + " Reminder",
                Text = "Time to eat!",
                Id = selalert.IDAlert + 20000,
                NotifyTime = DateTime.Now,
            });
            CalcNextAlert();
        }

        public async static Task FullServiceSend(User user, UserSettings usersettings)
        {
            try
            {
                //TODO: SYncdate and save serverid
                SQLiteConnection conn = StorageManager.GetConnection();
                string iduser = user.IDUser.ToString();

                long tick = usersettings.LastSyncDate.Ticks;

                //user
                IEnumerable<User> users = conn.Query<User>("SELECT * FROM User WHERE IDUser=" + iduser + " AND UpdateDate>= ?", tick);
                ModelService<User> srvNewUser = null;
                foreach (User us in users)
                {
                    srvNewUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/update", us);
                }
                
                //meal
                IEnumerable<Meal> meals = conn.Query<Meal>("SELECT * FROM Meal WHERE IDUser=" + iduser + " AND UpdateDate>= ?" , tick);
                ModelService<Meal> srvNewMeal = null;
                foreach (Meal meal in meals)
                {
                    srvNewMeal = await ServiceConnector.InsertServiceData<ModelService<Meal>>("/meal/save", meal);
                    if (srvNewMeal.ID != 0)
                    {
                        meal.IDServer = srvNewMeal.ID;
                        StorageManager.UpdateData<Meal>(meal);
                    }
                }
                
                //mealunit
                IEnumerable<MealUnit> mealunits = conn.Query<MealUnit>("SELECT * FROM MealUnit WHERE IDUser=" + iduser + " AND UpdateDate>= ?" , tick);
                ModelService<MealUnit> srvNewMealunits = null;
                foreach (MealUnit mealunit in mealunits)
                {
                    srvNewMealunits = await ServiceConnector.InsertServiceData<ModelService<MealUnit>>("/mealunit/save", mealunit);
                    if (srvNewMeal.ID != 0)
                    {
                        mealunit.IDServer = srvNewMeal.ID;
                        StorageManager.UpdateData<MealUnit>(mealunit);
                    }
                }

                //alert
                IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser + " AND UpdateDate>= ?" , tick);
                ModelService<Alert> srvNewAlert = null;
                foreach (Alert alt in alts)
                {
                    srvNewAlert = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alt);
                    if (srvNewAlert.ID != 0)
                    {
                        alt.IDServer = srvNewAlert.ID;
                        StorageManager.UpdateData<Alert>(alt);
                    }
                }
                //exercise
                IEnumerable<Exercise> exes = conn.Query<Exercise>("SELECT * FROM Exercise WHERE IDUser=" + iduser + " AND  UpdateDate>= ? " , tick);
                ModelService<Exercise> srvExercise = null;
                foreach (Exercise exe in exes)
                {
                    srvExercise = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exe);
                    if (srvExercise.ID != 0)
                    {
                        exe.IDServer = srvExercise.ID;
                        StorageManager.UpdateData<Exercise>(exe);
                    }
                }
                
                //plan
                IEnumerable<Plan> plns = conn.Query<Plan>("SELECT * FROM Plan WHERE IDUser=" + iduser + " AND  UpdateDate>= ? " , tick);
                ModelService<Plan> srvPlan = null;
                foreach (Plan pln in plns)
                {
                    srvPlan = await ServiceConnector.InsertServiceData<ModelService<Plan>>("/plan/save", pln);
                    if (srvPlan.ID != 0)
                    {
                        pln.IDServer = srvPlan.ID;
                        StorageManager.UpdateData<Plan>(pln);
                    }
                }
                
                //subscription
                IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + iduser + " AND UpdateDate>= ? " , tick);
                ModelService<Subscription> srvSubscription = null;
                foreach (Subscription sub in subs)
                {
                    srvSubscription = await ServiceConnector.InsertServiceData<ModelService<Subscription>>("/subscription/save", sub);
                    if (srvSubscription.ID != 0)
                    {
                        sub.IDServer = srvSubscription.ID;
                        StorageManager.UpdateData<Subscription>(sub);
                    }
                }
                if (((List<Subscription>)subs).Count > 0) {
                    StorageManager.GetConnectionInfo().LoadActiveSubscription();
                }
                
                //usermeal
                IEnumerable<UserMeal> umeals = conn.Query<UserMeal>("SELECT * FROM Usermeal WHERE IDUser=" + iduser + " AND UpdateDate>= ? " , tick);
                ModelService<UserMeal> srvUserMeal = null;
                foreach (UserMeal umeal in umeals)
                {
                    srvUserMeal = await ServiceConnector.InsertServiceData<ModelService<UserMeal>>("/usermeal/save", umeal);
                    if (srvUserMeal.ID != 0)
                    {
                        umeal.IDServer = srvUserMeal.ID;
                        StorageManager.UpdateData<UserMeal>(umeal);
                    }
                }
                //weight
                IEnumerable<Weight> wgts = conn.Query<Weight>("SELECT * FROM Weight WHERE IDUser=" + iduser + " AND UpdateDate>= ? " , tick);
                ModelService<Weight> srvWeight = null;
                foreach (Weight wgt in wgts)
                {
                    srvWeight = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", wgt);
                    if (srvWeight.ID != 0)
                    {
                        wgt.IDServer = srvWeight.ID;
                        StorageManager.UpdateData<Weight>(wgt);
                    }
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
                string gencall = "/accesstoken=" + user.AccessToken + "/updatedate=" + usersettings.LastSyncDate.ToString("yyyyMMddTHHmmss");
                string usercall = gencall + "/iduser=" + user.IDUser.ToString();


                ModelService<Sync> servsync = await ServiceConnector.GetServiceData<ModelService<Sync>>("/sync/syncdb" + usercall);

                //general calls
                if (servsync.data[0].Unit == 1)
                {
                    ModelService<Unit> servUnit = await ServiceConnector.GetServiceData<ModelService<Unit>>("/unit/getall" + gencall);
                    servUnit.SaveAllToDB();
                }
                if (servsync.data[0].Meal == 1)
                {
                    ModelService<Meal> servMeal = await ServiceConnector.GetServiceData<ModelService<Meal>>("/meal/getall" + usercall);
                    servMeal.SaveAllToDBWithServerID("IDMeal");
                }
                if (servsync.data[0].MealUnit == 1)
                {
                    ModelService<MealUnit> servMealUnit = await ServiceConnector.GetServiceData<ModelService<MealUnit>>("/mealunit/getall" + usercall);
                    servMealUnit.SaveAllToDBWithServerID("IDMealUnit");
                }
                if (servsync.data[0].Package == 1)
                {
                    ModelService<Package> servPackage = await ServiceConnector.GetServiceData<ModelService<Package>>("/package/getall" + gencall);
                    servPackage.SaveAllToDB();
                }
                if (servsync.data[0].Settings == 1)
                {
                    ModelService<Settings> servSettings = await ServiceConnector.GetServiceData<ModelService<Settings>>("/settings/getall" + gencall);
                    servSettings.SaveAllToDB();
                }
                //User calls
                if (servsync.data[0].Alert == 1)
                {
                    ModelService<Alert> servAlert = await ServiceConnector.GetServiceData<ModelService<Alert>>("/alert/getall" + usercall);
                    servAlert.SaveAllToDBWithServerID("IDAlert");
                }
                if (servsync.data[0].Exercise == 1)
                {
                    ModelService<Exercise> servExercise = await ServiceConnector.GetServiceData<ModelService<Exercise>>("/exercise/getall" + usercall);
                    servExercise.SaveAllToDBWithServerID("IDExercise");
                }
                if (servsync.data[0].Plan == 1)
                {
                    ModelService<Plan> servPlan = await ServiceConnector.GetServiceData<ModelService<Plan>>("/plan/getall" + usercall);
                    servPlan.SaveAllToDBWithServerID("IDPlan");
                }
                if (servsync.data[0].Subscription == 1)
                {
                    ModelService<Subscription> servSubscription = await ServiceConnector.GetServiceData<ModelService<Subscription>>("/subscription/getall" + usercall);
                    servSubscription.SaveAllToDBWithServerID("IDSubscription");
                }
                if (servsync.data[0].UserMeal == 1)
                {
                    ModelService<UserMeal> servUserMeal = await ServiceConnector.GetServiceData<ModelService<UserMeal>>("/usermeal/getall" + usercall);
                    servUserMeal.SaveAllToDBWithServerID("IDUserMeal");
                }
                if (servsync.data[0].Weight == 1)
                {
                    ModelService<Weight> servWeight = await ServiceConnector.GetServiceData<ModelService<Weight>>("/weight/getall" + usercall);
                    servWeight.SaveAllToDBWithServerID("IDWeight");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static void CheckMessages()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                User loginUser = StorageManager.GetConnectionInfo().LoginUser;

                string gencall = "/accesstoken=" + loginUser.AccessToken;
                string usercall = gencall + "/iduser=" + loginUser.IDUser.ToString();

                ModelService<Message> servMsg = await ServiceConnector.GetServiceData<ModelService<Message>>("/message/getall" + usercall);
                servMsg.SaveAllToDBWithServerID("IDMessage");

                SQLiteConnection conn = StorageManager.GetConnection();
                IEnumerable<Message> msgs = conn.Query<Message>("SELECT * FROM message WHERE IDUser=" + loginUser.IDUser + " AND seen=0 ");
                var notifier = DependencyService.Get<ICrossLocalNotifications>().CreateLocalNotifier();
                foreach (Message msg in msgs)
                {
                    notifier.Notify(new LocalNotification()
                    {
                        Title = msg.Title,
                        Text = msg.MessageText,
                        Id = msg.IDMessage+30000,
                        NotifyTime = DateTime.Now,
                    });
                    msg.seen = 1;
                    conn.Update(msg);
                    await ServiceConnector.InsertServiceData<ModelService<Message>>("/message/save", msg);
                }
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

        public static bool CheckValidMail(string email)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (email == null || !Regex.IsMatch(email, pattern))
            {                
                return false;
            }            
            return true;            
        }
    }
}
