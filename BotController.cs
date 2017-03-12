using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SlackBotLib
{
	public class BotController : ApiController
	{
		[HttpPost]
		public HttpResponseMessage Post()
		{
			Console.WriteLine("Received post.");
			NameValueCollection nvc = Request.Content.ReadAsFormDataAsync().Result;
			SlackPost slackPost = new SlackPost(nvc);

			Console.WriteLine(
				string.Format(
					"Text '{0}' from '{1}' in '{2}'", slackPost.Text, slackPost.User_Name, slackPost.Channel_Name));

			if (!SlackBot.AllowPost(slackPost))
			{
				Console.WriteLine("Request not allowed.");
				return Request.CreateResponse(HttpStatusCode.OK);
			}

			var command = SlackBot.GetResponseMethods()
			                      .Where(r => r.Command != null)
								  .FirstOrDefault(r => r.Command == slackPost.Trigger_Word) ??
	                      SlackBot.GetResponseMethods()
			                      .Where(r => r.RegexMatch != null)
								  .FirstOrDefault(r => r.RegexMatch.IsMatch(slackPost.Text));
			if (command == null)
			{
				Console.WriteLine("Command has no handler.");
				return Request.CreateResponse(HttpStatusCode.OK);
			}

			Console.WriteLine("Executing...");

			return Request.CreateResponse(HttpStatusCode.OK,
										  new
										  {
											  text = command.ResponseHandler(slackPost)
										  }
			                             );
		}
	}
}