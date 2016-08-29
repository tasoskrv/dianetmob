using System;
using Xamarin.Forms;

namespace Dianet.Pages
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
