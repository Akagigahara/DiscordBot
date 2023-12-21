using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
	internal class DatabaseHandler
	{
		SqliteConnection connection;
		public DatabaseHandler() 
		{
			connection = new SqliteConnection("Data Source=discord.db");
			connection.Open();
		}

		public void Close()
		{
			connection.Close();
		}
	}
}
