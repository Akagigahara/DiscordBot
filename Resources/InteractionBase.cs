namespace DiscordBot.Resources
{
	public class InteractionBase
	{
		public required string id { get; init; }
		public required string application_id { get; init; }
		public required InteractionType type { get; init; }
		public InteractionData? data { get; init; }
		public string? guild_id { get; init; }
		public Channel? channel { get; init; }
		public string? channel_id { get; init; }
		public GuildMember? member { get; init; }
		public User? user { get; init; }
		public required string token { get; init; }
		public required int version { get; init; }
		public MessageReceived? message { get; init; }
		public string? app_permissions { get; init; }
		public string? locale { get; init; }
		public string? guild_locale { get; init; }
		public required Entitlement[] entitlements { get; init; }


		public enum InteractionType
		{
			PING = 1,
			APPLICATION_COMMAND,
			MESSAGE_COMPONENT,
			APPLICATION_COMMAND_AUTOCOMPLETE,
			MODAL_SUBMIT,
		}

		public class InteractionData
		{
			public required string id { get; init; }
			public required string name { get; init; }
			public required int type { get; init; }
			public ResolvedData? resolved { get; init; }
			public CommandOptionData[]? options { get; init; }
			public string? guild_id { get; init; }
			public string? target_id { get; init; }
		}

		public class MessageComponent
		{
			public required string custom_id { get; init; }
			public required int component_type { get; init; }
			public OptionValue[]? values { get; init; }
			public ResolvedData? resolved { get; init; }
		}

		public class ModalSubmit
		{
			public required string custom_id { get; init; }
			public required MessageComponent[] components { get; init; }
		}

		public class ResolvedData
		{
			public Dictionary<string, User>? users { get; init; }
			public Dictionary<string, GuildMember>? members { get; init; }
			public Dictionary<string, Role>? roles { get; init; }
			public Dictionary<string, Channel>? channels { get; init; }
			public Dictionary<string, MessageReceived>? messages { get; init; }
			public Dictionary<string, Attachment>? attachments { get; init; }
		}

		public class CommandOptionData
		{
			public required string name { get; init; }
			public required int type { get; init; }
			public string? value { get; init; }
			public CommandOptionData[]? options { get; init; }
			public bool? focused { get; init; }
		}

		public class MessageInteraction
		{
			public required string id { get; init; }
			public InteractionType type { get; init; }
			public required string name { get; init; }
			public User? user { get; init; }
			public GuildMember? member { get; init; }

		}

		public class OptionValue
		{
			public required string label { get; init; }
			public required string value { get; init; }
			public string? description { get; init; }
			public Emoji? emoji { get; init; }
			public bool @default { get; init; }
		}

		public class Emoji
		{
			public string? id { get; init; }
			public string? name { get; init; }
			public Role[]? roles { get; init; }
			public User? user { get; init; }
			public bool? require_colons { get; init; }
			public bool? managed { get; init; }
			public bool? animated { get; init; }
			public bool? available { get; init; }
		}
	}

}
