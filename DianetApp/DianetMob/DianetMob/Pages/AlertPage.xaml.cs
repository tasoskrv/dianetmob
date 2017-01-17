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
        public static ObservableCollection<Alert> recordsAlt = null;
        private AlertPageDetail alertPageDt = new AlertPageDetail();
        private Alert alt;

        public AlertPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            setRecords();
        }

        public void setRecords()
        {
            ListViewAlerts.ItemsSource = null;
            recordsAlt = new ObservableCollection<Alert>();
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE Deleted=0 AND IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Alert alt in alts)
            {
                recordsAlt.Add(new Alert { IDAlert = alt.IDAlert, Recurrence = alt.Recurrence, Description = alt.Description, InsertDate = alt.InsertDate });
            }
            ListViewAlerts.ItemsSource = recordsAlt; 
        }
        
        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var selectedAlert = selectedItem.CommandParameter as Alert;

            if (selectedAlert.IDServer == 0)
            {
                recordsAlt.Remove(selectedAlert);
            }
            else
            {
                alt = new Alert();
                alt = conn.Get<Alert>(selectedAlert.IDAlert);
                alt.Deleted = 1;
                StorageManager.UpdateData(alt);
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
    }
}
