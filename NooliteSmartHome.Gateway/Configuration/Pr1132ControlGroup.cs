namespace NooliteSmartHome.Gateway.Configuration
{
	public class Pr1132ControlGroup
	{
		public Pr1132ControlGroup()
		{
			Sensors = new bool[4];
			ChannelNumbers = new int?[8];
		}

		public string Name { get; set; }

		public bool Enabled { get; set; }

		public int?[] ChannelNumbers { get; private set; }

		public bool[] Sensors { get; private set; }
	}
}