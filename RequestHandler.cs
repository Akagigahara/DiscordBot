using DiscordBot.Resources;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace DiscordBot
{
	public class RequestHandler
	{
		readonly HttpClient client;

		RequestHandler()
		{
			client = new();
		}

		public static void Listen()
		{
			HttpListener listener = new();
			listener.Prefixes.Add("http://*:15412/");
			try
			{
				listener.Start();
				Console.WriteLine("Listening Started");
				while (listener.IsListening)
				{
					HttpListenerContext context = listener.GetContext();
					Task.Run(() => RequestHandler.ResolveRequest(context));


					Console.WriteLine(new StreamReader(context.Request.InputStream).ReadToEnd());
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
			}
		}

		public string SendRequest(HttpRequestMessage Request)
		{
			HttpResponseMessage response = client.Send(Request);
			HttpContent content = response.Content;
			return content.ReadFromJsonAsync<string>(JsonSerializerOptions.Default).Result!;
		}

		public static void ResolveRequest(HttpListenerContext Request)
		{
			InteractionBase ResolvedRequest = JsonSerializer.Deserialize<InteractionBase>(Request.Request.InputStream)!;
			switch(ResolvedRequest.type)
			{
				case InteractionBase.InteractionType.PING:
					Request.Response.StatusCode = 200;
					Request.Response.ContentType = "application/json";
					JsonSerializer.Serialize(Request.Response.OutputStream, new InteractionResponse { type = InteractionResponse.InteractionCallbackType.PONG});
					Request.Response.Close();
					break;
				case InteractionBase.InteractionType.APPLICATION_COMMAND:
					break;
				case InteractionBase.InteractionType.MESSAGE_COMPONENT:
					break;
				case InteractionBase.InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE:
					break;
				case InteractionBase.InteractionType.MODAL_SUBMIT:
					break;
			}
		}
	}
}
