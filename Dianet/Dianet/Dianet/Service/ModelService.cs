using Dianet.DB;

namespace Dianet.Service
{
    class ModelService<T>
    {
        public int totalRows { get; set; }
        public bool success { get; set; }

        public T[] data { get; set; }

        public void InsertAllToDB()
        {
            for (var i = 0; i < totalRows; i++)
            {
                StorageManager.InsertData<T>(data[i]);
            }

        }

        public void InsertRecordToDB(T AObj)
        {
            StorageManager.InsertData<T>(AObj);
        }
    }

    class NewUserService<T>
    {
        public int UserId { get; set; }
        public bool success { get; set; }

        public T[] data { get; set; }
    }
}
