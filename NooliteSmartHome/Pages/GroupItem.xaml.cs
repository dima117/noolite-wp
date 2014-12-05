﻿using System;
using System.Linq;
using System.Windows;
using NooliteSmartHome.Gateway;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Resources;

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
			Switcher.Visibility = GetVisibility(value, Pr1132ChannelUiType.Switcher);
			PanelSlider.Visibility = GetVisibility(value, Pr1132ChannelUiType.Dimmer, Pr1132ChannelUiType.LED);
			PanelLed.Visibility = GetVisibility(value, Pr1132ChannelUiType.LED);
			PanelScene.Visibility = GetVisibility(value, Pr1132ChannelUiType.RestoreState);
		}

		private static Visibility GetVisibility(Pr1132ChannelUiType target, params Pr1132ChannelUiType[] types)
		{
			return types.Contains(target) ? Visibility.Visible : Visibility.Collapsed;
		}

		private void SetChannelName(string name)
		{
			Switcher.Header = name;
			LabelSlider.Text = string.Format("{0} ({1})", name, AppResources.GroupBrightness);
			LabelScene.Text = string.Format("{0} ({1})", name, AppResources.GroupScene);
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
				var value = e.NewValue as string;
				myControl.SetChannelName(value);
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

		#region index

		public static readonly DependencyProperty IndexProperty =
			DependencyProperty.Register("Index", typeof(byte), typeof(GroupItem), new PropertyMetadata(default(byte)));

		public byte Index
		{
			get { return (byte)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		#endregion

		public event Action<byte, GatewayCommand, byte> SendCommand;

		protected virtual void OnSendCommand(byte channel, GatewayCommand command, byte brightness = 0)
		{
			var handler = SendCommand;
			if (handler != null) handler(channel, command, brightness);
		}

		private void Switcher_OnClick(object sender, RoutedEventArgs e)
		{
			var command = Switcher.IsChecked.GetValueOrDefault() ? GatewayCommand.On : GatewayCommand.Off;
			OnSendCommand(Index, command);
		}
	}
}
