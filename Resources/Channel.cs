namespace DiscordBot.Resources
{
	public class Channel
	{
		public required string id { get; set; }
		public int type { get; set; }
		public string? guild_id { get; set; }
		public int position { get; set; }
		//public Overwrite permission_overwrites { get; set; }
		public string? name { get; set; }
		public string? topic { get; set; }
		public bool nsfw { get; set; }
		public string? last_message_id { get; set; }
		public enum ChannelType
		{
			GUILD_TEXT,
			DM,
			GUILD_VOICE,
			GROUP_DM,
			GUILD_CATEGORY,
			GUILD_ANNOUNCEMENT,
			ANNOUNCEMENT_THREAD = 10,
			PUBLIC_THREAD,
			PRIVATE_THREAD,
			GUILD_STAGE_VOICE,
			GUILD_DIRECTORY,
			GUILD_FORUM,
			GUILD_MEDIA
		}
	}
}
