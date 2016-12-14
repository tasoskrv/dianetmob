using DianetMob.DB;
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
        private ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
        public LogView()
        {
            InitializeComponent();
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

        public void RecreateData(IEnumerable<MapLogData> logrecords, DateTime date)
        {
            SelectedDate = date;
            for (int i=0;i<groupedItems.Count;i++)
                groupedItems[i].Clear();
            
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
