using Discord.Interactions;

namespace DiscordBot
{
    public class TestCommand : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("echo", "Repeats the input you used")]
        public async Task Echo([Summary(description: "This is the parameter for the text input")]String input)
        {
            await RespondAsync(input);
        }
    }
}
