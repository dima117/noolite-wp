using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Windows;
using Microsoft.Phone.Controls;
using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Pages
{
	public partial class Settings : PhoneApplicationPage
	{
		public Settings()
		{
			InitializeComponent();
			FillData();
		}

		private void FillData()
		{
			var settings = ApplicationData.Current;
			TbGatewayHost.Text = settings.Host ?? string.Empty;
		}

		private void BtnDownloadClick(object sender, RoutedEventArgs e)
		{
			UpdateSettings();
		}

		public async void UpdateSettings()
		{
			var client = new HttpClient();
			var url = string.Format("http://192.168.0.168/noolite_settings.bin?cache={0}", DateTime.Now.Ticks);
			var buf = await client.GetByteArrayAsync(url);

			var cfg = ApplicationData.SaveConfiguration(buf);
			var msg = cfg == null ? "Ошибка при синхронизации!" : "Настройки загружены";
			MessageBox.Show(msg);
		}

		private void CancelButton_OnClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}

		private void SaveButton_OnClick(object sender, EventArgs e)
		{
			ApplicationData.Current.Host = TbGatewayHost.Text;
			ApplicationData.SaveCurrentSettings();

			UpdateSettings();
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}
	}
}