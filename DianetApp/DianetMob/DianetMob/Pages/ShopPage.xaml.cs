using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
        {
            InitializeComponent();
            App.ViewModel.RefundCommand.Execute(null);
        }
    }
}
