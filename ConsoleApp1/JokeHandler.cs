using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
	class JokeHandler
	{
        private int maxQuantity;
        private JsonFeed chuckNorrisFeed =
            new JsonFeed("https://api.chucknorris.io");
        private List<string> jokeCategories;

        public JokeHandler(int maxQuantity)
        {
            this.maxQuantity = maxQuantity;
        }

        public List<string> GetJokes(string jokeCategory, bool usesCategory)
        {
            List<string> jokes = new List<string>();
            string url = "jokes/random";
            if (usesCategory)
                url += $"?category={jokeCategory}";

            foreach (int _ in Enumerable.Range(1, maxQuantity))
            {
                JObject result = chuckNorrisFeed.GetResponse(url);
                jokes.Add(result.Value<string>("value"));
            }

            return jokes;
        }

        public void GetCategories()
        {
            JArray result = chuckNorrisFeed.GetResponse("jokes/categories");
            jokeCategories = result.ToObject<List<string>>();
        }

        public string CategoriesToString()
        {
            string categoriesString = "";
            int counter = 0;
            foreach (string category in jokeCategories)
                categoriesString += $"{++counter}. {category}\n";
            return categoriesString;
        }

        public int CategoryCount()
        {
            return jokeCategories.Count();
        }

        public List<string> ListCategories()
        {
            return jokeCategories;
        }
    }
}
