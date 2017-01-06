using Microsoft.Owin.Hosting;
using System;
using System.Text;
using System.Collections.Generic;

namespace SlackBotLib
{
	public class SlackBot
	{
		public delegate string ResponseDelegate(string command);
		private string _baseAddress;
		private static string _token;
		private static List<ResponseMethods> _responseMethods;

		public SlackBot(string baseAddress, string apiToken, List<ResponseMethods> responseMethods)
		{
			_responseMethods.Add(
				new ResponseMethods
				{
					Command = "!help",
					Usage = "Get a list of all bot commands.",
					ResponseHandler = GetHelp
				}
			);
			_responseMethods.AddRange(responseMethods);
			_token = apiToken;
			_baseAddress = baseAddress;
		}

		public void StartBot()
		{
			using (WebApp.Start<Startup>(_baseAddress))
			{
				Console.WriteLine("Slackbot running on: " + _baseAddress);
				Console.WriteLine("Press any key to quit...");
				Console.ReadLine();
			}
		}

		public static string GetToken()
		{
			return _token;
		}

		public static List<ResponseMethods> GetResponseMethods()
		{
			return _responseMethods;
		}

		private static string GetHelp(string command)
		{
			StringBuilder sb = new StringBuilder();
			_responseMethods.ForEach(r => sb.AppendLine(
				string.Format("{0} - {1}", r.Command, r.Usage)));
			return sb.ToString();
		}
	}
}
