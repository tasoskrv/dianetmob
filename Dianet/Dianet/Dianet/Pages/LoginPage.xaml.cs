using Dianet.DB.Entities;
using Dianet.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Dianet.DB;
using System.Collections.Generic;
using Dianet.Factory;

namespace Dianet.Pages
{
    public partial class LoginPage : ContentPage
    {
        SQLiteConnection conn = null;
        public LoginPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignUpPage();
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (usernameEntry.Text == null || !Regex.IsMatch(usernameEntry.Text, pattern))
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
            MealService serv = await ServiceConnector.GetServiceData<MealService>("/meal/getall");
            serv.InsertMeals();
           //Κληση InsertMeal
                     
            var user = new User
            {
                Email = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            if (CheckValidMail())
            {
                var isValid = AreCredentialsCorrect(user);
                if (isValid)
                {
                    App.IsUserLoggedIn = true;
                    App.Current.MainPage = new MainPage();
                    //Navigation.InsertPageBefore(new MainPage(), this);
                }
            }
                        //await Navigation.PopAsync();
            //  }
            //   else
            //   {
            //       messageLabel.Text = "Login failed";
            //       passwordEntry.Text = string.Empty;
            //  }
        }

        bool AreCredentialsCorrect(User user)
        {
            //int usrCount = 0;
            int usrCount = 1;
            IEnumerable<User> usrs = conn.Query<User>("SELECT IdUser FROM User WHERE Email ='" + user.Email + "' AND Password ='" + user.Password + "'");
            foreach (User usr in usrs)
            {
                usrCount++;
                if (usr.IdUser != 0)
                    return true;
            }

            //int usrMlCount = 0;
            int usrMlCount = 1;
            IEnumerable<User> usrMls = conn.Query<User>("SELECT IdUser FROM User WHERE Email = '" + user.Email + "'");
            foreach (User usr in usrMls)
            {
                usrMlCount++;
                if (usr.IdUser != 0)
                    return true;
            }
            
            if (usrCount == 1)
            {
                return true;
            }
            else
            {
                if ((usrCount == 0) && (usrMlCount == 1))
                {
                    DisplayAlert("Sorry", "the password you entered is wrong", "Forgot it?");
                    return false;
                }
                else
                {
                    DisplayAlert("Sorry", "you are not logged in", "Please sign up");
                    return false;
                }
            }
            //return user.Username == Constants.Username && user.Password == Constants.Password;            
        }
    } 
}
