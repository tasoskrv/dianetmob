using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
{
    public partial class SubscriptionPage : ContentPage
    {
        public SubscriptionPage()
        {
            InitializeComponent();
            Browser.Source = "http://www.studiostars.gr";


            /*
            var html = new HtmlWebViewSource();
            html.Html = @"Hello World";
            Browser.Source = html;            
            */
        }
    }
}
