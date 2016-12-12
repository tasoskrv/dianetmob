using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DianetMob.TableMapping;

namespace DianetMob.DB
{
    public static class StorageManager
    {
        static SQLiteConnection db = null;

        static ConnectionInfo info = new ConnectionInfo();

        public static ConnectionInfo GetConnectionInfo()
        {
            return info;
        }
        public static SQLiteConnection GetConnection()
        {
            if (db == null)
            {
                db = DependencyService.Get<ISQLite>().GetConnection();
            }
            return db;
        }

        public static bool CreateDB(SQLiteConnection condb)
        {
            try
            {
                condb.CreateTable<User>();
                condb.CreateTable<Alert>();
                condb.CreateTable<Exercise>();
                condb.CreateTable<Meal>();
                condb.CreateTable<Unit>();
                condb.CreateTable<MealUnit>();
                condb.CreateTable<Package>();
                condb.CreateTable<Plan>();
                condb.CreateTable<Settings>();
                condb.CreateTable<Subscription>();
                condb.CreateTable<UserFood>();
                condb.CreateTable<UserMeal>();
                condb.CreateTable<Weight>();
                condb.CreateTable<UserSettings>();
                InsertData(condb);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void InsertData(SQLiteConnection condb)
        {
            Settings set = new Settings();
            set.IDSettings = 1;
            condb.Insert(set);
        }

        public static void InsertData<T>(T model)
        {
            db.Insert(model);
        }

        public static void UpdateData<T>(T model)
        {
            db.Update(model);
        }

        public static IEnumerable<MapLogData> LoadDataByDate(DateTime from, DateTime to)
        {
            string query = "Select um.IdUserMeal, um.idcategory,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ?";
            return db.Query<MapLogData>(query, from, to);
        }
    }
}
