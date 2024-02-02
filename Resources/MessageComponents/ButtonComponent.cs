using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Resources.MessageComponents
{
	internal class ButtonComponent : ComponentBase
	{
		override public required ComponentType type { get; init; } = ComponentType.Button;
		public required ButtonStyle style { get; set; }
		public string? label { get; set; }
		//public Emoji? emoji { get; set; }
		public string? url { get; set; }

		public enum ButtonStyle
		{
			Primary = 1,
			Secondary,
			Success,
			Danger,
			Link
		}
	}
}
