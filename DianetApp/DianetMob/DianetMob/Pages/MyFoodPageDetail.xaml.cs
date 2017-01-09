using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyFoodPageDetail : ContentPage
    {
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
            //DicCount2.Add("0", 0);
            //DicCount2.Add("1/8", 0.125);
            //DicCount2.Add("1/4", 0.25);
            //DicCount2.Add("1/3", 0.33);
            //DicCount2.Add("1/2", 0.5);
            //DicCount2.Add("2/3", 0.66);
            //DicCount2.Add("3/4", 0.75);
        }

        public void LoadData(int IDMeal=0)
        {
            if (IDMeal > 0)
            {
                uFood = conn.Get<Meal>(IDMeal);
                IEnumerable<MealUnit> ml = conn.Query<MealUnit>("SELECT Fat, Carb, Cholesterol, Fiber, Natrium, Potassium, SatFat, Protein, Sugar, UnSatFat  WHERE IDMeal={0} AND IDUser= {1}" + IDMeal, +StorageManager.GetConnectionInfo().LoginUser.IDUser);
            }
            else
            {
                uFood = new Meal();
                ml = new MealUnit();
                uFood.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;

            }
            BindingContext = uFood.Name;
            BindingContext = uFood.Description;
            BindingContext = ml.Fat;
            BindingContext = ml.Carb;
            BindingContext = ml.Cholesterol;
            BindingContext = ml.Fiber;
            BindingContext = ml.Natrium;
            BindingContext = ml.Potassium;
            BindingContext = ml.SatFat;
            BindingContext = ml.Protein;
            BindingContext = ml.Sugar;
            BindingContext = ml.UnSatFat;
           
        }

        private void OnSaveFoodClicked(object sender, EventArgs e)
        {
            uFood.UpdateDate = DateTime.UtcNow;
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

