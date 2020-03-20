using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
	public class NameSwapper
	{
		private JsonFeed namesFeed =
			new JsonFeed("http://uinames.com/api/");
		private string oldName;
		private string newName;
		private string newPossessive;
		private string oldPossessive;

		public NameSwapper(string oldName)
		{
			this.oldName = oldName;
			oldPossessive = MakePossessives(this.oldName);
		}

		public void DownloadNames()
		{
			JObject nameData = namesFeed.GetResponse("?region=canada&gender=male");
			newName = nameData.Value<string>("name") + " " + nameData.Value<string>("surname");
			newPossessive = MakePossessives(newName);
		}

		public void SwapNames(List<string> strList)
		{
			for (int i = 0; i < strList.Count(); i++)
				strList[i] = SwapNameAndPossessive(strList[i]);
		}

		public string SwapNameAndPossessive(string str)
		{
			str = str.Replace(oldPossessive, newPossessive);
			str = str.Replace(oldName, newName);
			return str;
		}

		public string MakePossessives(string name)
		{
			string possessive = name + "'";
			if (!name.EndsWith("s"))
				possessive += "s";
			return possessive;
		}
	}
}
