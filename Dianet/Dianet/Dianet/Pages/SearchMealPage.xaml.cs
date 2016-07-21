using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dianet.Model;

namespace Dianet.Pages
{
    public partial class SearchMealPage : ContentPage
    {
        private ObservableCollection<SearchRecord> records = new ObservableCollection<SearchRecord>();

        public int Mode { get; set; }
        public SearchMealPage()
        {
            InitializeComponent();
            ListViewSearch.ItemsSource = records;

        }

        public void OnSearchBarPressed(object sender, EventArgs eventArgs)
        {
           
        }

        public void OnSearchBarTextChanged(object sender, EventArgs eventArgs)
        {
            records.Add(new SearchRecord { DisplayName="OK" });
        }
        

        protected override void OnAppearing()
        {
            if (Mode == 1)
            {
                Title = "Breakfast";
            }
            else if (Mode == 2) {
                Title = "Lunch";
            }
            else if (Mode == 3)
            {
                Title = "Dinner";
            }
            else if (Mode == 4)
            {
                Title = "Snack";
            }
        }
    }
}
