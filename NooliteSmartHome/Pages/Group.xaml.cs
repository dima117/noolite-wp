﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;
using NooliteSmartHome.Resources;

namespace NooliteSmartHome.Pages
{
	public partial class Group
	{
		public Group()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var index = GetIntParameter("index");
			var config = ApplicationData.GetConfiguration();

			var model = BuildGroupModel(config, index);

			TbGroupName.Text = model.Name.ToLower();
			ChannelList.DataContext = new ObservableCollection<ChannelModel>(model.Channels);

			BuildLocalizedApplicationBar();
			UpdateSensorData(config, index);
		}

		#region app bar

		private void BuildLocalizedApplicationBar()
		{
			ApplicationBar = new ApplicationBar();

			ApplicationBar.Buttons.Add(
				BuildAppBarButton(AppResources.AppBarButtonSync, "/Assets/AppBar/sync.png", BtnSyncClick));
		}

		private void BtnSyncClick(object sender, EventArgs e)
		{
			var index = GetIntParameter("index");
			var config = ApplicationData.GetConfiguration();
			UpdateSensorData(config, index);
		}

		#endregion

		private async void UpdateSensorData(Pr1132Configuration config, int index)
		{	
			var group = config.Groups[index];

			var cnt = group.Sensors.Count(x => x);

			if (cnt > 0)
			{
				var data = await ApplicationData.Settings.CreateGateway().LoadSensorData();

				Sensors.Blocks.Clear();

				if (data != null)
				{
					for (int i = 0; i < group.Sensors.Length; i++)
					{
						var sensorData = data[i];

						if (group.Sensors[i] && sensorData != null)
						{
							var para = PrepareSensorData(cnt > 1, i, sensorData);

							Sensors.Blocks.Add(para);
						}
					}
				}
			}
		}

		private Paragraph PrepareSensorData(bool addLabel, int index, Pr1132SensorData sensorData)
		{
			var para = new Paragraph();

			if (sensorData.Temperature.HasValue)
			{
				var text = addLabel
					? string.Format("{0} {1}: {2} ", AppResources.Group_Sensor, index + 1, AppResources.Group_Temperature.ToLower())
					: string.Format("{0} ", AppResources.Group_Temperature);

				para.Inlines.Add(text);
				para.Inlines.Add(CreateBold(sensorData.Temperature, "{0}°C"));

				if (sensorData.Humidity.HasValue)
				{
					para.Inlines.Add(string.Format(", {0} ", AppResources.Group_Humidity.ToLower()));
					para.Inlines.Add(CreateBold(sensorData.Humidity, "{0}%"));
				}
			}
			return para;
		}

		private GroupDetailsModel BuildGroupModel(Pr1132Configuration config, int index)
		{
			var group = config.Groups[index];
			var icon = ApplicationData.Settings.GetIcon(index);
			var groupModel = new GroupDetailsModel(group, icon, index);

			foreach (var channelNumber in group.ChannelNumbers)
			{
				if (channelNumber.HasValue)
				{
					var channel = config.Channels[channelNumber.Value];
					var channelModel = new ChannelModel(channel, (byte)channelNumber.Value);
					groupModel.Channels.Add(channelModel);
				}
			}

			return groupModel;
		}
		
		private void GroupItem_OnSendCommand(object sender, SendCommandEventArgs e)
		{
			try
			{
				ApplicationData
					.Settings
					.CreateGateway()
					.SendCommandAsync(e.command, e.channel, e.brightness);
			}
			catch (Exception)
			{
				MessageBox.Show(AppResources.Common_SendCommandError);
			}
		}


		#region labels

		private Bold CreateBold(object value, string valueFormat)
		{
			var str = string.Format(valueFormat, value);
			var bold = new Bold
			{
				Foreground = new SolidColorBrush(IconGridItem.AccentColor),
				Inlines = { str }
			};

			return bold;
		}

		#endregion
	}
}