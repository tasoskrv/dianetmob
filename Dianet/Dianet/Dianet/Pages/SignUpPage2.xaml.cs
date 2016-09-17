using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SignUpPage2 : ContentPage
    {
        public SignUpPage2()
        {
            InitializeComponent();
        }

        public void OnSexChanged(object sender, EventArgs eventArgs)
        {
            
        }

        public void OnHeightChanged(object sender, EventArgs eventArgs)
        {

        }

        public void OnWeightChanged(object sender, EventArgs eventArgs)
        {

        }

        public void OnWristChanged(object sender, EventArgs eventArgs)
        {

        }

        public void OnStartButtonClicked(object sender, EventArgs eventArgs)
        {
            App.Current.MainPage = new MainPage();
        }
        
    }
}
