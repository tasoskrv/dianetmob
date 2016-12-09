using DianetMob.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.Service
{
    public class ModelService<T>
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

        public void SaveAllToDB()
        {
            for (var i = 0; i < totalRows; i++)
            {
                try
                {
                    StorageManager.InsertData<T>(data[i]);
                }
                catch
                {
                    StorageManager.UpdateData<T>(data[i]);
                }
            }

        }

        public void SaveRecordToDB(T AObj)
        {
            try
            {
                StorageManager.InsertData<T>(AObj);
            }
            catch
            {
                StorageManager.UpdateData<T>(AObj);
            }
        }
    }
}
