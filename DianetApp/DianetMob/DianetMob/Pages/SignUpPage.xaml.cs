using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using DianetMob.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            StorageManager.GetConnection();
            nameEntry.Completed += (object sender, EventArgs e) => { surnameEntry.Focus(); };
            surnameEntry.Completed += (object sender, EventArgs e) => { emailEntry.Focus(); };
            emailEntry.Completed += (object sender, EventArgs e) => { passwdEntry.Focus(); };
            passwdEntry.Completed += (object sender, EventArgs e) => { passwdRetype.Focus(); };
        }

        private void OnCancelButtonTap(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        async void OnSubmitButtonTap(object sender, EventArgs e)
        {
            try
            {
                if (AllFieldsAreFilled() && CheckValidMail() && CheckPassword())
                {
                    //submitBtn.IsEnabled = false;
                    int token = GetRegistrationToken(emailEntry.Text, passwdEntry.Text);
                    User user = new User();
                    user.FirstName = nameEntry.Text;
                    user.LastName = surnameEntry.Text;
                    user.Email = emailEntry.Text;
                    user.Password = passwdEntry.Text;
                    user.InsertDate = DateTime.UtcNow;
                    user.UpdateDate = user.InsertDate;
                    //user.Birthdate = birthDate.Date;
                    user.AccessToken = token.ToString();
                    ModelService<User> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/insert/", user);
                    if ((srvNewUser.success == true) && (srvNewUser.ID > 0) && !(srvNewUser.ErrorCode > 0))
                    {
                        // user.InsertDate = DateTime.UtcNow;
                        // user.UpdateDate = user.InsertDate;
                        user.IDUser = srvNewUser.ID;
                        user.AccessToken = srvNewUser.AccessToken;
                        srvNewUser.InsertRecordToDB(user);
                        submitBtn.IsEnabled = true;
                        App.Current.MainPage = new Registered();
                        return;
                    }
                    else if (srvNewUser.ErrorCode == 2)
                    {
                        MessageLabel.Text = "User already exists";
                    }
                    else
                    {
                        MessageLabel.Text = "Warning - " + srvNewUser.message;
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text = "Error" + ex.Message;
            }
        }

        private int GetRegistrationToken(string mail, string pass)
        {
            return (mail.Length * pass.Length * 9999888);
        }

        void OnValidateEmail(object sender, EventArgs e)
        {
            CheckValidMail();
        }

        private bool CheckValidMail()
        {
            bool isValid = GenLib.CheckValidMail(emailEntry.Text);
            if (!isValid)
            {
                MessageLabel.Text = "Please enter a valid email";
            }
            return isValid;
        }

        private bool AllFieldsAreFilled()
        {
            if (emailEntry.Text == null || nameEntry.Text == null || surnameEntry.Text == null || passwdEntry.Text == null || passwdRetype.Text == null ||
                emailEntry.Text == "" || nameEntry.Text == "" || surnameEntry.Text == "" || passwdEntry.Text == "" || passwdRetype.Text == "")
            {
                MessageLabel.Text = "Please fill all fields";
                return false;
            }
            return true;            
        }

        private bool CheckPassword()
        {
            if (passwdEntry.Text != passwdRetype.Text)
            {
                MessageLabel.Text = "Passwords do not match";
                return false;
            }
            return true;
        }
    }
}
