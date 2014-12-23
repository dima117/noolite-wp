using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;
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
			var currentIcon = ApplicationData.Settings.GetIcon(index);

			TbGroupName.Text = config.Groups[index].Name.ToLower();

			var items = Enum.GetValues(typeof(IconOfGroup)).Cast<IconOfGroup>().ToArray();
			IconGrid.DataContext = BuildGroupListModel(items, currentIcon);
		}

		private ObservableCollection<IconItemModel> BuildGroupListModel(IconOfGroup[] items, IconOfGroup currentIcon)
		{
			var collection = new ObservableCollection<IconItemModel>();

			foreach (var item in items)
			{
				var model = new IconItemModel(item)
				{
					IsSelected = item == currentIcon
				};

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

		private void IconGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Get item of LongListSelector. 
			var userControlList = new List<IconGridItem>();
			GetItemsRecursive(IconGrid, ref userControlList);

			foreach (var userControl in userControlList)
			{
				userControl.IsSelected = false;
			}

			// Selected. 
			if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
			{
				foreach (var userControl in userControlList)
				{
					if (e.AddedItems[0].Equals(userControl.DataContext))
					{
						userControl.IsSelected = true;
					}
				}
			}
		}

		public static void GetItemsRecursive<T>(DependencyObject parents, ref List<T> objectList) where T : DependencyObject
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount(parents);

			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parents, i);


				if (child is T)
				{
					objectList.Add(child as T);
				}

				GetItemsRecursive(child, ref objectList);
			}
		} 
	}
}