using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyDay : TabbedPage
    {
        public MyDay()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            //var answer = await DisplayActionSheet("Load Data.. ?", "cancel", null, "ΝΑΙ", "ΟΧΙ");
        }
    }
}
