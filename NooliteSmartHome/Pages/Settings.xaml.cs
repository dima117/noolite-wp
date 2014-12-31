using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Resources;

namespace NooliteSmartHome.Pages
{
	public partial class Settings
	{
		public Settings()
		{
			InitializeComponent();
			BuildLocalizedApplicationBar();
		}

		#region app bar

		private void BuildLocalizedApplicationBar()
		{
			ApplicationBar = new ApplicationBar();
			
			ApplicationBar.Buttons.Add(
				BuildAppBarButton(AppResources.AppBarButtonSave, "/Assets/AppBar/save.png", SaveButtonClick));

			ApplicationBar.Buttons.Add(
				BuildAppBarButton(AppResources.AppBarButtonCancel, "/Assets/AppBar/cancel.png", CancelButtonClick));
		}

		private void SaveButtonClick(object sender, EventArgs e)
		{
			SaveSettings();
			UpdateConfiguration();
		}

		private void CancelButtonClick(object sender, EventArgs e)
		{
			Navigate("/Pages/MainPage.xaml");
		}

		#endregion

		#region load

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var settings = ApplicationData.Settings;
			TbGatewayHost.Text = settings.Host ?? string.Empty;
			CbUseAuth.IsChecked = settings.AuthInfo != null;

			if (settings.AuthInfo != null)
			{
				TbLogin.Text = settings.AuthInfo.User;
				TbPassword.Password = settings.AuthInfo.Password;
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
					Password = TbPassword.Password
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
			SystemTray.ProgressIndicator.Text = AppResources.Common_ConfigurationIsLoading;
			SystemTray.ProgressIndicator.IsIndeterminate = true;
			SystemTray.ProgressIndicator.IsVisible = true;

			byte[] buf = null;

			try
			{
				var gateway = ApplicationData.Settings.CreateGateway();
				buf = await gateway.LoadConfigurationAsync();
			}
			catch
			{
			}

			if (buf != null)
			{
				var cfg = ApplicationData.SaveConfiguration(buf);

				if (cfg == null)
				{
					MessageBox.Show(AppResources.Common_LoadingConfigurationError);
				}
			}
			else
			{
				MessageBox.Show(AppResources.Common_LoadingConfigurationError);
			}

			SystemTray.ProgressIndicator.IsVisible = false;
			Navigate("/Pages/MainPage.xaml");
		}

		#endregion

		private void AuthFieldsIsVisible_OnClick(object sender, RoutedEventArgs e)
		{
			PanelAuth.Visibility = CbUseAuth.IsChecked.GetValueOrDefault() 
				? Visibility.Visible : Visibility.Collapsed;
		}
	}
}