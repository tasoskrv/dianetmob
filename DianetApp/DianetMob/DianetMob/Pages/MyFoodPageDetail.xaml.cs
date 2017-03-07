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
        private Meal uFood;
        private MealUnit ml;
        private IEnumerable<Unit> mUnit = null;
        public Action setRecordsAction { get; set; }


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

        public void LoadData(int IDMeal)
        {
            
            if (IDMeal > 0)
            {
                uFood = conn.Get<Meal>(IDMeal);
                ml = new MealUnit();
                IEnumerable<MealUnit> cusMeal = conn.Query<MealUnit>("SELECT * FROM MealUnit WHERE IDMeal=" + IDMeal.ToString() );
                MealUnit mapMeal = cusMeal.First();
                txtName.Text= uFood.Name;
                txtDescription.Text= uFood.Description;

                string Uname = "";
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
                uFood = new Meal();
                ml = new MealUnit();
                unitPicker.SelectedIndex = -1;
                txtName.Text = "";
                txtDescription.Text = "";

                uFood.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;

            }
            BindingContext = ml;

        }

        private void OnSaveFoodClicked(object sender, EventArgs e)
        {
            
            if ((txtName.Text == null) || txtName.Text.Equals(""))
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.fillname, "OK");
            else if ((txtDescription.Text == null) || txtDescription.Text.Equals(""))
            {
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.filldescr, "OK");
            }
            else if (unitPicker.SelectedIndex == -1)
            {
                DisplayAlert(Properties.LangResource.please, Properties.LangResource.selectunit, "OK");
            }
            else
            {
                DateTime utcdate = DateTime.UtcNow;
                uFood.UpdateDate = utcdate;
                uFood.Name = txtName.Text;
                uFood.Description = txtDescription.Text;
                ml.UpdateDate = utcdate;

                if (uFood.IDMeal == 0)
                {
                    uFood.IsActive = 1;
                    uFood.Deleted = 0;
                    uFood.IDLang = StorageManager.GetConnectionInfo().Settings.Lang;
                    uFood.InsertDate = utcdate;
                    StorageManager.InsertData(uFood);
                    ml.IDMeal = uFood.IDMeal;
                    ml.InsertDate = utcdate;
                    StorageManager.InsertData(ml);
                }
                else
                {
                    StorageManager.UpdateData(uFood);  
                    StorageManager.UpdateData(ml);
                }
                setRecordsAction();
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
                ml.IDUnit = unit.IDUnit;               
            }
        }
    }
}

