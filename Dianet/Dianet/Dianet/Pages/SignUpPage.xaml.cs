using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (CheckValidMail())
            {
                App.Current.MainPage = new MainPage();
            }
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (emailEntry.Text==null || !Regex.IsMatch(emailEntry.Text, pattern))
            {
                DisplayAlert("Please", "enter a valid email", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
