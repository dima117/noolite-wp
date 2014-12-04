using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using NooliteSmartHome.Gateway;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Pages
{
	public partial class MainPage : PhoneApplicationPage
	{
		private Pr1132Configuration configuration;

		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!isf.FileExists("noolite_settings.bin"))
				{
					EmptyTextBlock.Visibility = Visibility.Visible;
					GroupList.Visibility = Visibility.Collapsed;
				}
				else
				{
					using (var stream = isf.OpenFile("noolite_settings.bin", FileMode.Open))
					{
						configuration = Pr1132Configuration.Deserialize(stream);

						var collection = new ObservableCollection<Pr1132ControlGroup>(configuration.Groups);
						GroupList.DataContext = collection;
					}
				}
			}
		}

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
			var group = GroupList.SelectedItem as Pr1132ControlGroup;

			if (group != null)
			{
				string url = string.Format("/Pages/Group.xaml?msg={0}", group.Name);
				NavigationService.Navigate(new Uri(url, UriKind.Relative));
			}
		}
	}

}