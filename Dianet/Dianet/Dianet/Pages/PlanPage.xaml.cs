using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class PlanPage : ContentPage
    {
        private SQLiteConnection conn = null; 
        private ObservableCollection<Plan> records = new ObservableCollection<Plan>();
        private PlanPageDetail planPageDt = new PlanPageDetail();

        public PlanPage()
        {
            InitializeComponent();                               
            conn = StorageManager.GetConnection();
            ListViewPlans.ItemsSource = records;            
        }       

        protected override void OnAppearing()
        {
            records.Clear();
            IEnumerable<Plan> plans = conn.Query<Plan>("SELECT IDPlan, Goal, GoalDate FROM Plan WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Plan plan in plans)
            {
                records.Add(new Plan { IDPlan = plan.IDPlan, Goal = plan.Goal, GoalDate = plan.GoalDate });
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Plan myPlan = e.Item as Plan;                        
            planPageDt.LoadData(myPlan.IDPlan); 
            await Navigation.PushAsync(planPageDt);            
        }

        async void OnAddPlanClicked(object sender, EventArgs e)
        {
            planPageDt.LoadData(0);
            await Navigation.PushAsync(planPageDt);
        }        
    }
}
