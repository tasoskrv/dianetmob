using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
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
            {
                fSaveBtn.IsVisible = true;                
                wght = conn.Query<Weight>("SELECT WValue, InsertDate FROM Weight WHERE IDWeight=" + IDWeight.ToString())[0];
            }
            else
            {
                fSaveBtn.IsVisible = false;
                wght = new Weight();
            }
            BindingContext = wght;
        }

        private void OnSaveWeightClicked(object sender, EventArgs e)
        {
            if (fMyWeightEntry.Text == null || fMyWeightEntry.Text == "")
                DisplayAlert("Please", "fill in today's weight", "OK");
            else
                StorageManager.InsertData(wght);
        }
    }
}
