using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyFoodPageDetail : ContentPage
    {
        private UserFood uFood;
        private SQLiteConnection conn = null;

        public MyFoodPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        public void LoadData(int IDUserfood = 0)
        {
            if (IDUserfood > 0)
                uFood = conn.Get<UserFood>(IDUserfood);
            else
            {
                uFood = new UserFood();
                uFood.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
            BindingContext = uFood;
        }

        private void OnSaveFoodClicked(object sender, EventArgs e)
        {
            uFood.UpdateDate = DateTime.UtcNow;
            if (uFood.Name.Equals(""))
                DisplayAlert("Please", "fill name", "OK");
            else if (uFood.Description.Equals(""))
            {
                DisplayAlert("Please", "fill description", "OK");
            }
            else if (uFood.IDUserFood > 0)
            {
                StorageManager.UpdateData(uFood);
                Navigation.PopAsync();
            }
            else
            {
                uFood.InsertDate = uFood.UpdateDate;
                StorageManager.InsertData(uFood);
                Navigation.PopAsync();
            }
        }

    }
}
