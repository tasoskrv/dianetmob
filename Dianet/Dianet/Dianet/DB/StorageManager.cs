using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Dianet.Model;

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
                return true;
            }
            catch {
                return false;
            }
        }
    }
}

