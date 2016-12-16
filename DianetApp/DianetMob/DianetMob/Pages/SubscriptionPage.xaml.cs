using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class SubscriptionPage : ContentPage
    {

        private SQLiteConnection conn = null;
        private ObservableCollection<Subscription> records = new ObservableCollection<Subscription>();

        public SubscriptionPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            ListViewSubscription.ItemsSource = records;
        }

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<Subscription> subs = conn.Query<Subscription>("SELECT * FROM Subscription WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Subscription sub in subs)
            {
                records.Add(new Subscription { IDSubscription = sub.IDSubscription, BeginDate = sub.BeginDate, EndDate = sub.EndDate,
                    Price = sub.Price, IsActive = sub.IsActive });
            }
        }
    }
}






