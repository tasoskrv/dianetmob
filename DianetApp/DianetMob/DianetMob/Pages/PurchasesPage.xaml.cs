using DianetMob.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class PurchasesPage : ContentPage
    {
        public PurchasesPage()
        {
            InitializeComponent();
            BindingContext = StorageManager.GetConnectionInfo().ActiveSubscription;
        }
    }
}
