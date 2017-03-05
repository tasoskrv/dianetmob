using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Pages;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Views
{
    public partial class AddMealView : ContentView
    {
        private SearchMealPage searchPage = new SearchMealPage();
        private AddExercisePage exerPage = new AddExercisePage();
        private DateTime SelectedDate;
        private SelectMealPage selectPage = new SelectMealPage();
        private ConnectionInfo info;
        public MyDay MyDayPage { get; set; }
        private SQLiteConnection conn = null;

        public AddMealView()
        {
            InitializeComponent();
            info = StorageManager.GetConnectionInfo();
            conn = StorageManager.GetConnection();
        }
        public async void GotoPage(int mode)
        {
            if ((info.isTrial) || ((info.ActiveSubscription != null) && (info.ActiveSubscription.EndDate >= DateTime.UtcNow)))
            {
                searchPage.Mode = mode;
                //MyDayPage.ToggleAddView();

                if (mode == 5)
                {                    
                    IEnumerable<Meal> meals = conn.Query<Meal>("SELECT IDMeal,Name,Identifier FROM meal WHERE Deleted=0 AND Identifier=1 LIMIT 1");
                    foreach (Meal meal in meals)
                    {
                        //records.Add(new Meal { Name = meal.Name, IDMeal = meal.IDMeal });
                        //Meal myMeal = e.Item as Meal;
                       // selectPage.IDMealSelected = meal.IDMeal;
                        selectPage.IDCategorySelected = mode;
                        selectPage.SelectedDate = SelectedDate;
                        selectPage.CalcUnits(meal.IDMeal);                        
                    }
                    await Navigation.PushAsync(selectPage);                   
                }
                else if (mode == 6) {
                    exerPage.LoadData(SelectedDate);
                    await Navigation.PushAsync(exerPage);
                }
                else
                {
                    await Navigation.PushAsync(searchPage);
                }                    
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Subscription", "Your subscription has expired. Please renew.", "OK");
            }
        }

        public void setDate(DateTime date) {
             SelectedDate = date;
             searchPage.SelectedDate = date;
        }

        public void OnAddBreakfastClicked(object sender, EventArgs e)
        {
            GotoPage(1);
        }

        public void OnAddLunchClicked(object sender, EventArgs e)
        {
            GotoPage(2);
        }

        public void OnAddDinnerClicked(object sender, EventArgs e)
        {
            GotoPage(3);
        }

        public void OnAddSnackClicked(object sender, EventArgs e)
        {
            GotoPage(4);
        }

        public void OnAddExerciseClicked(object sender, EventArgs e)
        {
            GotoPage(6);
        }

        void OnAddWaterClicked(object sender, EventArgs e)
        {
            GotoPage(5);
        }
    }
}
