using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class StartPage : ContentPage
    {
        private LoginPage FLoginPage { get; set; }
        private SignUpPage FSignUpPage { get; set; }

        public StartPage()
        {
            InitializeComponent();
        }

        void OnClickLoginPage(object sender, EventArgs e)
        {
            FLoginPage = new LoginPage();
            App.Current.MainPage = FLoginPage;
        }

        void OnClickSignUpPage(object sender, EventArgs e)
        {
            FSignUpPage = new SignUpPage();
            App.Current.MainPage = FSignUpPage;
        }
    }
}
