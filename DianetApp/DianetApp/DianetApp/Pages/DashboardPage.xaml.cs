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
        private ObservableCollection<Weight> records = new ObservableCollection<Weight>();

        public DashboardPage()
        {            
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            try
            {
                /*s
                records.Clear();
                IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, InsertDate FROM Weight WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
                foreach (Weight wght in wghts)
                {
                    records.Add(new Weight { IDWeight = wght.IDWeight, WValue = wght.WValue, InsertDate = wght.InsertDate });
                }
                */


                /**alert**
                Alert alert = new Alert();
                alert.IDUser = 39;
                alert.IDServer = -1;
                alert.AlertTime = DateTime.UtcNow;
                alert.Recurrence = "khfd";
                alert.Description = "jufd";
                alert.InsertDate = DateTime.UtcNow;
                alert.UpdateDate = alert.InsertDate;
                ModelService<Alert> srvAlert = await ServiceConnector.InsertServiceData<ModelService<Alert>>("/alert/save", alert);
                */
                /**exercise*
                Exercise exercise = new Exercise();
                exercise.IDUser = 39;
                exercise.IDServer = -1;
                exercise.Minutes = 15;
                exercise.TrainDate = DateTime.UtcNow;
                exercise.InsertDate = DateTime.UtcNow;
                exercise.UpdateDate = exercise.InsertDate;
                ModelService<Exercise> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Exercise>>("/exercise/save", exercise);                
                */
                /**plan**
                Plan plan = new Plan();
                plan.IDUser = 39;
                plan.IDServer = -1;
                plan.Goal = 10.54;
                plan.GoalDate = DateTime.UtcNow;
                plan.InsertDate = DateTime.UtcNow;
                plan.UpdateDate = plan.InsertDate;
                ModelService<Plan> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Plan>>("/plan/save", plan);
                */
                /**subscription**
                Subscription subscription = new Subscription();
                subscription.IDUser = 39;
                subscription.IDServer = -1;
                subscription.BeginDate = DateTime.UtcNow;
                subscription.EndDate = DateTime.UtcNow;
                subscription.Price = 10.55;
                subscription.IsActive = 1;
                subscription.InsertDate = DateTime.UtcNow;
                subscription.UpdateDate = subscription.InsertDate;
                ModelService<Subscription> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Subscription>>("/subscription/save", subscription);
                */
                /**userfood**
                UserFood userfood = new UserFood();
                userfood.IDUser = 39;
                userfood.IDServer = -1;
                userfood.Name = "dsfjk";
                userfood.Description = "dhf";
                userfood.InsertDate = DateTime.UtcNow;
                userfood.UpdateDate = userfood.InsertDate;
                ModelService<UserFood> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<UserFood>>("/userfood/save", userfood);
                */
                /**usermeal**/
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
                
                /**weight**
                Weight weight = new Weight();
                weight.IDUser = 39;// StorageManager.GetConnectionInfo().LoginUser.IDUser;
                weight.IDServer = -1;
                weight.WValue = 100;
                weight.WeightDate = DateTime.UtcNow;
                weight.InsertDate = DateTime.UtcNow;
                weight.UpdateDate = weight.InsertDate;                
                ModelService<Weight> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", weight);
                */



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