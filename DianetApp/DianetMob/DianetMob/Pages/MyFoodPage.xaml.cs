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
    public partial class MyFoodPage : ContentPage
    {
      
        private SQLiteConnection conn = null;
        private ObservableCollection<UserFood> records = new ObservableCollection<UserFood>();
        private MyFoodPageDetail myFoodDt = new MyFoodPageDetail();

        public MyFoodPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            ListViewMyFoods.ItemsSource = records;
        }

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<UserFood> foods = conn.Query<UserFood>("SELECT * FROM Userfood WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (UserFood food in foods)
            {
                records.Add(new UserFood { IDUserFood = food.IDUserFood, Name = food.Name, InsertDate = food.InsertDate });
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            UserFood myFood = e.Item as UserFood;
            myFoodDt.LoadData(myFood.IDUserFood);
            await Navigation.PushAsync(myFoodDt);
        }

        async void OnAddFoodClicked(object sender, EventArgs e)
        {
            myFoodDt.LoadData(0);
            await Navigation.PushAsync(myFoodDt);
        }


    }
}
