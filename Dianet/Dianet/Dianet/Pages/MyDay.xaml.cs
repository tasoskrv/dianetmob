using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class Myday : TabbedPage
    {
        public Myday()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            var answer = await DisplayActionSheet("Load Data.. ?", "cancel", null, "ΝΑΙ", "ΟΧΙ");
        }
    }
}
