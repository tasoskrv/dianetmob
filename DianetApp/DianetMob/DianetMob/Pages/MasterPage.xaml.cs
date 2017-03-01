using DianetMob.DB;
using DianetMob.Model;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public Button ButtonProfile { get { return btnProfile; } }

        public MasterPage()
        {
            InitializeComponent();

            BindingContext = StorageManager.GetConnectionInfo().LoginUser;

            var masterPageItems = new List<MenuElement>();
            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.myDay,
                IconSource = "mydayn.png",
                TargetType = typeof(MyDay)
            });

            /* masterPageItems.Add(new MenuElement
             {
                 Title = "Προφίλ",
                 IconSource = "profile.png",
                 TargetType = typeof(ProfilePage)
             });

             masterPageItems.Add(new MenuElement
             {
                 Title = "Στόχοι",
                 IconSource = "goal.png",
                 TargetType = typeof(PlanPage)
             }); */

            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.weight,
                IconSource = "weight.png",
                TargetType = typeof(MyWeight)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.alerts,
                IconSource = "alarm.png",
                TargetType = typeof(AlertPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.subscription,
                IconSource = "plan.png",
                TargetType = typeof(ShopPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.myfood,
                IconSource = "myfood.png",
                TargetType = typeof(MyFoodPage)
            });

            masterPageItems.Add(new MenuElement
            {
                Title = Properties.LangResource.exit,
                IconSource = "logout.png",
                TargetType = typeof(LoginPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    
    }
}
