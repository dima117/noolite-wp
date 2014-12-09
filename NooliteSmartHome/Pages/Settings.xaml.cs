using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Pages
{
	public partial class Settings
	{
		public Settings()
		{
			InitializeComponent();
		}

		#region load

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var settings = ApplicationData.Settings;
			TbGatewayHost.Text = settings.Host ?? string.Empty;
			CbUseAuth.IsChecked = settings.AuthInfo != null;

			if (settings.AuthInfo != null)
			{
				TbLogin.Text = settings.AuthInfo.User;
				TbPassword.Text = settings.AuthInfo.Password;
				PanelAuth.Visibility = Visibility.Visible;
			}
		}

		#endregion

		#region save

		private void SaveSettings()
		{
			ApplicationData.Settings.Host = TbGatewayHost.Text;

			if (CbUseAuth.IsChecked.GetValueOrDefault())
			{
				ApplicationData.Settings.AuthInfo = new AuthInfo
				{
					User = TbLogin.Text,
					Password = TbPassword.Text
				};
			}
			else
			{
				ApplicationData.Settings.AuthInfo = null;
			}

			ApplicationData.SaveCurrentSettings();
		}

		private async void UpdateConfiguration()
		{
			SystemTray.ProgressIndicator.Text = "идет обновление конфигурации";
			SystemTray.ProgressIndicator.IsIndeterminate = true;
			SystemTray.ProgressIndicator.IsVisible = true;

			var gateway = ApplicationData.Settings.CreateGateway();
			var buf = await gateway.LoadConfigurationAsync();

			if (buf != null)
			{
				var cfg = ApplicationData.SaveConfiguration(buf);

				if (cfg == null)
				{
					MessageBox.Show("Ошибка при синхронизации!");
				}
			}
			else
			{
				MessageBox.Show("Ошибка при синхронизации!");
			}

			SystemTray.ProgressIndicator.IsVisible = false;
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}

		private void SaveButton_OnClick(object sender, EventArgs e)
		{
			SaveSettings();
			UpdateConfiguration();
		}

		#endregion

		#region cancel

		private void CancelButton_OnClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}

		#endregion

		private void AuthFieldsIsVisible_OnClick(object sender, RoutedEventArgs e)
		{
			PanelAuth.Visibility = CbUseAuth.IsChecked.GetValueOrDefault() 
				? Visibility.Visible : Visibility.Collapsed;
		}
	}
}