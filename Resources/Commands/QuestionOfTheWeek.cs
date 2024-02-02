using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Resources.Commands
{
	internal class QuestionOfTheWeek : CommandBase
	{
		public QuestionOfTheWeek() 
		{
			name = "qotw";
			description = "Configure the Question of the Week function";
			default_member_permissions = 0;
			type = CommandType.MESSAGE;
			options = 
				[
					new()
					{
						name = "qotwchannel",
						description = "The Channel to which the question is sent",
						type = CommandOption.OptionType.CHANNEL,
						required = true,
					}
				];
		}

	}
}
