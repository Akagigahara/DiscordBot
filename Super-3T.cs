using Discord.Commands;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class Super_3T
    {
    }

    [Discord.Commands.Group("super3t")]
    public class Super3TCommands : InteractionModuleBase<SocketInteractionContext> 
    {
        [SlashCommand("start", "Starts a game of Super Tic-Tac-Toe")]
        public async Task StartS3T() 
        {

        }
    }
}
