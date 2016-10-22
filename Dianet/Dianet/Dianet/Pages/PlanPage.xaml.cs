using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class PlanPage : ContentPage
    {
        SQLiteConnection conn = null;

        public PlanPage()
        {
            InitializeComponent();
            BindingContext = StorageManager.GetConnectionInfo().LoginUserPlan;
            conn = StorageManager.GetConnection();
        }

        private void OnSavePlanClicked(object sender, EventArgs e)
        {
            if (fWeightEntry.Text == null || fWeightEntry.Text == "")
                DisplayAlert("Please", "fill in your desired weight", "OK");
            else
            {
                StorageManager.InsertData(StorageManager.GetConnectionInfo().LoginUserPlan);
                /*List<Plan> pln = conn.Query<Plan>("SELECT * FROM Plan");
                if (pln.Count > 0)
                {
                    //pln[0]                    
                    return;
                }*/
                StorageManager.GetConnectionInfo().LoginUserPlan.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;                
            }
        }

        private void OnGoalDateChanged(object sender, EventArgs e)
        {
            if (fGoalDate.Date < DateTime.Today)
            { 
                DisplayAlert("Sorry", "you cannot insert date less than today", "OK");
                fGoalDate.Date = DateTime.Today;
            }
        }        
    }
}
