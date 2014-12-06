namespace NooliteSmartHome.Gateway
{
    /// <summary>
    /// Available commands for device
    /// </summary>
	public enum GatewayCommand : byte
    {
		On = 0x02,
		Off = 0x00,
        Switch = 0x04,
        SetLevel = 0x06,

		LoadState = 0x07,
		SaveState = 0x08,

        Bind = 0x0f,
        UnBind = 0x09,

		LedStop = 0x0a,				// 10
		LedStart = 0x10,			// 16
		LedChangeColor = 0x11,		// 17
		LedSetColorMode = 0x12,		// 18
		LedSetColorSpeed = 0x13		// 19
    }
}