using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyWeight : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<Weight> records = new ObservableCollection<Weight>();
        private MyWeightDetail myWeightDt = new MyWeightDetail();

        public MyWeight()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            ListViewWeights.ItemsSource = records;
        }

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, InsertDate FROM Weight WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Weight wght in wghts)
            {
                records.Add(new Weight { IDWeight = wght.IDWeight, WValue = wght.WValue, InsertDate = wght.InsertDate });
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Weight myWght = e.Item as Weight;
            myWeightDt.LoadData(myWght.IDWeight);
            await Navigation.PushAsync(myWeightDt);
        }

        async void OnAddWeightClicked(object sender, EventArgs e)
        {
            myWeightDt.LoadData(0);
            await Navigation.PushAsync(myWeightDt);
        }
    } 
}
