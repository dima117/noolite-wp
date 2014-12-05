namespace NooliteSmartHome.Gateway
{
	public enum GatewayLedCommand : byte
	{
		SetLevel = 0x06,
		Stop = 0x0a,			// 10
		Start = 0x10,			// 16
		ChangeColor = 0x11,		// 17
		SetColorMode = 0x12,	// 18
		SetColorSpeed = 0x13	// 19
	}
}