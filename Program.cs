using System.Net;

namespace DiscordBot
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
			if(!HttpListener.IsSupported)
			{
				Console.WriteLine("Not Supported");
			}

			HttpListener listener = new();
			listener.Prefixes.Add("http://discord.akagigahara.site:80/api/");
			while (listener.IsListening)
			{
				try
				{ 
					listener.Start();
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.ToString());
					HttpListenerContext context = listener.GetContext();
					Console.WriteLine(context.Request.ToString);
				}
			}
		}
	}
}
