using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using DianetMob.TableMapping;
using System.Linq;

namespace DianetMob.Pages
{
    public partial class MyFoodPageDetail : ContentPage
    {
        private MapCustomMeal mapMeal = null;
        private Meal uFood;
        private MealUnit ml;
        private string Uname;
        private IEnumerable<Unit> mUnit = null;


        private SQLiteConnection conn = null;
        Dictionary<string, Unit> DicUnit = new Dictionary<string, Unit>();

        public MyFoodPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            mUnit = conn.Query<Unit>("SELECT IDUnit, Name  FROM Unit");
            foreach (Unit myunit in mUnit)
            {
                unitPicker.Items.Add(myunit.Name);
                DicUnit.Add(myunit.Name, myunit);
            }

        }

        public void LoadData(int IDMeal, int IDUser)
        {
            uFood = new Meal();
            ml = new MealUnit();
            

            if (IDMeal > 0)
            {
                IEnumerable<MapCustomMeal> cusMeal = conn.Query<MapCustomMeal>("SELECT N.Name, N.Description, N.IDUser, N.IDMeal, M.IdMealUnit, M.IDUnit, M.Fat, M.Carb, M.Calories, M.Cholesterol, M.Fiber, M.Natrium, M.Potassium, M.SatFat, M.Protein, M.Sugar, M.UnSatFat FROM MealUnit as M JOIN Meal as N ON M.IDMeal= N.IDMeal WHERE N.IDMeal=" + IDMeal.ToString() + " AND N.IDUser=" + IDUser.ToString());
                mapMeal = cusMeal.First();
                foreach (Unit u1 in mUnit)
                {
                    if (u1.IDUnit == mapMeal.IDUnit)
                    {
                        Uname = u1.Name;
                        continue;
                    }
                }
                for (int i = 0; i < unitPicker.Items.Count; i++)
                {
                    if (unitPicker.Items[i] == Uname)
                    {
                        unitPicker.SelectedIndex = i;
                        continue;
                    }
                }
            }
            else
            {
                mapMeal = new MapCustomMeal();
                uFood.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;

            }
            BindingContext = mapMeal;

        }

        private void OnSaveFoodClicked(object sender, EventArgs e)
        {
            uFood.UpdateDate = DateTime.UtcNow;
            uFood.Name = mapMeal.Name;
            uFood.Description = mapMeal.Description;
            ml.Calories = mapMeal.Calories;
            ml.Carb = mapMeal.Carb;
            ml.Cholesterol = mapMeal.Cholesterol;
            ml.Sugar = mapMeal.Sugar;
            ml.Protein = mapMeal.Protein;
            ml.Fat = mapMeal.Fat;
            ml.SatFat = mapMeal.SatFat;
            ml.UnSatFat = mapMeal.UnSatFat;
            ml.Natrium = mapMeal.Natrium;
            ml.Potassium = mapMeal.Potassium;
            ml.IDUser = mapMeal.IDUser;
            ml.IDUnit = mapMeal.IDUnit;

            if ((uFood.Name == null) || uFood.Name.Equals(""))
                DisplayAlert("Please", "fill name", "OK");
            else if ((uFood.Description == null) || uFood.Description.Equals(""))
            {
                DisplayAlert("Please", "fill description", "OK");
            }
            else
            {
                uFood.InsertDate = uFood.UpdateDate;
                if (mapMeal.IDMeal == 0)
                {
                    StorageManager.InsertData(uFood);
                    MyFoodPage.recordsMeal.Add(uFood);
                    ml.IDMeal = uFood.IDMeal;
                    mapMeal.IDMeal = uFood.IDMeal;
                    ml.InsertDate = ml.UpdateDate;
                    StorageManager.InsertData(ml);
                    mapMeal.IdMealUnit = ml.IDMealUnit;
                }
                else
                {
                    uFood.IDMeal = mapMeal.IDMeal;
                    StorageManager.UpdateData(uFood);
                    ml.IDMeal = uFood.IDMeal;
                    mapMeal.IDMeal = uFood.IDMeal;
                    ml.InsertDate = ml.UpdateDate;
                    StorageManager.UpdateData(ml);
                    new MyFoodPage();
                }
                Navigation.PopAsync();
            }
        }
        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {

                string Name = unitPicker.Items[unitPicker.SelectedIndex];
                Unit unit = DicUnit[Name];
                mapMeal.IDUnit = unit.IDUnit;               
            }
        }
    }
}

