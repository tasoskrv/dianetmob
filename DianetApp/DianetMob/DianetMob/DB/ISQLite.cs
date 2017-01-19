using SQLite;

namespace DianetMob.DB
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
