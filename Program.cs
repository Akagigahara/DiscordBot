using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.VisualBasic;
using System.Reflection;

namespace DiscordBot
{
	public class Program
	{
		private static DiscordSocketClient _client = new();
		private static InteractionService _interactionService = new(_client.Rest);
		private static Dictionary<string, string> appSettings = ReadAppSettings();

		public static async Task Main()
		{
			_client.Log += Log;
			_client.InteractionCreated += async (interaction) => 
			{
                SocketInteractionContext ctx = new(_client, interaction);
                await _interactionService.ExecuteCommandAsync(ctx, null);
            };
			_client.Ready += async () => 
			{
				await _interactionService.RegisterCommandsGloballyAsync();
			};

			_interactionService.Log += Log;
			await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null);

			string token = appSettings["token"];
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			await Task.Delay(-1);
		}

		private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private static Dictionary<string, string> ReadAppSettings()
		{
			Dictionary<string, string> settings = new();
			string[] unparsedSettings = File.ReadAllLines("./settings.ini");
			foreach (string setting in unparsedSettings)
			{
				string[] parsedSetting = setting.Split('=');
				settings[parsedSetting[0].Trim()] = parsedSetting[1].Trim();
			}

			return settings;
		}
	}
}
