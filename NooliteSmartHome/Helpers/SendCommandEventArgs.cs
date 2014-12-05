using NooliteSmartHome.Gateway;

namespace NooliteSmartHome.Helpers
{
	public class SendCommandEventArgs
	{
		public GatewayCommand command;
		public byte channel;
		public byte brightness;
	}
}