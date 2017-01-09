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
                condb.CreateTable<UserMeal>();
                condb.CreateTable<Weight>();
                condb.CreateTable<UserSettings>();
                InsertData(condb);
                return true;
            }
            catch 
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

        public static int InsertData<T>(T model)
        {
            return db.Insert(model);
        }

        public static int UpdateData<T>(T model)
        {
            return db.Update(model);
        }

        public static int SaveData<T>(T model)
        {
            try
            {
                return InsertData<T>(model);
            }
            catch
            {
                return UpdateData<T>(model);
            }
        }

        public static IEnumerable<MapLogData> LoadDataByDate(DateTime from, DateTime to)
        {
            string query = "Select um.IdUserMeal, um.idcategory,  m.name as MealName from usermeal as um inner join mealunit as mu on um.IDMealUnit=mu.IDMealUnit inner join meal m on mu.idmeal=m.idmeal where um.iduser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and um.mealdate BETWEEN ? and ?";
            return db.Query<MapLogData>(query, from, to);
        }
    }
}
