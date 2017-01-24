using DianetMob.DB;
using DianetMob.Pages;
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
        private DateTime SelectedDate;
        public AddMealView()
        {
            InitializeComponent();

        }
        public async void GotoPage(int mode)
        {
            if (StorageManager.GetConnectionInfo().ActiveSubscription.EndDate < DateTime.UtcNow)
            {
                await App.Current.MainPage.DisplayAlert("Subscription", "Your subscription haw expired. Please renew.", "OK");
            }
            else
            {
                searchPage.Mode = mode;
                await Navigation.PushAsync(searchPage);
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

        void OnAddExerciseClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new AddMealPage());
        }

        void OnAddRecordWeightClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddMealPage());
        }
    }
}
