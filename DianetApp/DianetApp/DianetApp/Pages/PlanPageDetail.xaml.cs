using DianetApp.DB;
using DianetApp.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
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
                pln = conn.Get<Plan>(IDPlan);
            else
            {
                pln = new Plan();
                pln.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
            BindingContext = pln;
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            pln.UpdateDate = DateTime.UtcNow;
            if (pln.Goal <= 0)
                DisplayAlert("Please", "fill in your desired weight", "OK");
            else if (pln.IDPlan > 0)
            {
                StorageManager.UpdateData(pln);
                Navigation.PopAsync();
            }
            else
            {
                //pln.InsertDate = pln.UpdateDate;
                StorageManager.InsertData(pln);
                Navigation.PopAsync();
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
