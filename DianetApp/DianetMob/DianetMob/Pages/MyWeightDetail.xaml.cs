using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyWeightDetail : ContentPage
    {
        private Weight wght;
        private SQLiteConnection conn = null;

        public MyWeightDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        public void LoadData(int IDWeight = 0)
        {
            if (IDWeight > 0)
                wght = conn.Get<Weight>(IDWeight);
            else
            {
                wght = new Weight();
                wght.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
            BindingContext = wght;
        }

        private void OnSaveWeightClicked(object sender, EventArgs e)
        {
            wght.UpdateDate = DateTime.UtcNow;
            if (wght.WValue <= 0)
                DisplayAlert("Please", "fill in today's weight", "OK");
            else if (wght.IDWeight > 0)
            {
                StorageManager.UpdateData(wght);
                Navigation.PopAsync();
            }
            else
            {
                wght.InsertDate = wght.UpdateDate;
                StorageManager.InsertData(wght);
                Navigation.PopAsync();
            }
        }
    }
}
