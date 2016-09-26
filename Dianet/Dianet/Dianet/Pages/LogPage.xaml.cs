using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class LogPage : ContentPage
    {
        public LogPage()
        {
            InitializeComponent();
        }

        async void OnAddMealClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMealPage());
        }

        async void OnAddWeightClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWeightPage());
        }
        void OnPrevDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(-1);
        }

        void OnNextDayClicked(object sender, EventArgs e)
        {
            datePick.Date = datePick.Date.AddDays(1);
        }
    }
}
