using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
	class Program
	{
		static bool usesRandomName = false;
		static bool usesCategory = false;
		static string jokeCategory;
		static int jokeQuantity;
		static int maxQuantity = 9;
		static bool wantsMoreJokes = true;
		private static List<string> jokes;
		private static List<bool> trueFalseList;
		private static List<string> yesNoList;
		private static List<int> numbersList;
		private static MenuItem<bool> menuUseRandomName;
		private static MenuItem<bool> menuUseCategory;
		private static MenuItem<string> menuChooseCategory;
		private static MenuItem<int> menuChooseQuantity;
		private static MenuItem<bool> menuKeepRunning;
		private static NameSwapper nameSwapper = new NameSwapper("Chuck Norris");
		private static JokeHandler jokeHandler = new JokeHandler();

		static void Main(string[] args)
		{
			Setup();

			while (wantsMoreJokes)
			{
				usesCategory = menuUseCategory.Execute();

				if (usesCategory)
					jokeCategory = menuChooseCategory.Execute();

				jokeQuantity = menuChooseQuantity.Execute();
				jokes = jokeHandler.GetJokes(jokeCategory, usesCategory, jokeQuantity);

				usesRandomName = menuUseRandomName.Execute();

				if (usesRandomName)
				{
					nameSwapper.DownloadNames();
					nameSwapper.SwapNames(jokes);
				}

				PrintJokes();

				wantsMoreJokes = menuKeepRunning.Execute();
			}

			Teardown();
		}

		private static void Teardown()
		{
			Console.WriteLine("Goodbye");
		}

		private static void Setup()
		{
			jokeHandler.GetCategories();
			MakeMenuItems();
			Console.WriteLine("JOKE GENERATOR\n");
		}

		private static void PrintJokes()
		{
			foreach (int x in Enumerable.Range(1, jokeQuantity))
				Console.WriteLine($"{x}. {jokes[x - 1]}");
		}

		private static void MakeMenuItems()
		{
			MakeLists();

			menuUseRandomName = new MenuItem<bool>("Use random name?: ", trueFalseList);
			menuUseRandomName.SetPrintOptions(yesNoList);

			menuUseCategory = new MenuItem<bool>("Specify a category?: ", trueFalseList);
			menuUseCategory.SetPrintOptions(yesNoList);

			menuChooseCategory = new MenuItem<string>("Select a category: ", jokeHandler.ListCategories());

			menuChooseQuantity = new MenuItem<int>($"Number of jokes? 1-{maxQuantity}: ", numbersList);
			menuChooseQuantity.ShowPrintOptions(false);

			menuKeepRunning = new MenuItem<bool>("Run again?: ", trueFalseList);
			menuKeepRunning.SetPrintOptions(yesNoList);
		}

		private static void MakeLists()
		{
			trueFalseList = new List<bool> { true, false };
			yesNoList = new List<string> { "yes", "no" };
			numbersList = new List<int>();
			for (int i = 1; i <= maxQuantity; i++)
				numbersList.Add(i);
		}
	}
}
