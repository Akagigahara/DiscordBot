using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.VisualBasic;
using System.Reflection;
using static DiscordBot.GridGame;

namespace DiscordBot
{
	public class Program
	{
		private static DiscordSocketClient _client = new();
		private static InteractionService _interactionService = new(_client.Rest);
		private static readonly Dictionary<string, string> appSettings = ReadAppSettings();
		internal static Dictionary<ulong, IGame> runningUniqueGames = [];
		internal static Dictionary<ulong, IGame[]> runningGenericGames = [];

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
			_client.ButtonExecuted += ButtonInteraction;
			_client.ModalSubmitted += ModalHandler;

			_interactionService.Log += Log;
			await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), null);

			string token = appSettings["token"];
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			 await Task.Delay(-1);
		}

		public static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private static async Task ModalHandler(SocketModal modal)
		{
			Log(new(LogSeverity.Info, "Modal handler", $"Modal interaction with ID {modal.Data.CustomId}"));
			string[] parsedCustomId = modal.Data.CustomId.Split('-');
			switch(parsedCustomId[0])
			{
				case "GridGameAnswerModal":
					runningUniqueGames[(ulong)modal.GuildId].HandleAnswer(modal);
					break;
			}
		}

		private static async Task ButtonInteraction(SocketMessageComponent component)
		{
			string[] parsedCustomId = component.Data.CustomId.Split('-');
			switch (parsedCustomId[0])
			{
				case "GridGameAnswerBtn":
					//await component.RespondAsync("You clicked me!");
					runningUniqueGames[(ulong)component.GuildId!].HandleButton(component);
                    break;
				case "GridGameNewGameBtn":
					GridGame.StartNewGame(component);
					break;
				case "GridGameSkipBtn":
					(runningUniqueGames[(ulong)component.GuildId!] as GridGame)!.SkipCurrentGame(component);
					break;

            }
		}

		private static Dictionary<string, string> ReadAppSettings()
		{
			Dictionary<string, string> settings = [];
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
