using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static string[] results = new string[50];
        static Tuple<string, string> names;
        static List<Action> menuMethods = new List<Action>();
        static int menuIndex = 0;
        static bool usesRandomName = false;
        static bool usesCategory = false;
        static List<string> jokeCategories;
        static string jokeCategory;
        static int jokeQuantity;
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

        static void Main(string[] args)
        {
            GetCategories();
            mGetNames();
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

                menuUseRandomName.Execute();
                usesRandomName = yesNoInputDict[menuUseRandomName.GetInput()];

                menuChooseQuantity.Execute();
                jokeQuantity = oneToNineInputDict[menuChooseQuantity.GetInput()];

                mPrintJokes();

                menuKeepRunning.Execute();
                wantsMoreJokes = yesNoInputDict[menuKeepRunning.GetInput()];
                
            }
            Console.WriteLine("Goodbye");
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
            string[] result = chuckNorrisFeed.GetResponse<string[]>("jokes/categories");
            jokeCategories = new List<string>(result);
        }

        private static void mGetNames()
        {
            if (!usesRandomName) return;
            Dictionary<string, string> result = namesFeed.GetResponse<Dictionary<string, string>>("");
            names = Tuple.Create(result["name"], result["surname"]);
            menuIndex++;
        }

        private static void mPrintJokes()
        {
            string url = "jokes/random";
            if (usesCategory)
                url += $"?category={jokeCategory}";
            Newtonsoft.Json.Linq.JObject result = chuckNorrisFeed.GetResponse<Newtonsoft.Json.Linq.JObject>(url);
            string joke = result.Value<string>("value");
            Console.WriteLine("[" + string.Join(",", result) + "]");
            menuIndex++;
        }

        private static string CategoriesToString()
        {
            string categoriesString = "";
            int counter = 0;
            foreach (string category in jokeCategories)
                categoriesString += $"{++counter}. {category}\n";
            return categoriesString;
        }

        private static string getUserInput(List<string> validInputs)
        {
            string userInput = Console.ReadLine();
            if (validInputs.Contains(userInput))
                return userInput;
            else
            {
                return invalidUserInput();
            }
        }

        private static string invalidUserInput()
        {
            Console.WriteLine("Invalid input.");
            return "";
        }
    }
}
