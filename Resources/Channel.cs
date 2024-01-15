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
	}
}
