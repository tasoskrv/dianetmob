using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class AlertPage : ContentPage
    {
        private SQLiteConnection conn = null;
        public static ObservableCollection<Alert> recordsAlt = new ObservableCollection<Alert>();
        private AlertPageDetail alertPageDt = new AlertPageDetail();

        public AlertPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            setRecords();
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Alert myAlert = e.Item as Alert;
            alertPageDt.LoadData(myAlert);
            await Navigation.PushAsync(alertPageDt);
        }
        
        public void setRecords()
        {            
            ListViewAlerts.ItemsSource = null;
            recordsAlt.Clear();
            string iduser = StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + iduser);

            string statusDisplay = "";
            bool breakfast = true;
            bool lunch = true;
            bool dinner = true;
            bool snack = true;

            foreach (Alert alt in alts)
            {
                if (alt.Status == 1)
                    statusDisplay = "On";
                else
                    statusDisplay = "Off";

                if (alt.MealType == 1 && alt.IDAlert > 0)
                    breakfast = false;
                else if (alt.MealType == 2 && alt.IDAlert > 0)
                    lunch = false;
                else if (alt.MealType == 3 && alt.IDAlert > 0)
                    dinner = false;
                else if (alt.MealType == 4 && alt.IDAlert > 0)
                    snack = false;

                recordsAlt.Add(new Alert { IDUser = alt.IDUser, IDAlert = alt.IDAlert, AlertTime = alt.AlertTime, MealType = alt.MealType, Status = alt.Status, InsertDate = alt.InsertDate, StatusDisplay = statusDisplay });
            }

            if(breakfast)
                recordsAlt.Add(new Alert { IDUser = Convert.ToInt16(iduser), IDAlert = 0, AlertTime = "", MealType = 1, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off" });
            if(lunch)
                recordsAlt.Add(new Alert { IDUser = Convert.ToInt16(iduser), IDAlert = 0, AlertTime = "", MealType = 2, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off" });
            if(dinner)
                recordsAlt.Add(new Alert { IDUser = Convert.ToInt16(iduser), IDAlert = 0, AlertTime = "", MealType = 3, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off" });
            if(snack)
                recordsAlt.Add(new Alert { IDUser = Convert.ToInt16(iduser), IDAlert = 0, AlertTime = "", MealType = 4, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off" });
            
            ListViewAlerts.ItemsSource = recordsAlt;             
        }        
    }
}
