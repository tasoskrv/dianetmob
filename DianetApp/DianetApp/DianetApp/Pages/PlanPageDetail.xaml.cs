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
        private Weight wgh=null;
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

            /*
            List<Weight> wghs = conn.Query<Weight>("Select * from weight order by insertdate limit 1");
            if (wghs.Count > 0)
            {
                wgh = wghs[0];
            }
            else {
                DisplayAlert("Please", "fill in your current weight", "OK");
                Navigation.PopAsync();
            }
            */
        }

        private void OnSavePlanClicked(object sender, EventArgs e)
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
