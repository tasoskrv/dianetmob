using Dianet.DB;
using SQLite;
using System;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class MyWeight : ContentPage
    {
        SQLiteConnection conn = null;

        public MyWeight()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private void OnSaveWeightClicked(object sender, EventArgs e)
        {
            if (fMyWeightEntry.Text == null || fMyWeightEntry.Text == "")
                DisplayAlert("Please", "fill in today's weight", "OK");
            else
            {
                //StorageManager.InsertData(StorageManager.GetConnectionInfo().LoginUserWeight);
                /*List<Weight> wght = conn.Query<Weight>("SELECT * FROM Weight");
                if (wght.Count > 0)
                {
                    //wght[0]                    
                    return;
                }*/
                //StorageManager.GetConnectionInfo().LoginUserWeight.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
        }
    }
}
