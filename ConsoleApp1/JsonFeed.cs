using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class JsonFeed
    {
		private HttpClient client;

        public JsonFeed(string uri)
        {
			client = new HttpClient();
			client.BaseAddress = new Uri(uri);
		}

        public dynamic GetResponse(string url)
		{
			string resultJson = client.GetStringAsync(url).Result;
			return JsonConvert.DeserializeObject(resultJson);
		}


    }
}
