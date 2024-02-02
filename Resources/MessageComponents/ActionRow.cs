using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Resources.MessageComponents
{
	internal class ActionRow : ComponentBase
	{
		override public ComponentType type { get; init; } = ComponentType.Action_Row;
		public required ComponentBase[] component { get; init; }
	}
}
