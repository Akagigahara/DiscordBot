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
			listener.Prefixes.Add("http://localhost/");
			try
			{ 
				listener.Start();
				Console.WriteLine("Started");
				while(listener.IsListening)
				{
					HttpListenerContext context = listener.GetContext();
					Console.WriteLine(context.Request.ToString());
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
				
			}
		}
	}
}
