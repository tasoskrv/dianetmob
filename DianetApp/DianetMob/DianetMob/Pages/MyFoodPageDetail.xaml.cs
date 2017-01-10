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
        private SQLiteConnection conn = null;

       //Dictionary<string, MapMealUnit> DicUnit = new Dictionary<string, MapMealUnit>();
        //Dictionary<string, double> DicCount2 = new Dictionary<string, double>();

        public MyFoodPageDetail()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();           
            IEnumerable<Unit> mUnit = conn.Query<Unit>("SELECT IDUnit, Name  FROM Unit");
            foreach (Unit myunit in mUnit)
            {
                unitPicker.Items.Add(myunit.Name);             
            }
            for (int i = 0; i < 1000; i++)
            {
                counter1Picker.Items.Add(i.ToString());
            }
            counter2Picker.Items.Add("0");
            counter2Picker.Items.Add("1/8");
            counter2Picker.Items.Add("1/4");
            counter2Picker.Items.Add("1/3");
            counter2Picker.Items.Add("1/2");
            counter2Picker.Items.Add("2/3");
            counter2Picker.Items.Add("3/4");
        }

        public void LoadData(int IDMeal, int IDUser)
        {
            
            if (IDMeal > 0)
            {
                IEnumerable<MapCustomMeal> cusMeal = conn.Query<MapCustomMeal>("SELECT N.Name, N.Description, N.IDUser, M.IdMealUnit, M.Fat, M.Carb, M.Cholesterol, M.Fiber, M.Natrium, M.Potassium, M.SatFat, M.Protein, M.Sugar, M.UnSatFat FROM MealUnit as M JOIN Meal as N ON M.IDMeal= N.IDMeal WHERE N.IDMeal="+IDMeal.ToString()+ " AND N.IDUser="+IDUser.ToString());
                mapMeal = cusMeal.First();
            }
            else
            {
                uFood = new Meal();
                ml = new MealUnit();
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
            ml.SatFat = mapMeal.SatFat;
            ml.UnSatFat = mapMeal.UnSatFat;
            ml.Natrium = mapMeal.Natrium;
            ml.Potassium = mapMeal.Potassium;
            ml.IDUser = mapMeal.IDUser;

            if ((uFood.Name == null) || uFood.Name.Equals(""))
                DisplayAlert("Please", "fill name", "OK");
            else if ((uFood.Description == null) || uFood.Description.Equals(""))
            {
                DisplayAlert("Please", "fill description", "OK");
            }
            //else if (uFood.IDUser > 0)
            //{
            //    StorageManager.UpdateData(uFood);
            //    Navigation.PopAsync();
            //}
            else
            {
                uFood.InsertDate = uFood.UpdateDate;
                StorageManager.InsertData(uFood);
                ml.IDMeal = uFood.IDMeal;
                mapMeal.IDMeal = uFood.IDMeal;
                ml.InsertDate = ml.UpdateDate;
                StorageManager.InsertData(ml);
                Navigation.PopAsync();
            }
        }

        void OnUnitChosen(object sender, EventArgs e)
        {
            //Picker unitPicker = (Picker)sender;
            //if (unitPicker.SelectedIndex >= 0)
            //{
            //    string Name = unitPicker.Items[unitPicker.SelectedIndex];
            //    string dekadikaName = counter2Picker.Items[counter2Picker.SelectedIndex];
            //    MapMealUnit mealUnitmap = DicUnit[Name];
            //    double perc = SelMapMealUnit.Calories / mealUnitmap.Calories;
            //    double dekadika = perc * DicCount2[dekadikaName];
            //    int count1 = int.Parse(counter1Picker.Items[counter1Picker.SelectedIndex]);
            //    double result = dekadika + (perc * count1);
            //    counter1Picker.SelectedIndex = (int)Math.Floor(result);
            //    dekadika = (result - Math.Truncate(result));

            //    Tuple<double, KeyValuePair<string, double>> bestMatch = null;
            //    foreach (var ec in DicCount2)
            //    {
            //        var dif = Math.Abs(ec.Value - dekadika);
            //        if (bestMatch == null || dif < bestMatch.Item1)
            //            bestMatch = Tuple.Create(dif, ec);
            //    }
            //    for (int i = 0; i < counter2Picker.Items.Count; i++)
            //    {
            //        if (counter2Picker.Items[i] == bestMatch.Item2.Key)
            //        {
            //            counter2Picker.SelectedIndex = i;
            //            break;
            //        }
            //    }
            //    SelMapMealUnit = mealUnitmap;

            }
        }


    }

