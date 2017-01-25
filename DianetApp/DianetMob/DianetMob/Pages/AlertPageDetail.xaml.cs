using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class AlertPageDetail : ContentPage
    {
        private Alert myalt;
        private SQLiteConnection conn = null;
        
        public AlertPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        public void LoadData(Alert alt)
        {
            if (alt.IDAlert > 0)
                alt = conn.Get<Alert>(alt.IDAlert);
            else
            {
                myalt = new Alert();
                myalt.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                myalt.MealType = alt.MealType;
                myalt.MealDisplay = alt.MealDisplay;
                /*
                myalt.AlertTime = alt.AlertTime;
                myalt.MealType = alt.MealType;
                myalt.Status = alt.Status;*/
                //genders[genderPicker.Items[genderPicker.SelectedIndex]]
                //remindTime.SelectedIndex = remindTime.Items[remindTime.SelectedIndex];
            }
            BindingContext = alt;
        }
        
        public void OnSaveAlertClicked(object sender, EventArgs e)
        {
            myalt.UpdateDate = DateTime.UtcNow;

            myalt.AlertTime = remindTime.Items[remindTime.SelectedIndex];
            myalt.Status = (remindSelect.IsToggled) ? 1 : 0 ;
            //myalt.MealType = 


            if (myalt.AlertTime == null)
                DisplayAlert("Please", "Fill Alert Time", "OK");
            else if (myalt.AlertTime.Equals("") || myalt.AlertTime == null)
            {
                DisplayAlert("Please", "Fill Recurrence", "OK");
            }/*
            else if (alt.Description == null)
            {
                DisplayAlert("Please", "Fill Description", "OK");
            }*/
            else if (myalt.IDAlert > 0)
            {
                StorageManager.UpdateData(myalt);
                new AlertPage();
                Navigation.PopAsync();
            }
            else
            {
                myalt.InsertDate = myalt.UpdateDate;
                StorageManager.InsertData(myalt);
                //AlertPage.recordsAlt.Add(alt);                
                Navigation.PopAsync();
            }  
                          
        }
    }
}
