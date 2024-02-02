namespace DiscordBot.Resources
{
	public class MessageReceived
	{
		public required string id { get; init; }
		public required string channel_id { get; init; }
		public required User author { get; init; }
		public required string content { get; init; }
		public required DateTime timestamp { get; init; }
		public required DateTime edited_timestamp { get; init; }

	}

	public class MessageSent
	{
		public string? content { get; set; }
		public MessageComponents.ComponentBase[]? components { get; set; }
	}
}
