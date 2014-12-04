using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Model
{
	public class GroupControlModel
	{
		public GroupControlModel(Pr1132Channel channel)
		{
			Name = channel.Name;
			Type = channel.Type;
		}

		public string Name { get; set; }

		public Pr1132ChannelUiType Type { get; set; }
	}
}
