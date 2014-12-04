using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Model
{
	public class GroupModel
	{
		public GroupModel(Pr1132ControlGroup group, int index)
		{
			Index = index;
			Name = group.Name;
		}

		public int Index { get; set; }

		public string Name { get; set; }
	}
}