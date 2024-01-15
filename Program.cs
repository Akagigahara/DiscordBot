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
			RequestHandler.Listen().Start();
			

			DatabaseHandler databaseHandler = new();
			databaseHandler.Close();
		}
	}
}
