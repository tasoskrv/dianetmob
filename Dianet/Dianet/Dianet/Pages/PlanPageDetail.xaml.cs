using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;
using System;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class PlanPageDetail : ContentPage
    {
        private Plan pln;
        private SQLiteConnection conn = null;

        public PlanPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        public void LoadData(int IDPlan = 0)
        {
            if (IDPlan > 0)
            {
                fSaveBtn.IsVisible = true;
                pln = conn.Query<Plan>("SELECT Goal,GoalDate FROM Plan where IDPlan=" + IDPlan.ToString())[0];
            }
            else
            {
                fSaveBtn.IsVisible = false;
                pln = new Plan();        
            }
            BindingContext = pln;
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (fWeightEntry.Text == null || fWeightEntry.Text == "")
                DisplayAlert("Please", "fill in your desired weight", "OK");
            else            
                StorageManager.InsertData(pln);                        
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
