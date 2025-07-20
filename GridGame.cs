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
		private readonly List<Grid> correctGrids = [];
		private readonly string[] currentSet;
		private ulong gridUI;

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
				if(index%10 == 0)
				{
					result += "|";
					result += "\n+--+--+--+--+--+--+--+--+--+--+\n";
				}
				index++;
			}

            return Format.Code(result);
        }

		public GridGame()
		{
			Random rand = new();
			if (!Directory.Exists("./pictures/"))
			{
				throw new Exception("Directory not found");
			}
			string[] collectionOfPictures = Directory.GetDirectories("./pictures/");
			currentSet = Directory.GetFiles(collectionOfPictures[rand.Next(0,collectionOfPictures.Length-1)]);
			foreach(string file in currentSet)
			{
				if (file.Contains("picture.metadata"))
				{
					string[] metadata = File.ReadAllLines(file);
					string[] correctSpaces = metadata[0].Split('=')[1].Split(',');
					foreach(string space in correctSpaces)
					{
						byte index = byte.Parse(space);
						GetGrid(index).IsTarget = true;
						correctGrids.Add(GetGrid(index));

					}
				}
			}
		}

		private class Grid
		{
			internal bool HasBeenGuessed = false;
			internal bool IsTarget = false;
			internal ulong guessedBy;
		}

		public class GridGameCommands : InteractionModuleBase<SocketInteractionContext>
		{
			[EnabledInDm(false)]
			//[DefaultMemberPermissions(GuildPermission.ManageChannels)]
            [SlashCommand("startgame", "starts a new grid finder game")]
			public async Task StartNewGame([ChannelTypes(ChannelType.Text)]IChannel? targetChannel = null)
			{
                if (!Program.runningGames.ContainsKey(Context.Guild.Id))
                {
					GridGame newGame = new();
					Program.runningGames.Add(Context.Guild.Id, newGame);
					if(targetChannel == null)
					{
						ComponentBuilder builder = new ComponentBuilder().WithButton("Submit answer", $"GridGameAnswerBtn-{Context.Guild.Id}");
						await RespondWithFileAsync((Program.runningGames[Context.Guild.Id] as GridGame).currentSet[2], components: builder.Build());
						newGame.gridUI = FollowupAsync(newGame.GridToString()).Result.Id;
					}
					else
					{
						await RespondAsync($"Starting a new game in <#{targetChannel.Id}>", ephemeral: true);
						await ((IMessageChannel)targetChannel).SendFileAsync("mietzi.jpg");
					}
                }
				else
				{
					await RespondAsync("There is currently a running game.", ephemeral: true);
				}
			}
		}

		private bool AreAllGuessed()
		{
			bool AllGuessed = false;
			if (correctGrids[0].HasBeenGuessed) AllGuessed = true;

			foreach(Grid grid in correctGrids)
			{
				AllGuessed &= grid.HasBeenGuessed;
			}
            return AllGuessed;
        }
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

		public void HandleAnswer(SocketModal modal)
		{
			byte answer = 0;
			try
			{
				answer = byte.Parse(modal.Data.Components.First(x => x.CustomId == "numericAnswer").Value);
				if(answer < 1 && answer > 100)
				{
					throw new FormatException();
				}
			}
			catch(FormatException ex)
			{
				_ = ex;
				modal.RespondAsync("Incorrect number, be sure to only use digits and for the number to be between 1-100", ephemeral:true);
			}
			Grid selectedGrid = GetGrid(answer);

			if (!selectedGrid.HasBeenGuessed)
			{
				selectedGrid.HasBeenGuessed = true;
				selectedGrid.guessedBy = modal.User.Id;

				if (selectedGrid.IsTarget)
				{
					modal.RespondAsync("You guessed correctly!", ephemeral: true);

				}
				else
				{
					modal.RespondAsync("You guessed incorrectly, try again,", ephemeral: true);
				}

				modal.Channel.ModifyMessageAsync(gridUI,(properties) =>
				{
					properties.Content = GridToString();
				} );
			}
			else
			{
				modal.RespondAsync("Space has already been guessed. Please select another one", ephemeral:true);
			}

			if (AreAllGuessed())
			{
				string FoundBy = "";
				byte index = 1;
				foreach(Grid correctSpace in correctGrids)
				{
					FoundBy += $"Space {index} was found by <@{correctSpace.guessedBy}>\n";
					index++;
				}
				modal.FollowupAsync("All spaces have been found!");
				modal.FollowupWithFilesAsync([new FileAttachment(currentSet[1]), new FileAttachment(currentSet[0])], $"Here is the solution and the reward!\n"+FoundBy);
				Program.runningGames.Remove((ulong)modal.GuildId);
			}
		}
	}
}
