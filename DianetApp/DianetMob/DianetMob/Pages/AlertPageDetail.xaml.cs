using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Utils;
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
            if (myalt.IDAlert <= 0) {
                alt = new Alert();
                alt.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                alt.MealType = myalt.MealType;
                remindTime.SelectedIndex = 0;
            }
            else{
                alt = conn.Get<Alert>(myalt.IDAlert);
                for (int i = 0; i < remindTime.Items.Count - 1; i++) {
                    if (remindTime.Items[i] == alt.AlertTime)
                    {
                        remindTime.SelectedIndex = i;
                        break;
                    }
                        
                 }
            }
            
            Title = alt.MealDisplay+" Notification";
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
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.fillrecur, "OK");
            }
            else if (alt.IDAlert > 0)
            {
                StorageManager.UpdateData(alt);
                NotifyLoacalServices(alt);
                new AlertPage();
                Navigation.PopAsync();
            }
            else
            {
                alt.InsertDate = alt.UpdateDate;
                StorageManager.InsertData(alt);
                NotifyLoacalServices(alt);
                new AlertPage();
                Navigation.PopAsync();
            }  
                                           
        }

        private void NotifyLoacalServices(Alert alt) {
            if (alt.Status == 0)
            {
                if (GenLib.NotifAlerts.ContainsKey(alt.IDAlert))
                {
                    GenLib.NotifAlerts.Remove(alt.IDAlert);
                    GenLib.CalcNextAlert();
                }
            }
            else if (alt.Status == 1){
                if (GenLib.NotifAlerts.ContainsKey(alt.IDAlert))
                {
                    GenLib.CalcNextAlert();
                }
                else {
                    GenLib.NotifAlerts.Add(alt.IDAlert, alt);
                    GenLib.CalcNextAlert();
                }
            }
        }
    }
}
