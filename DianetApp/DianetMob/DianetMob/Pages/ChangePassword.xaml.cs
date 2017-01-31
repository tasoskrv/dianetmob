using Dianet.Notification;
using DianetMob.DB;
using DianetMob.DB.Entities;
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
        
        private void OnUpdatePasswordClicked(object sender, EventArgs e)
        {                       
            if (newPassword.Equals("") || newPassword == null || newPasswordRetype.Equals("") || newPasswordRetype == null)
            {

            }
            else
            {
                //  var user = StorageManager.GetConnectionInfo().LoginUser;
                user.UpdateDate = DateTime.UtcNow;
                //  int heightType = heights[fHeightPicker.Items[fHeightPicker.SelectedIndex]];
                //  int genderType = genders[fSexPicker.Items[fSexPicker.SelectedIndex]];
                //  user.FirstName = fFirstNameEntry.Text;
                //  user.LastName = fSurNameEntry.Text;
                //  user.Birthdate = fbirthDatePicker.Date;
                //  user.Gender = genderType;
                //  user.HeightType = heightType;
                //    user.Height = Convert.ToDouble(fHeightEntry.Text);
                //  user.Skeleton = Convert.ToDouble(fSkeletonEntry.Text);
                //   user.Location = fLocationEntry.Text;                        
                StorageManager.UpdateData(user);
            }                        
        }             
    }
}
