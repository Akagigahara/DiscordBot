namespace DiscordBot.Resources
{
	public class User
	{
		public required string id { get; set; }
		public required string username { get; set; }
		public required string discriminator { get; set; }
		public required string global_name { get; set; }
		public string? avatar { get; set; }
		public bool bot { get; set; }

	}
}
