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
    public partial class PlanPageDetail : ContentPage
    {
        private Plan pln;
        private SQLiteConnection conn = null;

        public PlanPageDetail()
        {
            InitializeComponent();

            
            conn = StorageManager.GetConnection();
        }

        void OnPrevDayClicked(object sender, EventArgs e)
        {
            goaldate.Date = goaldate.Date.AddDays(-1);
        }

        void OnNextDayClicked(object sender, EventArgs e)
        {
            goaldate.Date = goaldate.Date.AddDays(1);
        }

        protected override void OnAppearing()
        {
            GoalEntry.Focus();
        }

        public void LoadData()
        {
            IEnumerable<Plan> plans = conn.Query<Plan>("SELECT * FROM Plan WHERE Deleted=0 AND IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString()+" limit 1");
            foreach (Plan plan in plans)
            {
                pln = plan;
                break;
            }
            if (pln == null)
                pln = new Plan();
            BindingContext = pln;
        }

        private void OnSavePlanClicked(object sender, EventArgs e)
        {
            pln.UpdateDate = DateTime.UtcNow;
            if (pln.Goal <= 0)
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.desiredweight, "OK");
            else if (pln.IDPlan > 0)
            {
                StorageManager.UpdateData(pln);
                Navigation.PopAsync();
            }
            else
            {
                pln.InsertDate = pln.UpdateDate;
                StorageManager.InsertData(pln);
                Navigation.PopAsync();
            }
        }

        private void OnWeighChanged(object sender, TextChangedEventArgs e)
        {
            //wgh.WValue
        }
    }
}
