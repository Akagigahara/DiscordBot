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
			listener.Start();
			HttpListenerContext context = listener.GetContext();
			Console.WriteLine(context.Request.ToString);
		}
	}
}
