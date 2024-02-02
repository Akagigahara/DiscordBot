using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Resources
{
	public class GuildMember
	{
		public User? user { get; set; }
		public string? nick { get; set; }
		public string? avatar { get; set; }
		public required string[] roles { get; set; }
		public required DateTime joined_at { get; set; }
		public DateTime? premium_since { get; set; }
		public required bool deaf { get; set; }
		public required bool mute { get; set; }
		public required int flags { get; set; }
		public bool pending { get; set; }
		public string? permissions { get; set; }
		public DateTime? communication_disabled_until { get; set; }
	}
}
