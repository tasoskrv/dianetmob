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
        private ObservableCollection<Meal> records = new ObservableCollection<Meal>();
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
            IEnumerable<Meal> foods = conn.Query<Meal>("SELECT * FROM Meal WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Meal food in foods)
            {
                records.Add(new Meal { IDUser = food.IDUser, IDMeal= food.IDMeal, Name = food.Name, Description= food.Description, InsertDate = food.InsertDate });
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Meal myFood = e.Item as Meal;
            myFoodDt.LoadData(myFood.IDMeal, myFood.IDUser);
            await Navigation.PushAsync(myFoodDt);
        }

        async void OnAddFoodClicked(object sender, EventArgs e)
        {
            myFoodDt.LoadData(0,0);
            await Navigation.PushAsync(myFoodDt);
        }


    }
}
