using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NooliteSmartHome.Helpers
{
	public class NshBasePage : PhoneApplicationPage
	{
		#region app bar

		protected ApplicationBarIconButton BuildAppBarButton(string text, string icon, EventHandler handler)
		{
			var iconUri = new Uri(icon, UriKind.Relative);

			var appBarButton = new ApplicationBarIconButton(iconUri) { Text = text };
			appBarButton.Click += handler;

			return appBarButton;
		}

		protected ApplicationBarMenuItem BuildAppBarMenuItem(string text, EventHandler handler)
		{
			var appBarButton = new ApplicationBarMenuItem { Text = text };
			appBarButton.Click += handler;

			return appBarButton;
		}

		#endregion

		public void Navigate(string url, params object[] args)
		{
			string url2 = string.Format(url, args);
			NavigationService.Navigate(new Uri(url2, UriKind.Relative));
		}

		protected int GetIntParameter(string name)
		{
			string strIndex;

			if (NavigationContext.QueryString.TryGetValue(name, out strIndex))
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
}
