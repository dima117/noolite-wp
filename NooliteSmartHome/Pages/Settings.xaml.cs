using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Pages
{
	public partial class Settings : PhoneApplicationPage
	{
		public Settings()
		{
			InitializeComponent();
		}

		private void BtnDownloadClick(object sender, RoutedEventArgs e)
		{
			UpdateSettings();
		}

		public async void UpdateSettings()
		{
			var client = new HttpClient();
			var buf = await client.GetByteArrayAsync("http://192.168.0.168/noolite_settings.bin");

			using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (var stream = isf.OpenFile("noolite_settings.bin", FileMode.OpenOrCreate))
				{
					stream.Write(buf, 0, buf.Length);
					MessageBox.Show("Настройки загружены");
				}
			}
		}
	}
}