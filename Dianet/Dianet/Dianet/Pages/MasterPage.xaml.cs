using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dianet.Model;

using Xamarin.Forms;

namespace Dianet.Pages
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

            masterPageItems.Add(new MenuElement
            {
                Title = "Προφιλ",
                IconSource = "profile.png",
                TargetType = typeof(ProfilePage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = "Στόχοι",
                IconSource = "goal.png",
                TargetType = typeof(GoalPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = "Βάρος",
                IconSource = "weight.png",
                TargetType = typeof(MyWeight)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = "Alerts",
                IconSource = "alarm.png",
                TargetType = typeof(AlertPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = "Συνδρομή",
                IconSource = "myday.png",
                TargetType = typeof(SubscriptionPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = "Φαγητά μου",
                IconSource = "myfood.png",
                TargetType = typeof(MyFoodPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }

   
}
