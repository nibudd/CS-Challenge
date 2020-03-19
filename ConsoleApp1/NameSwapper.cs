using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class NameSwapper
	{
		private JsonFeed namesFeed = 
			new JsonFeed("http://uinames.com/api/");
        private string oldFirst;
        private string oldLast;
        private string newFirst;
        private string newLast;
        private string newFirstPossessive;
        private string newLastPossessive;
        private string oldFirstPossessive;
        private string oldLastPossessive;

        public NameSwapper(string first, string second, string gender)
        {
            oldFirst = first;
            oldLast = second;
            oldFirstPossessive = MakePossessives(oldFirst);
            oldLastPossessive = MakePossessives(oldLast);
        }

        public void GetNames()
		{
			JObject nameData = namesFeed.GetResponse("?region=canada&gender=male");
            newFirst = nameData.Value<string>("name");
            newLast = nameData.Value<string>("surname");

            newFirstPossessive = MakePossessives(newFirst);
            newLastPossessive = MakePossessives(newLast);
        }

        public void ChangeName(List<string> strList)
        {
            for (int i = 0; i < strList.Count(); i++)
                strList[i] = SwapNames(strList[i]);
        }
        
        private string SwapNames(string str)
        {
            str = str.Replace(oldFirstPossessive, newFirstPossessive);
            str = str.Replace(oldLastPossessive, newLastPossessive);
            str = str.Replace(oldFirst, newFirst);
            str = str.Replace(oldLast, newLast);
            return str;
        }

        private string MakePossessives(string name)
        {
            string possessive = name + "'";
            if (!name.EndsWith("s"))
                possessive += "s";
            return possessive;
        }
    }
}
