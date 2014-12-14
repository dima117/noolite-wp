using System;
using System.Windows;
using Microsoft.Phone.Tasks;

namespace NooliteSmartHome.Pages
{
	public partial class About
	{
		public About()
		{
			InitializeComponent();
		}

		private void HyperlinkOfficialSiteOnClick(object sender, RoutedEventArgs e)
		{
			new WebBrowserTask{Uri = new Uri("http://www.noo.com.by/")}.Show();
		}

		private void ButtonRateOnClick(object sender, RoutedEventArgs e)
		{
			new MarketplaceReviewTask().Show();
		}
	}
}