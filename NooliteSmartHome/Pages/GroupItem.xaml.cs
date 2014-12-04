using System.Linq;
using System.Windows;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Pages
{
	public partial class GroupItem
	{
		public GroupItem()
		{
			InitializeComponent();
		}

		#region channel name

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

		#endregion

		#region channel type

		public static readonly DependencyProperty ChannelTypeProperty =
			DependencyProperty.Register("ChannelType", typeof(Pr1132ChannelUiType), typeof(GroupItem), new PropertyMetadata(default(Pr1132ChannelUiType), ChannelTypePropertyChanged));

		private static void ChannelTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var value = (Pr1132ChannelUiType)e.NewValue;
			var myControl = d as GroupItem;
			if (myControl != null)
			{
				myControl.PanelSlider.Visibility = GetVisibility(
					value,
					Pr1132ChannelUiType.Dimmer,
					Pr1132ChannelUiType.LED);

				myControl.PanelLed.Visibility = GetVisibility(
					value,
					Pr1132ChannelUiType.LED);
			}
		}

		public Pr1132ChannelUiType ChannelType
		{
			get { return (Pr1132ChannelUiType)GetValue(ChannelTypeProperty); }
			set { SetValue(ChannelTypeProperty, value); }
		}

		private static Visibility GetVisibility(Pr1132ChannelUiType target, params Pr1132ChannelUiType[] types)
		{
			return types.Contains(target) ? Visibility.Visible : Visibility.Collapsed;
		}

		#endregion
	}
}
