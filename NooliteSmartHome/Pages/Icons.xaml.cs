using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;
using NooliteSmartHome.Resources;

namespace NooliteSmartHome.Pages
{
	public partial class Icons
	{
		public Icons()
		{
			InitializeComponent();
			BuildLocalizedApplicationBar();
		}


		#region app bar

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
			var item = IconGrid.SelectedItem as IconItemModel;

			if (item != null)
			{
				var index = GetIntParameter("index");
				ApplicationData.Settings.SetIcon(index, item.icon);
				ApplicationData.SaveCurrentSettings();
			}

			Navigate("/Pages/MainPage.xaml");
		}

		private void CancelButtonClick(object sender, EventArgs e)
		{
			Navigate("/Pages/MainPage.xaml");
		}

		#endregion

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var index = GetIntParameter("index");
			var config = ApplicationData.GetConfiguration();
			var currentIcon = ApplicationData.Settings.GetIcon(index);

			TbGroupName.Text = config.Groups[index].Name.ToLower();

			var items = Enum.GetValues(typeof(IconOfGroup)).Cast<IconOfGroup>().ToArray();
			var collection = BuildGroupListModel(items, currentIcon);
			IconGrid.DataContext = collection;
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

		private void IconGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var userControlList = new List<IconGridItem>();
			GetItemsRecursive(IconGrid, ref userControlList);

			if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
			{
				foreach (var userControl in userControlList)
				{
					userControl.IsSelected = e.AddedItems[0].Equals(userControl.DataContext);
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