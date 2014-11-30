using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
					NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
				}
				else
				{
					using (var stream = isf.OpenFile("noolite_settings.bin", FileMode.Open))
					{
						configuration = Pr1132Configuration.Deserialize(stream);
						var msg = string.Join("\r\n", configuration.Groups.Select(x => x.Name));

						MessageBox.Show(msg);
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
	}

}