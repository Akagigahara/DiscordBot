using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Text;
using SL = SixLabors.ImageSharp;

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
		/// <summary>
		/// List containing the snowflakes of players on cooldown
		/// </summary>
		private readonly List<ulong> playersOnCD = [];
		/// <summary>
		/// A dictionary containing the meta data for the image currently in play
		/// </summary>
		private Dictionary<string, string> metaData = [];
		/// <summary>
		/// Dictionary containing the current settings for the current game
		/// </summary>
		private Dictionary<string, string> settings = [];
		/// <summary>
		/// Image containing the current state of the base grid.
		/// </summary>
		private readonly SL.Image baseGrid = SL.Image.Load("./pictures/gridBase.png");
		private int skipCounter = 0; 
		private readonly List<ulong> playersThatVotedSkip = [];

        /// <summary>
        /// Creates a <see langword="string"/> representation of the game grid.
        /// </summary>
        /// <returns>The stringified version of the   grid.</returns>
        [Obsolete("This is a legacy implementation of the grid, it is replaced by the image version of the grid.")]
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
		/// Drawing answer on the <see cref="baseGrid"/>
		/// </summary>
		/// <param name="Grid">The space guessed</param>
		public void MarkOnGrid(int position)
		{
			SL.Color wrongSpace = SL.Color.Red;
			SL.Color correctSpace = SL.Color.Green;


			PointF[] topLeftLine = { new(0, 0), new(30, 30) };
			PointF[] TopRightLine = [new(30, 0), new(0, 30)];

			int x, y;

			if (position % 10 == 0)
			{
				y = position / 10 - 1;
				x = 9;
			}
			else
			{
				y = (int)Math.Floor(position / 10.0);
				x = position % 10 - 1;
			}

			topLeftLine[0].Offset(x * 30, y * 30);
			topLeftLine[1].Offset(x * 30, y * 30);
			TopRightLine[0].Offset(x * 30, y * 30);
			TopRightLine[1].Offset(x * 30, y * 30);

			baseGrid.Mutate(image =>
			{
				image.DrawLine(GetGrid(position).IsTarget ? correctSpace : wrongSpace, 5, topLeftLine);
				image.DrawLine(GetGrid(position).IsTarget ? correctSpace : wrongSpace, 5, TopRightLine);
			});

		}

		/// <summary>
		/// Initializes a new grid game.
		/// </summary>
		/// <exception cref="DirectoryNotFoundException">Exeption thrown when the directory containing the images, /pictures/, does not exist</exception>
		public GridGame(ulong guildId)
		{
			Random rand = new();
			if (!Directory.Exists("./pictures/"))
			{
				throw new DirectoryNotFoundException("Directory not found");
			}

			LoadSettings(guildId);
			string[] collectionOfPictures = Directory.GetDirectories("./pictures/");
			currentSet = Directory.GetFiles(collectionOfPictures[rand.Next(0, collectionOfPictures.Length - 1)]);

			string metaDataFile = currentSet.First(x => x.Contains("picture.metadata"));
			foreach (string metaData in File.ReadAllLines(metaDataFile))
			{
				string[] parsedMetaData = metaData.Split('=');
				this.metaData.Add(parsedMetaData[0].Trim(), parsedMetaData[1].Trim());
			}
			if (!metaData.ContainsKey("correctSpaces"))
			{
				foreach (KeyValuePair<string, string> pair in metaData)
				{
					Console.WriteLine(pair.Key + pair.Value);
				}
			}
			foreach (string square in metaData["correctSpaces"].Split(','))
			{
				byte index = byte.Parse(square);
				Grid grid = GetGrid(index);
				grid.IsTarget = true;
				correctGrids.Add(grid);
			}
			Program.Log(new(LogSeverity.Info, $"{guildId}", "Game created"));
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
			[SlashCommand("start", "Starts a new grid finder game")]
			public async Task StartNewGame()
			{
				if (!Program.runningUniqueGames.ContainsKey(Context.Guild.Id))
				{
					GridGame newGame = new(Context.Guild.Id);
					Program.runningUniqueGames.Add(Context.Guild.Id, newGame);
					MemoryStream fileStream = new();
					await RespondWithFileAsync(
						newGame.currentSet.First(file => file.Contains("3.")),
						text: newGame.settings["role"] != "null" ? $"<@&{newGame.settings["role"]}>" : "",
						allowedMentions: AllowedMentions.All
					);

					newGame.baseGrid.Save(fileStream, new PngEncoder());
					newGame.gridUI = FollowupWithFileAsync(fileStream, "grid.png",
						components: buildGameButtons(Context.Guild.Id)).Result.Id;
				}
				else
				{
					_ = RespondAsync("There is currently a running game.", ephemeral: true);
				}
			}

			[SlashCommand("end", "Ends a currently running game")]
			public async Task EndGame()
			{
				if (Program.runningUniqueGames.ContainsKey(Context.Guild.Id))
				{
					_ = RespondAsync("Ending game.");
					Program.runningUniqueGames.Remove(Context.Guild.Id);
				}
				else
				{
					_ = RespondAsync("No game is currently running", ephemeral: true);
				}
			}

			[SlashCommand("restart", "Ends the currently running game and starts a new one")]
			public async Task RestartGame()
			{
				if (Program.runningUniqueGames.ContainsKey(Context.Guild.Id))
				{
					_ = RespondAsync("Ending game.");
					Program.runningUniqueGames.Remove(Context.Guild.Id);
					GridGame newGame = new(Context.Guild.Id);
					Program.runningUniqueGames.Add(Context.Guild.Id, newGame);
					MemoryStream fileStream = new();
					await FollowupWithFileAsync(
						(Program.runningUniqueGames[Context.Guild.Id] as GridGame)!.currentSet.First(file => file.Contains("3.")),
						text: newGame.settings["role"] != "null" ? $"<@&{newGame.settings["role"]}>" : "",
						allowedMentions: AllowedMentions.All
					);
					newGame.baseGrid.Save(fileStream, new PngEncoder());
					newGame.gridUI = FollowupWithFileAsync(
						fileStream, "grid.png",
						components: buildGameButtons(Context.Guild.Id)
					).Result.Id;
				}
				else
				{
					RespondAsync("No game is currently running", ephemeral: true);
				}
			}
			[RequireUserPermission(GuildPermission.Administrator)]
			[SlashCommand("setting", "Change one of the settings for grid game.")]
			public async Task ChangeSettings(string? cooldown = null, IRole? role = null)
			{
				RespondAsync("Settings received.", ephemeral: true);
				if (Program.runningUniqueGames.ContainsKey(Context.Guild.Id))
				{
					(Program.runningUniqueGames[Context.Guild.Id] as GridGame)!.UpdateSettings(cooldown, role, Context.Guild.Id);
				}
				else
				{
					string configs = File.ReadAllText($"./configs/{Context.Guild.Id}/gridGame.config");
					string[] parsedSettingsLines = configs.Split("\n");
					if (cooldown is not null)
					{
						configs = configs.Replace(parsedSettingsLines[0].Split('=')[1].Trim(), cooldown);
					}
					if (role is not null)
					{
						configs = configs.Replace(parsedSettingsLines[1].Split('=')[1].Trim(), $"{role.Id}");
					}
					File.WriteAllBytes($"./configs/{Context.Guild.Id}/gridGame.config", Encoding.UTF8.GetBytes(configs));
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
				y = position / 10 - 1;
				x = 9;
			}
			else
			{
				y = (int)Math.Floor(position / 10.0);
				x = position % 10 - 1;
			}
			return grid[y, x];
		}

		/// <summary>
		/// Function handling the answer submitted by the user in the modal.
		/// </summary>
		/// <param name="modal">Modal that created the interaction</param>
		public void HandleAnswer(SocketModal modal)
		{
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
						modal.FollowupAsync($"Your {answerIndex}. guess, {answer}, was correct!");
					}
					else
					{
						modal.FollowupAsync($"Your {answerIndex}. guess, {answer}, was incorrect, try again!", ephemeral: true);
					}
					answerIndex++;
					MarkOnGrid(answer);
				}
				else
				{
					modal.FollowupAsync("Space has already been guessed. Please select another one", ephemeral: true);
				}

				if (AreAllGuessed())
				{
					string FoundBy = "";
					string Artist = metaData.TryGetValue("artist", out string? value) ? $"Artist/Model: {value}\n" : "";
					byte index = 1;
					foreach (Grid correctSpace in correctGrids)
					{
						FoundBy += $"Space {index++} was found by <@{correctSpace.guessedBy}>\n";
					}
					modal.FollowupAsync("All spaces have been found!");
					modal.FollowupWithFilesAsync(
						[new FileAttachment(currentSet.First(file => file.Contains("2."))),
						new FileAttachment(currentSet.First(file => file.Contains("1.")))],
						$"Here is the solution and the reward!\n" + FoundBy + Artist + $"submitted by <@{metaData["submittedBy"]}>",
						components: new ComponentBuilder().WithButton("Start new game", $"GridGameNewGameBtn-{modal.GuildId}").Build()
					);
					Program.runningUniqueGames.Remove((ulong)modal.GuildId!);
				}
			}
			MemoryStream fileStream = new();
			baseGrid.Save(fileStream, new PngEncoder());

			modal.Channel.ModifyMessageAsync(gridUI, (properties) =>
			{
				properties.Attachments = new Optional<IEnumerable<FileAttachment>>([new FileAttachment(fileStream, "grid.png")]);
			});
			playersOnCD.Add(modal.User.Id);
			RemovePlayerFromCD(modal.User.Id);
		}

		public void HandleButton(SocketMessageComponent button)
		{
			if (playersOnCD.Contains(button.User.Id))
			{
				button.RespondAsync("You are currently on cooldown", ephemeral: true);
				return;
			}

			byte num = 1;
			ModalBuilder modalBuilder = new ModalBuilder().WithTitle("Submit your number").WithCustomId($"GridGameAnswerModal-{button.GuildId}");
			foreach (Grid correctGrid in correctGrids.Where(x => !x.HasBeenGuessed))
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
		/// <summary>
		/// Used to remove a player from the <see cref="GridGame.playersOnCD"/> list.
		/// </summary>
		/// <param name="playerId">The Discord snowflake of the user that is to be removed.</param>
		private async void RemovePlayerFromCD(ulong playerId)
		{
			await Task.Delay(TimeSpan.FromSeconds(int.Parse(settings["cooldown"])));
			this.playersOnCD.Remove(playerId);
		}


		/// <summary>
		/// Handles the start of a new grid game through the button on the finished game.
		/// </summary>
		/// <param name="button">The component which caused this interaction</param>
		public static async void StartNewGame(SocketMessageComponent button)
		{
			ulong guildId = (ulong)button.GuildId!;
			GridGame newGame = new(guildId);
			Program.runningUniqueGames.Add((ulong)button.GuildId!, newGame);
			MemoryStream fileStream = new();
			await button.RespondWithFileAsync(
				newGame.currentSet.First(file => file.Contains("3.")),
				text: newGame.settings["role"] != "null" ? $"<@&{newGame.settings["role"]}>" : "",
				allowedMentions: AllowedMentions.All);
			newGame.baseGrid.Save(fileStream, new PngEncoder());
			newGame.gridUI = button.FollowupWithFileAsync(fileStream, "grid.png",
				components: buildGameButtons(guildId)).Result.Id;
		}

		public async void SkipCurrentGame(SocketMessageComponent button)
		{
			ulong guildId = (ulong)button.GuildId!;
			if (++skipCounter == 3)
			{
				button.RespondAsync("Skipping current game.");
				Program.runningUniqueGames.Remove(guildId);
				GridGame newGame = new(guildId);
				Program.runningUniqueGames.Add(guildId, newGame);
				MemoryStream fileStream = new();
                await button.FollowupWithFileAsync(
                    newGame.currentSet.First(file => file.Contains("3.")),
                    text: newGame.settings["role"] != "null" ? $"<@&{newGame.settings["role"]}>" : "",
                    allowedMentions: AllowedMentions.All);
                newGame.baseGrid.Save(fileStream, new PngEncoder());
                newGame.gridUI = button.FollowupWithFileAsync(fileStream, "grid.png",
                    components: buildGameButtons(guildId)).Result.Id;
            }
			else if(playersThatVotedSkip.Contains(button.User.Id))
			{
				button.RespondAsync("You have already voted to skip this game.", ephemeral: true);
			}
            else
			{
				button.FollowupAsync("Skip vote registered.", ephemeral: true);
				playersThatVotedSkip.Add(button.User.Id);
                await button.UpdateAsync(message =>
				{
					message.Components = buildGameButtons(guildId, skipCounter);
				});
			}
		}

		/// <summary>
		/// Function to update the settings of a game currently in play.
		/// </summary>
		/// <param name="cooldown">The time the cooldown should be set to</param>
		/// <param name="role">The role to ping on new game creation</param>
		/// <param name="guildId">The ID of the server that is currently running this game.</param>
		public void UpdateSettings(string? cooldown, IRole? role, ulong guildId)
		{
			string configs = File.ReadAllText($"./configs/{guildId}/gridGame.config");
			if (cooldown is not null)
			{
				configs = configs.Replace(settings["cooldown"], cooldown);
				settings["cooldown"] = cooldown;
			}
			if (role is not null)
			{
				configs = configs.Replace(settings["role"], $"{role.Id}");
				settings["role"] = $"{role.Id}";
			}
			File.WriteAllBytes($"./configs/{guildId}/gridGame.config", Encoding.UTF8.GetBytes(configs));
		}

		public async void LoadSettings(ulong guildId)
		{
			if (!File.Exists($"./configs/{guildId}/gridGame.config"))
			{
				Directory.CreateDirectory($"./configs/{guildId}/");
				FileStream config = File.Create($"./configs/{guildId}/gridGame.config");
				string settings = "cooldown = 30 \n" +
								  "role = null";
				config.Write(Encoding.UTF8.GetBytes(settings));
				this.settings.Add("cooldown", "30");
				this.settings.Add("role", "null");
			}
			else
			{
				foreach (string setting in File.ReadAllLines($"./configs/{guildId}/gridGame.config"))
				{
					string[] parsedSettings = setting.Split('=');
					this.settings.Add(parsedSettings[0].Trim(), parsedSettings[1].Trim());
				}
			}
			Program.Log(new(LogSeverity.Info, $"{guildId}", "Game settings loaded"));
		}

		private static MessageComponent buildGameButtons(ulong GuildId, int skipCounter = 0)
		{
			ComponentBuilder builder = new ComponentBuilder();
			builder.WithButton("Submit answer", $"GridGameAnswerBtn-{GuildId}")
				.WithButton($"Skip image {skipCounter}/3", $"GridGameSkipBtn-{GuildId}", ButtonStyle.Danger, new Emoji("\U000023E9"));

            return builder.Build();
		}
	}
}
