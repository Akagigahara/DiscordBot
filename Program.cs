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

			string token = "OTIxMzYzNTgyMTkxNTY2ODc4.GyNygq.HxRtRoi7-S8DrQw_Q28cB8qLG5W0yyf-fgnNXA";
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			await Task.Delay(-1);
		}

		private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
