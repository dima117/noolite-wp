namespace NooliteSmartHome.Helpers
{
	public enum IconOfGroup
	{
		Bulb,
		Plug,

		Cofee,
		Kitchen,
		Laptop,
		Monitor,

		Child,

		Sliders,

		Door,

		Home,
		Car,
		Sun,
		Tree
	}

	public static class IconOfGroupExtensions
	{
		public static string GetIconPath(this IconOfGroup icon)
		{
			return string.Format("../Assets/Groups/{0}.png", icon);
		}

		public static string GetTileIconPath(this IconOfGroup icon)
		{
			return string.Format("/Assets/Groups/Tiles/{0}.png", icon);
		}
	}
}
