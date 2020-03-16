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

        static void Main(string[] args)
        {
            Console.WriteLine("Press ? to get instructions.");
            if (Console.ReadLine() == "?")
            {
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
