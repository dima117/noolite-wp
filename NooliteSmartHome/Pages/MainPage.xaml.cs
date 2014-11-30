using System;
using System.IO;
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
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			//var gate = new Pr1132Gateway("192.168.0.168");
			//var cfg = gate.LoadConfiguration();
			//cfg.Wait();

			//foreach (var gr in cfg.Result.Groups)
			//{
			//	TbContent.Text += "\r\n" + gr.Name;
			//}

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}

		public async void Wevwev()
		{
			var client = new HttpClient();
			var t = await client.GetByteArrayAsync("http://192.168.0.168/noolite_settings.bin");

			using (var stream = new MemoryStream(t))
			{
				var cfg = Pr1132Configuration.Deserialize(stream);

				var msg = string.Join("\r\n", cfg.Groups.Select(x => x.Name));

				MessageBox.Show(msg);
			}

		}

		private void XXX(object sender, EventArgs e)
		{
			MessageBox.Show("xxx-xx");
			Wevwev();
			MessageBox.Show("yyy-yy");
		}
	}

}