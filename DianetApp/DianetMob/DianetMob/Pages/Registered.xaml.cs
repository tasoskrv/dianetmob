using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class Registered : ContentPage
    {
        public Registered()
        {
            InitializeComponent();
        }

        public void OnClickLogin(object sender, EventArgs eventArgs)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}
