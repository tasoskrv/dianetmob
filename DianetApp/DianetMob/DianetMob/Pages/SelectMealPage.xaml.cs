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
        private MealUnit SelMealUnit = null;
        MapMealUnit mealUnitmap = null;
        private int counter1 = 0;
        private double counter2 = 0;
        private double counter = 0;
        double cnt2Tmp = 0;
        public DateTime SelectedDate { get; set; }

        public int IDMealSelected { get; set; }
        public int IDCategorySelected { get; set; }
        public SelectMealPage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
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
            IEnumerable<MapMealUnit> mlUnit = conn.Query<MapMealUnit>("SELECT M.IdMealUnit as IdMealUnit , U.Name as UName, M.calories as Calories, M.protein as Protein, M.Carb as Carb, M.fat as Fat, M.satfat as Satfat, M.unsatfat as Unsatfat, M.cholesterol as Cholesterol, M.sugar as Sugar, M.natrium as Natrium, M.potassium as Potassium, M.fiber as Fiber FROM MealUnit as M  JOIN Unit as U ON M.idUnit = U.idUnit WHERE M.IDMeal=" + this.IDMealSelected);
            SelMapMealUnit = mlUnit.First();
            

            foreach (MapMealUnit mealunit in mlUnit)
            {
                 unitPicker.Items.Add(mealunit.UName);
                 DicUnit.Add(mealunit.UName, mealunit); 
            }

            ClearDetail();
            unitPicker.SelectedIndexChanged -= OnUnitChosen;
            unitPicker.SelectedIndex = 0;
            unitPicker.SelectedIndexChanged += OnUnitChosen;
            counter1Picker.SelectedIndexChanged -= OnCount1Chosen;
            counter1Picker.SelectedIndex = 0;
            counter1Picker.SelectedIndexChanged += OnCount1Chosen;
            counter2Picker.SelectedIndexChanged -= OnCount2Chosen;
            counter2Picker.SelectedIndex = 0;
            counter2Picker.SelectedIndexChanged += OnCount2Chosen;

        }

        void OnCount1Chosen(object sender, EventArgs e)
        {
            Picker pick1 = (Picker)sender;
            if (pick1.SelectedIndex >= 0)
            {
                counter = counter - counter1;
                counter1 = pick1.SelectedIndex;
                counter = counter + counter1;
                ClearDetail();
                MealDetails();
            }
        }

        void OnCount2Chosen(object sender, EventArgs e)
        {
            Picker pick2 = (Picker)sender;
            if (pick2.SelectedIndex >= 0)
            {
                counter = counter - cnt2Tmp;
                counter2 = pick2.SelectedIndex;
                cnt2Tmp = DicCount2[counter2Picker.Items[counter2Picker.SelectedIndex]];
                counter = counter + cnt2Tmp;
                ClearDetail();
                MealDetails();
                
            }

        }


        void OnUnitChosen(object sender, EventArgs e)
        {
            Picker unitPicker = (Picker)sender;
            if (unitPicker.SelectedIndex >= 0)
            {
                string Name = unitPicker.Items[unitPicker.SelectedIndex];
                string dekadikaName = counter2Picker.Items[counter2Picker.SelectedIndex];
                mealUnitmap = DicUnit[Name];
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
                ClearDetail();
                MealDetails();

            }
        }

        void MealDetails()
        {
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
            
        }

        void ClearDetail()
        {
            Calories.Text = "";
            Fats.Text = "";
            UnSatFats.Text = "";
            SatFats.Text = "";
            SatFats.Text = "";
            Protein.Text = "";
            Fiber.Text = "";
            Natrium.Text = "";
            Potassium.Text = "";
            Carbs.Text = "";
            Cholesterol.Text = "";
            Sugars.Text = "";
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
