namespace DiscordBot.Resources
{
	public class Attachment
	{
		public required string id { get; set; }
		public required string filename { get; set; }
		public string? description { get; set; }
		public string? content_type { get; set; }
		public int size { get; set; }
		public required string url { get; set; }
		public required string proxy_url { get; set; }
		public int? height { get; set; }
		public int? width { get; set; }
		public bool? ephemeral { get; set; }
		public float? duration_secs { get; set; }
		public string? waveform { get; set; }
		public int? flags { get; set; }
	}
}