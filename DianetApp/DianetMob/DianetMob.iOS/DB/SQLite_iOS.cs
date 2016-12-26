using System;
using Xamarin.Forms;
using System.IO;
using SQLite;
using DianetMob.IOS.DB;
using DianetMob.DB;

[assembly: Dependency(typeof(SQLite_iOS))]

namespace DianetMob.IOS.DB
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS()
        {
        }

        #region ISQLite implementation
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "Dianet.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            //File.Delete(path);
            bool dbexist = File.Exists(path);

            var conn = new SQLite.SQLiteConnection(path);
            if (!dbexist)
                StorageManager.CreateDB(conn);
            // Return the database connection
            return conn;
        }
        #endregion
    }
}

