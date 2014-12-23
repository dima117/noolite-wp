using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Model
{
	public class IconItemModel
	{
		public readonly IconOfGroup icon;

		public IconItemModel(IconOfGroup item)
		{
			icon = item;
		}

		public bool IsSelected { get; set; }

		public string Path
		{
			get { return string.Format("../Assets/Groups/{0}.png", icon); }
		}
	}
}