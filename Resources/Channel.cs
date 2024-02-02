namespace DiscordBot.Resources
{
	public class Channel
	{
		public required string id { get; set; }
		public int type { get; set; }
		public string? guild_id { get; set; }
		public int position { get; set; }
		public Overwrite? permission_overwrites { get; set; }
		public string? name { get; set; }
		public string? topic { get; set; }
		public bool nsfw { get; set; }
		public string? last_message_id { get; set; }
		public int? bitrate { get; set; }
		public int? user_limit { get; set; }
		public int? rate_limit_per_user { get; set; }
		public User[]? recipients { get; set; }
		public string? icon { get; set; }
		public string? owner_id { get; set; }
		public string? application_id { get; set; }
		public bool managed { get; set; }
		public string? parent_id { get; set; }
		public DateTime? last_pin_timestamp { get; set; }
		public string? rtc_region { get; set; }
		public int video_quality_mode { get; set; }
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

		public class Overwrite
		{
			public required string id { get; set; }
			public int type { get; set; }
			public required string allow { get; set; }
			public required string deny { get; set; }
		}
	}
}
