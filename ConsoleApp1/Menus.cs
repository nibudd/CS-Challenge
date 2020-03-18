using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
	public class MenuItem
	{
		private string promptText;
		private string userInput;
		private List<string> validInput;
		private bool itemCompleted;

		public MenuItem(string promptText, List<string> validInput)
		{
			this.promptText = promptText;
			this.validInput = validInput;
			itemCompleted = false;
		}

		public void Execute()
		{
			itemCompleted = false;
			while (!itemCompleted)
			{
				PrintPrompt();
				GetUserInput();
				if (IsValidInput())
					itemCompleted = true;
			}
		}

		public string GetInput()
		{
			return userInput;
		}

		private void PrintPrompt()
		{
			if (promptText != "")
				Console.Write(promptText);
		}

		private void GetUserInput()
		{
			string userInput = Console.ReadLine();
			if (validInput.Contains(userInput))
				this.userInput = userInput;
			else
				InvalidUserInput();
		}

		private void InvalidUserInput()
		{
			Console.WriteLine("Invalid input.");
			userInput = "";
		}

		private bool IsValidInput()
		{
			return (userInput == "") ? false : true;
		}
	}
}
