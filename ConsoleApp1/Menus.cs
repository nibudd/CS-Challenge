using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
	public class MenuItem<T>
	{
		private string promptText;
		private T userInput;
		List<T> inputOptions;
		List<string> printOptions;
		private bool hasValidInput;
		private bool showOptions;

		public MenuItem(string promptText, List<T> inputOptions)
		{
			this.promptText = promptText;
			this.inputOptions = inputOptions;
			hasValidInput = false;
			showOptions = true;
		}

		public void SetPrintOptions(List<string> printOptions)
		{
			this.printOptions = printOptions;
		}

		public void ShowPrintOptions(bool showOptions)
		{
			this.showOptions = showOptions;
		}

		public T Execute()
		{
			hasValidInput = false;
			while (!hasValidInput)
			{
				PrintPrompt();
				GetUserInput();
			}
			return userInput;
		}

		private void PrintPrompt()
		{
			if (showOptions)
				PrintOptions();
			Console.Write(promptText);
		}

		private void PrintOptions()
		{
			for (int i = 1; i <= inputOptions.Count; i++)
			{
				if (printOptions == null)
					Console.WriteLine($"{i}. {inputOptions[i - 1]}");
				else
					Console.WriteLine($"{i}. {printOptions[i - 1]}");
			}
		}

		private void GetUserInput()
		{
			try
			{
				ValidateInput(Int32.Parse(Console.ReadLine()));
			}
			catch (FormatException)
			{
				InvalidInput();
			}
		}

		private void ValidateInput(int numInput)
		{
			if (0 < numInput && numInput <= inputOptions.Count)
			{
				this.userInput = inputOptions[numInput - 1];
				hasValidInput = true;
			}
			else
				InvalidInput();

		}

		private void InvalidInput()
		{
			Console.WriteLine("Invalid input.");
			hasValidInput = false;
		}
	}
}
