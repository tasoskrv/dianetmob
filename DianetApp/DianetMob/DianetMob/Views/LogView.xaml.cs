using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Pages;
using DianetMob.TableMapping;
using DianetMob.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Views
{
    public partial class LogView : ContentView
    {
        private SQLiteConnection conn = null;
        private DateTime SelectedDate;
        private SearchMealPage searchPage = null;
        private ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
        private ConnectionInfo info;
        private Points points = new Points();
        private SelectMealPage selectPage = null;
        private Points pointss = null;
        private DateTime datee;
        public LogView()
        {
            InitializeComponent();
            info = StorageManager.GetConnectionInfo();
            //LogListView.ItemsSource = groupedItems;
            LogList.ItemsSource = groupedItems;
        }

        public void RecreateData(Points points, IEnumerable<MapLogData> logrecords, DateTime date)
        {
            BindingContext = points;
            SelectedDate = date;
            for (int i = 0; i < groupedItems.Count; i++)
                groupedItems[i].Clear();
            groupedItems.Clear();
            Group group = new Group("Breakfast: "+ points.Breakfast, "1");
            groupedItems.Add(group);
            group = new Group("Lunch: "+ points.Lunch, "2");
            groupedItems.Add(group);
            group = new Group("Dinner: "+ points.Dinner, "3");
            groupedItems.Add(group);
            group = new Group("Snack: "+ points.Snack, "4");
            groupedItems.Add(group);

            foreach (MapLogData logrecord in logrecords)
            {
                Item item = new Item(logrecord.MealName, PointSystem.PointCalculate(logrecord.Calories).ToString(), logrecord.IDUserMeal);
                groupedItems[logrecord.IDCategory - 1].Add(item);
            }
            pointss = points;
            datee = date;

        }



        public async void OnClickGridButton(object sender, EventArgs e)
        {
            if ((info.isTrial) ^ ((info.ActiveSubscription == null) || (info.ActiveSubscription.EndDate < DateTime.UtcNow)))
            {
                await App.Current.MainPage.DisplayAlert("Subscription", "Your subscription haw expired. Please renew.", "OK");
            }
            else
            {
                var item = (Xamarin.Forms.Button)sender;
                if (searchPage == null)
                {
                    searchPage = new SearchMealPage();
                }
                searchPage.SelectedDate = SelectedDate;
                searchPage.Mode = int.Parse(item.CommandParameter.ToString());
                await Navigation.PushAsync(searchPage);
            }
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var item = selectedItem.CommandParameter as Item;
            UserMeal usermeal = StorageManager.GetConnection().Get<UserMeal>(item.IDUserMeal);

            if (usermeal.IDServer == 0)
            {           
                StorageManager.DeleteData(usermeal);
            }
            else
            {
                usermeal.Deleted = 1;
                StorageManager.UpdateData(usermeal);

            }
            // provlima me to logrecords2
            string query = "Select um.IdUserMeal, um.idcategory, (mu.Calories*um.QTY) as Calories,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + info.LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ? and um.deleted=0";           
            IEnumerable<MapLogData> logrecords2 = conn.Query<MapLogData>(query, datee, datee);
            RecreateData(pointss, logrecords2, datee);

        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
           // var selectedItem = (MenuItem)sender;
        }
    }






    public class Group : ObservableCollection<Item>
    {
        public String Name { get; set; }
        public String CategoryID { get; set; }

        public Group(String Name, String CategoryID)
        {
            this.Name = Name;
            this.CategoryID = CategoryID;
        }

        // Whatever other properties
    }
    public class Item
    {
        public String Title { get;  set; }
        public String Description { get;  set; }

        public int IDUserMeal { get; set; }

        public Item(String title, String description, int idusermeal)
        {
            Title = title;
            Description = description;
            IDUserMeal = idusermeal;
        }

        // Whatever other properties
    }
}
