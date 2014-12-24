using System.Collections.Generic;
using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Model
{
	public class GroupDetailsModel : GroupModel
	{
		public GroupDetailsModel(Pr1132ControlGroup channel, IconOfGroup icon, int index)
			: base(channel, icon, index)
		{
			Channels = new List<ChannelModel>();
		}

		public List<ChannelModel> Channels { get; set; } 
	}
}
