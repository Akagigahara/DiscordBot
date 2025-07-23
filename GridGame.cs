using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiscordBot
{
	public class GridGame : IGame
	{
		/// <summary>
		/// 2-Dimensional Array representing the game grid. Each square/space of the game grid is represented by a <see cref="Grid"/> object. 
		/// </summary>
		private readonly Grid[,] grid =
		{
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			},
			{
				new(),new(),new(),new(),new(),new(),new(),new(),new(),new(),
			}
		};
		/// <summary>
		/// A list containing all correct <see cref="Grid"/>s.
		/// </summary>
		private readonly List<Grid> correctGrids = [];
		/// <summary>
		/// A list containing the current files that are used in the game.
		/// </summary>
		private readonly string[] currentSet;
		/// <summary>
		/// Field containing the message ID of the message containing a visual representation of the game grid.
		/// </summary>
		private ulong gridUI;
		private readonly List<ulong> playersOnCD = [];

		/// <summary>
		/// Creates a <see langword="string"/> representation of the game grid.
		/// </summary>
		/// <returns>The stringified version of the game grid.</returns>
		public string GridToString()
		{
			string result = $"";
			byte index = 1;
			result += "+--+--+--+--+--+--+--+--+--+--+\n";
			foreach (Grid space in grid)
			{
				result += "|";
				if (space.HasBeenGuessed)
				{
					if (space.IsTarget)
					{
						result += " O";
					}
					else
					{
						result += " X";
					}
				}
				else
				{
					result += index < 10 ? $"0{index}" : index;
				}
				if (index % 10 == 0)
				{
					result += "|";
					result += "\n+--+--+--+--+--+--+--+--+--+--+\n";
				}
				index++;
			}

			return Format.Code(result);
		}

		/// <summary>
		/// Initializes a new grid game.
		/// </summary>
		/// <exception cref="FileNotFoundException">Exeption thrown when the directory containing the images, /pictures/, does not exist</exception>
		public GridGame()
		{
			Random rand = new();
			if (!Directory.Exists("./pictures/"))
			{
				throw new FileNotFoundException("Directory not found");
			}
			string[] collectionOfPictures = Directory.GetDirectories("./pictures/");
			currentSet = Directory.GetFiles(collectionOfPictures[rand.Next(0, collectionOfPictures.Length - 1)]);
			foreach (string file in currentSet)
			{
				if (file.Contains("picture.metadata"))
				{
					string[] metadata = File.ReadAllLines(file);
					string[] correctSpaces = metadata[0].Split('=')[1].Split(',');
					foreach (string space in correctSpaces)
					{
						byte index = byte.Parse(space);
						GetGrid(index).IsTarget = true;
						correctGrids.Add(GetGrid(index));

					}
				}
			}
		}

		/// <summary>
		/// Class representing a single grid space in the game.
		/// </summary>
		private class Grid
		{
			/// <summary>
			/// <see cref="bool"/> representing whether the space has been guessed or not.
			/// </summary>
			internal bool HasBeenGuessed = false;
			/// <summary>
			/// <see cref="bool"/> representing whether the space is one of the correct spaces.
			/// </summary>
			internal bool IsTarget = false;
			/// <summary>
			/// Contains the ID of the user that guessed this space correctly.
			/// </summary>
			internal ulong guessedBy;
		}
        /// <summary>
        /// Class containing all slash commands for the grid game.
        /// </summary>
        [NsfwCommand(true)]
        [CommandContextType(InteractionContextType.Guild)]
        [Discord.Interactions.Group("gridgame", "All commands related to grid game")]
		public class GridGameCommands : InteractionModuleBase<SocketInteractionContext>
		{
			/// <summary>
			/// This function starts a new grid finder game.
			/// </summary>
			/// <param name="targetChannel">The channel the game is supposed to be played in.</param>
			/// <returns>Task representing the action</returns>
			//[NsfwCommand(true)]
			//[CommandContextType(InteractionContextType.Guild)]
			//[DefaultMemberPermissions(GuildPermission.ManageChannels)]
			[SlashCommand("start", "Starts a new grid finder game")]
			public async Task StartNewGame([ChannelTypes(ChannelType.Text)] IChannel? targetChannel = null)
			{
				if (!Program.runningGames.ContainsKey(Context.Guild.Id))
				{
					GridGame newGame = new();
					Program.runningGames.Add(Context.Guild.Id, newGame);
					if (targetChannel == null)
					{
						ComponentBuilder builder = new ComponentBuilder().WithButton("Submit answer", $"GridGameAnswerBtn-{Context.Guild.Id}");
						await RespondWithFileAsync((Program.runningGames[Context.Guild.Id] as GridGame).currentSet[2], components: builder.Build());
						newGame.gridUI = FollowupAsync(newGame.GridToString()).Result.Id;
					}
					else
					{
						await RespondAsync($"Starting a new game in <#{targetChannel.Id}>", ephemeral: true);
                        ComponentBuilder builder = new ComponentBuilder().WithButton("Submit answer", $"GridGameAnswerBtn-{Context.Guild.Id}");
                        await ((IMessageChannel)targetChannel).SendFileAsync((Program.runningGames[Context.Guild.Id] as GridGame).currentSet[2], components: builder.Build());
                        newGame.gridUI = FollowupAsync(newGame.GridToString()).Result.Id;
                    }
				}
				else
				{
					await RespondAsync("There is currently a running game.", ephemeral: true);
				}
			}

			[SlashCommand("end", "Ends a currently running game")]
			public async Task EndGame()
			{
				if (Program.runningGames.ContainsKey(Context.Guild.Id))
				{
					_ = RespondAsync("Ending game.");
					Program.runningGames.Remove(Context.Guild.Id);
				}
				else
				{
					_ = RespondAsync("No game is currently running", ephemeral:true);
				}
			}

			[SlashCommand("restart", "Ends the currently running game and starts a new one")]
			public async Task RestartGame([ChannelTypes(ChannelType.Text)] IChannel? targetChannel = null)
			{
                if (Program.runningGames.ContainsKey(Context.Guild.Id))
                {
                    _= RespondAsync("Ending game.");
                    Program.runningGames.Remove(Context.Guild.Id);
                    GridGame newGame = new();
                    Program.runningGames.Add(Context.Guild.Id, newGame);
                    if (targetChannel == null)
                    {
                        ComponentBuilder builder = new ComponentBuilder().WithButton("Submit answer", $"GridGameAnswerBtn-{Context.Guild.Id}");
                        await FollowupWithFileAsync((Program.runningGames[Context.Guild.Id] as GridGame).currentSet[2], components: builder.Build());
                        newGame.gridUI = FollowupAsync(newGame.GridToString()).Result.Id;
                    }
                    else
                    {
                        await FollowupAsync($"Starting a new game in <#{targetChannel.Id}>", ephemeral: true);
                        ComponentBuilder builder = new ComponentBuilder().WithButton("Submit answer", $"GridGameAnswerBtn-{Context.Guild.Id}");
                        await ((IMessageChannel)targetChannel).SendFileAsync((Program.runningGames[Context.Guild.Id] as GridGame).currentSet[2], components: builder.Build());
                        newGame.gridUI = FollowupAsync(newGame.GridToString()).Result.Id;
                    }
                }
                else
                {
                    RespondAsync("No game is currently running", ephemeral: true);
                }
            }

		}

		/// <summary>
		/// Checks all <see cref="correctGrids"/> and their <see cref="Grid.HasBeenGuessed"/> properties.
		/// </summary>
		/// <returns>True when all <see cref="correctGrids"/> have been guessed</returns>
		private bool AreAllGuessed()
		{
			bool AllGuessed = false;
			if (correctGrids[0].HasBeenGuessed) AllGuessed = true;

			foreach (Grid grid in correctGrids)
			{
				AllGuessed &= grid.HasBeenGuessed;
			}
			return AllGuessed;
		}

		/// <summary>
		/// Function that returns the grid at the specified position.
		/// </summary>
		/// <param name="position">The desired <see cref="grid"/>.A number between 1-100</param>
		/// <returns>The <see cref="Grid"/> at the specified position</returns>
		private Grid GetGrid(int position)
		{
			int x = 0;
			int y = 0;

			if (position % 10 == 0)
			{
				x = position / 10 - 1;
				y = 9;
			}
			else
			{
				x = (int)Math.Floor(position / 10.0);
				y = position % 10 - 1;
			}
			return grid[x, y];
		}

		/// <summary>
		/// Function handling the answer submitted by the user in the modal.
		/// </summary>
		/// <param name="modal">Modal that created the interaction</param>
		public void HandleAnswer(SocketModal modal)
		{
			if (playersOnCD.Contains(modal.User.Id))
			{
				modal.RespondAsync("You are currently on cooldown", ephemeral: true);
				return;
			}

			byte answer = 0;
			byte answerIndex = 1;

			modal.DeferAsync();
			foreach (SocketMessageComponentData data in modal.Data.Components)
			{
				try
				{
					answer = byte.Parse(data.Value);
					if (answer < 1 && answer > 100)
					{
						throw new FormatException();
					}
				}
				catch (FormatException ex)
				{
					modal.RespondAsync("Incorrect number, be sure to only use digits and for the number to be between 1-100", ephemeral: true);
				}
				Grid selectedGrid = GetGrid(answer);

				if (!selectedGrid.HasBeenGuessed)
				{
					selectedGrid.HasBeenGuessed = true;
					selectedGrid.guessedBy = modal.User.Id;

                    if (selectedGrid.IsTarget)
					{
                        modal.FollowupAsync($"Your {answerIndex}. guess, {answer}, was correct!",ephemeral:true);
					}
                    else
					{
                        modal.FollowupAsync($"Your {answerIndex}. guess, {answer}, was incorrect, try again!", ephemeral: true);
					}
					answerIndex++;
				}
				else
				{
					modal.FollowupAsync("Space has already been guessed. Please select another one", ephemeral: true);
				}

				if (AreAllGuessed())
				{
					string FoundBy = "";
					byte index = 1;
					foreach (Grid correctSpace in correctGrids)
					{
						FoundBy += $"Space {index++} was found by <@{correctSpace.guessedBy}>\n";
					}
					modal.FollowupAsync("All spaces have been found!");
					modal.FollowupWithFilesAsync([new FileAttachment(currentSet[1]), new FileAttachment(currentSet[0])], $"Here is the solution and the reward!\n" + FoundBy);
					Program.runningGames.Remove((ulong)modal.GuildId);
				}
			}
            modal.Channel.ModifyMessageAsync(gridUI, (properties) =>
            {
                properties.Content = GridToString();
            });
			playersOnCD.Add(modal.User.Id);
			RemovePlayerFromCD(modal.User.Id);
        }
		public void HandleButton(SocketMessageComponent button)
		{
			byte num = 1;
			ModalBuilder modalBuilder = new ModalBuilder().WithTitle("Submit your number").WithCustomId($"GridGameAnswerModal-{button.GuildId}");
			foreach (GridGame.Grid correctGrid in this.correctGrids)
			{
				modalBuilder.AddTextInput(new TextInputBuilder()
					.WithLabel($"Enter your {num}. number")
					.WithRequired(true)
					.WithPlaceholder("0-100")
					.WithCustomId($"numericAnswer{num++}")
					.WithMinLength(1)
					.WithMaxLength(3));
			}
			button.RespondWithModalAsync(modalBuilder.Build());
		}
        
		private async void RemovePlayerFromCD(ulong playerId)
        {
            await Task.Delay(TimeSpan.FromMinutes(1.0));
            this.playersOnCD.Remove(playerId);
        }
    }
}
