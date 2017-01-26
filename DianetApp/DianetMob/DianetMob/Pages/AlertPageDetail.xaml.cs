using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;

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

        public void LoadData(Alert myalt)
        {
            if (myalt.IDAlert > 0)
                alt = conn.Get<Alert>(myalt.IDAlert);
            else
            {
                alt = new Alert();
                alt.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                alt.MealType = myalt.MealType;
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
                new AlertPage();
                Navigation.PopAsync();
            }                                 
        }
    }
}
