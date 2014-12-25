using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;
using NooliteSmartHome.Resources;

namespace NooliteSmartHome.Pages
{
	public partial class MainPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
			BuildLocalizedApplicationBar();
		}

		#region app bar

		private ApplicationBarIconButton BuildAppBarButton(string text, string icon, EventHandler handler)
		{
			var iconUri = new Uri(icon, UriKind.Relative);

			var appBarButton = new ApplicationBarIconButton(iconUri) { Text = text };
			appBarButton.Click += handler;

			return appBarButton;
		}

		private ApplicationBarMenuItem BuildAppBarMenuItem(string text, EventHandler handler)
		{
			var appBarButton = new ApplicationBarMenuItem { Text = text };
			appBarButton.Click += handler;

			return appBarButton;
		}

		private void BuildLocalizedApplicationBar()
		{
			ApplicationBar = new ApplicationBar();


			ApplicationBar.Buttons.Add(
				BuildAppBarButton(AppResources.AppBarButtonSync, "/Assets/AppBar/sync.png", BtnSyncClick));

			ApplicationBar.MenuItems.Add(
				BuildAppBarMenuItem(AppResources.AppBarButtonSettings, BtnSettingsClick));

			ApplicationBar.MenuItems.Add(
				BuildAppBarMenuItem(AppResources.AppBarButtonAbout, BtnAboutClick));
		}

		private void BtnSyncClick(object sender, EventArgs e)
		{
			UpdateConfiguration();
		}

		private void BtnSettingsClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
		}

		private void BtnAboutClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
		}

		#endregion

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			UpdateGroupList();
		}

		private void UpdateGroupList()
		{
			var config = ApplicationData.GetConfiguration();

			if (config == null)
			{
				EmptyTextBlock.Visibility = Visibility.Visible;
				GroupList.Visibility = Visibility.Collapsed;
			}
			else
			{
				GroupList.DataContext = BuildGroupListModel(config);
			}
		}

		private ObservableCollection<GroupModel> BuildGroupListModel(Pr1132Configuration config)
		{
			var collection = new ObservableCollection<GroupModel>();

			for (int i = 0; i < config.Groups.Length; i++)
			{
				var group = config.Groups[i];

				if (group.Enabled)
				{
					var icon = ApplicationData.Settings.GetIcon(i);
					var model = new GroupModel(group, icon, i);
					collection.Add(model);
				}
			}

			return collection;
		}

		// todo: дублирование кода на странице настроек
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
				else
				{
					MessageBox.Show(AppResources.Common_SynchronizationIsCompletedSuccessfully);
					UpdateGroupList();
				}
			}
			else
			{
				MessageBox.Show(AppResources.Common_LoadingConfigurationError);
			}

			SystemTray.ProgressIndicator.IsVisible = false;
		}

		private void GroupListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var group = GroupList.SelectedItem as GroupModel;

			if (group != null)
			{
				string url = string.Format("/Pages/Group.xaml?index={0}", group.Index);
				NavigationService.Navigate(new Uri(url, UriKind.Relative));
			}
		}

		private void ChangeIconTapMenuOnClick(object sender, RoutedEventArgs e)
		{
			var item = sender as Microsoft.Phone.Controls.MenuItem;

			if (item != null)
			{
				var arg = item.CommandParameter;
				string url = string.Format("/Pages/Icons.xaml?index={0}", arg);
				NavigationService.Navigate(new Uri(url, UriKind.Relative));
			}
		}

		private void PinToStartTapMenuOnClick(object sender, RoutedEventArgs e)
		{
			var item = sender as Microsoft.Phone.Controls.MenuItem;

			if (item != null)
			{
				var arg = (item.CommandParameter ?? string.Empty).ToString();

				int index;
				if (int.TryParse(arg, out index))
				{
					var group = ApplicationData.GetConfiguration().Groups[index];
					var icon = ApplicationData.Settings.GetIcon(index);

					string pageUrl = string.Format("/Pages/Group.xaml?index={0}", index);
					string iconUrl = string.Format("/Assets/Groups/{0}.png", icon);

					var secTileData = new StandardTileData
					{
						Title = group.Name,
						BackgroundImage = new Uri(iconUrl, UriKind.RelativeOrAbsolute)
					};

					ShellTile.Create(new Uri(pageUrl, UriKind.RelativeOrAbsolute), secTileData);
				}
			}
		}
	}
}