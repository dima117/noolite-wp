using System.IO.IsolatedStorage;

namespace NooliteSmartHome.Gateway.Settings
{
	public class ApplicationSettings
	{	
		public string Host { get; set; }

		public string User { get; set; }

		public string Password { get; set; }

		private static ApplicationSettings _current;
		private static readonly object lockObject = new object();
		private const string KEY = "12F164FB-8BF4-4E70-8F3B-74E22BC1DE75";
		private static readonly IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

		public static ApplicationSettings Current
		{
			get
			{
				if (_current == null)
				{
					lock (lockObject)
					{
						if (_current == null)
						{
							if (settings.Contains(KEY))
							{
								_current = (settings[KEY] as ApplicationSettings) 
									?? new ApplicationSettings();
							}

						}
					}
				}

				return _current;
			}
		}

		public static void SaveCurrentSettings()
		{
			lock (lockObject)
			{
				settings[KEY] = Current;
				settings.Save();
			}
		}
	}
}
