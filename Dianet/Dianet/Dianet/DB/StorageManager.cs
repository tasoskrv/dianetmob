using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Dianet.DB.Entities;
using Dianet.Service;

namespace Dianet.DB
{

    public static class StorageManager
    {
        static SQLiteConnection db=null;

        public static SQLiteConnection GetConnection()
        {
            if (db == null) {
                db = DependencyService.Get<ISQLite>().GetConnection();     
            }
            return db;
        }
        
        public static bool CreateDB(SQLiteConnection condb) {
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
                InsertDemoData(condb);
                return true;
            }
            catch {
                return false;
            }
        }
        public static void InsertDemoData(SQLiteConnection condb)
        {
            User user = new User();
            user.IdUser = 1;
            user.FirstName = "Spyros";
            user.LastName = "Karavanis";
            user.Height = 180;
            user.Birthdate = new DateTime(1987, 08, 04);
            user.Email = "spiroskaravanis2@gmail.com";
            user.Gender = 1;
            user.HeightType = 1;
            user.InsertDate = DateTime.Now;
            user.Password = "12345";
            user.UpdateDate = DateTime.Now;
            condb.Insert(user);

            user = new User();
            user.IdUser = 2;
            user.FirstName = "Mara";
            user.LastName = "Patroni";
            user.Height = 170;
            user.Birthdate = new DateTime(1985, 07, 07);
            user.Email = "patroni85@gmail.com";
            user.Gender = 2;
            user.HeightType = 1;
            user.InsertDate = DateTime.Now;
            user.Password = "1234";
            user.UpdateDate = DateTime.Now;
            condb.Insert(user);

            user = new User();
            user.IdUser = 3;
            user.FirstName = "Thanos";
            user.LastName = "Koutsopoulos";
            user.Height = 180;
            user.Birthdate = new DateTime(1986, 08, 26);
            user.Email = "koutsopoulosath@gmail.com";
            user.Gender = 1;
            user.HeightType = 1;
            user.InsertDate = DateTime.Now;
            user.Password = "1234";
            user.UpdateDate = DateTime.Now;
            condb.Insert(user);

            Meal meal = new Meal();
            meal.IDMeal = 1;
            meal.Name = "Μπριζόλα";
            meal.Description = "me ladaki";
            meal.IDLang = 1;
            meal.Fertility = "";
            meal.InsertDate = DateTime.Now;
            meal.UpdateDate = DateTime.Now;
            condb.Insert(meal);

            meal = new Meal();
            meal.IDMeal = 2;
            meal.Name = "Μπριζόλα με πατάτες";
            meal.Description = "me ladaki";
            meal.IDLang = 1;
            meal.Fertility = "";
            meal.InsertDate = DateTime.Now;
            meal.UpdateDate = DateTime.Now;
            condb.Insert(meal);

            meal = new Meal();
            meal.IDMeal = 3;
            meal.Name = "Μπριζόλα με ρύζι";
            meal.Description = "me ladaki";
            meal.IDLang = 1;
            meal.Fertility = "";
            meal.InsertDate = DateTime.Now;
            meal.UpdateDate = DateTime.Now;
            condb.Insert(meal);

            meal = new Meal();
            meal.IDMeal = 4;
            meal.Name = "Ντομάτα";
            meal.Description = "me ladaki";
            meal.IDLang = 1;
            meal.Fertility = "";
            meal.InsertDate = DateTime.Now;
            meal.UpdateDate = DateTime.Now;
            condb.Insert(meal);
        }

        //public static void InsertMealData(SQLiteConnection condb, Meal newMeal)
        //{
        //    Meal meal = new Meal();
        //    meal.Name = newMeal.Name;
        //    meal.Description = newMeal.Description;
        //    meal.IDLang = newMeal.IDLang;
        //    meal.Fertility = newMeal.Fertility;
        //    condb.Insert(meal);
        //}

        public static void InsertData<T>(T model)
        {
            db.Insert(model);
        }

        public static void UpdateData<T>(T model)
        {
            db.Update(model);
        }
    }
}

