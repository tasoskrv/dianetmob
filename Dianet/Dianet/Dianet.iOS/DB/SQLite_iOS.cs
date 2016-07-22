using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using MonoTouch.Foundation;
//using MonoTouch.UIKit;
using System.IO;
using Dianet.DB;
using Dianet.iOS.DB;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace Dianet.iOS.DB
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "TodoSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}