using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;

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
            saveBtn.IsEnabled = false;
            if (heighttype.SelectedIndex == -1 || genderPicker.SelectedIndex == -1 || height.Text.Equals("") || height.Text == null
                || weight.Text.Equals("") || weight.Text == null || WeightDatePicker.Date.Equals("") || WeightDatePicker.Date == null
                || goal.Text.Equals("") || goal.Text == null)
            {
                MessageLabel.Text = "Fill all fields";
            }
            else
            {
                int heightType = heights[heighttype.Items[heighttype.SelectedIndex]];
                int genderType = genders[genderPicker.Items[genderPicker.SelectedIndex]];

                //weight
                Weight wght = new Weight();
                wght.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                wght.WValue = Convert.ToInt16(weight.Text);
                wght.WeightDate = WeightDatePicker.Date;
                wght.Deleted = 0;
                wght.UpdateDate = DateTime.UtcNow;
                wght.InsertDate = wght.UpdateDate;
                StorageManager.InsertData(wght);

                //goal
                Plan plan = new Plan();
                plan.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                plan.Goal = Convert.ToInt16(goal.Text);
                plan.Status = 1;
                plan.Deleted = 0;
                plan.UpdateDate = DateTime.UtcNow;
                plan.InsertDate = plan.UpdateDate;
                StorageManager.InsertData(plan);

                //TODO update user data
                ConnectionInfo info = StorageManager.GetConnectionInfo();
                info.LoginUser.Gender = genderType;
                info.LoginUser.HeightType = heightType;
                info.LoginUser.Height = Convert.ToDouble(height.Text);
                info.LoginUser.UpdateDate = DateTime.UtcNow;
                StorageManager.UpdateData(info.LoginUser);
                App.Current.MainPage = new MainPage();
            }
            saveBtn.IsEnabled = true;
        }        
    }
}
