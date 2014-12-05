using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Model
{
	public class ChannelModel
	{
		public ChannelModel(Pr1132Channel channel, byte index)
		{
			Index = index;
			Name = channel.Name;
			Type = channel.Type;
		}

		public byte Index { get; set; }

		public string Name { get; set; }

		public Pr1132ChannelUiType Type { get; set; }
	}
}
