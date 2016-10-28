using DianetApp.DB;
using DianetApp.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetApp.Pages
{
    public class SelMeal
    {

        string UName { get; set; }
        int idUnit { get; set; }

        public SelMeal()
        {
            UName = "";
            idUnit = 0;
        }

    }




    public partial class SelectMealPage : ContentPage
    {
        private SQLiteConnection conn = null;
        // private ObservableCollection<Unit> Units = new ObservableCollection<Unit>();
        Dictionary<string, MealUnit> DicUnit = new Dictionary<string, MealUnit>();
        public int IDMealSelected { get; set; }
        public SelectMealPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            for (int i = 0; i < 10000; i++)
            {
                counter1Picker.Items.Add(i.ToString());
            }
        }
        public void CalcUnits()
        {
            unitPicker.Items.Clear();
            //--------------- an evlepe ta pedia tisSelMeal  public ola tha itan super!!!!!!!

            IEnumerable<SelMeal> mlUnit = conn.Query<SelMeal>("SELECT M.idMealUnit as idUnit , U.Name as UName FROM MealUnit as M  JOIN Unit as U ON M.idUnit = U.idUnit WHERE M.IDMeal=" + this.IDMealSelected);



            foreach (SelMeal mealunit in mlUnit)
            {
                // unitPicker.Items.Add(mealunit.Name);
                // DicUnit.Add(mealunit.Name, mealunit); 
            }

        }



        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {
                string Name = unitPicker.Items[unitPicker.SelectedIndex];
                MealUnit mealUnit = DicUnit[Name];
                //DisplayAlert("You selected", mealUnit.Name, "OK");
                counter1Picker.IsEnabled = true;
            }
            else
            {
                counter1Picker.IsEnabled = false;
            }
        }

        void OnChosen1(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {
                counter2Picker.IsEnabled = true;
            }
        }

        void OnChosen2(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {
                DisplayAlert("Quantity Calculation on process...", "", "OK");
            }
        }

        async void OnChooseBtnClicked(object sender, EventArgs e)
        {

            await Navigation.PopToRootAsync(true);

        }
    }
}
