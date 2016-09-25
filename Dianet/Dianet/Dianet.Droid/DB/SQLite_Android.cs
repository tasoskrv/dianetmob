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
using Dianet.DB;
using System.IO;
using Xamarin.Forms;
using Dianet.Droid.DB;

[assembly: Dependency(typeof(SQLite_Android))]
namespace Dianet.Droid.DB
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            
            var sqliteFilename = "Dianet.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // για να σβήσουμε την βάση
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