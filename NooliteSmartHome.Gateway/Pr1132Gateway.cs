using System;
using System.Net.Http;
using System.Threading.Tasks;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Gateway
{
	public class Pr1132Gateway
	{
		private static readonly HttpClient client = new HttpClient();
		public Uri Host { get; private set; }

		public Pr1132Gateway(string host)
		{
			Host = new Uri("http://" + host);
		}

		public async Task<Pr1132Configuration> LoadConfiguration()
		{
			var url = GetUrl("noolite_settings.bin");
			
			var client = new HttpClient();
			
			using (var stream = await client.GetStreamAsync(url))
			{
				return Pr1132Configuration.Deserialize(stream);
			}
		}

		//public Pr1132SensorData[] LoadSensorData()
		//{
		//	var url = GetUrl("sens.xml");

		//	var xml = client.DownloadStringAsync(url);
		//	var doc = XDocument.Parse(xml);

		//	var result = new Pr1132SensorData[4];

		//	var root = doc.Element("response");

		//	if (root != null)
		//	{
		//		for (int i = 0; i < 4; i++)
		//		{

		//			string strT = root.Element("snst" + i).Value;
		//			string strH = root.Element("snsh" + i).Value;
		//			string strState = root.Element("snt" + i).Value;

		//			var data = new Pr1132SensorData
		//			{
		//				State = (SensorState)Convert.ToInt32(strState)
		//			};

		//			decimal t;
		//			if (decimal.TryParse(strT, out t))
		//			{
		//				data.Temperature = t;
		//			}

		//			int h;
		//			if (int.TryParse(strH, out h))
		//			{
		//				data.Humidity = h;
		//			}

		//			result[i] = data;
		//		}
		//	}

		//	return result;
		//}

		private Uri GetUrl(string relativeUrl)
		{
			return new Uri(Host, relativeUrl);
		}

		private async void SendRequest(Uri url)
		{
			await client.GetByteArrayAsync(url);
		}

		public void SendCommand(
			byte cmd,
			byte channel,
			byte level)
		{
			const string URL_FORMAT = "api.htm?cache={0}&ch={1}&cmd={2}&br={3}";
			var relativeUrl = string.Format(URL_FORMAT, DateTime.Now.Ticks, channel, cmd, level);

			var url = GetUrl(relativeUrl);
			SendRequest(url);
		}
	}
}
