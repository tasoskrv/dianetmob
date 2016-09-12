
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dianet.Model;
using Dianet.DB;
using SQLite;
using Dianet.DB.Entities;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using Dianet.Service;
using System.Net.Http;

namespace Dianet.Pages
{
    public partial class SearchMealPage : ContentPage
    {
        private ObservableCollection<SearchRecord> records = new ObservableCollection<SearchRecord>();
        protected string url = "http://dianet.cloudocean.gr/api/v1/meal/getall";
        public int Mode { get; set; }
        SQLiteConnection conn=null;
        public SearchMealPage()
        {
            InitializeComponent();
            ListViewSearch.ItemsSource = records;
            conn =StorageManager.GetConnection();

        }

        public void OnSearchBarPressed(object sender, EventArgs eventArgs)
        {
           
        }

        public void OnSearchBarTextChanged(object sender, EventArgs eventArgs)
        {
            records.Clear();
            IEnumerable<Meal> meals= conn.Query<Meal>("SELECT name FROM meal WHERE name LIKE ?", "%"+ASearchBar.Text+"%");
            foreach (Meal meal in meals)
            {
                records.Add(new SearchRecord { DisplayName = meal.Name });
            }
        }

        protected override async void OnAppearing()
        {
            if (Mode == 1)
            {
                Title = "Breakfast";
            }
            else if (Mode == 2) {
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
