using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Service
{
    public static class ServiceConnector
    {
        private static string BaseUrl = "http://dianet.cloudocean.gr/api/v1";
        private static HttpClient client = new HttpClient();
        public static async Task<T> GetServiceData<T>(string url)
        {
            var uri = new Uri(BaseUrl + url);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default(T);
        }

        public static async Task<T> InsertServiceData<T>(string url)
        {
            var uri = new Uri(BaseUrl + url);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();                
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default(T);
        }
    }
}
