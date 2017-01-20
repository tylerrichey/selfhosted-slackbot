using System;
using System.Collections.Specialized;

namespace SlackBotLib
{
	public class SlackPost
	{
		public string Token { get; set; }
		public string Team_ID { get; set; }
		public string Team_Domain { get; set; }
		public string Channel_ID { get; set; }
		public string Channel_Name { get; set; }
		public float Timestamp { get; set; }
		public string User_ID { get; set; }
		public string User_Name { get; set; }
		public string Text { get; set; }
		public string Trigger_Word { get; set; }

		public static SlackPost GetFromPost(NameValueCollection nvc)
		{
			return new SlackPost
			{
				Token = nvc["token"],
				Team_ID = nvc["team_id"],
				Team_Domain = nvc["team_domain"],
				Channel_ID = nvc["channel_id"],
				Channel_Name = nvc["channel_name"],
				Timestamp = Convert.ToSingle(nvc["timestamp"]),
				User_ID = nvc["user_id"],
				User_Name = nvc["user_name"],
				Text = nvc["text"].Trim(),
				Trigger_Word = nvc["trigger_word"].Trim()
			};
		}
	}
}
