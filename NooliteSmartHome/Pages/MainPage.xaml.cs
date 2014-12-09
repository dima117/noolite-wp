using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;

namespace NooliteSmartHome.Pages
{
	public partial class MainPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
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
					var model = new GroupModel(group, i);
					collection.Add(model);
				}
			}

			return collection;
		}

		// todo: дублирование кода на странице настроек
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
				else
				{
					MessageBox.Show("Синхронизация прошла успешно");
				}
			}
			else
			{
				MessageBox.Show("Ошибка при синхронизации!");
			}

			SystemTray.ProgressIndicator.IsVisible = false;
		}

		#region navigation

		private void BtnSettingsClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
		}

		private void BtnAboutClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
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

		#endregion

		private void BtnSyncClick(object sender, EventArgs e)
		{
			UpdateConfiguration();
		}
	}

}