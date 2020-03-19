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
        private string oldGender;
        private string newFirst;
        private string newLast;
        private string newGender;
        private List<string> malePronouns = new List<string>()
        {
            " his ", " he ", " him ", " His ", " He ", " him.", " his."
        };
        private List<string> femalePronouns = new List<string>()
        {
            " hers ", " she ", " her ", " Hers ", " She ", " her.", " hers."
        };
        private Dictionary<string, string> pronounsDict;
        private string newFirstPossessive;
        private string newLastPossessive;
        private string oldFirstPossessive;
        private string oldLastPossessive;

        public NameSwapper(string first, string second, string gender)
        {
            oldFirst = first;
            oldLast = second;
            oldGender = gender;
            oldFirstPossessive = MakePossessives(oldFirst);
            oldLastPossessive = MakePossessives(oldLast);
        }

        public void GetNames()
		{
			JObject nameData = namesFeed.GetResponse("");
            newFirst = nameData.Value<string>("name");
            newLast = nameData.Value<string>("surname");
            newGender = nameData.Value<string>("gender");

            MakePronounsDict();
            newFirstPossessive = MakePossessives(newFirst);
            newLastPossessive = MakePossessives(newLast);
        }

        public void ChangeName(List<string> strList)
        {
            for (int i = 0; i < strList.Count(); i++)
            {
                string str = strList[i];
                str = SwapPronouns(str);
                str = SwapNames(str);
                strList[i] = str;
            }
        }
        
        private void MakePronounsDict()
        {
            pronounsDict = new Dictionary<string, string>();
            List<string> keys, vals;
            if (oldGender == "male")
            {
                keys = malePronouns;
                vals = femalePronouns;
            }
            else
            {
                keys = femalePronouns;
                vals = malePronouns;
            }

            for (int i = 0; i < keys.Count(); i++)
                pronounsDict[keys[i]] = vals[i];
        }

        private string SwapPronouns(string str)
        {
            if (oldGender != newGender)
                foreach (KeyValuePair<string, string> p in pronounsDict)
                    str = str.Replace(p.Key, p.Value);
            return str;
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
