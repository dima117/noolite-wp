using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
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

			var index = GetGroupIndex();
			var config = ApplicationData.GetConfiguration();

			var model = BuildGroupModel(config, index);

			TbGroupName.Text = model.Name.ToLower();
			ChannelList.DataContext = new ObservableCollection<ChannelModel>(model.Channels);

			UpdateSensorData(config, index);
		}

		private async void UpdateSensorData(Pr1132Configuration config, int index)
		{
			var group = config.Groups[index];

			if (group.Sensors.Any(x => x))
			{
				var data = await ApplicationData.Settings.CreateGateway().LoadSensorData();

				if (data != null)
				{
					for (int i = 0; i < group.Sensors.Length; i++)
					{
						if (group.Sensors[i] && data[i] != null)
						{
							T.Inlines.Add(string.Format("{0}°C", data[i].Temperature));
							H.Inlines.Add(string.Format("{0}%", data[i].Humidity));
						}
					}
				}
			}
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

		private int GetGroupIndex()
		{
			string strIndex;

			if (NavigationContext.QueryString.TryGetValue("index", out strIndex))
			{
				int index;
				if (int.TryParse(strIndex, out index))
				{
					return index;
				}
			}

			throw new ArgumentException();
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
	}
}