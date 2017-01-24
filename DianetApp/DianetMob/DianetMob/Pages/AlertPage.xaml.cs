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

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert("jdf", "jhdf", "iudf");
        }
        
        public void setRecords()
        {
            
            ListViewAlerts.ItemsSource = null;
            recordsAlt.Clear();
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());

            recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = DateTime.Now, MealType = 1, Status = 0, InsertDate = DateTime.Now });
            recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = DateTime.Now, MealType = 2, Status = 0, InsertDate = DateTime.Now });
            recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = DateTime.Now, MealType = 3, Status = 0, InsertDate = DateTime.Now });
            recordsAlt.Add(new Alert { IDAlert = 0, AlertTime = DateTime.Now, MealType = 4, Status = 0, InsertDate = DateTime.Now });

            /*
            foreach (Alert alt in alts)
            {
                recordsAlt.Add(new Alert { IDAlert = alt.IDAlert, AlertTime = alt.AlertTime, MealType = alt.MealType, Status = alt.Status, InsertDate = alt.InsertDate });
                
            }
            */
            ListViewAlerts.ItemsSource = recordsAlt;             
        }

        /*
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert("jdf", "jhdf", "iudf");
        }


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

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Alert myAlert = e.Item as Alert;
            alertPageDt.LoadData(myAlert.IDAlert);
            await Navigation.PushAsync(alertPageDt);
        }
        */
    }
}
