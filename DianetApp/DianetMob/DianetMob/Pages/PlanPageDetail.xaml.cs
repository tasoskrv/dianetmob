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

        Dictionary<string, int> status = new Dictionary<string, int>
        {
            { "In Progress", 1 }, { "Completed", 2 }, { "Cancelled", 3 }
        };

        public PlanPageDetail()
        {
            InitializeComponent();

            foreach (string sts in status.Keys)
            {
                fStatus.Items.Add(sts);
            }
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
