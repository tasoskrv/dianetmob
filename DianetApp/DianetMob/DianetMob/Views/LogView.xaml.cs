using DianetMob.DB;
using DianetMob.Pages;
using DianetMob.TableMapping;
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
        public LogView()
        {
            InitializeComponent();
            //LogListView.ItemsSource = groupedItems;
            LogList.ItemsSource = groupedItems;
            Group group = new Group("Breakfast: 0", "1");
            groupedItems.Add(group);
            group = new Group("Lunch: 0", "2");
            groupedItems.Add(group);
            group = new Group("Dinner: 0", "3");
            groupedItems.Add(group);
            group = new Group("Snack: 0", "4");
            groupedItems.Add(group);
        }

        public void RecreateData(IEnumerable<MapLogData> logrecords, DateTime date)
        {
            SelectedDate = date;
            for (int i = 0; i < groupedItems.Count; i++)
                groupedItems[i].Clear();

            foreach (MapLogData logrecord in logrecords)
            {
                Item item = new Item(logrecord.MealName, logrecord.Calories.ToString());
                groupedItems[logrecord.IDCategory - 1].Add(item);
            }

        }



        public async void OnClickGridButton(object sender, EventArgs e)
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
    public class Group : ObservableCollection<Item>
    {
        public String Name { get; private set; }
        public String CategoryID { get; private set; }

        public Group(String Name, String CategoryID)
        {
            this.Name = Name;
            this.CategoryID = CategoryID;
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
