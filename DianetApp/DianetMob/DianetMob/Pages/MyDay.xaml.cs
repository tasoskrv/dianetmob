using DianetMob.DB;
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

        public MyDay()
        {
            InitializeComponent();

            conn = StorageManager.GetConnection();
        }
        private void OnSyncButtonClicked(object sender, EventArgs e)
        {
            GenLib.FullSynch();
        }
        protected override void OnAppearing()
        {
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
            
            string query = "Select um.IdUserMeal, um.idcategory, (mu.Calories*um.QTY) as Calories,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ?";

            IEnumerable<MapLogData> logrecords = conn.Query<MapLogData>(query, datePick.Date, datePick.Date);
            logview.RecreateData(logrecords, datePick.Date);
            dashboardview.FillPieContent(logrecords);
        }

        public void OnAddMealClicked(object sender, EventArgs e)
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
            else {
                dashboardview.Opacity = 1;
            }
            logview.Opacity = dashboardview.Opacity;
            //await Navigation.PushAsync(new AddMealPage(datePick.Date));
        }
    }
}
