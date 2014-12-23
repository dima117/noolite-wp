using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NooliteSmartHome.Pages
{
	public partial class IconGridItem : UserControl
	{
		private static Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
		private static Color inactiveColor = (Color)Application.Current.Resources["PhoneInactiveColor"];


		public IconGridItem()
		{
			InitializeComponent();
		}

		#region path

		public static readonly DependencyProperty PathProperty =
			DependencyProperty.Register("Path", typeof(string), typeof(IconGridItem), new PropertyMetadata(default(string), PathPropertyChanged));

		private static void PathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var myControl = d as IconGridItem;
			if (myControl != null)
			{
				var value = (string)e.NewValue;
				var uri = new Uri(value, UriKind.Relative);
				myControl.Image.Source = new BitmapImage(uri);
			}
		}

		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		#endregion

		#region is selected

		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(IconGridItem), new PropertyMetadata(default(bool), IsSelectedPropertyChanged));

		private static void IsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var myControl = d as IconGridItem;
			if (myControl != null)
			{
				var isSelected = (bool)e.NewValue;
				var color = isSelected ? accentColor : inactiveColor;
				myControl.Border.Background = new SolidColorBrush(color);
			}
		}

		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		#endregion
	}
}
