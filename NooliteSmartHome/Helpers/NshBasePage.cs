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
	}
}
