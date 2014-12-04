using System.Collections.Generic;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Model
{
	public class GroupDetailsModel : GroupModel
	{
		public GroupDetailsModel(Pr1132ControlGroup channel, int index) : base(channel, index)
		{
			Channels = new List<ChannelModel>();
		}

		public List<ChannelModel> Channels { get; set; } 
	}
}
