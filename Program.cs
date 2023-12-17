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
		}
	}
}
