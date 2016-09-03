using Dianet.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (usernameEntry.Text==null || !Regex.IsMatch(usernameEntry.Text, pattern))
            {
                DisplayAlert("Please", "enter a valid email", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //Παράδειγμα κλήσης service
            //MealService serv = await ServiceConnector.GetServiceData<MealService>("/meal/getall");
            if (CheckValidMail())
            { 
               App.Current.MainPage = new MainPage();
            }
            // var user = new User
            // {
            //     Username = usernameEntry.Text,
            //     Password = passwordEntry.Text
            // };

            // var isValid = AreCredentialsCorrect(user);
            // if (isValid)
            // {
            //     App.IsUserLoggedIn = true;
            //      Navigation.InsertPageBefore(new MainPage(), this);
            //await Navigation.PopAsync();
            //  }
            //   else
            //   {
            //       messageLabel.Text = "Login failed";
            //       passwordEntry.Text = string.Empty;
            //  }
        }

       // bool AreCredentialsCorrect(User user)
       // {
          //  return user.Username == Constants.Username && user.Password == Constants.Password;
      //  }
    }
}
