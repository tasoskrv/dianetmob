using DianetApp.DB;
using DianetApp.DB.Entities;
using DianetApp.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DianetApp.Pages
{

    public partial class DashboardPage : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<Alert> alertRecords = new ObservableCollection<Alert>();
        private ObservableCollection<Exercise> exerciseRecords = new ObservableCollection<Exercise>();
        private ObservableCollection<Plan> planRecords = new ObservableCollection<Plan>();
        private ObservableCollection<Subscription> subscriptionRecords = new ObservableCollection<Subscription>();
        private ObservableCollection<UserFood> userfoodRecords = new ObservableCollection<UserFood>();

        public DashboardPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string idUser = StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();
                /**alert**/
                alertRecords.Clear();
                IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + idUser);
                Alert alert = new Alert();
                ModelService<Alert> srvAlert = null;
                foreach (Alert alt in alts)
                {
                    alert.IDUser = alt.IDUser;
                    alert.IDServer = alt.IDServer;
                    alert.AlertTime = alt.AlertTime;
                    alert.Recurrence = alt.Recurrence;
                    alert.Description = alt.Description;
                    alert.InsertDate = alt.InsertDate;
                    alert.UpdateDate = alt.UpdateDate;
                    srvAlert = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alert);
                }
                //ModelService<Alert> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alert);
                //ModelService<Alert> srvNewUser = await ServiceConnector.InsertServiceBulkData<ModelService<Alert>>("/alert/save", alertRecords);

                /**exercise**
                exerciseRecords.Clear();
                IEnumerable<Exercise> exercs = conn.Query<Exercise>("SELECT * FROM Exercise WHERE IDUser=" + idUser);
                Exercise exercise = new Exercise();
                ModelService<Exercise> srvExercise = null;
                foreach (Exercise exerc in exercs)
                {
                    exercise.IDUser = exerc.IDUser;
                    exercise.IDServer = exerc.IDServer;
                    exercise.Minutes = exerc.Minutes;
                    exercise.TrainDate = exerc.TrainDate;
                    exercise.InsertDate = exerc.InsertDate;
                    exercise.UpdateDate = exerc.UpdateDate;
                    srvExercise = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exercise);
                }
                */
                /**plan**/
                planRecords.Clear();
                IEnumerable<Plan> plns = conn.Query<Plan>("SELECT * FROM Plan WHERE IDUser=" + idUser);
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
                /**subscription**
                subscriptionRecords.Clear();
                IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + idUser);
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
                */
                /**userfood**/
                userfoodRecords.Clear();
                IEnumerable<UserFood> foods = conn.Query<UserFood>("SELECT * FROM Userfood WHERE IDUser=" + idUser);
                UserFood userfood = new UserFood();
                ModelService<UserFood> srvUserFood = null;
                foreach (UserFood food in foods)
                {
                    userfood.IDUser = food.IDUser;
                    userfood.IDServer = food .IDServer;
                    userfood.Name = food.Name;
                    userfood.Description = food.Description;
                    userfood.InsertDate = food.InsertDate;
                    userfood.UpdateDate = food.UpdateDate;
                    srvUserFood = await ServiceConnector.InsertServiceData<ModelService<UserFood>>("/userfood/save", userfood);
                }
                /**usermeal**
                UserMeal usermeal = new UserMeal();
                usermeal.IDUser = 39;
                usermeal.IDServer = -1;
                usermeal.IDCategory = 1;
                usermeal.IDMealUnit = 7;
                usermeal.Qty = 1;
                usermeal.MealDate = DateTime.UtcNow;
                usermeal.InsertDate = DateTime.UtcNow;
                usermeal.UpdateDate = usermeal.InsertDate;
                ModelService<UserMeal> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<UserMeal>>("/usermeal/save", usermeal);
                */
                /**weight**
                Weight weight = new Weight();
                weight.IDUser = 39;
                weight.IDServer = -1;
                weight.WValue = 100;
                weight.WeightDate = DateTime.UtcNow;
                weight.InsertDate = DateTime.UtcNow;
                weight.UpdateDate = weight.InsertDate;                
                ModelService<Weight> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", weight);
                */

                if ((srvPlan.success == true) && (srvPlan.ID > 0) && !(srvPlan.ErrorCode > 0))
                {
                    await DisplayAlert("Complete", "Sync Completed!", "OK");
                    return;
                }
                else if (srvPlan.ErrorCode == 2)
                {
                    await DisplayAlert("Warning", "O Χρήστης υπάρχει ήδη!", "OK");
                }
                else
                {
                    await DisplayAlert("Warning", srvPlan.message, "OK");
                }
                return;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
