using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Resources
{
	internal class Permissions
	{
		[Flags]
		public enum PermissionFlags : ulong
		{
			Create_Instant_Invite = 1ul << 0,
			Kick_Members = 1ul << 1,
			Ban_Members = 1ul<< 2,
			Administrator = 1ul<< 3,
			Manage_Channels = 1ul<< 4,
			Manage_Guild = 1ul<< 5,
			Add_Reactions = 1ul<< 6,
			View_Audit_Log = 1ul<< 7,
			Priority_Speaker = 1ul<< 8,
			Stream = 1ul<< 9,
			View_Channel = 1ul<< 10,
			Send_Messages = 1ul<< 11,
			Send_TTS_Messages = 1ul<< 12,
			Manage_Messages = 1ul<< 13,
			Embed_Links = 1ul<< 14,
			Attach_Files = 1ul<< 15,
			Read_Message_History = 1ul<< 16,
			Mention_Everyone = 1ul<< 17,
			Use_External_Emojis = 1ul<< 18,
			View_Guild_Insights = 1ul<< 19,
			Connect = 1ul<< 20,
			Speak = 1ul<< 21,
			Mute_Members = 1ul<< 22,
			Deafen_Members = 1ul<< 23,
			Move_Members = 1ul<< 24,
			Use_VAD = 1ul<< 25,
			Change_Nickname = 1ul<< 26,
			Manage_Nicknames = 1ul<< 27,
			Manage_Roles = 1ul<< 28,
			Manage_Webhooks = 1ul<< 29,
			Manage_Guild_Expressions = 1ul<< 30,
			Use_Application_Commands = 1ul << 31,
			Request_To_Speak = 1ul << 32,
			Manage_Events = 1ul << 33,
			Manage_Threads = 1ul << 34,
			Create_Public_Threads = 1ul << 35,
			Create_Private_Threads = 1ul<< 36,
			Use_External_Stickers = 1ul<< 37,
			Send_Messages_In_Threads = 1ul<< 38,
			Use_Embedded_Activities = 1ul<< 39,
			Moderate_Members = 1ul<< 40,
			View_Creator_Monetizations_Analytics = 1ul<< 41,
			Use_SoundBoard = 1ul<< 42,
			Create_Guild_Expressions = 1ul<< 43,
			Create_Events = 1ul<< 44,
			Use_External_Sounds = 1ul<< 45,
			Send_Voice_Messages = 1ul<< 46
		}
	}
}
