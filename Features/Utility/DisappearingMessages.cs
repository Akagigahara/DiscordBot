using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Channels;

namespace DiscordBot.Features.Utility
{
    internal class DisappearingMessages
    {
        private TimeSpan timeToDelete;
        public DisappearingMessages(ref Dictionary<ulong, DisappearingMessages> DisappearingChannels, ulong channelId, int timeToDelete)
        {
            if (DisappearingChannels.ContainsKey(channelId))
            {
                throw new Exception("Channel is already registered for disappearing messages.");
            }
            this.timeToDelete = TimeSpan.FromSeconds(timeToDelete);
            DisappearingChannels[channelId] = this;
            
        }

        public async void DeleteMessage(SocketMessage message)
        {
            await Task.Delay(timeToDelete).ContinueWith(async _ =>
            {
                await message.DeleteAsync();
            });
        }

        public static void UpdatePersistentStorage()
        {
            List<string> channelList = [];
            foreach (KeyValuePair<ulong, DisappearingMessages> pair in Program.disappearingMessages)
            {
                channelList.Add($"{pair.Key}={pair.Value.timeToDelete.TotalSeconds} \n");
            }
            File.WriteAllLines("./disappearing_channels.ini", channelList);
        }

        [CommandContextType(InteractionContextType.Guild)]
        [Group("disappearing-messages", "Commands for managing disappearing messages in channels.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class DisappearingMessagesCommands: InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("register","Register a new channel to have messages disappear after a set amount of time.")]
            public async Task RegisterNewChannel([ChannelTypes(ChannelType.Text)]IChannel channel, [MinValue(0)] int timeToDelete = 5)
            {
                try
                {
                    _ = new DisappearingMessages(ref Program.disappearingMessages, channel.Id, timeToDelete);
                    await RespondAsync($"Registered channel <#{channel.Id}> to have messages disappear after {timeToDelete} seconds.", ephemeral: true);
                    File.AppendAllText("./disappearing_channels.ini", $"{channel.Id}={timeToDelete} \n");
                }
                catch(Exception ex)
                {
                    await RespondAsync($"Error registering channel: {ex.Message}", ephemeral: true);
                    return;
                }
            }
            [SlashCommand("unregister", "Unregister a channel from having disappearing messages.")]
            public async Task UnregisterChannel([ChannelTypes(ChannelType.Text)] IChannel channel)
            {
                if (Program.disappearingMessages.Remove(channel.Id))
                {
                    await RespondAsync($"Unregistered channel <#{channel.Id}> from having disappearing messages.", ephemeral: true);
                    UpdatePersistentStorage();
                }
                else
                {
                    await RespondAsync($"Channel <#{channel.Id}> is not registered for disappearing messages.", ephemeral: true);
                }
            }
            [SlashCommand("settings", "Change the settings channel's disappearing messages.")]
            public async Task ChangeSettings([ChannelTypes(ChannelType.Text), Summary(description:"The channel to changed the settings of.")] IChannel channel, [MinValue(0), Summary(description:"The time it takes for a message to disappear, in seconds")] int timeToDelete)
            {
                if (Program.disappearingMessages.TryGetValue(channel.Id, out DisappearingMessages? dm))
                {
                    dm.timeToDelete = TimeSpan.FromSeconds(timeToDelete);
                    await RespondAsync($"Updated disappearing messages time to {Format.Code(timeToDelete.ToString())} seconds for channel <#{channel.Id}>.", ephemeral: true);
                    UpdatePersistentStorage();
                }
                else
                {
                    await RespondAsync($"Channel <#{channel.Id}> is not registered for disappearing messages.", ephemeral: true);
                }
            }
        }
    }
}
