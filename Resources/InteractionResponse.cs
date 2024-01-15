using System.ComponentModel;
using System.Xml.Serialization;

namespace DiscordBot.Resources
{
	internal class InteractionResponse
	{
		public InteractionCallbackType type { get; set; }
		public InteractionCallbackBase? data { get; set; }

		public enum InteractionCallbackType
		{
			PONG = 1,
			CHANNEL_MESSAGE_WITH_SOURCE = 4,
			DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE,
			DEFERRED_UPDATE_MESSAGE,
			UPDATE_MESSAGE,
			APPLICATION_COMMAND_AUTOCOMPLETE_RESULT,
			MODAL,
			PREMIUM_REQUIRED
		}
		public abstract class InteractionCallbackBase;

		public class CallbackMessage : InteractionCallbackBase
		{
			public bool? tts { get; set; }
			public string? content { get; set; }
			public Embed[]? embeds { get; set; }
			//public AllowedMentions? allowed_mentions { get; set; }
			public int? flags { get; set; }
			public Component[]? components { get; set; }
			public Attachment[]? attachments { get; set; }
		}

		public class CallbackAutocomplete : InteractionCallbackBase
		{
			//public Choice[]? choices { get; set; }
		}

		public class CallbackModal : InteractionCallbackBase
		{
			public required string custom_id { get; set; }
			public required string title { get; set; }
			public Component[] components { get; set; }
		}

	}
}