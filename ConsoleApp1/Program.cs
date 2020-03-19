using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static bool usesRandomName = false;
        static bool usesCategory = false;
        static List<string> jokeCategories;
        static string jokeCategory;
        static int jokeQuantity;
        static int maxQuantity = 9;
        static bool wantsMoreJokes = true;
        static JsonFeed chuckNorrisFeed = new JsonFeed("https://api.chucknorris.io");
        private static MenuItem menuUseRandomName;
        private static MenuItem menuUseCategory;
        private static MenuItem menuChooseCategory;
        private static MenuItem menuChooseQuantity;
        private static MenuItem menuKeepRunning;
        private static Dictionary<string, bool> yesNoInputDict;
        private static Dictionary<string, int> oneToNineInputDict;
        private static Dictionary<string, string> categoriesInputDict;
        private static List<string> jokes;
        private static NameSwapper nameSwapper = new NameSwapper("Chuck", "Norris", "male");

        static void Main(string[] args)
        {
            GetCategories();
            MakeInputDictionaries();
            MakeMenuItems();

            Console.WriteLine("JOKE GENERATOR\n");
            while (wantsMoreJokes)
            {
                nameSwapper.GetNames();

                usesCategory = RunMenuItem<bool>(menuUseCategory, yesNoInputDict);
                
                if (usesCategory)
                    jokeCategory = RunMenuItem<string>(menuChooseCategory, categoriesInputDict);

                GetJokes();

                usesRandomName = RunMenuItem<bool>(menuUseRandomName, yesNoInputDict);

                if (usesRandomName)
                {
                    nameSwapper.ChangeName(jokes);
                }

                jokeQuantity = RunMenuItem<int>(menuChooseQuantity, oneToNineInputDict);
                
                PrintJokes();

                wantsMoreJokes = RunMenuItem<bool>(menuKeepRunning, yesNoInputDict);
            }
            Console.WriteLine("Goodbye");
        }

        private static T RunMenuItem<T>(MenuItem menuItem, Dictionary<string, T> inputDict)
        {
            menuItem.Execute();
            return inputDict[menuItem.GetInput()];
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
            yesNoInputDict = MakeYesNoDict();
            oneToNineInputDict = MakeOneToNineDict();
            categoriesInputDict = MakeCategoriesDict();
        }

        private static Dictionary<string, string> MakeCategoriesDict()
        {
            Dictionary<string, string> categoriesInputDict = new Dictionary<string, string>();
            foreach (int x in Enumerable.Range(0, jokeCategories.Count()))
                categoriesInputDict[(x+1).ToString()] = jokeCategories[x];
            return categoriesInputDict;
        }

        private static Dictionary<string, int> MakeOneToNineDict()
        {
            Dictionary<string, int> oneToNineInputDict = new Dictionary<string, int>();
            foreach (int x in Enumerable.Range(1, 9))
                oneToNineInputDict[x.ToString()] = x;
            return oneToNineInputDict;
        }

        private static Dictionary<string, bool> MakeYesNoDict()
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
