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
        public  ObservableCollection<Meal> recordsMeal = new ObservableCollection<Meal>();
        private MyFoodPageDetail myFoodDt = new MyFoodPageDetail();

        public MyFoodPage()
        {
            InitializeComponent();
            ListViewMyFoods.ItemsSource = recordsMeal;
            conn = StorageManager.GetConnection();
            setRecords();
            myFoodDt.setRecordsAction = setRecords;
        }

        public void setRecords()
        {
            ListViewMyFoods.BeginRefresh();
            try
            {
                recordsMeal.Clear();
                IEnumerable<Meal> foods = conn.Query<Meal>("SELECT * FROM Meal WHERE Deleted=0 AND IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
                foreach (Meal food in foods)
                {
                    recordsMeal.Add(new Meal { IDUser = food.IDUser, IDServer = food.IDServer, IDMeal = food.IDMeal, Name = food.Name, Description = food.Description, UpdateDate = food.UpdateDate });
                }
            }
            finally {
                ListViewMyFoods.EndRefresh();
            }
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var item = (Button)sender;
            int idmeal = Convert.ToInt32(item.CommandParameter.ToString());            
            Meal mealObj = StorageManager.GetConnection().Get<Meal>(idmeal);
           
            if (mealObj.IDServer == 0)
            {
                StorageManager.DeleteData<Meal>(mealObj);
            }
            else
            {
                mealObj.Deleted = 1;
                StorageManager.UpdateData(mealObj);
            }            
            setRecords();
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
