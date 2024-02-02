using DiscordBot.Resources;
using DiscordBot.Resources.Commands;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using static DiscordBot.Resources.InteractionResponse;

namespace DiscordBot
{
	public class RequestHandler
	{
		static readonly Dictionary<string, Action<InteractionBase.InteractionData>> Commands = new() { {"create", Create.HandleCommand }, };

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
					Task.Run(() => ResolveRequest(context));
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
			}
		}

		public static string SendRequest(HttpRequestMessage Request)
		{
			HttpClient requester = new()
			{
				BaseAddress = new Uri("https://discord.com/api/v10/"),
			};
			requester.DefaultRequestHeaders.Add("Authorization", $"Bot {ConfigurationManager.AppSettings["token"]}");
			HttpResponseMessage response = requester.Send(Request);
			HttpContent content = response.Content;
			using (StreamReader reader = new(content.ReadAsStream()))
			{
				return reader.ReadToEnd();
			}
		}

		public static void ResolveRequest(HttpListenerContext Context)
		{
			HttpListenerRequest Request = Context.Request;
			HttpListenerResponse Response = Context.Response;

			string ParsedRequest = new StreamReader(Request.InputStream, Request.ContentEncoding).ReadToEnd();
			bool IsVerified = VerifyRequest(Request.Headers, ParsedRequest);

			if (!IsVerified)
			{
				Response.StatusCode = 401;
				using (StreamWriter Write = new(Response.OutputStream))
				{
					Write.Write("invalid request signature");

				}
				Response.Close();
				return;
			}

			InteractionBase Interaction = JsonSerializer.Deserialize<InteractionBase>(ParsedRequest)!;
			Console.WriteLine(ParsedRequest);
			InteractionResponse? ResponseToSend = null;
			switch (Interaction.type)
			{
				case InteractionBase.InteractionType.PING:
					Response.StatusCode = 200;
					Response.ContentType = "application/json";
					ResponseToSend =  new InteractionResponse { type = InteractionResponse.InteractionCallbackType.PONG };
					break;
				case InteractionBase.InteractionType.APPLICATION_COMMAND:
					InteractionBase.InteractionData data = Interaction.data!;
					Response.StatusCode = 200;
					Response.ContentType = "application/json";
					ResponseToSend = new()
					{
						type = InteractionResponse.InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE,
						data = new CallbackMessage()
						{
							content = "Proccessing request"
						}
					};
					Task.Run(() => Commands[data.name].Invoke(data));
					break;
				case InteractionBase.InteractionType.MESSAGE_COMPONENT:
					break;
				case InteractionBase.InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE:
					break;
				case InteractionBase.InteractionType.MODAL_SUBMIT:
					break;
			}
			JsonSerializer.Serialize(Response.OutputStream, ResponseToSend);
			Console.WriteLine("Sending Message");
			Response.Close();
		}

		private static bool VerifyRequest(NameValueCollection Headers, string ParsedRequest)
		{
			Ed25519PublicKeyParameters PublicKeyParameter = new(Hex.DecodeStrict(ConfigurationManager.AppSettings["PublicKey"]!));
			Byte[] DataToVerify = Encoding.UTF8.GetBytes(Headers["X-Signature-Timestamp"]! + ParsedRequest);
			Byte[] SignatureBytes = Convert.FromHexString(Headers["X-Signature-Ed25519"]!);

			Ed25519Signer Verifier = new();
			Verifier.Init(false, PublicKeyParameter);
			Verifier.BlockUpdate(DataToVerify, 0, DataToVerify.Length);
			bool IsVerified = Verifier.VerifySignature(SignatureBytes);
			return IsVerified;
		}
	}
}
