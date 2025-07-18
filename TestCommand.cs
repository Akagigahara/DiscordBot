using Discord;
using Discord.Interactions;

namespace DiscordBot
{
	public class TestCommand : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("echo", "Repeats the input you used")]
		public async Task Echo([Summary(description: "This is the parameter for the text input")]string input)
		{
			await RespondAsync(input, ephemeral: true);
			ComponentBuilder builder = new ComponentBuilder().WithButton("Click me!", "btn-1");
			await FollowupWithFileAsync("mietzi.jpg", components: builder.Build());
        }
	}
}
