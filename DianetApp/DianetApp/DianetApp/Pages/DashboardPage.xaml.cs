using DianetApp.DB;
using DianetApp.DB.Entities;
using DianetApp.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DianetApp.Pages
{
    public partial class DashboardPage : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<Weight> records = new ObservableCollection<Weight>();

        public DashboardPage()
        {            
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            try
            {
                records.Clear();
                IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, InsertDate FROM Weight WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
                foreach (Weight wght in wghts)
                {
                    records.Add(new Weight { IDWeight = wght.IDWeight, WValue = wght.WValue, InsertDate = wght.InsertDate });
                }

                /*****/
                Weight weight = new Weight();
                weight.IDUser = 100;// StorageManager.GetConnectionInfo().LoginUser.IDUser;
                weight.WValue = 100;
                weight.WeightDate = DateTime.UtcNow;
                weight.InsertDate = DateTime.UtcNow;
                weight.UpdateDate = weight.InsertDate;
                ModelService<Weight> srvNewUser = await ServiceConnector.InsertServiceData<ModelService<Weight>>("/weight/save", weight);
                if ((srvNewUser.success == true) && (srvNewUser.ID > 0) && !(srvNewUser.ErrorCode > 0))
                {
                    // user.InsertDate = DateTime.UtcNow;
                    // user.UpdateDate = user.InsertDate;
                    //user.IDUser = srvNewUser.ID;
                    //user.AccessToken = srvNewUser.AccessToken;
                    //srvNewUser.InsertRecordToDB(user);
                    //App.Current.MainPage = new SignUpPage2();
                    return;
                }
                else if (srvNewUser.ErrorCode == 2)
                {
                    await DisplayAlert("Warning", "O Χρήστης υπάρχει ήδη!", "OK");
                }
                else
                {
                    await DisplayAlert("Warning", srvNewUser.message, "OK");
                }
                return;
            }
            catch (Exception ex)
            {

            }
        }

    }
}