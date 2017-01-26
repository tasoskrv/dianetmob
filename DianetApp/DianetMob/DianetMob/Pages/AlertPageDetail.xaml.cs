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
        private Alert alt;
        private SQLiteConnection conn = null;
        

        public AlertPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        public void LoadData(/*Alert alt*/int IDAlert)
        {
            if (/*alt.IDAlert*/ IDAlert > 0)
                alt = conn.Get<Alert>(IDAlert/*alt.IDAlert*/);
            else
            {
                alt = new Alert();
                alt.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                alt.MealType = alt.MealType;
                alt.MealDisplay = alt.MealDisplay;
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
            alt.UpdateDate = DateTime.UtcNow;

            if (remindTime.SelectedIndex != -1)
                alt.AlertTime = remindTime.Items[remindTime.SelectedIndex];
            else
                alt.AlertTime = "";

            alt.Status = (remindSelect.IsToggled) ? 1 : 0 ;
            //myalt.MealType = 
            
            if ((alt.AlertTime.Equals("") || alt.AlertTime == null) && alt.Status == 1)
            {
                DisplayAlert("Please", "Fill Recurrence", "OK");
            }
            else if (alt.IDAlert > 0)
            {
                StorageManager.UpdateData(alt);
                new AlertPage();
                Navigation.PopAsync();
            }
            else
            {
                alt.InsertDate = alt.UpdateDate;
                StorageManager.InsertData(alt);         
                Navigation.PopAsync();
            }                            
        }
    }
}
