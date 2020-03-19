using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System.Collections.Generic;

namespace JokeGeneratorTests
{
	[TestClass]
	class JokeHandlerTests
	{
		[TestMethod]
		public void GetJokes_ReturnsCorrectQuantity()
		{
			JokeHandler jokeHandler = new JokeHandler();
			List<string> jokes;
			for (int i = 1; i <= 9; i++)
			{
				jokes = jokeHandler.GetJokes("", false, i);
				Assert.AreEqual(i, jokes.Count);
			}
		}
	}
}
