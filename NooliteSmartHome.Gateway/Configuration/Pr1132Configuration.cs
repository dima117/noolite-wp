using System.IO;
using System.Linq;
using NooliteSmartHome.Gateway.Encodings;

namespace NooliteSmartHome.Gateway.Configuration
{
	public class Pr1132Configuration
	{
		public Pr1132Configuration()
		{
			Groups = new Pr1132ControlGroup[16];
			Channels = new Pr1132Channel[32];
			Timers = new Pr1132Timer[7];
		}

		public Pr1132ControlGroup[] Groups { get; private set; }

		public Pr1132Channel[] Channels { get; private set; }

		public Pr1132Timer[] Timers { get; set; }

		public bool AutoUpdateTime { get; set; }

		public bool UseDst { get; set; }

		public int TimeOffset { get; set; }

		public static Pr1132Configuration Deserialize(Stream file)
		{
			if (!Validate(file))
			{
				return null;
			}

			var cfg = new Pr1132Configuration();

			for (int i = 0; i < 16; i++)
			{
				var buf = new byte[32];
				file.Read(buf, 0, 32);
				cfg.Groups[i] = ParseGroup(buf);
			}

			for (int i = 0; i < 32; i++)
			{
				var buf = new byte[25];
				file.Read(buf, 0, 25);
				cfg.Channels[i] = ParseChannel(buf);
			}

			ParseTimeSettings(file.ReadByte(), ref cfg);

			for (int i = 0; i < 7; i++)
			{
				var buf = new byte[7];
				file.Read(buf, 0, 7);
				cfg.Timers[i] = ParseTimer(buf);
			}

			return cfg;
		}

		private static bool Validate(Stream stream)
		{
			if (stream.Length != 4102)
			{
				return false;
			}

			var buf = new byte[6];
			stream.Read(buf, 0, 6);

			if (Windows1251Encoding.Instance.GetString(buf, 0, 6).ToLower() != "pr1132")
			{
				return false;
			}

			return true;
		}

		private static Pr1132Timer ParseTimer(byte[] buf)
		{
			var timer = new Pr1132Timer
			{
				Enabled = buf[0] != 0,
				RunOnce = buf[1] != 0,
				Hours = buf[2],
				Minutes = buf[3],
				Channel = buf[5],
				Command = (Pr1132TimerCommad) buf[5]
			};

			var days = buf[4];

			for (int d = 0; d < 7; d++)
			{
				timer.Days[d] = ((days >> (d + 1)) & 1) > 0;
			}

			return timer;
		}

		private static void ParseTimeSettings(int buf, ref Pr1132Configuration cfg)
		{
			if (buf != -1)
			{
				var timeSettings = (byte) buf;

				cfg.TimeOffset = (timeSettings & 63) - 11;
				cfg.UseDst = (timeSettings & 64) > 0;
				cfg.AutoUpdateTime = (timeSettings & 128) > 0;
			}
		}

		private static Pr1132ControlGroup ParseGroup(byte[] buf)
		{
			var group = new Pr1132ControlGroup();

			var nameBytes = buf.Take(24).ToArray();
			group.Name = Windows1251Encoding.Instance.GetString(nameBytes, 0, 24).TrimEnd((char)0);
			group.Enabled = buf[24] < 64;

			for (int j = 0; j < 4; j++)
			{
				group.Sensors[j] = buf[25 + j] >= 64;
			}

			for (int j = 0; j < 8; j++)
			{
				var channel = buf[24 + j] & 63;
				group.ChannelNumbers[j] = channel == 0 ? (int?)null : channel - 1;
			}

			return group;
		}

		private static Pr1132Channel ParseChannel(byte[] buf)
		{
			var channel = new Pr1132Channel();

			var nameBytes = buf.Take(24).ToArray();
			channel.Name = Windows1251Encoding.Instance.GetString(nameBytes, 0, 24).TrimEnd((char)0);

			channel.Type = (Pr1132ChannelUiType)buf[24];

			return channel;
		}

	}
}
