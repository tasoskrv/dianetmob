using DianetMob.DB;
using DianetMob.TableMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public void SaveAllToDBWithServerID(string field)
        {
            for (var i = 0; i < totalRows; i++)
            {
                Type t = data[i].GetType();

                PropertyInfo prop = GetProperty(t.GetTypeInfo(), "IDServer");
                var value = prop.GetValue(data[i]).ToString();
                List<MapID> alts = StorageManager.GetConnection().Query<MapID>("SELECT "+field+" as ID FROM " + data[i].GetType().Name + "  WHERE IDServer=" + value);
                if (alts.Count() == 0) {
                    StorageManager.InsertData<T>(data[i]);
                }
                else
                {
                    prop = GetProperty(t.GetTypeInfo(), field);
                    prop.SetValue(data[i], alts[i].ID);
                    StorageManager.UpdateData<T>(data[i]);
                }
            }

        }

        private static PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
        {
            var propertyInfo = typeInfo.GetDeclaredProperty(propertyName);
            if (propertyInfo == null && typeInfo.BaseType != null)
            {
                propertyInfo = GetProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);
            }
            return propertyInfo;
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
