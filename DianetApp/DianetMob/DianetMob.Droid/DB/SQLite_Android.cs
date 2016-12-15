using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DianetMob.Droid.DB;
using Xamarin.Forms;
using DianetMob.DB;
using System.IO;

[assembly: Dependency(typeof(SQLite_Android))]
namespace DianetMob.Droid.DB
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {

            var sqliteFilename = "Dianet.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            //delete DB
            // File.Delete(path);
            bool dbexist = File.Exists(path);

            var conn = new SQLite.SQLiteConnection(path);
            if (!dbexist)
                StorageManager.CreateDB(conn);
            // Return the database connection
            return conn;
        }


    }
}