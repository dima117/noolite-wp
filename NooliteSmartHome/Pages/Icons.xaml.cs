using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Pages
{
	public partial class Icons : PhoneApplicationPage
	{
		public Icons()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

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