using System.Configuration;
using System.Text.Json.Serialization;

namespace DiscordBot.Resources.Commands
{
	public abstract class CommandBase
	{
		/// <summary>
		/// Unique ID of Command
		/// </summary>
		public required string id { get; init; }
		/// <summary>
		/// Type of the command, defaults to <see cref="CommandType.CHAT_INPUT"/>
		/// </summary>
		[JsonInclude]
		public CommandType? type = CommandType.CHAT_INPUT;
		/// <summary>
		/// ID of the Bot. DO NOT OVERRIDE
		/// </summary>
		public static string application_id = ConfigurationManager.AppSettings["ApplicationId"]!;
		/// <summary>
		/// ID of a guild, if command is guild specific
		/// </summary>
		public string? guild_id { get; init; }
		/// <summary>
		/// Name of the command
		/// </summary>
		public required string name { get; init; }
		/// <summary>
		/// Localization dictionary of the command’s name
		/// </summary>
		[JsonInclude]
		public Dictionary<string, string>? name_localizations;
		/// <summary>
		/// Description of the command
		/// </summary>
		public string description { get; init; }
		/// <summary>
		/// Localization of the command’s descriptions
		/// </summary>
		[JsonInclude]
		public Dictionary<string, string>? description_localizations;
		/// <summary>
		/// Array of options for the command
		/// </summary>
		public CommandOption[]? options { get;	init; }
		/// <summary>
		/// Default permissions needed for the command
		/// </summary>
		public int default_member_permissions { get; init; }
		/// <summary>
		/// Whether the command can be seen/used in DMs
		/// </summary>
		public bool dm_permission { get; init; }
		/// <summary>
		/// Indicates if the command is nsfw
		/// </summary>
		public bool nsfw { get; init; }
		/// <summary>
		/// Version of the command
		/// </summary>
		public long version { get; init; }
	}

	public enum CommandType
	{
		/// <summary>
		/// Denotes the Command as a /-Command
		/// </summary>
		CHAT_INPUT = 1,
		/// <summary>
		/// Denotes the Command as a command used on a user via user context
		/// </summary>
		USER,
		/// <summary>
		/// Denotes the command as a command used on messages via message context
		/// </summary>
		MESSAGE
	}

	public class CommandOption
	{
		public required OptionType type { get; init; }
		public required string name { get; init; }
		public Dictionary<string, string>? name_localizations { get; init; }
		public required string description { get; init; }
		public Dictionary<string, string>? description_localization { get; init; }
		public bool required { get; init; }
		public OptionChoice[]? choices { get; init; }
		public CommandOption[]? options { get; init; }
		public Channel.ChannelType[]? channel_types { get; init; }
		public int min_value { get; init; }
		public int max_value { get; init; }
		public int min_length { get; init; }
		public int max_length { get; init; }
		public bool autocomplete { get; init; }

		public enum OptionType
		{
			SUB_COMMAND = 1,
			SUB_COMMAND_GROUP,
			STRING,
			INTEGER,
			BOOLEAN,
			USER,
			CHANNEL,
			ROLE,
			MENTIONABLE,
			NUMBER,
			ATTACHMENT

		}

		public abstract class OptionChoice
		{
			public required string name { get; init; }
			public Dictionary<string, string>? name_localization { get; init; }
		}

		public class OptionChoice<T>
		{
			public required T value { get; init; }
		}
		
	}

	public class CommandOption<T> : CommandOption 
	{
		new public T? min_value { get; init; }
		new public T? max_value { get; init; }
	}
}
