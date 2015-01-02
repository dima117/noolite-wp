using System;
using System.Collections.ObjectModel;
using System.Linq;
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
			Navigate("/Pages/Settings.xaml");
		}

		private void BtnAboutClick(object sender, EventArgs e)
		{
			Navigate("/Pages/About.xaml");
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

		private async void UpdateConfiguration()
		{
			ShowProgress(AppResources.Common_ConfigurationIsLoading);

			try
			{
				var gateway = ApplicationData.Settings.CreateGateway();
				byte[] buf = await gateway.LoadConfigurationAsync();

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
			}
			catch
			{
			}

			HideProgress();
		}

		private void GroupListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var group = GroupList.SelectedItem as GroupModel;

			if (group != null)
			{
				Navigate("/Pages/Group.xaml?index={0}", group.Index);
			}
		}

		private void ChangeIconTapMenuOnClick(object sender, RoutedEventArgs e)
		{
			var item = sender as Microsoft.Phone.Controls.MenuItem;

			if (item != null)
			{
				Navigate("/Pages/Icons.xaml?index={0}", item.CommandParameter);
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

					string pageUrl = string.Format("/Pages/Group.xaml?index={0}&cache={1:N}", index, Guid.NewGuid());
					string iconUrl = icon.GetTileIconPath();

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