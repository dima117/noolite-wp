using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NooliteSmartHome.Gateway
{
	public class Pr1132Gateway
	{
		private readonly HttpClient client;

		public Pr1132Gateway(string host)
		{
			var baseAddress = new Uri("http://" + host);

			client = new HttpClient
			{
				BaseAddress = baseAddress
			};
		}

		public Pr1132Gateway(string host, string login, string password) : this(host)
		{
			var baseAddress = new Uri("http://" + host);

			var msgHandler = new HttpClientHandler
			{
				Credentials = new NetworkCredential(login, password),
			};

			client = new HttpClient(msgHandler)
			{
				BaseAddress = baseAddress
			};
		}

		#region helpers

		private async Task<byte[]> SendRequest(string url)
		{
			return await client.GetByteArrayAsync(url);
		}

		#endregion

		public async Task<byte[]> LoadConfigurationAsync()
		{
			var url = string.Format("noolite_settings.bin?cache={0}", DateTime.Now.Ticks);
			return await SendRequest(url);
		}

		public async void SendCommandAsync(
			GatewayCommand cmd,
			byte channel,
			byte level)
		{
			const string URL_FORMAT = "api.htm?cache={0}&ch={1}&cmd={2}&br={3}";
			var url = string.Format(URL_FORMAT, DateTime.Now.Ticks, channel, (byte)cmd, level);

			await SendRequest(url);
		}
	}
}
