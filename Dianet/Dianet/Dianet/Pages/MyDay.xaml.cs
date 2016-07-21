using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class Myday : ContentPage
    {
        public Myday()
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
    }
}
