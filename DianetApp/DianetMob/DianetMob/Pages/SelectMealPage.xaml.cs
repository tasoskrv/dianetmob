using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.TableMapping;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class SelectMealPage : ContentPage
    {
        private SQLiteConnection conn = null;
        // private ObservableCollection<Unit> Units = new ObservableCollection<Unit>();
        Dictionary<string, MapMealUnit> DicUnit = new Dictionary<string, MapMealUnit>();
        Dictionary<string, double> DicCount2 = new Dictionary<string, double>();
        private MapMealUnit SelMapMealUnit = null;
        public DateTime SelectedDate { get; set; }

        public int IDMealSelected { get; set; }
        public int IDCategorySelected { get; set; }
        public SelectMealPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            for (int i = 0; i < 10000; i++)
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
            DicCount2.Add("0", 0);
            DicCount2.Add("1/8", 0.125);
            DicCount2.Add("1/4", 0.25);
            DicCount2.Add("1/3", 0.33);
            DicCount2.Add("1/2", 0.5);
            DicCount2.Add("2/3", 0.66);
            DicCount2.Add("3/4", 0.75);
        }
        public void CalcUnits()
        {
            unitPicker.Items.Clear();
            DicUnit.Clear();
            IEnumerable<MapMealUnit> mlUnit = conn.Query<MapMealUnit>("SELECT M.IdMealUnit as IdMealUnit , U.Name as UName, M.calories as Calories FROM MealUnit as M  JOIN Unit as U ON M.idUnit = U.idUnit WHERE M.IDMeal=" + this.IDMealSelected);
            SelMapMealUnit = mlUnit.First();
            
            foreach (MapMealUnit mealunit in mlUnit)
            {
                 unitPicker.Items.Add(mealunit.UName);
                 DicUnit.Add(mealunit.UName, mealunit); 
            }
            unitPicker.SelectedIndexChanged -= OnUnitChosen;
            unitPicker.SelectedIndex = 0;
            unitPicker.SelectedIndexChanged += OnUnitChosen;
            counter1Picker.SelectedIndex = 0;
            counter2Picker.SelectedIndex = 0;
        }



        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {
                string Name = unitPicker.Items[unitPicker.SelectedIndex];
                string dekadikaName = counter2Picker.Items[counter2Picker.SelectedIndex];
                MapMealUnit mealUnitmap = DicUnit[Name];
                double perc = SelMapMealUnit.Calories / mealUnitmap.Calories;
                double dekadika = perc  * DicCount2[dekadikaName];
                int count1=int.Parse(counter1Picker.Items[counter1Picker.SelectedIndex]);
                double result = dekadika + (perc * count1);
                counter1Picker.SelectedIndex =(int)Math.Floor(result) ;
                dekadika = (result - Math.Truncate(result));

                Tuple<double, KeyValuePair<string, double>> bestMatch = null;
                foreach (var ec in DicCount2)
                {
                    var dif = Math.Abs(ec.Value - dekadika);
                    if (bestMatch == null || dif < bestMatch.Item1)
                        bestMatch = Tuple.Create(dif, ec);
                }
                for (int i = 0; i < counter2Picker.Items.Count; i++) {
                    if (counter2Picker.Items[i] == bestMatch.Item2.Key) {
                        counter2Picker.SelectedIndex = i;
                        break;
                    }
                }
                SelMapMealUnit = mealUnitmap;

            }
        }


        async void OnChooseBtnClicked(object sender, EventArgs e)
        {
            UserMeal usermeal = new UserMeal();
            usermeal.IDCategory = IDCategorySelected;
            usermeal.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            usermeal.IDMealUnit = SelMapMealUnit.IdMealUnit;
            usermeal.InsertDate= DateTime.UtcNow;
            usermeal.UpdateDate = usermeal.InsertDate;
            usermeal.MealDate = SelectedDate;
            usermeal.Qty = (int)(counter1Picker.SelectedIndex) + DicCount2[counter2Picker.Items[counter2Picker.SelectedIndex]];
            StorageManager.InsertData<UserMeal>(usermeal);

            await Navigation.PopToRootAsync(true);

        }
    }
}
