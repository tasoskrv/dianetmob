using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
{
    public partial class AddMealPage : ContentPage
    {
        private SearchMealPage searchPage = new SearchMealPage();
        private DateTime SelectedDate;
        public AddMealPage( DateTime date)
        {
            SelectedDate = date;
            searchPage.SelectedDate = date;
            InitializeComponent();
        }
        async void OnAddBreakfastClicked(object sender, EventArgs e)
        {
            searchPage.Mode = 1;
            await Navigation.PushAsync(searchPage);
        }
        async void OnAddLunchClicked(object sender, EventArgs e)
        {
            searchPage.Mode = 2;
            await Navigation.PushAsync(searchPage);
        }
        async void OnAddDinnerClicked(object sender, EventArgs e)
        {
            searchPage.Mode = 3;
            await Navigation.PushAsync(searchPage);
        }
        async void OnAddSnackClicked(object sender, EventArgs e)
        {
            searchPage.Mode = 4;
            await Navigation.PushAsync(searchPage);
        }
        async void OnAddExerciseClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new AddMealPage());
        }
        async void OnAddRecordWeightClicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new AddMealPage());
        }
    }
}
