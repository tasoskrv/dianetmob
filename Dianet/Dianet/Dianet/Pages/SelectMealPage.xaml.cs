using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SelectMealPage : ContentPage
    {
        public SelectMealPage()
        {           
            InitializeComponent();
        }

        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;

            if (unitPicker.SelectedIndex > 1)
            {
                counter1Picker.IsEnabled = true;
                counter2Picker.IsEnabled = true;
                counter1Picker.SelectedIndex = 1;
                counter2Picker.SelectedIndex = 1;
            }
            else
            {
                counter1Picker.SelectedIndex = 0;
                counter2Picker.SelectedIndex = 0;
                counter1Picker.IsEnabled = false;
                counter2Picker.IsEnabled = false;
            }            
        }

        async void OnChooseBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Myday());
        }
    }
}
