using Discord.Commands;
using SixLabors.ImageSharp;
using Discord.Interactions;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Discord.WebSocket;

namespace DiscordBot
{

	internal class Super_3T : IGame
	{
		public ulong gameId;
		private ulong gameUI;
		private readonly Image<Rgb24> image = new(900, 900);

		public Super_3T(long gameId)
		{
			this.gameId = (ulong)gameId;
			image.Mutate(
				x =>
				{
					int width = image.Width;
					int height = image.Height;

                    for (int i = 1; i < 9; i++)
                    {
                        //vertical lines
                        x.DrawLine(Color.Blue, 5, [new Point(width * (i) / 9, 0), new Point(width * (i) / 9, height)]);
                        //horizontal lines
                        x.DrawLine(Color.Blue, 5, [new Point(0, height * (i) / 9), new Point(width, height * (i) / 9)]);
                    }
                    for (byte i = 1; i < 4; i++)
					{
						x.DrawLine(Color.Red, 5, [new Point(width * (i) / 3, 0), new Point(width * (i) / 3, height)]);
						x.DrawLine(Color.Red, 5, [new Point(0, height * (i) / 3), new Point(width, height * (i) / 3)]);
					}
				});
		}

		SmallGrid[,] Grid =
		{
			{ new(), new(), new() },
			{ new(), new(), new() },
			{ new(), new(), new() },
		};

			/*
		private string GridToString()
		{
			Image<Rgb24> png = new (55, 55);
			png.mut
			return;
		}
			*/

		public void CheckGameState()
		{
			State WonBy;
			//Top left, bot right
			if (Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2] && Grid[1, 1] != State.None)
			{
				WonBy = Grid[1, 1].GameState;
				return;
			}
			//Top right, bot left
			else if (Grid[0, 2] == Grid[1, 1] && Grid[1, 1] == Grid[2, 0] && Grid[1, 1] != State.None)
			{
				WonBy = Grid[1, 1].GameState;
				return;
			}
			//All horizontal and vertical patterns
			foreach (int x in Enumerable.Range(0, 2))
			{
				if (Grid[x, 0] == Grid[x, 1] && Grid[x, 1] == Grid[x, 2] && Grid[x, 1] != State.None)
				{
					WonBy = Grid[x, 1].GameState;//all horizontals
					break;
				}
				else if (Grid[0, x] == Grid[1, x] && Grid[1, x] == Grid[2, x] && Grid[1, x] != State.None)
				{
					WonBy = Grid[1, x].GameState;//all verticals
					break;
				}
			}


		}

		public void EndGame()
		{

		}

        public void HandleAnswer(SocketModal modal)
        {
            throw new NotImplementedException();
        }

        public void HandleButton(SocketMessageComponent button)
        {
            throw new NotImplementedException();
        }
    }

	internal class SmallGrid
	{
		State[,] Grid = {
			{new(), new(), new()},
			{new(), new(), new() },
			{new(), new(), new()}
		};

		public State GameState = State.None;

		public void CheckGameState()
		{
			//Top left, bot right
			if (Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2] && Grid[1, 1] != State.None)
			{
				GameState = Grid[1, 1];
				return;
			}
			//Top right, bot left
			else if (Grid[0, 2] == Grid[1, 1] && Grid[1, 1] == Grid[2, 0] && Grid[1, 1] != State.None)
			{
				GameState = Grid[1, 1];
				return;
			}
			//All horizontal and vertical patterns
			foreach (int x in Enumerable.Range(0, 2))
			{
				if (Grid[x, 0] == Grid[x, 1] && Grid[x, 1] == Grid[x, 2] && Grid[x, 1] != State.None)
				{
					GameState = Grid[x, 1];//all horizontals
					break;
				}
				else if (Grid[0, x] == Grid[1, x] && Grid[1, x] == Grid[2, x] && Grid[1, x] != State.None) 
				{
					GameState = Grid[1, x];//all verticals
					break;
				}
			}

			/*
			if (Grid[0, 0] == Grid[0, 1] && Grid[0, 1] == Grid[0, 2] && Grid[0, 1] != State.None) GameState = Grid[0, 1]; //All top row
			else if (Grid[1, 0] == Grid[1, 1] && Grid[1, 1] == Grid[1, 2] && Grid[1, 1] != State.None) GameState = Grid[1, 1]; //All mid row
			else if (Grid[2, 0] == Grid[2, 1] && Grid[2, 1] == Grid[2, 2] && Grid[2, 1] != State.None) GameState = Grid[2, 1]; //All bot row
			else if (Grid[0, 0] == Grid[1, 0] && Grid[1, 0] == Grid[2, 0] && Grid[1, 0] != State.None) GameState = Grid[1, 0]; //All left col
			else if (Grid[0, 1] == Grid[1, 1] && Grid[1, 1] == Grid[2, 1] && Grid[1, 1] != State.None) GameState = Grid[1, 1]; //All mid col
			else if (Grid[0, 2] == Grid[1, 2] && Grid[1, 2] == Grid[2, 2] && Grid[1, 2] != State.None) GameState = Grid[1, 2]; //All right col
			else if (Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2] && Grid[1, 1] != State.None) GameState = Grid[1, 1]; //Top left, bot right
			else if (Grid[0, 2] == Grid[1, 1] && Grid[1, 1] == Grid[2, 0] && Grid[1, 1] != State.None) GameState = Grid[1, 1]; //Top right, bot left
			 */
		}

		public static bool operator ==(SmallGrid x,SmallGrid y)
		{
			return x.GameState == y.GameState;
		}

		public static bool operator !=(SmallGrid x, SmallGrid y)
		{
			return !(x == y);
		}

		public static bool operator !=(SmallGrid x, State state)
		{
			return x.GameState != state;
		}
		public static bool operator ==(SmallGrid x, State state)
		{
			return x.GameState == state;
		}

	}

	[Flags]
	internal enum State
	{
		None = 0,
		X = 1,
		O = 2,
	}

	[Discord.Commands.Group("super3t")]
	public class Super3TCommands : InteractionModuleBase<SocketInteractionContext> 
	{
		[SlashCommand("start", "Starts a game of Super Tic-Tac-Toe")]
		public async Task StartS3T() 
		{
			if (!Program.runningGenericGames.ContainsKey(Context.Guild.Id))
			{
				Program.runningGenericGames.Add(Context.Guild.Id, []);
			}
			Super_3T newGame = new(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
		}
	}
}
