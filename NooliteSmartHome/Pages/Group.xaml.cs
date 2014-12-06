using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using NooliteSmartHome.Gateway;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;
using NooliteSmartHome.Model;

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
		}

		private GroupDetailsModel BuildGroupModel(Pr1132Configuration config, int index)
		{
			var group = config.Groups[index];
			var groupModel = new GroupDetailsModel(group, index);

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
			var gateway = new Pr1132Gateway("192.168.0.168");
			gateway.SendCommand((byte)e.command, e.channel, e.brightness);
		}
	}
}