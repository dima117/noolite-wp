using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Model
{
	public class GroupModel
	{
		public GroupModel(Pr1132ControlGroup channel, int index)
		{
			Index = index;
			Name = channel.Name;
		}

		public int Index { get; set; }

		public string Name { get; set; }
	}
}