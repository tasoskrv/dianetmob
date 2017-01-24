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
        /*
        public void LoadData(int IDAlert = 0)
        {            
            if (IDAlert > 0)
                alt = conn.Get<Alert>(IDAlert);
            else
            {
                alt = new Alert();
                alt.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }            
            BindingContext = alt;
            
            //List<Alert> alts = conn.Query<Alert>("Select * from alert order by insertdate limit 1");
            
            //if (alts.Count > 0)
            //{
            //    //wgh = wghs[0];
            //}
            //else
            //{
            //    DisplayAlert("Please", "fill in your current weight", "OK");
            //    Navigation.PopAsync();
            //}            
        }

        public void OnSaveAlertClicked(object sender, EventArgs e)
        {
            alt.UpdateDate = DateTime.UtcNow;
            if (alt.AlertTime == null)
                DisplayAlert("Please", "Fill Alert Time", "OK");
            else if (alt.Recurrence == null)
            {
                DisplayAlert("Please", "Fill Recurrence", "OK");
            }
            else if (alt.Description == null)
            {
                DisplayAlert("Please", "Fill Description", "OK");
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
                AlertPage.recordsAlt.Add(alt);                
                Navigation.PopAsync();
            }    
        }
        */
    }
}
