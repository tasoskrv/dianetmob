using DianetApp.DB;
using DianetApp.DB.Entities;
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
    public partial class AlertPage : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<Alert> records = new ObservableCollection<Alert>();
        private AlertPageDetail alertPageDt = new AlertPageDetail();

        public AlertPage()
        {
            InitializeComponent();            
        }

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<Alert> alts = conn.Query<Alert>("SELECT IDAlert, Description, InsertDate FROM Alert WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Alert alt in alts)
            {
                records.Add(new Alert { IDAlert = alt.IDAlert, Description = alt.Description, InsertDate = alt.InsertDate });
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
