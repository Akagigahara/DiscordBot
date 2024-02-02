using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBot.Resources.MessageComponents
{
	internal class ActionRow : ComponentBase
	{
		override public ComponentType type { get; init; } = ComponentType.Action_Row;
		[JsonIgnore]
		public override string custom_id { get => base.custom_id; init => base.custom_id = value; }
	}
}
