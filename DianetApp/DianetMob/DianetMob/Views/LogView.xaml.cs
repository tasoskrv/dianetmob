using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Pages;
using DianetMob.TableMapping;
using DianetMob.Utils;
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
        private DateTime SelectedDate;
        private SearchMealPage searchPage = null;
        private ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
        private ConnectionInfo info;
        private Points points = new Points();
        private SelectMealPage selectPage = null;

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
                Item item = new Item(logrecord.MealName, PointSystem.PointCalculate(logrecord.Calories).ToString());
                groupedItems[logrecord.IDCategory - 1].Add(item);
            }

            

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
            //var selectedUserMeal = selectedItem.CommandParameter as ;
            

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

        public Item(String title, String description)
        {
            Title = title;
            Description = description;
        }

        // Whatever other properties
    }
}
