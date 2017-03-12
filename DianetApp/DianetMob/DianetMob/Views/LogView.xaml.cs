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
using System.ComponentModel;
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
       // private SelectMealPage selectPage = null;
        private AddExercisePage exerPage = null;
        public Action RecreateDataAction { get; set; }
        public LogView()
        {
            InitializeComponent();
            info = StorageManager.GetConnectionInfo();
            //LogListView.ItemsSource = groupedItems;
            LogList.ItemsSource = groupedItems;
            conn = StorageManager.GetConnection();
        }

        public void RecreateData(Points points, IEnumerable<MapLogData> logrecords, IEnumerable<Exercise> exercises, DateTime date)
        {
            BindingContext = points;
            SelectedDate = date;
            for (int i = 0; i < groupedItems.Count; i++)
                groupedItems[i].Clear();
            groupedItems.Clear();
            Group group = new Group(Properties.LangResource.breakfast + ": " + points.Breakfast, "1", "lock.png");
            groupedItems.Add(group);
            group = new Group(Properties.LangResource.lunch + ": " + points.Lunch, "2", "lock.png");
            groupedItems.Add(group);
            group = new Group(Properties.LangResource.dinner + ": " + points.Dinner, "3", "lock.png");
            groupedItems.Add(group);
            group = new Group(Properties.LangResource.snack + ": " + points.Snack, "4", "lock.png");
            groupedItems.Add(group);
            group = new Group(Properties.LangResource.water + ": 0" , "5", "lock.png");
            groupedItems.Add(group);
            group = new Group(Properties.LangResource.exercise + ": " + points.Exercise, "6","lock.png");
            groupedItems.Add(group);

            foreach (MapLogData logrecord in logrecords)
            {
                Item item = new Item(logrecord.MealName, PointSystem.PointCalculate(logrecord.Calories).ToString(), logrecord.IDUserMeal, logrecord.IDCategory,false);
                groupedItems[logrecord.IDCategory - 1].Add(item);
            }

            foreach (Exercise exercise in exercises)
            {
                Item item = new Item(Properties.LangResource.exercise, PointSystem.PointExerciseCalculate(exercise.Minutes).ToString(), exercise.IDExercise,6, false);
                groupedItems[5].Add(item);
            }

        }



        public async void OnClickGridButton(object sender, EventArgs e)
        {
            if ((info.isTrial) || ((info.ActiveSubscription != null) && (info.ActiveSubscription.EndDate >= DateTime.UtcNow)))
            {
                var item = (Xamarin.Forms.Button)sender;
                int mode = int.Parse(item.CommandParameter.ToString());
                if (mode == 5)
                {
                    if (searchPage == null)
                    {
                        searchPage = new SearchMealPage();
                    }
                    SelectMealPage selectPage = searchPage.GetSelectMealPage();
                    IEnumerable <Meal> meals = conn.Query<Meal>("SELECT IDMeal,Name,Identifier FROM meal WHERE Deleted=0 AND Identifier=1 LIMIT 1");
                    foreach (Meal meal in meals)
                    {
                        //records.Add(new Meal { Name = meal.Name, IDMeal = meal.IDMeal });
                        //Meal myMeal = e.Item as Meal;
                        
                        //selectPage.IDMealSelected = meal.IDMeal;
                        selectPage.IDCategorySelected = mode;
                        selectPage.SelectedDate = SelectedDate;
                        selectPage.CalcUnits(meal.IDMeal);
                    }
                    await Navigation.PushAsync(selectPage);


                }
                else if (mode == 6)
                {
                    if (exerPage == null) {
                        exerPage = new AddExercisePage();
                    }
                    exerPage.LoadData(SelectedDate);
                    await Navigation.PushAsync(exerPage);
                }
                else
                {
                    if (searchPage == null)
                    {
                        searchPage = new SearchMealPage();
                    }
                    searchPage.SelectedDate = SelectedDate;
                    searchPage.Mode = mode;
                    await Navigation.PushAsync(searchPage);
                }
            }
            else
            {
                var answer = await App.Current.MainPage.DisplayAlert(Properties.LangResource.subscription, Properties.LangResource.subAlert, Properties.LangResource.yes, Properties.LangResource.no);
                if (answer == true)
                {
                    await Navigation.PushAsync(new ShopPage());
                }
            }

        }

        public async void OnClickDeleteButton(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            Item mode = (Item)(item.CommandParameter);
            for (int i = 0; i < groupedItems.Count; i++)
            {
                foreach (Item it in groupedItems[i])
                {
                    if (mode.ID == it.ID && mode.IDCategory == it.IDCategory)
                    {
                        if (it.IDCategory == 6)
                        {
                            Exercise exercise = conn.Get<Exercise>(it.ID);

                            if (exercise.IDServer == 0)
                            {
                                StorageManager.DeleteData<Exercise>(exercise);
                            }
                            else
                            {
                                exercise.Deleted = 1;
                                StorageManager.UpdateData<Exercise>(exercise);
                            }
                        }
                        else
                        {
                            UserMeal usermeal = conn.Get<UserMeal>(it.ID);

                            if (usermeal.IDServer == 0)
                            {
                                StorageManager.DeleteData<UserMeal>(usermeal);
                            }
                            else
                            {
                                usermeal.Deleted = 1;
                                StorageManager.UpdateData<UserMeal>(usermeal);
                            }
                        }
                        //sss
                        RecreateDataAction();
                        break;
                    }
                }
            }
        }
        public async void OnClickLockButton(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            int mode = int.Parse(item.CommandParameter.ToString());
            for (int i = 0; i < groupedItems.Count; i++)
            {
                foreach (Item it in groupedItems[i])
                {
                    if (mode == it.IDCategory)
                    {
                        it.isVisible = !it.isVisible;
                        if (it.isVisible && groupedItems[i].ImageN == "lock.png") {
                            groupedItems[i].ImageN = "unlock.png";
                        }
                        else if (!it.isVisible && groupedItems[i].ImageN == "unlock.png")
                        {
                            groupedItems[i].ImageN = "lock.png";
                        }
                    }
                }
            }
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var item = selectedItem.CommandParameter as Item;

            if (item.IDCategory == 6)
            {
                Exercise exercise = conn.Get<Exercise>(item.ID);

                if (exercise.IDServer == 0)
                {
                    StorageManager.DeleteData<Exercise>(exercise);
                }
                else
                {
                    exercise.Deleted = 1;
                    StorageManager.UpdateData<Exercise>(exercise);
                }
            }
            else
            {
                UserMeal usermeal = conn.Get<UserMeal>(item.ID);

                if (usermeal.IDServer == 0)
                {
                    StorageManager.DeleteData<UserMeal>(usermeal);
                }
                else
                {
                    usermeal.Deleted = 1;
                    StorageManager.UpdateData<UserMeal>(usermeal);
                }
            }
            //sss
            RecreateDataAction();
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if ((info.isTrial) || ((info.ActiveSubscription != null) && (info.ActiveSubscription.EndDate >= DateTime.UtcNow)))
            {
                int mode = ((Item)e.Item).IDCategory;
                if (mode == 6)
                {
                    if (exerPage == null)
                    {
                        exerPage = new AddExercisePage();
                    }
                    exerPage.LoadData(SelectedDate, ((Item)e.Item).ID);
                    await Navigation.PushAsync(exerPage);
                }
                else
                {
                    UserMeal usermeal = conn.Get<UserMeal>(((Item)e.Item).ID);
                    var item = (Item)e.Item;

                    if (searchPage == null)
                    {
                        searchPage = new SearchMealPage();
                    }
                    int IDMealSelected = conn.Get<MealUnit>(usermeal.IDMealUnit).IDMeal;
                    searchPage.GetSelectMealPage().CalcUnits(IDMealSelected,usermeal);
                    await Navigation.PushAsync(searchPage.GetSelectMealPage());
                }
            }
            else
            {
                var answer = await App.Current.MainPage.DisplayAlert(Properties.LangResource.subscription, Properties.LangResource.subAlert, Properties.LangResource.yes, Properties.LangResource.no);
                if (answer == true)
                {
                    await Navigation.PushAsync(new ShopPage());
                }
            }

        }
    }






    public class Group : ObservableCollection<Item>, INotifyPropertyChanged
    {
        public String Name { get; set; }
        public String CategoryID { get; set; }
        private string imageN;

        public string ImageN
        {
            get
            {
                return imageN;
            }
            set
            {
                if (imageN != value)
                {
                    imageN = value;
                    OnPropertyChanged("ImageN");
                }
            }
        }

        public Group(String Name, String CategoryID,string imageN)
        {
            this.Name = Name;
            this.CategoryID = CategoryID;
            this.imageN = imageN;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Whatever other properties
    }
    public class Item: INotifyPropertyChanged
    {
        public String Title { get;  set; }
        public String Description { get;  set; }

        public int ID { get; set; }
        public int IDCategory { get; set; }
        private bool isvisible;
        public Item itemS { get; set; }

        public bool isVisible
        {
            get
            {
                return isvisible;
            }
            set
            {
                if (isvisible != value)
                {
                    isvisible = value;
                    OnPropertyChanged("isVisible");
                }
            }
        }

        public Item(String title, String description, int id, int idcategory,bool isvisible)
        {
            Title = title;
            Description = description;
            ID = id;
            IDCategory = idcategory;
            isVisible = isvisible;
            itemS = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Whatever other properties
    }
}
