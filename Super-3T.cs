using Discord.Commands;
using Discord.Interactions;

namespace DiscordBot
{

	internal class Super_3T
	{
		private ulong gameUI;

		SmallGrid[,] Grid =
		{
			{ new(), new(), new() },
			{ new(), new(), new() },
			{ new(), new(), new() },
		};

		private string GridToString()
		{
			return $"{}|{}|{}┃{}|{}|{}┃{}|{}|{}" +
				   $"-+-+-┃-+-+-┃-+-+-" +
				   $"{}|{}|{}┃{}|{}|{}┃{}|{}|{}" +
                   $"-+-+-┃-+-+-┃-+-+-" +
                   $"{}|{}|{}┃{}|{}|{}┃{}|{}|{}";
		}

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

		}
	}
}
