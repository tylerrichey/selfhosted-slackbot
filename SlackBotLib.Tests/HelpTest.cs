using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SlackBotLib.Tests
{
	[TestFixture]
	public class HelpTest
	{
		[Test]
		public void TestHelpIfNoCommands()
		{
			using (SlackBot s = new SlackBot("base", "token", new List<ResponseMethods>()))
			{
				Assert.AreSame(string.Empty, SlackBot.GetHelp(new SlackPost()));
			}
		}

		[Test]
		public void TestHelpIfCommands()
		{
			using (SlackBot s = new SlackBot("base", "token", new List<ResponseMethods> { new ResponseMethods { Command = "test" } }))
			{
				Assert.AreNotSame(string.Empty, SlackBot.GetHelp(new SlackPost()));
			}
		}

		[Test]
		public void TestHelpIfRegex()
		{
			using (SlackBot s = new SlackBot("base", "token", new List<ResponseMethods> { new ResponseMethods { RegexMatch = new Regex(".*") } }))
			{
				Assert.AreSame(string.Empty, SlackBot.GetHelp(new SlackPost()));
			}
		}
	}
}
