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
								type = CommandOption.OptionType.CHANNEL,
								required = true
							},
							 new CommandOption()
							{
								name = "number_of_menus",
								description = "The amount of menu’s you want for your role selection",
								type = CommandOption.OptionType.INTEGER,
								min_value = 1,
								max_value = 5
							}
						]
					},
				];

		}

		public static void HandleCommand(InteractionBase Interaction)
		{
			string ChannelID = (from option in (from option in Interaction.data!.options where option.name == "role_select" select option).First()!.options where option.name == "channel" select option).First().value!;
			string roleRequest = RequestHandler.SendRequest(new(HttpMethod.Get, $"guilds/{Interaction.guild_id}/roles"));
			Role[] Roles = JsonSerializer.Deserialize<Role[]>(roleRequest)!;

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
							components = [new SelectionComponent()
							{
									type = ComponentBase.ComponentType.Role_Select,
									custom_id = $"role_select_{ChannelID}",
									min_values = 1,
									max_values = 25,
									placeholder = "Select your first Roles"
							}]
						},
						new ActionRow()
						{
							type = ComponentBase.ComponentType.Action_Row,
							components=[new SelectionComponent()
							{
								type = ComponentBase.ComponentType.Role_Select,
								custom_id = $"role_select_{ChannelID}_2",
								min_values = 1,
								max_values = 10,
								placeholder = "How about some more?"
							}]
						}
					]
				}), Encoding.UTF8, MediaTypeNames.Application.Json),
			});
		}
	}
}
