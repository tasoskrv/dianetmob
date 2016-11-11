using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace DianetApp.Service
{
    public static class ServiceConnector
    {
        private static string BaseUrl = "http://dianet.cloudocean.gr/api/v1";//"http://akv.softone.gr:8080/dianetweb/Dianet/api/v1";
        private static HttpClient client = new HttpClient();
        private static IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        private static BooleanJsonConverter boolConverter = new BooleanJsonConverter();

        public static async Task<T> GetServiceData<T>(string url)
        {
            var uri = new Uri(BaseUrl + url);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, dateTimeConverter, boolConverter);
            }
            return default(T);
        }

        public static async Task<T> InsertServiceData<T>(string url, Object AObject)
        {                                    
            var stringContent = new StringContent(AObject.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

            var uri = new Uri(BaseUrl + url);
            var response = await client.PostAsync(uri, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, dateTimeConverter, boolConverter);
            }
            return default(T);
        }

        public static async Task<T> InsertServiceBulkData<T>(string url, Object AObject)//NOT USED FOR THE MOMENT
        {            
            System.Collections.IEnumerable enumerable = AObject as System.Collections.IEnumerable;
            string postData = "";
            
            if (enumerable != null)
            {
                foreach (object element in enumerable)
                {
                    postData = element.ToString();
                }
            }
            
            HttpContent stringContent = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

            var uri = new Uri(BaseUrl + url);
            var response = await client.PostAsync(uri, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, dateTimeConverter, boolConverter);
            }
            return default(T);
        }


    }

    public class BooleanJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value.ToString().ToLower().Trim())
            {
                case "true":
                case "yes":
                case "y":
                case "1":
                    return true;
                case "false":
                case "no":
                case "n":
                case "0":
                    return false;
            }

            // If we reach here, we're pretty much going to throw an error so let's let Json.NET throw it's pretty-fied error message.
            return new JsonSerializer().Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

    }
}
