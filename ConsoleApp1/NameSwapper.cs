using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
	class NameSwapper
	{
		private JsonFeed namesFeed = 
			new JsonFeed("http://uinames.com/api/");
        private string originalFirst;
        private string originalLast;
        private string originalGender;
        private string newFirst;
        private string newLast;
        private string newGender;


        public NameSwapper(string first, string second, string gender)
        {
            originalFirst = first;
            originalLast = second;
            originalGender = gender;
        }

		public void GetNames()
		{
			dynamic nameData = namesFeed.GetResponse("");
            newFirst = nameData.name;
            newLast = nameData.surname;
            newGender = nameData.gender;
		}

        public void ChangeName(List<string> strings)
        {
            bool isFemale = newGender == "female" ? true : false;
            Dictionary<string, string> pronouns = new Dictionary<string, string>();
            if (isFemale)
            {
                pronouns[" his "] = " hers ";
                pronouns[" he "] = " she ";
                pronouns[" him "] = " her ";
                pronouns[" His "] = " Hers ";
                pronouns[" He "] = " She ";
                pronouns[" him."] = " her.";
                pronouns[" his."] = " hers.";
            }

            string name_possessive = newFirst + "'";
            if (!newFirst.EndsWith("s"))
                name_possessive += "s";

            string surname_possessive = newLast + "'";
            if (!newLast.EndsWith("s"))
                surname_possessive += "s";


            foreach (int index in Enumerable.Range(0, strings.Count()))
            {
                string newJoke = strings[index];
                newJoke = newJoke.Replace("Chuck's", name_possessive);
                newJoke = newJoke.Replace("Norris'", surname_possessive);
                newJoke = newJoke.Replace("Norris's", surname_possessive);
                newJoke = newJoke.Replace("Chuck", newFirst);
                newJoke = newJoke.Replace("Norris", newLast);
                if (isFemale)
                    foreach (KeyValuePair<string, string> p in pronouns)
                        newJoke = newJoke.Replace(p.Key, p.Value);
                strings[index] = newJoke;
            }
        }
    }
}
