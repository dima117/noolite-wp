using System.Windows;

namespace NooliteSmartHome.Pages
{
	public partial class GroupItem
	{
		public GroupItem()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ChannelNameProperty =
			DependencyProperty.Register("ChannelName", typeof(string), typeof(GroupItem), new PropertyMetadata(default(string), ChannelNamePropertyChanged));

		private static void ChannelNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var myControl = d as GroupItem;
			if (myControl != null)
			{
				myControl.TbChannelName.Text = e.NewValue as string;
			}
		}

		public string ChannelName
		{
			get { return (string)GetValue(ChannelNameProperty); }
			set { SetValue(ChannelNameProperty, value); }
		}
	}
}
