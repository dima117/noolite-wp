using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
			base.OnNavigatedTo(e);

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
	}

}