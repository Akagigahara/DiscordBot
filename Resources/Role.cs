namespace DiscordBot.Resources
{
	public class Role
	{
		public required string id { get; set; }
		public required string name { get; set; }
		public int color { get; set; }
		public bool hoist { get; set; }
		public string? icon { get; set; }
		public string? unicode_emoji { get; set; }
		public int position { get; set; }
		public required string permissions { get; set; }
		public bool managed { get; set; }
		public RoleTag? tags { get; set; }
		public RoleFlag flags { get; set; }

		public class RoleTag
		{
			public string? bot_id { get; set; }
			public string? integration_id { get; set; }
			public string? subscription_listing_id { get; set; }

		}

		[Flags]
		public enum RoleFlag
		{
			In_Prompt = 1 << 0
		}
	}
}