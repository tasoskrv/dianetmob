using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dianet.Model;

using Xamarin.Forms;

namespace Dianet
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MenuElement>();
            masterPageItems.Add(new MenuElement
            {
                Title = "Myday",
                IconSource = "myday.png",
                TargetType = typeof(Myday)
            });
            
            listView.ItemsSource = masterPageItems;
        }
    }

   
}
