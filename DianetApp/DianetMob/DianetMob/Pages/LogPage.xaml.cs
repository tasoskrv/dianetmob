using DianetMob.DB;
using DianetMob.TableMapping;
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
    public partial class LogPage : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<MapLogData> records = new ObservableCollection<MapLogData>();
        ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
        public LogPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            //LogListView.ItemsSource = groupedItems;
            LogList.ItemsSource = groupedItems;
            Group group = new Group("Breakfast", "1");
            groupedItems.Add(group);
            group = new Group("Lunch", "2");
            groupedItems.Add(group);
            group = new Group("Dinner", "3");
            groupedItems.Add(group);
            group = new Group("Snack", "4");
            groupedItems.Add(group);
        }

        async void OnAddMealClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMealPage(datePick.Date));
        }

        async void OnAddWeightClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWeightPage());
        }
        void OnPrevDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(-1);
            RecreateData();
        }

        void OnNextDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(1);
            RecreateData();
        }

        protected async override void OnAppearing()
        {
            RecreateData();
        }

        public void RecreateData()
        {
            for (int i=0;i<groupedItems.Count;i++)
                groupedItems[i].Clear();
            string query = "Select um.IdUserMeal, um.idcategory,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ?";

            IEnumerable<MapLogData> logrecords = conn.Query<MapLogData>(query, datePick.Date, datePick.Date);
            foreach (MapLogData logrecord in logrecords)
            {
                Item item = new Item(logrecord.MealName, logrecord.MealName);
                groupedItems[logrecord.IDCategory-1].Add(item);
            }

        }
    }

    public class Group : ObservableCollection<Item>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public Group(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }

        // Whatever other properties
    }
    public class Item
    {
        public String Title { get; private set; }
        public String Description { get; private set; }

        public Item(String title, String description)
        {
            Title = title;
            Description = description;
        }

        // Whatever other properties
    }
}
