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
			return new NameSwapper("Chuck", "Norris");
		}

		[TestMethod]
		public void DownloadNames_NoExceptions()
		{
			NameSwapper nameSwapper = MakeNameSwapper();
			try
			{
				nameSwapper.DownloadNames();
			}
			catch
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void ChangeName_NoExceptions()
		{
			NameSwapper nameSwapper = MakeNameSwapper();
			nameSwapper.DownloadNames();
			List<string> strList = new List<string>
			{
				"Chuck Norris",
				"Chuck's Norris",
				"Chuck Norris'"
			};
			
			try
			{
				nameSwapper.ChangeName(strList);
			}
			catch
			{
				Assert.Fail();
			}
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
			string newFirst = nameSwapper.SwapNames("Chuck");
			string newLast = nameSwapper.SwapNames("Norris");

			List<string> oldNames = new List<string> 
			{ ". Chuck Norris", "Chuck Norris.", "Chucky" };
			List<string> newNames = new List<string> 
			{ $". {newFirst} {newLast}", $"{newFirst} {newLast}.", "Chucky" };

			for (int i = 0; i < oldNames.Count; i++)
			{
				string swappedName = nameSwapper.SwapNames(oldNames[i]);
				StringAssert.Contains(swappedName, newNames[i]);
			}
		}
	}
}
