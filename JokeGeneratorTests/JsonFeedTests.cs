using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using Newtonsoft.Json.Linq;

namespace JokeGeneratorTests
{
	[TestClass]
	public class JsonFeedTests
	{
		[TestMethod]
		public void GetResponse_TestReturnType()
		{
			JsonFeed jsonFeed = new JsonFeed("http://uinames.com/api/");
			Assert.IsInstanceOfType(jsonFeed.GetResponse(""), (new JObject()).GetType());
		}
	}
}
