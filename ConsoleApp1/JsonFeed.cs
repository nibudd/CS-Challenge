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
        
		public string[] GetRandomJokes(string firstname, string lastname, string category)
		{
			string url = "jokes/random";
			if (category != null)
			{
				if (url.Contains('?'))
					url += "&";
				else url += "?";
				url += "category=";
				url += category;
			}

            string joke = client.GetStringAsync(url).Result;

            if (firstname != null && lastname != null)
            {
                int index = joke.IndexOf("Chuck Norris");
                string firstPart = joke.Substring(0, index);
                string secondPart = joke.Substring(0 + index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
                joke = firstPart + firstname + " " + lastname + secondPart;
            }

            return new string[] { JsonConvert.DeserializeObject<dynamic>(joke).value };
        }

        public dynamic GetResponse<T>(string url)
		{
			string resultJson = client.GetStringAsync(url).Result;
			return JsonConvert.DeserializeObject<T>(resultJson);
		}


    }
}
