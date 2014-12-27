using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Gateway.Encodings;

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

		public Pr1132Gateway(string host, string login, string password)
			: this(host)
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
			byte[] result;

			try
			{
				result = await client.GetByteArrayAsync(url);
			}
			catch
			{
				result = null;
			}

			return result;
		}

		#endregion

		public async Task<byte[]> LoadConfigurationAsync()
		{
			var url = string.Format("noolite_settings.bin?cache={0}", DateTime.Now.Ticks);
			return await SendRequest(url);
		}

		public async Task<Pr1132SensorData[]> LoadSensorData()
		{
			var url = string.Format("sens.xml?cache={0}", DateTime.Now.Ticks);
			var response = await SendRequest(url);

			if (response == null)
			{
				return null;
			}

			var xml = Windows1251Encoding.Instance.GetString(response, 0, response.Length);

			//var xml =
			//	"<response><snst0>-</snst0><snsh0>-</snsh0><snt0>1</snt0><snst1>23,8</snst1><snsh1>-</snsh1><snt1>0</snt1><snst2>-</snst2><snsh2>-</snsh2><snt2>1</snt2><snst3>-</snst3><snsh3>-</snsh3><snt3>1</snt3></response>";
			var doc = XDocument.Parse(xml);

			var result = new Pr1132SensorData[4];

			var root = doc.Element("response");

			if (root != null)
			{
				for (int i = 0; i < 4; i++)
				{
					// state
					var elState = root.Element("snt" + i);
					if (elState != null)
					{
						var state = (SensorState) Convert.ToInt32(elState.Value);
						var data = new Pr1132SensorData {State = state};

						// temperature
						var elT = root.Element("snst" + i);
						if (elT != null)
						{
							decimal t;
							if (decimal.TryParse(elT.Value, out t))
							{
								data.Temperature = t;
							}
						}

						// humidity
						var elH = root.Element("snsh" + i);
						if (elH != null)
						{
							int h;
							if (int.TryParse(elH.Value, out h))
							{
								data.Humidity = h;
							}
						}

						result[i] = data;
					}
					else
					{
						result[i] = null;
					}
				}
			}

			return result;
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
