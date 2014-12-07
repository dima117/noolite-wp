using NooliteSmartHome.Gateway;

namespace NooliteSmartHome.Helpers
{
	public class ApplicationSettings
	{
		public string Host { get; set; }

		public AuthInfo AuthInfo { get; set; }

		public Pr1132Gateway CreateGateway()
		{
			return AuthInfo == null
				? new Pr1132Gateway(Host)
				: new Pr1132Gateway(Host, AuthInfo.User, AuthInfo.Password);
		}
	}
}
