namespace NooliteSmartHome.Gateway.Configuration
{
	public class Pr1132Timer
	{
		public Pr1132Timer()
		{
			Days = new bool[7];
		}

		public bool Enabled { get; set; }
		public bool RunOnce { get; set; }
		public int Hours { get; set; }
		public int Minutes { get; set; }
		public int Channel { get; set; }
		public bool[] Days { get; set; }
		public Pr1132TimerCommad Command { get; set; }
	}
}