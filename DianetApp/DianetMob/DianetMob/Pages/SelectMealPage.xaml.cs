using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.TableMapping;
using DianetMob.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public class Counter {
        public string Caption { get; set; }
        public double Value { get; set; }
    }
    public partial class SelectMealPage : ContentPage
    {
        private SQLiteConnection conn = null;
        public ObservableCollection<MapMealUnit> recordsUnits = new ObservableCollection<MapMealUnit>();
        List<Counter> DicCount1 = new List<Counter>();
        List<Counter> DicCount2 = new List<Counter>();
        private UserMeal usermeal;
        private MapMealUnit SelMapMealUnit = null;

        public DateTime SelectedDate { get; set; }

        public int IDMealSelected { get; set; }
        public int IDCategorySelected { get; set; }

        public SelectMealPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();

            for (int i = 0; i < 1000; i++)
            {
                DicCount1.Add(new Counter() { Value = i });
            }

            DicCount2.Add(new Counter() { Caption = "0", Value = 0 });
            DicCount2.Add(new Counter() { Caption = "1/8", Value = 0.125 });
            DicCount2.Add(new Counter() { Caption = "1/4", Value = 0.25 });
            DicCount2.Add(new Counter() { Caption = "1/3", Value = 0.33 });
            DicCount2.Add(new Counter() { Caption = "1/2", Value = 0.5 });
            DicCount2.Add(new Counter() { Caption = "2/3", Value = 0.66 });
            DicCount2.Add(new Counter() { Caption = "3/4", Value = 0.75 });

            counter1Picker.ItemsSource = DicCount1;
            counter2Picker.ItemsSource = DicCount2;
            unitPicker.ItemsSource = recordsUnits;
            ClearDetail();
        }

        public void CalcUnits(UserMeal usermeal = null)
        {
            this.usermeal = usermeal;
            recordsUnits.Clear();
            if (usermeal != null)
            {
                IDMealSelected = conn.Get<MealUnit>(usermeal.IDMealUnit).IDMeal;
            }
                // DicUnit.Clear();
            IEnumerable<MapMealUnit> mlUnit = conn.Query<MapMealUnit>("SELECT M.IdMealUnit as IdMealUnit , U.Name as UName, M.calories as Calories, M.protein as Protein, M.Carb as Carb, M.fat as Fat, M.satfat as Satfat, M.unsatfat as Unsatfat, M.cholesterol as Cholesterol, M.sugar as Sugar, M.natrium as Natrium, M.potassium as Potassium, M.fiber as Fiber FROM MealUnit as M  JOIN Unit as U ON M.idUnit = U.idUnit WHERE M.IDMeal=" + this.IDMealSelected);
            SelMapMealUnit = mlUnit.First();


            foreach (MapMealUnit mealunit in mlUnit)
            {
                if (usermeal != null && mealunit.IdMealUnit == usermeal.IDMealUnit)
                {
                    SelMapMealUnit = mealunit;
                }
                recordsUnits.Add(mealunit);
            }

            ClearDetail();
            unitPicker.ItemSelected -= OnUnitChosen;
            unitPicker.SelectedItem = SelMapMealUnit;
            unitPicker.ItemSelected += OnUnitChosen;
            if (usermeal != null)
            {

                counter1Picker.ItemSelected -= OnCount1Chosen;
                int count = (int)usermeal.Qty;
                counter1Picker.SelectedItem = DicCount1[count];
                counter1Picker.ItemSelected += OnCount1Chosen;
                counter1Picker.ScrollTo(counter1Picker.SelectedItem, ScrollToPosition.Center, true);

                double dekadika = (usermeal.Qty - Math.Truncate(usermeal.Qty));
                Tuple<double, Counter> bestMatch = null;
                foreach (var ec in DicCount2)
                {
                    var dif = Math.Abs(ec.Value - dekadika);
                    if (bestMatch == null || dif < bestMatch.Item1)
                        bestMatch = Tuple.Create(dif, ec);
                }
                counter2Picker.ItemSelected -= OnCount2Chosen;
                counter2Picker.SelectedItem = bestMatch.Item2;
                counter2Picker.ItemSelected += OnCount2Chosen;
                counter2Picker.ScrollTo(counter2Picker.SelectedItem, ScrollToPosition.Center, true);
                MealDetails();
            }
            else
            {
                counter1Picker.ItemSelected -= OnCount1Chosen;
                counter1Picker.SelectedItem = DicCount1[0];
                counter1Picker.ItemSelected += OnCount1Chosen;
                counter2Picker.ItemSelected -= OnCount2Chosen;
                counter2Picker.SelectedItem = DicCount2[0];
                counter2Picker.ItemSelected += OnCount2Chosen;
            }
        }

        void OnCount1Chosen(object sender, SelectedItemChangedEventArgs e)
        {
            MealDetails();
        }

        void OnCount2Chosen(object sender, SelectedItemChangedEventArgs e)
        {
            MealDetails();
        }

        void OnUnitChosen(object sender, SelectedItemChangedEventArgs e)
        {
            MapMealUnit mapmeal = e.SelectedItem as MapMealUnit;

            double perc = SelMapMealUnit.Calories / mapmeal.Calories;
            double dekadika = perc * ((Counter)counter2Picker.SelectedItem).Value;
            int count1 = (int)((Counter)counter1Picker.SelectedItem).Value;
            double result = dekadika + (perc * count1);

            counter1Picker.ItemSelected -= OnCount1Chosen;
            counter1Picker.SelectedItem = DicCount1[(int)Math.Floor(result)];
            counter1Picker.ItemSelected += OnCount1Chosen;
            counter1Picker.ScrollTo(counter1Picker.SelectedItem, ScrollToPosition.Center, true);

            dekadika = (result - Math.Truncate(result));

            Tuple<double, Counter> bestMatch = null;
            foreach (var ec in DicCount2)
            {
                var dif = Math.Abs(ec.Value - dekadika);
                if (bestMatch == null || dif < bestMatch.Item1)
                    bestMatch = Tuple.Create(dif, ec);
            }

            counter2Picker.ItemSelected -= OnCount2Chosen;
            counter2Picker.SelectedItem = bestMatch.Item2;
            counter2Picker.ItemSelected -= OnCount2Chosen;
            counter2Picker.ScrollTo(counter2Picker.SelectedItem, ScrollToPosition.Center, true);

            SelMapMealUnit = mapmeal;

            MealDetails();
        }

        private void MealDetails()
        {
            MapMealUnit mealUnitmap = (MapMealUnit)unitPicker.SelectedItem;
            double counter = ((Counter)counter1Picker.SelectedItem).Value + ((Counter)counter2Picker.SelectedItem).Value;
            Calories.Text = (counter * mealUnitmap.Calories).ToString();
            Fats.Text = (counter * mealUnitmap.Fat).ToString();
            UnSatFats.Text = (counter * mealUnitmap.UnSatFat).ToString();
            SatFats.Text = (counter * mealUnitmap.UnSatFat).ToString();
            SatFats.Text = (counter * mealUnitmap.UnSatFat).ToString();
            Protein.Text = (counter * mealUnitmap.Protein).ToString();
            Fiber.Text = (counter * mealUnitmap.Fiber).ToString();
            Natrium.Text = (counter * mealUnitmap.Natrium).ToString();
            Potassium.Text = (counter * mealUnitmap.Potassium).ToString();
            Carbs.Text = (counter * mealUnitmap.UnSatFat).ToString();
            Cholesterol.Text = (counter * mealUnitmap.Cholesterol).ToString();
            Sugars.Text = (counter * mealUnitmap.Sugar).ToString();
            Points.Text = PointSystem.PointCalculate(counter * mealUnitmap.Calories).ToString();
        }

        private void ClearDetail()
        {
            Calories.Text = "0";
            Fats.Text = "0";
            UnSatFats.Text = "0";
            SatFats.Text = "0";
            SatFats.Text = "0";
            Protein.Text = "0";
            Fiber.Text = "0";
            Natrium.Text = "0";
            Potassium.Text = "0";
            Carbs.Text = "0";
            Cholesterol.Text = "0";
            Sugars.Text = "0";
            Points.Text = "0";
        }


        async void OnChooseBtnClicked(object sender, EventArgs e)
        {
            double counter = ((Counter)counter1Picker.SelectedItem).Value + ((Counter)counter2Picker.SelectedItem).Value;
            if (usermeal == null)
            {
                usermeal = new UserMeal();
                usermeal.IDCategory = IDCategorySelected;
                usermeal.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
                usermeal.IDMealUnit = SelMapMealUnit.IdMealUnit;
                usermeal.InsertDate = DateTime.UtcNow;
                usermeal.UpdateDate = usermeal.InsertDate;
                usermeal.MealDate = SelectedDate;
                usermeal.Qty = counter;
                StorageManager.InsertData<UserMeal>(usermeal);
            }
            else
            {
                usermeal.IDMealUnit = SelMapMealUnit.IdMealUnit;
                usermeal.UpdateDate = usermeal.InsertDate;
                usermeal.Qty = counter;
                StorageManager.UpdateData<UserMeal>(usermeal);
            }

            await Navigation.PopToRootAsync(true);
        }
    }
}
