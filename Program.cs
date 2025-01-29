using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using DiscordBot;
using DiscordBot.Resources;
using DiscordBot.Resources.Commands;
using Microsoft.Data.Sqlite;

namespace DiscordBot
{
	internal class Program
	{
		private static readonly CommandBase[] commands = [];
		static void Main()
		{
			Console.WriteLine("Hello, World!");
			if (ConfigurationManager.AppSettings["UpdateCommands"] is "true")
			{
				foreach (CommandBase command in commands)
				{
					HttpRequestMessage request = new()
					{
						Content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, MediaTypeNames.Application.Json),
						RequestUri = new Uri($"applications/{ConfigurationManager.AppSettings["ApplicationId"]}/commands", UriKind.Relative),
						Method = HttpMethod.Post,
					};
					Console.WriteLine(RequestHandler.SendRequest(request));
				}
			}
			else
			{

				Task.Run(() => RequestHandler.Listen());

				DatabaseHandler databaseHandler = new();
				databaseHandler.Close();

				while(true)
				{
					Console.ReadLine();
				}
			}
		}
	}
}
