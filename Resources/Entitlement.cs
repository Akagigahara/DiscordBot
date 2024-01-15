namespace DiscordBot.Resources
{
	public class Entitlement
	{
		public required string id {  get; set; }
		public required string sku_id { get; set; }
		public required string application_id { get; set; }
		public string? user_id { get; set; }
		public EntitlementType type { get; set; }
		public required bool deleted { get; set; }
		public DateTime? starts_at { get; set; }
		public DateTime? ends_at { get; set; }
		public string? guild_id { get; set; }
	}

	public enum EntitlementType
	{
		ApplicationSubscription = 8
	}
}