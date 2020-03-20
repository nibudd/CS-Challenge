using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
	public class JokeHandler
	{
		private JsonFeed chuckNorrisFeed =
			new JsonFeed("https://api.chucknorris.io");
		private List<string> jokeCategories;

		public JokeHandler() { }

		public List<string> GetJokes(string jokeCategory, bool usesCategory, int quantity)
		{
			List<string> jokes = new List<string>();
			string url = "jokes/random";
			if (usesCategory)
				url += $"?category={jokeCategory}";

			foreach (int _ in Enumerable.Range(1, quantity))
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
