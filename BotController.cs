using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
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

			if (nvc["user_id"] == "USLACKBOT")
			{
				Console.WriteLine("Ignoring post back from a bot response.");
				return Request.CreateResponse(HttpStatusCode.OK);
			}

			if (nvc["token"] != SlackBot.GetToken())
			{
				Console.WriteLine("Post received with invalid token, ignoring.");
				return Request.CreateResponse(HttpStatusCode.Forbidden);
			}

			string textCommand = nvc["text"].Trim();
			Console.WriteLine("Executing: " + textCommand);

			var command = SlackBot.GetResponseMethods()
			                      .FirstOrDefault(r => r.Command == textCommand.Split(' ')[0]);
			if (command == null)
			{
				Console.WriteLine("Command has no handler.");
				return Request.CreateResponse(HttpStatusCode.OK);
			}

			return Request.CreateResponse(HttpStatusCode.OK, PackResponse(command.ResponseHandler(textCommand)));
		}

		private static Dictionary<string, string> PackResponse(string text)
		{
			var d = new Dictionary<string, string>();
			d.Add("text", text);
			return d;
		}
	}
}