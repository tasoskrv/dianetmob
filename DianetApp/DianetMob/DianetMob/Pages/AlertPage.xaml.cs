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
        private ObservableCollection<Alert> records = new ObservableCollection<Alert>();
        private AlertPageDetail alertPageDt = new AlertPageDetail();

        private Alert alt;

        public AlertPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            ListViewAlerts.ItemsSource = records;
        }

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT * FROM Alert WHERE Deleted=0 AND IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Alert alt in alts)
            {
                records.Add(new Alert { IDAlert = alt.IDAlert, Recurrence = alt.Recurrence, Description = alt.Description, InsertDate = alt.InsertDate });
            }
        }

        public void OnRemoveAlertClicked(object sender, EventArgs e)
        {
            /*
            Alert myAlert = ListViewAlerts.SelectedItem as Alert;
            if (myAlert != null)
            {
                ListViewAlerts.BeginRefresh();
                myAlert.IsVisible = true;
                ListViewAlerts.EndRefresh();
            }
            */
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var selectedAlert = selectedItem.CommandParameter as Alert;
            
            alt = new Alert();
            alt = conn.Get<Alert>(selectedAlert.IDAlert);
            alt.Deleted = 1;
            StorageManager.UpdateData(alt);            
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
