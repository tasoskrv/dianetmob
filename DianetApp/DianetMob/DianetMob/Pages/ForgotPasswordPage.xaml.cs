using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Model;
using DianetMob.Service;
using DianetMob.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class ForgotPasswordPage : ContentPage
    {
        SQLiteConnection conn = null;

        public ForgotPasswordPage()
        {
            InitializeComponent(); 
        }

        private async void OnMailSendButtonClicked(object sender, EventArgs e)
        {
            if (emailEntry.Text == "" || emailEntry.Text == null)
            {
                MessageLabel.Text = "Enter your email";
                return;
            }

            bool isValid = GenLib.CheckValidMail(emailEntry.Text);
            if (!isValid)
            {
                MessageLabel.Text = "Please enter a valid email";
            }
            else
            {                
                mailSend.IsEnabled = false;
                User user = new User();
                user.Email = emailEntry.Text;                
                ModelService<User> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/recover/", user);
                if (srvNewUser.success == true)
                {
                    user.IDUser = srvNewUser.ID;
                    user.AccessToken = srvNewUser.AccessToken;
                    srvNewUser.InsertRecordToDB(user);
                    mailSend.IsEnabled = true;
                    App.Current.MainPage = new LoginPage();
                    return;
                }
                else if (srvNewUser.ErrorCode == 2)
                {
                    MessageLabel.Text = "User not found";
                }
                else
                {
                    MessageLabel.Text = "Warning - " + srvNewUser.message;
                }
                return;


























            }
        }        
    }
}
