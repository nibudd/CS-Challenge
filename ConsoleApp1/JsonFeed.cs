using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace ConsoleApp1
{
	public class JsonFeed
	{
		private HttpClient client;
		private MenuItem<bool> menuConnectivityProblem;
		private List<bool> trueFalseList = new List<bool> { true, false };
		private List<string> tryAgainQuitList = new List<string> { "try again", "quit" };

		public JsonFeed(string uri)
		{
			client = new HttpClient();
			client.BaseAddress = new Uri(uri);

			menuConnectivityProblem = new MenuItem<bool>(
				"Connectivity Issue. Try again or quit?: ", trueFalseList);
			menuConnectivityProblem.SetPrintOptions(tryAgainQuitList);
		}

		public dynamic GetResponse(string url)
		{
			string resultJson;
			while (true)
			{
				try
				{
					resultJson = client.GetStringAsync(url).Result;
					break;
				}
				catch (HttpRequestException e)
				{
					RespondToExceptions(e);
				}
				catch (SocketException e)
				{
					RespondToExceptions(e);
				}
				catch (AggregateException ae)
				{
					ae.Handle((e) =>
					{
						if (e is HttpRequestException || e is SocketException)
						{
							RespondToExceptions(e);
							return true;
						}
						return false;
					});
				}
			}
			return JsonConvert.DeserializeObject(resultJson);
		}

		private void RespondToExceptions(Exception e)
		{
			bool tryAgain;
			tryAgain = menuConnectivityProblem.Execute();
			if (!tryAgain)
			{
				Console.WriteLine("Quitting...");
				Environment.Exit(0);
			}
		}
	}
}
