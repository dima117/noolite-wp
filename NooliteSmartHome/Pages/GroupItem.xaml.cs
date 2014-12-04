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
			SetChannelType(default(Pr1132ChannelUiType));
		}

		#region set channel type

		private void SetChannelType(Pr1132ChannelUiType value)
		{
			PanelSlider.Visibility = GetVisibility(value, Pr1132ChannelUiType.Dimmer, Pr1132ChannelUiType.LED);
			PanelLed.Visibility = GetVisibility(value, Pr1132ChannelUiType.LED);
		}

		private static Visibility GetVisibility(Pr1132ChannelUiType target, params Pr1132ChannelUiType[] types)
		{
			return types.Contains(target) ? Visibility.Visible : Visibility.Collapsed;
		}
		
		#endregion

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
			
			var myControl = d as GroupItem;
			if (myControl != null)
			{
				var value = (Pr1132ChannelUiType)e.NewValue;
				myControl.SetChannelType(value);
			}
		}

		public Pr1132ChannelUiType ChannelType
		{
			get { return (Pr1132ChannelUiType)GetValue(ChannelTypeProperty); }
			set { SetValue(ChannelTypeProperty, value); }
		}

		#endregion
	}
}
