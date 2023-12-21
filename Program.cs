using System.Net;
using Microsoft.Data.Sqlite;

namespace DiscordBot
{
	internal class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("Hello, World!");
			HttpListener listener = new();
			listener.Prefixes.Add("http://localhost:15411/");
			try
			{
				listener.Start();
				Console.WriteLine("Listening Started");
				while(listener.IsListening)
				{
					HttpListenerContext context = listener.GetContext();

					Console.WriteLine(context.Request.InputStream);
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
			}

			DatabaseHandler databaseHandler = new();
			databaseHandler.Close();
		}
	}
}
