using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class Program
    {
        private static dynamic nameData;
        static bool usesRandomName = false;
        static bool usesCategory = false;
        static List<string> jokeCategories;
        static string jokeCategory;
        static int jokeQuantity;
        static int maxQuantity = 9;
        static bool wantsMoreJokes = true;
        static JsonFeed chuckNorrisFeed = new JsonFeed("https://api.chucknorris.io");
        static JsonFeed namesFeed = new JsonFeed("http://uinames.com/api/");
        private static MenuItem menuUseRandomName;
        private static MenuItem menuUseCategory;
        private static MenuItem menuChooseCategory;
        private static MenuItem menuChooseQuantity;
        private static MenuItem menuKeepRunning;
        private static Dictionary<string, bool> yesNoInputDict;
        private static Dictionary<string, int> oneToNineInputDict;
        private static Dictionary<string, string> categoriesInputDict;
        private static List<string> jokes;

        static void Main(string[] args)
        {
            GetCategories();
            MakeInputDictionaries();
            MakeMenuItems();

            Console.WriteLine("JOKE GENERATOR\n");
            while (wantsMoreJokes)
            {
                menuUseCategory.Execute();
                usesCategory = yesNoInputDict[menuUseCategory.GetInput()];
                
                if (usesCategory)
                {
                    menuChooseCategory.Execute();
                    jokeCategory = categoriesInputDict[menuChooseCategory.GetInput()];
                }

                GetJokes();

                menuUseRandomName.Execute();
                usesRandomName = yesNoInputDict[menuUseRandomName.GetInput()];

                if (usesRandomName)
                {
                    GetNames();
                    ChangeName();
                }

                menuChooseQuantity.Execute();
                jokeQuantity = oneToNineInputDict[menuChooseQuantity.GetInput()];

                PrintJokes();

                menuKeepRunning.Execute();
                wantsMoreJokes = yesNoInputDict[menuKeepRunning.GetInput()];
                
            }
            Console.WriteLine("Goodbye");
        }

        private static void ChangeName()
        {
            bool isFemale = nameData.gender == "female" ? true : false;
            Dictionary<string, string> pronouns = new Dictionary<string, string>();
            if (isFemale)
            {
                pronouns["his"] = "hers";
                pronouns["he"] = "she";
                pronouns["him"] = "her";
            }

            string name = nameData.name;
            string surname = nameData.surname;

            string name_possessive = name + "'";
            if (!name.EndsWith("s"))
                name_possessive += "s";

            string surname_possessive = surname + "'";
            if (!surname.EndsWith("s"))
                surname_possessive += "s";


            foreach (int index in Enumerable.Range(0, jokes.Count()-1))
            {
                string newJoke = jokes[index];
                newJoke = newJoke.Replace("Chuck's", name_possessive);
                newJoke = newJoke.Replace("Norris'", surname_possessive);
                newJoke = newJoke.Replace("Chuck", name);
                newJoke = newJoke.Replace("Norris", surname);
                if (isFemale)
                    foreach (KeyValuePair<string, string> p in pronouns)
                        newJoke = newJoke.Replace(p.Key, p.Value);
                jokes[index] = newJoke;
            }
        }

        private static void GetJokes()
        {
            jokes = new List<string>();
            string url = "jokes/random";
            if (usesCategory)
                url += $"?category={jokeCategory}";

            foreach (int _ in Enumerable.Range(1, maxQuantity))
            {
                JObject result = chuckNorrisFeed.GetResponse(url);
                jokes.Add(result.Value<string>("value"));
            }

        }

        private static void PrintJokes()
        {
            foreach (int x in Enumerable.Range(1, jokeQuantity))
                Console.WriteLine($"{x}. {jokes[x - 1]}");
        }

        private static void MakeMenuItems()
        {
            menuUseRandomName = new MenuItem(
                "Use random name? y/n: ", new List<string>(yesNoInputDict.Keys));

            menuUseCategory = new MenuItem(
                "Specify a category? y/n: ", new List<string>(yesNoInputDict.Keys));

            string chooseCategoryString = CategoriesToString() + "Select a category: ";
            menuChooseCategory = new MenuItem(
                chooseCategoryString, new List<string>(categoriesInputDict.Keys));

            menuChooseQuantity = new MenuItem(
                "Number of jokes? 1-9: ", new List<string>(oneToNineInputDict.Keys));

            menuKeepRunning = new MenuItem(
                "Run again? y/n: ", new List<string>(yesNoInputDict.Keys));
        }

        private static void MakeInputDictionaries()
        {
            yesNoInputDict = makeYesNoDict();
            oneToNineInputDict = makeOneToNineDict();
            categoriesInputDict = makeCategoriesDict();
        }

        private static Dictionary<string, string> makeCategoriesDict()
        {
            Dictionary<string, string> categoriesInputDict = new Dictionary<string, string>();
            foreach (int x in Enumerable.Range(1, jokeCategories.Count()))
                categoriesInputDict[x.ToString()] = jokeCategories[x - 1];
            return categoriesInputDict;
        }

        private static Dictionary<string, int> makeOneToNineDict()
        {
            Dictionary<string, int> oneToNineInputDict = new Dictionary<string, int>();
            foreach (int x in Enumerable.Range(1, 9))
                oneToNineInputDict[x.ToString()] = x;
            return oneToNineInputDict;
        }

        private static Dictionary<string, bool> makeYesNoDict()
        {
            Dictionary<string, bool> yesNoInputDict = new Dictionary<string, bool>();
            yesNoInputDict["y"] = true;
            yesNoInputDict["n"] = false;
            return yesNoInputDict;
        }

        private static void GetCategories()
        {
            JArray result = chuckNorrisFeed.GetResponse("jokes/categories");
            jokeCategories = result.ToObject<List<string>>();
        }

        private static void GetNames()
        {
            nameData = namesFeed.GetResponse("");
        }

        private static string CategoriesToString()
        {
            string categoriesString = "";
            int counter = 0;
            foreach (string category in jokeCategories)
                categoriesString += $"{++counter}. {category}\n";
            return categoriesString;
        }
    }
}
