using NooliteSmartHome.Gateway;

namespace NooliteSmartHome.Helpers
{
	public class ApplicationSettings
	{
		public string Host { get; set; }

		public AuthInfo AuthInfo { get; set; }

		public IconOfGroup[] Icons { get; set; }

		public Pr1132Gateway CreateGateway()
		{
			return AuthInfo == null
				? new Pr1132Gateway(Host)
				: new Pr1132Gateway(Host, AuthInfo.User, AuthInfo.Password);
		}

		public IconOfGroup GetIcon(int index)
		{
			return Icons != null && Icons.Length > index ? Icons[index] : default(IconOfGroup);
		}

		public void SetIcon(int index, IconOfGroup icon)
		{
			(Icons ?? (Icons = new IconOfGroup[256]))[index] = icon;
		}
	}
}
