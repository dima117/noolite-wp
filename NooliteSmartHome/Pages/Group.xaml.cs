using System;
using System.Collections.Generic;
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

			var index = GetGroupIndex();
			var config = ApplicationData.GetConfiguration();

			var model = BuildGroupModel(config, index);

			TbGroupName.Text = model.Name.ToLower();
			ChannelList.DataContext = new ObservableCollection<ChannelModel>(model.Channels);

			BuildLocalizedApplicationBar();
			UpdateSensorData(config, index);
		}

		#region app bar

		private ApplicationBarIconButton BuildAppBarButton(string text, string icon, EventHandler handler)
		{
			var iconUri = new Uri(icon, UriKind.Relative);

			var appBarButton = new ApplicationBarIconButton(iconUri) { Text = text };
			appBarButton.Click += handler;

			return appBarButton;
		}

		private void BuildLocalizedApplicationBar()
		{
			ApplicationBar = new ApplicationBar();


			ApplicationBar.Buttons.Add(
				BuildAppBarButton(AppResources.AppBarButtonSync, "/Assets/AppBar/sync.png", BtnSyncClick));
		}

		private void BtnSyncClick(object sender, EventArgs e)
		{
			var index = GetGroupIndex();
			var config = ApplicationData.GetConfiguration();
			UpdateSensorData(config, index);
		}

		#endregion

		private async void UpdateSensorData(Pr1132Configuration config, int index)
		{
			Sensors.Blocks.Clear();
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
							var para = new Paragraph();

							if (data[i].Temperature.HasValue)
							{
								para.Inlines.Add("температура ");
								para.Inlines.Add(CreateBold(data[i].Temperature, "{0}°C"));

								if (data[i].Humidity.HasValue)
								{
									para.Inlines.Add(", влажность ");
									para.Inlines.Add(CreateBold(data[i].Humidity, "{0}%"));
								}
							}

							Sensors.Blocks.Add(para);
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