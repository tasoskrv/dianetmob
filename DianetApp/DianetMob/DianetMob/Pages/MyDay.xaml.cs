using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.TableMapping;
using DianetMob.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyDay : ContentPage
    {
        private SQLiteConnection conn = null;
        private Subscription subscription = null;
        private ConnectionInfo info;
        private Dictionary<int, double> DashboardDic = new Dictionary<int, double>();

        public MyDay()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            info = StorageManager.GetConnectionInfo();
            subscription = info.LoadActiveSubscription();
            
            GenLib.StartUp(loader);
            addmealview.MyDayPage = this;
            logview.RecreateDataAction = RecreateData;
        }

        protected async override void OnAppearing()
        {
            if (subscription == null)
            {
                if (info.isTrial)
                {
                  await  DisplayAlert("Trial", (info.LoginUser.InsertDate.AddDays(info.Settings.TrialPeriod).Subtract(DateTime.UtcNow)).Days + " Days left! Please subscribe.", "Yes");
                }
            }
            RecreateData();
        }
        void OnDashboardClicked(object sender, EventArgs e)
        {
            logview.IsVisible = false;
            dashboardview.IsVisible = true;
        }

        void OnLogClicked(object sender, EventArgs e)
        {
            dashboardview.IsVisible = false;
            logview.IsVisible = true;
        }

        void OnPrevDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(-1);
            RecreateData();
        }

        void OnNextDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(1);
            RecreateData();
        }

        public void RecreateData()
        {
            
            string query = "Select um.IdUserMeal, um.idcategory, (mu.Calories*um.QTY) as Calories,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ? and um.deleted=0";

            string query2 = "Select  SUM(mu.Calories*um.QTY) as Calories, um.MealDate as MealDate  from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ? and um.deleted=0 GROUP BY um.mealdate";

            string query3 = "Select IDExercise, Minutes, TrainDate from exercise where iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and TrainDate BETWEEN ? and ? and deleted=0";


            IEnumerable<MapLogData> logrecords = conn.Query<MapLogData>(query, datePick.Date, datePick.Date);
            DashboardDic.Clear();
            DashboardDic.Add(1, 0);
            DashboardDic.Add(2, 0);
            DashboardDic.Add(3, 0);
            DashboardDic.Add(4, 0);
            DashboardDic.Add(6, 0);
            foreach (MapLogData logrecord in logrecords)
            {
                if (logrecord.IDCategory!=5)//oxi nero
                    DashboardDic[logrecord.IDCategory] += logrecord.Calories;
            }
            
            Points points = new Points();
            points.Breakfast = DashboardDic[1];
            points.Lunch = DashboardDic[2];
            points.Dinner = DashboardDic[3];
            points.Snack = DashboardDic[4];

            IEnumerable<MapLogData> Weekrecords = conn.Query<MapLogData>(query2, datePick.Date.AddDays(-6.0).Ticks, datePick.Date.Ticks);

            IEnumerable<Exercise> exerciseRecords = conn.Query<Exercise>(query3, datePick.Date, datePick.Date);
            foreach (Exercise exrecord in exerciseRecords)
            {
                DashboardDic[6] += exrecord.Minutes;
            }
            points.Exercise = DashboardDic[6];

            logview.RecreateData(points, logrecords, exerciseRecords, datePick.Date);
            dashboardview.FillPieContent(DashboardDic, points.Food, Weekrecords);
        }

        public async void OnAddMealClicked(object sender, EventArgs e)
        {
            if ((info.isTrial) ||((subscription != null) && (subscription.EndDate >= DateTime.UtcNow)))
            {
                ToggleAddView();
            }
            else {
                var answer = await DisplayAlert(Properties.LangResource.subscription, Properties.LangResource.subAlert, Properties.LangResource.yes, Properties.LangResource.no);
                if (answer==true) {
                    await Navigation.PushAsync(new ShopPage());
                }
            }
        }

        public void OnSynchClicked(object sender, EventArgs e)
        {
            GenLib.FullSynch(loader,true);
        }
        
        public void ToggleAddView()
        {
            addmealview.IsVisible = !addmealview.IsVisible;
            addmealview.setDate(datePick.Date);
            datepickpanel.IsVisible = !addmealview.IsVisible;
            dashboardview.IsEnabled = datepickpanel.IsVisible;
            logview.IsEnabled = dashboardview.IsEnabled;

            if (addmealview.IsVisible)
            {
                dashboardview.Opacity = 0.5;
            }
            else
            {
                dashboardview.Opacity = 1;
            }
            logview.Opacity = dashboardview.Opacity;
        }
    }
}
