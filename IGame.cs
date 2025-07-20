using Discord.WebSocket;

namespace DiscordBot
{
    internal interface IGame
    {
        void HandleAnswer(SocketModal modal);
    }
}