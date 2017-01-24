using Microsoft.Owin.Hosting;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace SlackBotLib
{
	public class SlackBot
	{
		public delegate string ResponseDelegate(SlackPost slackPost);
		public delegate bool AllowPostDelegate(SlackPost slackPost);
		private string _baseAddress;
		private static string _token;
		private static string _defaultHelpCommand = "!help";
		private string _generalGroup = "General";
		private static List<ResponseMethods> _responseMethods = new List<ResponseMethods>();
		private static AllowPostDelegate _allowPostDelegate = AllowPostDefault;

		public SlackBot(string baseAddress, string apiToken, List<ResponseMethods> responseMethods)
		{
			_responseMethods.Add(
				new ResponseMethods
				{
					Command = _defaultHelpCommand,
					Usage = "Get a list of all bot commands.",
					ResponseHandler = GetHelp,
					Group = _generalGroup
				}
			);
			_responseMethods.AddRange(responseMethods);
			_responseMethods.Where(r => r.Group == string.Empty || r.Group == null)
							.ToList()
							.ForEach(r => r.Group = _generalGroup);
			_token = apiToken;
			_baseAddress = baseAddress;
		}

		public SlackBot(string baseAddress, string apiToken, List<ResponseMethods> responseMethods, string helpCommand)
			: this(baseAddress, apiToken, responseMethods)
		{
			_responseMethods.FirstOrDefault(r => r.Command == _defaultHelpCommand).Command = helpCommand;
		}

		public SlackBot(string baseAddress, string apiToken, List<ResponseMethods> responseMethods, string helpCommand, AllowPostDelegate allowPostDelegate)
			: this(baseAddress, apiToken, responseMethods, helpCommand)
		{
			_allowPostDelegate = allowPostDelegate;
		}

		public SlackBot(string baseAddress, string apiToken, List<ResponseMethods> responseMethods, AllowPostDelegate allowPostDelegate)
			: this(baseAddress, apiToken, responseMethods, _defaultHelpCommand, allowPostDelegate) { }

		public void StartBot()
		{
			using (WebApp.Start<Startup>(_baseAddress))
			{
				Console.WriteLine("Slackbot running on: " + _baseAddress);
				Console.WriteLine("Press any key to quit...");
				Console.ReadLine();
			}
		}

		public static List<ResponseMethods> GetResponseMethods()
		{
			return _responseMethods;
		}

		public static bool AllowPost(SlackPost slackPost)
		{
			return (slackPost.Token == _token) && _allowPostDelegate(slackPost);
		}

		private static bool AllowPostDefault(SlackPost slackPost)
		{
			return true;
		}

		private static string GetHelp(SlackPost slackPost)
		{
			StringBuilder sb = new StringBuilder();
			if (_responseMethods.Select(r => r.Group).Distinct().Count() == 1)
			{
				sb.AppendLine("Bot Command - Usage");
				_responseMethods.ForEach(r => sb.AppendLine(
					string.Format("{0} - {1}", r.Command, r.Usage)));
			}
			else
			{
				var responseGroups = _responseMethods.GroupBy(r => r.Group)
													 .Select(r => new { r.Key, responseMethods = r.ToList() });
				foreach (var groups in responseGroups)
				{
					sb.AppendLine(string.Format("{0}:", groups.Key));
					groups.responseMethods.ForEach(g =>
											  sb.AppendLine(
												  string.Format("\t{0} - {1}", g.Command, g.Usage)
												 )
											 );
				}
				
			}
			return sb.ToString();
		}
	}
}
