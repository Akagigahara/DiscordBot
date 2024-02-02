using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBot.Resources.MessageComponents
{
	internal class InputComponent : ComponentBase
	{
		override public ComponentType type { get; init; } = ComponentType.Text_Input;
		public InputStyle style { get; init; }
		public required string label { get; init; }
		public bool required { get; init; }
		public string? value { get; init; }
		public string? placeholder { get; init; }

		public enum InputStyle
		{
			Short = 1,
			Paragraph
		}
	}
}
