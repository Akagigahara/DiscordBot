using System.Configuration;
using System.Text.Json.Serialization;

namespace DiscordBot.Resources.MessageComponents
{
	internal class SelectionComponent : ComponentBase
	{
		override required public ComponentType type { get; init; }
		override public required string custom_id { get; init; }
		public SelectionOption[]? options { get; init; }
		public Channel.ChannelType[]? channels_types { get; init; }
		[JsonInclude]
		public string? placeholder { get; init; }
		public DefaultValue[]? default_values { get; init; }
		public int min_values { get; init; }
		public int max_values { get; init; }

		public class SelectionOption
		{
			public required string label { get; init; }
			public required string value { get; init; }
			public string? description { get; init; }
			// TODO: create Emoji Object
			//public Emoji? emoji { get; init; }
			[JsonPropertyName("default")]
			public bool IsDefault { get; init; }
		}

		public class DefaultValue
		{
			public required string snowflake { get; init; }
			public required string type { get; init; }
		}
	}
}
