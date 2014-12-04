using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NooliteSmartHome.Pages
{
	public partial class Group : PhoneApplicationPage
	{
		public Group()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			string strIndex;

			if (NavigationContext.QueryString.TryGetValue("index", out strIndex))
			{
				int index;
				if (int.TryParse(strIndex, out index))
				{
					MessageBox.Show(index.ToString(), "Yes!", MessageBoxButton.OK);
				}

				TbGroupName.Text = strIndex;
			}


		}
	}
}