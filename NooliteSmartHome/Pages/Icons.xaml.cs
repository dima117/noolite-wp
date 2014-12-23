using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Resources;

namespace NooliteSmartHome.Pages
{
	public partial class Icons : PhoneApplicationPage
	{
		public Icons()
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
		}

		private void CancelButtonClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}

		#endregion

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var index = GetGroupIndex();
			var config = ApplicationData.GetConfiguration();

			TbGroupName.Text = config.Groups[index].Name.ToLower();

			var items = Enum.GetValues(typeof (IconOfGroup)).Cast<IconOfGroup>().ToArray();
			IconGrid.DataContext = BuildGroupListModel(items);
		}

		private ObservableCollection<IconItemModel> BuildGroupListModel(IconOfGroup[] items)
		{
			var collection = new ObservableCollection<IconItemModel>();

			foreach (var item in items)
			{
				var model = new IconItemModel(item);
				collection.Add(model);
			}

			return collection;
		}

		private int GetGroupIndex()
		{
			string strIndex;

			if (NavigationContext.QueryString.TryGetValue("index", out strIndex))
			{
				int index;
				if (int.TryParse(strIndex, out index))
				{
					return index;
				}
			}

			throw new ArgumentException();
		}
	}

	public class IconItemModel
	{
		public readonly IconOfGroup icon;

		public IconItemModel(IconOfGroup item)
		{
			icon = item;
		}

		public string Path
		{
			get { return string.Format("../Assets/Groups/{0}.png", icon); }
		}
	}
}