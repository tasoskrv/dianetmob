using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Service
{
    class ServiceConnection
    {
        private static string BaseUrl = "http://dianet.cloudocean.gr/api/v1/";
        private static HttpClient client = new HttpClient();
        public async static Task<T> GetServiceData<T>( string url)
        {

            var response = await client.GetAsync(new Uri(BaseUrl+ url));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T> (content);
            }
            return default(T);
        }

    }
}
