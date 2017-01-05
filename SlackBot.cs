using Microsoft.Owin.Hosting;
using System;
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
			_responseMethods = responseMethods;
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
	}
}
