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
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());

            string statusDisplay = "";
            string mealDisplay = "";
            bool breakfast = false;
            bool lunch = false;
            bool dinner = false;
            bool snack = false;

            foreach (Alert alt in alts)
            {
                if (alt.Status == 1)
                {
                    statusDisplay = "On";
                }
                else
                {
                    statusDisplay = "Off";
                }

                if (alt.MealType == 1)
                {
                    mealDisplay = "Breakfast";
                    breakfast = true;
                }
                else if (alt.MealType == 2)
                {
                    mealDisplay = "Lunch";
                    lunch = true;
                }
                else if (alt.MealType == 3)
                {
                    mealDisplay = "Dinner";
                    dinner = true;
                }
                else if (alt.MealType == 4)
                {
                    mealDisplay = "Snack";
                    snack = true;
                }

                recordsAlt.Add(new Alert { IDAlert = alt.IDAlert, AlertTime = alt.AlertTime, MealType = alt.MealType, Status = alt.Status,
                                           InsertDate = alt.InsertDate, MealDisplay = mealDisplay, StatusDisplay = statusDisplay });
            }

            if(!breakfast)
                recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = "", MealType = 1, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off", MealDisplay = "Breakfast" });
            if(!lunch)
                recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = "", MealType = 2, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off", MealDisplay = "Lunch" });
            if(!dinner)
                recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = "", MealType = 3, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off", MealDisplay = "Dinner" });
            if(!snack)
                recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = "", MealType = 4, Status = 0, InsertDate = DateTime.Now, StatusDisplay = "Off", MealDisplay = "Snack" });
            
            ListViewAlerts.ItemsSource = recordsAlt;             
        }

        /*        
        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var selectedAlert = selectedItem.CommandParameter as Alert;
            if (selectedAlert.IDServer == 0)
            {
                recordsAlt.Remove(selectedAlert);
                StorageManager.DeleteData(selectedAlert);
            }
            else
            {
                selectedAlert.Deleted = 1;
                StorageManager.UpdateData(selectedAlert);
                setRecords();
            }
        }
        
        async void OnAddAlertClicked(object sender, EventArgs e)
        {
            alertPageDt.LoadData(0);
            await Navigation.PushAsync(alertPageDt);
        }
        
        */
    }
}
