using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using DianetMob.Utils;
using Java.IO;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class ChangePassword : ContentPage
    {
        private SQLiteConnection conn = null;
        User user = null;

        
        public ChangePassword()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private void OnProfileSubmitClicked(object sender, EventArgs e) { }
        private void OnUploadClicked(object sender, EventArgs e) { }
        

        async void OnUpdatePasswordClicked(object sender, EventArgs e)
        {                       
            if (newPassword.Text == null || newPasswordRetype.Text == null || newPassword.Text.Equals("") || newPasswordRetype.Text.Equals(""))
            {
                await DisplayAlert("Warning", "Fill all fields", "OK");
            }
            else
            {                               
                try
                {
                    User user = new User();
                    user.Password = newPassword.Text;
                    user.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                    ModelService<User> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<User>>("/user/update/", user);
                    if ((srvNewUser.success == true) && (srvNewUser.ID > 0) && !(srvNewUser.ErrorCode > 0))
                    {
                        await DisplayAlert("Message", "Password updated", "OK");
                        /*
                        user.IDUser = srvNewUser.ID;
                        user.AccessToken = srvNewUser.AccessToken;
                        srvNewUser.InsertRecordToDB(user);
                        App.Current.MainPage = new Registered();
                        */
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Warning", srvNewUser.message, "OK");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");                    
                }
            }                        
        }             
    }
}
