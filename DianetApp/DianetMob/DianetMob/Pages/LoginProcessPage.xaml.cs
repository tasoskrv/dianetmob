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
    public partial class LoginProcessPage : ContentPage
    {
        SQLiteConnection conn = null;

        Dictionary<string, int> heights = new Dictionary<string, int>
        {
            { "Cm", 1 }, { "In", 2 } 
        };

        Dictionary<string, int> genders = new Dictionary<string, int>
        {
            { "Male", 1 }, { "Female", 2 }
        };
        
        public LoginProcessPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            foreach (string height in heights.Keys)
            {
                heighttype.Items.Add(height);
            }
            foreach (string gender in genders.Keys)
            {
                genderPicker.Items.Add(gender);
            }
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {

            if (heighttype.SelectedIndex == -1 || genderPicker.SelectedIndex == -1 || height.Text.Equals("") || height.Text == null
                || weight.Text.Equals("") || weight.Text == null || WeightDatePicker.Date.Equals("") || WeightDatePicker.Date == null
                || goal.Text.Equals("") || goal.Text == null || GoalDatePicker.Date.Equals("") || GoalDatePicker.Date == null)
            {
                MessageLabel.Text = "Fill all fields";
            }
            else
            {
                int heightType = heights[heighttype.Items[heighttype.SelectedIndex]];
                int genderType = genders[genderPicker.Items[genderPicker.SelectedIndex]];



                Weight wght = new Weight();
                wght.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                wght.WValue = 150;
                wght.WeightDate = DateTime.UtcNow;
                wght.Deleted = 0;
                wght.UpdateDate = DateTime.UtcNow;
                wght.InsertDate = wght.UpdateDate;
                StorageManager.InsertData<Weight>(wght);

                //TODO update user data
                StorageManager.UpdateData<User>(StorageManager.GetConnectionInfo().LoginUser);
            }
        }        
    }
}
