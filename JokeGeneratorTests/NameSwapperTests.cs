using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System.Collections.Generic;

namespace JokeGeneratorTests
{
	[TestClass]
	public class NameSwapperTests
	{
		public NameSwapper MakeNameSwapper()
		{
			return new NameSwapper("Chuck Norris");
		}

		[TestMethod]
		public void MakePossessive_AddsSWhenAppropriate()
		{
			NameSwapper nameSwapper = MakeNameSwapper();
			List<string> namesSingular = new List<string> { "Sam", "Lars" };
			List<string> namesPlural = new List<string> { "Sam's", "Lars'" };
			for (int i = 0; i < 2; i++)
			{
				string pluralizedName = nameSwapper.MakePossessives(namesSingular[i]);
				StringAssert.Contains(pluralizedName, namesPlural[i]);
			}

		}

		[TestMethod]
		public void SwapNames_CorrectlyExchangesNames()
		{
			NameSwapper nameSwapper = MakeNameSwapper();
			nameSwapper.DownloadNames();
			string oldName = "Chuck Norris";
			string oldPossessive = "Chuck Norris'";
			string newName = nameSwapper.SwapNameAndPossessive(oldName);
			string newPossessive = nameSwapper.SwapNameAndPossessive(oldPossessive);

			List<string> oldNames = new List<string> 
			{ $". {oldName}", $"{oldName}.", $"{oldPossessive}" };
			List<string> newNames = new List<string>
			{ $". {newName}", $"{newName}.", $"{newPossessive}" };

			for (int i = 0; i < oldNames.Count; i++)
			{
				string swappedName = nameSwapper.SwapNameAndPossessive(oldNames[i]);
				StringAssert.Contains(swappedName, newNames[i]);
			}
		}
	}
}
