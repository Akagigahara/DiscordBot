using System.Net;
using DiscordBot;
using Microsoft.Data.Sqlite;

namespace DiscordBot
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
			Task.Run(() => RequestHandler.Listen());
			

			DatabaseHandler databaseHandler = new();
			databaseHandler.Close();
		}
	}
}
