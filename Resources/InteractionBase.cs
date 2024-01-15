namespace DiscordBot.Resources
{
	public class InteractionBase
	{
		public required string id { get; set; }
		public required string application_id { get; set; }
		public required InteractionType type { get; set; }
		public InteractionData? data { get; set; }
		public string? guild_id { get; set; }
		public GuildMember? member { get; set; }
		public User? user { get; set; }
		public required string token { get; set; }
		public required int version { get; set; }
		public Message? message { get; set; }
		public string? app_permissions { get; set; }
		public string? locale { get; set; }
		public required Entitlement[] entitlements { get; set; }


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
			public required string id { get; set; }
			public required string name { get; set; }
			public required int type { get; set; }
			public ResolvedData? resolved { get; set; }
			public CommandOptionData[]? options { get; set; }
			public string? guild_id { get; set; }
			public string? target_id { get; set; }
		}

		public class MessageComponent
		{
			public required string custom_id { get; set; }
			public required int component_type { get; set; }
			public OptionValue[]? values { get; set; }
			public ResolvedData? resolved { get; set; }
		}

		public class ModalSubmit
		{
			public required string custom_id { get; set; }
			public required MessageComponent[] components { get; set; }
		}

		public class ResolvedData
		{
			public Dictionary<string, User>? users { get; set; }
			public Dictionary<string, GuildMember>? members { get; set; }
			public Dictionary<string, Role>? roles { get; set; }
			public Dictionary<string, Channel>? channels { get; set; }
			public Dictionary<string, Message>? messages { get; set; }
			public Dictionary<string, Attachment>? attachments { get; set; }
		}

		public class CommandOptionData
		{
			public required string name { get; set; }
			public required int type { get; set; }
			public string? value { get; set; }
			public CommandOptionData[]? options { get; set; }
			public bool? focused { get; set; }
		}

		public class MessageInteraction
		{
			public required string id { get; set; }
			public InteractionType type { get; set; }
			public required string name { get; set; }
			public User? user { get; set; }
			public GuildMember? member { get; set; }

		}

		public class OptionValue
		{
			public required string label { get; set; }
			public required string value { get; set; }
			public string? description { get; set; }
			public Emoji? emoji { get; set; }
			public bool @default { get; set; }
		}

		public class Emoji
		{
			public string? id { get; set; }
			public string? name { get; set; }
			public Role[]? roles { get; set; }
			public User? user { get; set; }
			public bool? require_colons { get; set; }
			public bool? managed { get; set; }
			public bool? animated { get; set; }
			public bool? available { get; set; }
		}
	}

}
