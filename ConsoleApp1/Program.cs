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
        static char key;
        static Tuple<string, string> names;
        static List<Action> menuMethods = new List<Action>();
        static int menuIndex = 0;
        static bool usesRandomName = false;
        static bool usesCategory = false;
        static List<string> jokeCategories;
        static string category;
        static int jokeQuantity;
        static bool wantsMoreJokes = true;

        static void Main(string[] args)
        {
            GetCategories();
            menuMethods.Add(mUseRandomName);
            menuMethods.Add(mGetNames);
            menuMethods.Add(mUseCategory);
            menuMethods.Add(mSelectCategory);
            menuMethods.Add(mSelectJokeQuantity);
            menuMethods.Add(mPrintJokes);
            menuMethods.Add(mContinue);

            Console.WriteLine("JOKE GENERATOR\n");
            while (wantsMoreJokes)
            {
                menuMethods[menuIndex]();
            }
            Console.WriteLine("Goodbye");
        }

        private static void GetCategories()
        {
            new JsonFeed("https://api.chucknorris.io", 0);
            results = JsonFeed.GetCategories();
            jokeCategories = new List<string>(results);
        }

        private static void mUseRandomName()
        {
            Console.WriteLine("Want to use a random name? y/n: ");
            List<string> validInputs = new List<string>() { "y", "n" };
            string userInput = getUserInput(validInputs);
            if (userInput != "")
            {
                usesRandomName = userInput == "y" ? true : false;
                menuIndex++;
            }

        }

        private static void mGetNames()
        {
            if (!usesRandomName) return;
            new JsonFeed("http://uinames.com/api/", 0);
            dynamic result = JsonFeed.Getnames();
            names = Tuple.Create(result.name.ToString(), result.surname.ToString());
            menuIndex++;
        }

        private static void mUseCategory()
        {
            Console.WriteLine("Want to select a category? y/n: ");
            List<string> validInputs = new List<string>() { "y", "n" };
            string userInput = getUserInput(validInputs);
            if (userInput != "")
            {
                usesCategory = userInput == "y" ? true : false;
                menuIndex++;
            }
        }

        private static void mSelectCategory()
        {
            if (!usesCategory) return;
            printCategories();
            Console.WriteLine("Enter category number: ");
            List<string> validInputs = getRangeList(jokeCategories.Count());
            string userInput = getUserInput(validInputs);
            if (userInput != "")
            {
                category = jokeCategories[Int32.Parse(userInput)-1];
                menuIndex++;
            }
    }

        private static void mSelectJokeQuantity()
        {
            Console.WriteLine("How many jokes do you want? (1-9): ");
            List<string> validInputs = getRangeList(9);
            string userInput = getUserInput(validInputs);
            if (userInput != "")
            {
                jokeQuantity = Int32.Parse(userInput);
                menuIndex++;
            }
        }

        private static void mPrintJokes()
        {
            new JsonFeed("https://api.chucknorris.io", jokeQuantity);
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category);
            Console.WriteLine("[" + string.Join(",", results) + "]");
            menuIndex++;
        }

        private static void mContinue()
        {
            Console.WriteLine("Want to get more jokes? y/n: ");
            List<string> validInputs = new List<string>() { "y", "n" };
            string userInput = getUserInput(validInputs);
            if (userInput != "")
            {
                wantsMoreJokes = userInput == "y" ? true : false;
                menuIndex = 0;
            }
        }

        private static List<string> getRangeList(int maxVal)
        {
            string[] numArray = new string[maxVal];
            foreach (int x in Enumerable.Range(1, maxVal))
            {
                numArray[x - 1] = x.ToString();
            }
            return new List<string>(numArray);
        }

        private static void printCategories()
        {
            int counter = 0;
            foreach (string category in jokeCategories)
            {
                Console.WriteLine($"{++counter}. {category}");
            }
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
