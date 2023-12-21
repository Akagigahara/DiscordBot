using System.Net.Http.Json;

namespace DiscordBot
{
	internal class HttpHandler
	{
		readonly HttpClient client;

		HttpHandler()
		{
			client = new();
		}

		public string SendRequest(HttpRequestMessage Request)
		{
			HttpResponseMessage response = client.Send(Request);
			HttpContent content = response.Content;
			return content.ReadFromJsonAsync<string>(System.Text.Json.JsonSerializerOptions.Default).Result!;
		}
	}
}
