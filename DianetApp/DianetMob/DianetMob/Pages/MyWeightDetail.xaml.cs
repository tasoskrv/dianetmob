﻿using DianetMob.DB;
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
    public partial class MyWeightDetail : ContentPage
    {
        private Weight wght;
        private SQLiteConnection conn = null;
        public Action setRecordsAction { get; set; }

        public MyWeightDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        protected override void OnAppearing()
        {
            WeightEntry.Focus();
        }

        public void LoadData(int IDWeight = 0)
        {
            if (IDWeight > 0)
                wght = conn.Get<Weight>(IDWeight);
            else
            {
                wght = new Weight();
                wght.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
            BindingContext = wght;
        }

        void OnPrevDayClicked(object sender, EventArgs e)
        {
            weightdate.Date = weightdate.Date.AddDays(-1);
        }

        void OnNextDayClicked(object sender, EventArgs e)
        {
            weightdate.Date = weightdate.Date.AddDays(1);
        }

        private void OnSaveWeightClicked(object sender, EventArgs e)
        {
            wght.UpdateDate = DateTime.UtcNow;
            wght.WeightDate = wght.WeightDate.Date;//only date
            if (wght.WValue <= 0)
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.filltoday, "OK");
            else if (wght.IDWeight > 0)
            {
                StorageManager.UpdateData(wght);
                setRecordsAction();
                Navigation.PopAsync();
            }
            else
            {
                List<Weight> wghts = conn.Query<Weight>("SELECT IDWeight FROM Weight WHERE Deleted=0 AND WeightDate = ? ", weightdate.Date.Ticks);
                if (wghts.Count > 0)
                {
                    wght.UpdateDate = DateTime.UtcNow;
                    wght.InsertDate = wght.UpdateDate;
                    wght.IDWeight = wghts[0].IDWeight;
                    StorageManager.UpdateData(wght);
                }
                else
                {
                    wght.InsertDate = wght.UpdateDate;
                    StorageManager.InsertData(wght);
                }
                setRecordsAction();
                Navigation.PopAsync();
            }
        }
    }
}
