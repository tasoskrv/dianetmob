using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Service
{
    public static class ServiceConnector
    {
        private static string BaseUrl = "http://dianet.cloudocean.gr/api/v1";
        private static HttpClient client = new HttpClient();
        private static IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };
        public static async Task<T> GetServiceData<T>(string url)
        {
            var uri = new Uri(BaseUrl + url);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, dateTimeConverter);
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
                return JsonConvert.DeserializeObject<T>(content, dateTimeConverter);
            }
            return default(T);
        }
    }
}
