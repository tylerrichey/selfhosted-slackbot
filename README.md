This is a self-hosted Slack bot for outgoing webhooks using the OWIN .NET self-hosting libraries. You will need to start a new console application targeting Mono or .NET v4.5 and provide a base url, an API token from Slack, and a List of objects that define the commands available to your bot. 

You can install the library via nuget with: Install-Package SlackBotLib

Here's an example usage:
```c#
using SlackBotLib;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ResponseMethods> responseMethods = new List<ResponseMethods>
            {
                new ResponseMethods
                {
                    Command = "!status",
                    Usage = "Displays the system status.",
                    ResponseHandler = GetStatus
                }
            };
						
			//Example of the overload to specify the help command in channel
			//SlackBot slackBot = new SlackBot("http://myhostname.com:9000", "asdasdasdasd", responseMethods, ".help");
            SlackBot slackBot = new SlackBot("http://myhostname.com:9000", "asdasdasdasd", responseMethods);
            slackBot.StartBot();
        }

        private static string GetStatus(string command)
        {
            return "This is the response to !status";
        }
    }
}
```

The full API URL you will put in Slack, will be: baseaddress/api/bot, i.e.: http://myhostname.com:9000/api/bot

The bot comes with one pre-defined command, "!help", which lists out the 'Command' and 'Usage' fields from every item in the List of ResponseMethods. This command can be overridden in an overload of the constructor as well.
