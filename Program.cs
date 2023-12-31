﻿using System.Net;
using Microsoft.Data.Sqlite;

namespace DiscordBot
{
	internal class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("Hello, World!");
			HttpListener listener = new();
			listener.Prefixes.Add("http://*:15412/");
			try
			{
				listener.Start();
				Console.WriteLine("Listening Started");
				while(listener.IsListening)
				{
					HttpListenerContext context = listener.GetContext();

					Console.WriteLine(new StreamReader(context.Request.InputStream).ReadToEnd());
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
