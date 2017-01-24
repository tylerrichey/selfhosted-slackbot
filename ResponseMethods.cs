namespace SlackBotLib
{
	public class ResponseMethods
	{
		public string Command { get; set; }
		public string Usage { get; set; }
		public SlackBot.ResponseDelegate ResponseHandler { get; set; }
		public string Group { get; set; }
	}
}
