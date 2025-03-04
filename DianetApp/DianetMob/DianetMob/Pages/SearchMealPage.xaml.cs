﻿using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Utils;
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
    public partial class SearchMealPage : ContentPage
    {
        private SelectMealPage selectPage = new SelectMealPage();
        private ObservableCollection<Meal> records = new ObservableCollection<Meal>();
        protected string url = "http://dianet.cloudocean.gr/api/v1/meal/getall";
        public int Mode { get; set; }
        public DateTime SelectedDate { get; set; }
        SQLiteConnection conn = null;
        ConnectionInfo info=null;

        public SearchMealPage()
        {
            InitializeComponent();
            ListViewSearch.ItemsSource = records;
            conn = StorageManager.GetConnection();
            info = StorageManager.GetConnectionInfo();

        }

        public SelectMealPage GetSelectMealPage() {
            return selectPage;
        }


        public void OnSearchBarPressed(object sender, EventArgs eventArgs)
        {

        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Meal myMeal = e.Item as Meal;
            //var pg = new SelectMealPage();
            //var answer = await DisplayActionSheet("Add " + Title + " ?", "cancel", null, "ΝΑΙ", "ΟΧΙ");
            //await Navigation.PushModalAsync(SelectMealPage);

            //await  Navigation.PushModalAsync(pg);
            //if (answer == "ΝΑΙ")
            //{
               // selectPage.IDMealSelected = myMeal.IDMeal;
                selectPage.IDCategorySelected = Mode;
                selectPage.SelectedDate = SelectedDate;
                selectPage.CalcUnits(myMeal.IDMeal);
                ASearchBar.Text = "";
                await Navigation.PushAsync(selectPage);
            //}
        }

        public void OnSearchBarTextChanged(object sender, EventArgs eventArgs)
        {
            records.Clear();
            string str= GenLib.NormalizeGreek(GenLib.GreeklishToGreek(ASearchBar.Text));
            if (ASearchBar.Text.Equals(""))
                return;

            IEnumerable<Meal> meals = null;
            if (info.LoginUser.Fertility == 0)
            {
                meals = conn.Query<Meal>("SELECT name, IDMeal, NormalizedName, fertility FROM meal WHERE Deleted=0 AND (NormalizedName LIKE '" + str + "%' or NormalizedName LIKE '" + ASearchBar.Text + "%') " + " and IDLang=" + info.Settings.Lang  + " and isactive=1");
            }
            else
            {
                meals = conn.Query<Meal>("SELECT name, IDMeal, NormalizedName FROM meal WHERE Deleted=0 AND (NormalizedName LIKE '" + str + "%' or NormalizedName NormalizedNameLIKE '" + ASearchBar.Text + "%') and IDLang=" + info.Settings.Lang + " and isactive=1 and fertility like '%" + info.LoginUser.Fertility.ToString() + "%'");
            }

            foreach (Meal meal in meals)
            {
                records.Add(new Meal { Name = meal.Name, IDMeal = meal.IDMeal });
            }
        }

        protected override void OnAppearing()
        {
            if (Mode == 1)
            {
                Title = "Breakfast";
            }
            else if (Mode == 2)
            {
                Title = "Lunch";
            }
            else if (Mode == 3)
            {
                Title = "Dinner";
            }
            else if (Mode == 4)
            {
                Title = "Snack";
            }
            records.Clear();
            // string url = "http://dianet.cloudocean.gr/api/v1/meal/getall";


            //Task<MealService> response = ServiceConnection.GetServiceData<MealService>("meal/getall");
            //-------------------------------------------------------------------------------------
            // HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            // request.ContentType = "application/json";
            // request.Method = "GET";

            //WebResponse response = await request.GetResponseAsync();
            // Stream stream = response.GetResponseStream();
            //-------------------------------------------------------------------------------------
            //var httpClient = new HttpClient();
            // HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            //var response = await httpClient.SendAsync(request);

            //   MealService service = JsonConvert.DeserializeObject<MealService>(json);

        }
    }
}
