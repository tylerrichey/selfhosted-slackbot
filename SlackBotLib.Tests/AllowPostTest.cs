using NUnit.Framework;
using System.Collections.Generic;

namespace SlackBotLib.Tests
{
	[TestFixture]
	public class AllowPostTest
	{
		[Test]
		public void AllowPostGoodTokenTest()
		{
			var token = "testtoken";
			var p = new SlackPost();
			p.Token = token;
			using (SlackBot s = new SlackBot("base", token, new List<ResponseMethods>()))
			{
				Assert.AreEqual(true, SlackBot.AllowPost(p));
			}
		}

		[Test]
		public void AllowPostBadTokenTest()
		{
			var token = "testtokenbad";
			var p = new SlackPost();
			p.Token = "testtoken";
			using (SlackBot s = new SlackBot("base", token, new List<ResponseMethods>()))
			{
				Assert.AreEqual(false, SlackBot.AllowPost(p));
			}
		}

		[Test]
		public void AllowPostDelegateFalseTest()
		{
			var token = "testtoken";
			var p = new SlackPost();
			p.Token = token;
			using (SlackBot s = new SlackBot("base", token, new List<ResponseMethods>(), AllowPost))
			{
				Assert.AreEqual(false, SlackBot.AllowPost(p));
			}
		}

		private bool AllowPost(SlackPost slackPost)
		{
			return false;
		}
	}
}
