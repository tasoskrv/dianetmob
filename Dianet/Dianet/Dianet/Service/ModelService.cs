using Dianet.DB;

namespace Dianet.Service
{
    class ModelService<T>
    {
        public int totalRows { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int ID { get; set; }
        public string AccessToken { get; set; }
        public int ErrorCode { get; set; }


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

        public void UpdateData(T Aobj)
        {
            StorageManager.UpdateData<T>(Aobj);
        }
    }    
}
