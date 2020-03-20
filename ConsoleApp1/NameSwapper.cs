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
		private string oldBadPossessive;

		public NameSwapper(string oldName)
		{
			this.oldName = oldName;
			oldPossessive = MakePossessive(this.oldName);
			oldBadPossessive = MakeBadPossessive(this.oldName);
		}

		public void DownloadNames()
		{
			JObject nameData = namesFeed.GetResponse("?region=canada&gender=male");
			newName = nameData.Value<string>("name") + " " + nameData.Value<string>("surname");
			newPossessive = MakePossessive(newName);
		}

		public void SwapNames(List<string> strList)
		{
			for (int i = 0; i < strList.Count(); i++)
				strList[i] = SwapNameAndPossessive(strList[i]);
		}

		public string SwapNameAndPossessive(string str)
		{
			str = str.Replace(oldPossessive, newPossessive);
			str = str.Replace(oldBadPossessive, newPossessive);
			str = str.Replace(oldName, newName);
			return str;
		}

		public string MakePossessive(string name)
		{
			return name + "'s";
		}

		public string MakeBadPossessive(string name)
		{
			return name + "'";
		}
	}
}
