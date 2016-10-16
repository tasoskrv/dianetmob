﻿using Dianet.DB;
using Dianet.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SelectMealPage : ContentPage
    {
       private SQLiteConnection conn = null;

        // private ObservableCollection<Unit> Units = new ObservableCollection<Unit>();
        Dictionary<string, MealUnit> DicUnit = new Dictionary<string, MealUnit>();
        public int IDMealSelected{ get; set; }
        public SelectMealPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            for (int i = 0; i < 10000; i++)
            {
                counter1Picker.Items.Add(i.ToString());
            }
        }
        public void CalcUnits() {
            unitPicker.Items.Clear();
            //SELECT M.idMealUnit, M.IdUnit, M.Calories FROM MealUnit M, Unit U where M.idUnit= U.idUnit -- Κανονικο query
            // IEnumerable<MealUnit> UnitList = conn.Query<MealUnit>("SELECT M.idMealUnit, M.IdUnit, M.Calories, U.Name FROM MealUnit M, Unit U ");
            IEnumerable<Unit> UnitList = conn.Query<Unit>("SELECT idUnit, Name FROM Unit");
            IEnumerable<MealUnit> Mealunit = conn.Query<MealUnit>("SELECT idMealUnit, idUnit, idMeal, Calories, Protein, Carb, Fat  FROM MealUnit");
            foreach (Unit unit in UnitList)
            {
                unitPicker.Items.Add(unit.Name);
               // DicUnit.Add(mealunit.Name, mealunit); // ο Πινακας mealUnit θα χρησιμοποιηθει για να ενημερωθει ο usermeal

            }
            
        }
    
          

        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;

            if (unitPicker.SelectedIndex >= 0)
            {
               string Name = unitPicker.Items[unitPicker.SelectedIndex];
                // MealUnit mealUnit = DicUnit[Name];

                // DisplayAlert("You selected", mealUnit.Name, "OK");
                DisplayAlert("You selected", Name, "OK");


                counter1Picker.IsEnabled = true;
                counter2Picker.IsEnabled = true;
                counter1Picker.SelectedIndex = 1;
                counter2Picker.SelectedIndex = 1;
            }
            else
            {
                counter1Picker.SelectedIndex = 0;
                counter2Picker.SelectedIndex = 0;
                counter1Picker.IsEnabled = false;
                counter2Picker.IsEnabled = false;
            }            
        }

        async void OnChooseBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            
        }
    }
}
