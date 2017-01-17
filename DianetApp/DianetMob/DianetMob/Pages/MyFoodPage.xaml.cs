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
        public static ObservableCollection<Meal> recordsMeal = null;
        private MyFoodPageDetail myFoodDt = new MyFoodPageDetail();
        private Meal meal;

        public MyFoodPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            setRecords();
        }

        public void setRecords()
        {
            ListViewMyFoods.ItemsSource = null;
            recordsMeal = new ObservableCollection<Meal>();
            IEnumerable<Meal> foods = conn.Query<Meal>("SELECT * FROM Meal WHERE Deleted=0 AND IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Meal food in foods)
            {
                recordsMeal.Add(new Meal { IDUser = food.IDUser, IDMeal = food.IDMeal, Name = food.Name, Description = food.Description, InsertDate = food.InsertDate });
            }
            ListViewMyFoods.ItemsSource = recordsMeal;            
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var selectedMeal = selectedItem.CommandParameter as Meal;

            if (selectedMeal.IDServer == 0)
            {
                recordsMeal.Remove(selectedMeal);
            }
            else
            {
                meal = new Meal();
                meal = conn.Get<Meal>(selectedMeal.IDMeal);
                meal.Deleted = 1;
                StorageManager.UpdateData(meal);
                setRecords();
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
