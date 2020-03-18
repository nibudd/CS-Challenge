using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
	public abstract class MenuItem
	{
		private string promptText;
		private string userInput;
		private Dictionary<string, object> validInputDict;
		private object result;
		private bool itemCompleted;

		public MenuItem(string promptText, Dictionary<string, object> validInputDict)
		{
			this.promptText = promptText;
			this.validInputDict = validInputDict;
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
				{
					InterpretInput();
					itemCompleted = true;
				}
			}
		}

		private object GetResult()
		{
			return result;
		}

		private void PrintPrompt()
		{
			if (promptText != "")
				Console.Write(promptText);
		}

		private void GetUserInput()
		{
			string userInput = Console.ReadLine();
			if (GetValidInputs().Contains(userInput))
				this.userInput = userInput;
			else
				InvalidUserInput();
		}

		private void InterpretInput()
		{
			result = validInputDict[userInput];
		}

		private List<string> GetValidInputs()
		{
			return new List<string>(validInputDict.Keys);
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
