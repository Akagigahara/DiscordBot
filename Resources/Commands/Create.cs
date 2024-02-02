using DiscordBot.Resources.MessageComponents;
using Org.BouncyCastle.Utilities;
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
								required = true,
							},
							 new CommandOption()
							{
								name = "number_of_menus",
								description = "The amount of menu’s you need for your role selection. 1 Menu fits 25 Roles.",
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
			int NumberOfMenus = int.Parse((from option in (from option in Interaction.data!.options where option.name == "role_select" select option).First()!.options where option.name == "number_of_menus" select option).First().value!);
			string roleRequest = RequestHandler.SendRequest(new(HttpMethod.Get, $"guilds/{Interaction.guild_id}/roles"));
			Role[] Roles = JsonSerializer.Deserialize<Role[]>(roleRequest)!.TakeWhile(x => x.permissions == "0").ToArray();

			ComponentBase[] components = new ComponentBase[NumberOfMenus];

			foreach (int menu in Enumerable.Range(0, NumberOfMenus))
			{
				components[menu] = new ActionRow()
				{
					type = ComponentBase.ComponentType.Action_Row,
					components = [new SelectionComponent()
					{
						type = ComponentBase.ComponentType.String_Select,
						custom_id = $"role_select_G{Interaction.guild_id}_Ch{ChannelID}_Me{menu}",
						placeholder = menu > 0 ? "Choose your Role" : $"Roles missing from {menu + 1}",
						min_values = 0,
						max_values = 25
					}]
				};
			}

			MessageSent messagetoBeSent = new()
			{
				content = "Please choose a role",
				components = components
			};

			

			RequestHandler.SendRequest(new(HttpMethod.Post, $"channels/{ChannelID}/messages")
			{
				Content = new StringContent(JsonSerializer.Serialize(messagetoBeSent), Encoding.UTF8, MediaTypeNames.Application.Json),
			});
		}
	}
}