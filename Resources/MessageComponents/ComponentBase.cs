using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBot.Resources.MessageComponents
{
	[JsonDerivedType(typeof(ActionRow))]
	[JsonDerivedType(typeof(ButtonComponent))]
	[JsonDerivedType(typeof(InputComponent))]
	[JsonDerivedType(typeof(SelectionComponent))]
	public abstract class ComponentBase
	{
		public abstract ComponentType type { get; init; }
		public virtual string custom_id { get; init; } = "";
		public virtual bool disabled { get; init; }
		public virtual ComponentBase[] components { get; init; } = [];

		public enum ComponentType
		{
			Action_Row = 1,
			Button,
			String_Select,
			Text_Input,
			User_Select,
			Role_Select,
			Mentionable_Select,
			Channel_Select,
		}
	}
}
