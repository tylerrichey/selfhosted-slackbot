[![Build status](https://ci.appveyor.com/api/projects/status/rkj9m6ccfbq23koc?svg=true)](https://ci.appveyor.com/project/tylerrichey/selfhosted-slackbot)

This is a self-hosted Slack bot for outgoing webhooks using the OWIN .NET self-hosting libraries. You will need to start a new console application targeting Mono or .NET v4.5 and provide a base URL, an API token from Slack, and a List of objects that define the commands available to your bot. 

You can install the library via nuget with: Install-Package SlackBotLib

Here's a basic example usage:
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
						
			SlackBot slackBot = new SlackBot("http://myhostname.com:9000", "APIToken", responseMethods);
			slackBot.StartBot();
        }

        private static string GetStatus(SlackPost slackPost)
        {
            return "This is the response to !status for " + slackPost.User_Name;
        }
    }
}
```

More advanced usage:

```c#
using SlackBotLib;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
    	private string _buildGroup = "Build Commands";
        static void Main(string[] args)
        {
            List<ResponseMethods> responseMethods = new List<ResponseMethods>
            {
                new ResponseMethods
                {
                    Command = ".status",
                    Usage = "Displays the system status.",
                    ResponseHandler = GetStatus,
		    		Group = _buildGroup
                },
				new ResponseMethods
				{
					Command = ".build",
					Usage = "Start a build.",
					ResponseHandler = StartBuild,
					Group = _buildGroup
				}
            };
	    
			SlackBot slackBot = new SlackBot("http://myhostname.com:9000", 
											"APIToken", 
											responseMethods, 
											".help", 
											AllowPost);
			slackBot.StartBot();
        }
	
		private static bool AllowPost(SlackPost slackPost)
		{
			return (slackPost.User_Name != "OtherSlackBot");
		}

        private static string GetStatus(SlackPost slackPost)
        {
            return "This is the response to !status for " + slackPost.User_Name;
        }
	
		private static string StartBuild(SlackPost slackPost)
        {
			var buildThis = slackPost.Args.FirstOrDefault();
			if (buildThis == null)
			{
				return "Must supply the name of a build...";
			}

			return SomeBuildTools.Start(buildThis, slackPost.User_Name);
        }
    }
}
```

The full API URL you will put in Slack, will be: baseaddress/api/bot, i.e.: http://myhostname.com:9000/api/bot

The 'SlackPost' object is a mirror of what's sent by Slack for outgoing webhooks. You can see data examples here: https://api.slack.com/outgoing-webhooks

The bot comes with one pre-defined command, "!help", which lists out the 'Command' and 'Usage' fields from every item in the List of ResponseMethods. This command's trigger word can be overridden in an overload of the constructor as well.

There is also an overload of the constructor available to pass in a method that returns a boolean for whether or not a post is allowed, i.e., you want to prevent other bots from triggering this one, etc.
