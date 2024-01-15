namespace DiscordBot.Resources
{
	public class Embed
	{
		public string? title { get; set; }
		public string? type { get; set; }
		public string? description { get; set; }
		public string? url { get; set; }
		public DateTime? timestamp { get; set; }
		public int color { get; set; }
		public Footer? footer { get; set; }
		public EmbedMedia? image { get; set; }
		public EmbedMedia? thumbnail { get; set; }
		public EmbedMedia? video { get; set; }
		public Provider? provider { get; set; }
		public Author? author { get; set; }
		public Field[]? fields { get; set; }
	}

	public class Footer
	{
		public required string text { get; set; }
		public string? icon_url { get; set; }
		public string? proxy_icon_url { get; set; }
	}

	public abstract class EmbedMedia
	{
		public string? url { get; set; }
		public string? proxy_url { get; set; }
		public int height { get; set; }
		public int width { get; set; }
	}

	public class Provider
	{
		public string? name { get; set; }
		public string? url { get; set; }
	}

	public class Author
	{
		public required string name { get; set; }
		public string? url { get; set; }
		public string? icon_url { get; set; }
		public string? proxy_icon_url { get; set; }
	}

	public class Field
	{
		public required string name { get; set; }
		public required string value { get; set; }
		public bool inline { get; set; }
	}
}