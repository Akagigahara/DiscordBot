using Discord.WebSocket;

namespace DiscordBot.Bases
{
    internal interface IGame
    {
        void HandleAnswer(SocketModal modal);
        void HandleButton(SocketMessageComponent button);
    }
}