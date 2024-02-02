using DiscordBot.Resources.MessageComponents;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace DiscordBot.Resources.Commands
{
	internal class Create : CommandBase
	{
		public Create()
		{
			name = "create";
			description = "Command group for creating components";
			default_member_permissions = 0;
			type = CommandType.CHAT_INPUT;
			options =
				[
					new CommandOption()
					{
						name = "role_select",
						description = "Create a role selection modal",
						type = CommandOption.OptionType.SUB_COMMAND,
						options =
						[
							new CommandOption()
							{
								name = "channel",
								description = "Channel in which the modal is supposed to be sent to",
								type = CommandOption.OptionType.CHANNEL
							},
							/* new CommandOption()
							{
								name = "",
								description = "",
								type = Command,
							}*/
						]
					},
				];

		}

		public static void HandleCommand(InteractionBase.InteractionData data)
		{
			string ChannelID = (from option in (from option in data.options where option.name == "role_select" select option).First()!.options where option.name == "channel" select option).First().value!;
			RequestHandler.SendRequest(new(HttpMethod.Post, $"channels/{ChannelID}/messages")
			{
				Content = new StringContent(JsonSerializer.Serialize(new MessageSent()
				{
					content = "Please choose a role",
					components =
					[
						new ActionRow() 
						{
							type = ComponentBase.ComponentType.Action_Row,
							component = new SelectionComponent[]
							{
								new()
								{
									type = ComponentBase.ComponentType.Role_Select,
									custom_id = $"role_select_{ChannelID}"
								}
							}
						}
					]
				}), Encoding.UTF8, MediaTypeNames.Application.Json),
			});
		}
	}
}
