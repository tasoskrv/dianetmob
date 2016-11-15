using DianetApp.DB;
using DianetApp.DB.Entities;
using DianetApp.Service;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DianetApp.Pages
{

    public partial class DashboardPage : ContentPage
    {
        private SQLiteConnection conn = null;

        public DashboardPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            try
            {
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

                if ((srvNewUser.success == true) && (srvNewUser.ID > 0) && !(srvNewUser.ErrorCode > 0))
                {
                    await DisplayAlert("Complete", "Sync Completed!", "OK");
                    return;
                }
                else if (srvNewUser.ErrorCode == 2)
                {
                    await DisplayAlert("Warning", "O Χρήστης υπάρχει ήδη!", "OK");
                }
                else
                {
                    await DisplayAlert("Warning", srvNewUser.message, "OK");
                }
                return;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
