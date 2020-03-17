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

        static void Main(string[] args)
        {
            GetCategories();
            menuMethods.Add(mUseRandomName);

            Console.WriteLine("JOKE GENERATOR\n");
            while (true)
            {
                Console.WriteLine("Press c to get categories");
                Console.WriteLine("Press r to get random jokes");
                key = Console.ReadKey().KeyChar;
                if (key == 'c')
                {
                    getCategories();
                    PrintResults();
                }
                if (key == 'r')
                {
                    Console.WriteLine("Want to use a random name? y/n");
                    key = Console.ReadKey().KeyChar;
                    if (key == 'y')
                        GetNames();
                    Console.WriteLine("Want to specify a category? y/n");
                    if (key == 'y')
                    {
                        Console.WriteLine("How many jokes do you want? (1-9)");
                        int n = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Enter a category;");
                        GetRandomJokes(Console.ReadLine(), n);
                        PrintResults();
                    }
                    else
                    {
                        Console.WriteLine("How many jokes do you want? (1-9)");
                        int n = Int32.Parse(Console.ReadLine());
                        GetRandomJokes(null, n);
                        PrintResults();
                    }
                }
                names = null;
            }
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
                usesRandomName = userInput == "y" ? true : false;
        }

        private static void mUseCategory()
        {
            Console.WriteLine("Want to select a category? y/n: ");
            List<string> validInputs = new List<string>() { "y", "n" };
            string userInput = getUserInput(validInputs);
            if (userInput != "")
                usesCategory = userInput == "y" ? true : false;
        }

        private static void mSelectCategory()
        {
            if (!usesCategory) return;
            printCategories();
            Console.WriteLine("Enter category number: ");
            List<string> validInputs = getRangeList(jokeCategories.Count());
            string userInput = getUserInput(validInputs);
            if (userInput != "")
                category = validInputs[Int32.Parse(userInput)];
        }

        private static void mSelectJokeQuantity()
        {
            Console.WriteLine("How many jokes do you want? (1-9): ");
            List<string> validInputs = new List<string>() { "y", "n" };
            string userInput = getUserInput(validInputs);
            if (userInput != "")
                jokeQuantity = Int32.Parse(userInput);
        }

        private static void mPrintJokes()
        {
            new JsonFeed("https://api.chucknorris.io", jokeQuantity);
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category);
            Console.WriteLine("[" + string.Join(",", results) + "]");

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
                Console.WriteLine($"{counter++}. {category}");
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

        private static void PrintResults()
        {
            Console.WriteLine("[" + string.Join(",", results) + "]");
        }

        private static void GetRandomJokes(string category, int number)
        {
            new JsonFeed("https://api.chucknorris.io", number);
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category);
        }

        private static void getCategories()
        {
            new JsonFeed("https://api.chucknorris.io", 0);
            results = JsonFeed.GetCategories();
        }

        private static void GetNames()
        {
            new JsonFeed("http://uinames.com/api/", 0);
            dynamic result = JsonFeed.Getnames();
            names = Tuple.Create(result.name.ToString(), result.surname.ToString());
        }
    }
}
