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

        Dictionary<string, int> weights = new Dictionary<string, int>
        {
            { Properties.LangResource.kgs, 1 }, {  Properties.LangResource.pounds, 2 }, {  Properties.LangResource.ounces, 3 }
        };

        Dictionary<string, int> diets = new Dictionary<string, int>
        {
            { Properties.LangResource.normal, 1 }, { "NK", 2 }, {  Properties.LangResource.ovum, 3 }, {  Properties.LangResource.sperm, 4 }
        };

        Dictionary<string, int> genders = new Dictionary<string, int>
        {
            { Properties.LangResource.male, 1 }, { Properties.LangResource.female, 2 }
        };

        public LoginProcessPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            foreach (string height in heights.Keys)
            {
                heighttype.Items.Add(height);
            }
            foreach (string weight in weights.Keys)
            {
                weighttype.Items.Add(weight);
            }
            foreach (string diet in diets.Keys)
            {
                fDietTypePicker.Items.Add(diet);
            }

            foreach (string gender in genders.Keys)
            {
                genderPicker.Items.Add(gender);
            }
            heighttype.SelectedIndexChanged += (object sender, EventArgs e) => { height.Focus(); };
            height.Completed += (object sender, EventArgs e) => { genderPicker.Focus(); };

            weighttype.SelectedIndexChanged += (object sender, EventArgs e) => { weight.Focus(); };
            weight.Completed += (object sender, EventArgs e) => { genderPicker.Focus(); };

            // genderPicker.SelectedIndexChanged += (object sender, EventArgs e) => { AgePicker.Focus(); };
            AgePicker.DateSelected += (object sender, DateChangedEventArgs e) => { weight.Focus(); };
        //    weight.Completed += (object sender, EventArgs e) => { WeightDatePicker.Focus(); };
            WeightDatePicker.DateSelected += (object sender, DateChangedEventArgs e) => { goal.Focus(); };
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (heighttype.SelectedIndex == -1 || genderPicker.SelectedIndex == -1 || height.Text.Equals("") || height.Text == null
                || weight.Text.Equals("") || weight.Text == null || WeightDatePicker.Date.Equals("") || WeightDatePicker.Date == null
                || goal.Text.Equals("") || goal.Text == null || AgePicker.Date.Equals(""))
            {
                await DisplayAlert("Error", "Please fill all the fields!", "OK");
            }
            else
            {
                if (AgePicker.Date.Year > DateTime.Now.Year - 6) {
                    await DisplayAlert("Error", "Too young to be on diet :D", "OK");
                    return;
                }
                saveBtn.IsEnabled = false;
                try
                {
                    int heightType = heights[heighttype.Items[heighttype.SelectedIndex]];
                    int weightType = heights[weighttype.Items[weighttype.SelectedIndex]];
                    int genderType = genders[genderPicker.Items[genderPicker.SelectedIndex]];

                    User loginuser = StorageManager.GetConnectionInfo().LoginUser;

                    //weight
                    Weight wght = new Weight();
                    wght.IDUser = loginuser.IDUser;
                    wght.WValue = Convert.ToInt16(weight.Text);
                    wght.WeightDate = WeightDatePicker.Date;
                    wght.Deleted = 0;
                    wght.UpdateDate = DateTime.UtcNow;
                    wght.InsertDate = wght.UpdateDate;
                    StorageManager.InsertData<Weight>(wght);

                    //goal
                    Plan plan = new Plan();
                    plan.IDUser = loginuser.IDUser;
                    plan.Goal = Convert.ToInt16(goal.Text);
                    plan.StartGoal = DateTime.Now.Date;
                    plan.Deleted = 0;
                    plan.UpdateDate = DateTime.UtcNow;
                    plan.InsertDate = plan.UpdateDate;
                    StorageManager.InsertData<Plan>(plan);

                    //TODO update user data
                    loginuser.Gender = genderType;
                    loginuser.HeightType = heightType;
                    loginuser.WeightType = weightType;
                    loginuser.Height = Convert.ToDouble(height.Text);
                    loginuser.Birthdate = AgePicker.Date;
                    loginuser.UpdateDate = DateTime.UtcNow;
                    StorageManager.UpdateData<User>(loginuser);
                    App.Current.MainPage = new MainPage();
                }
                finally {
                    saveBtn.IsEnabled = true;
                }
            }
            
        }        
    }
}
