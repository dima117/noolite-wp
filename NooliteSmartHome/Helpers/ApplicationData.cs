using System.IO;
using System.IO.IsolatedStorage;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Helpers
{
	public static class ApplicationData
	{
		#region configuration

		private const string NOOLITE_SETTINGS_FILENAME = "noolite_settings.bin";

		private static Pr1132Configuration config;

		public static Pr1132Configuration SaveConfiguration(byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				var cfg = Pr1132Configuration.Deserialize(stream);

				if (cfg != null)
				{
					config = cfg;

					using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
					{
						using (var file = isf.OpenFile(NOOLITE_SETTINGS_FILENAME, FileMode.OpenOrCreate))
						{
							file.Write(bytes, 0, bytes.Length);
						}
					}
				}

				return cfg;
			}
		}

		public static Pr1132Configuration ReloadConfiguration()
		{
			using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!isf.FileExists(NOOLITE_SETTINGS_FILENAME))
				{
					return null;
				}

				using (var stream = isf.OpenFile(NOOLITE_SETTINGS_FILENAME, FileMode.Open))
				{
					return config = Pr1132Configuration.Deserialize(stream);
				}
			}
		}

		public static Pr1132Configuration GetConfiguration()
		{
			return config ?? ReloadConfiguration();
		}
		
		public static Pr1132Configuration ClearCachedConfiguration()
		{
			return config = null;
		}

		#endregion

		#region settings

		private static ApplicationSettings current;
		private const string KEY = "12F164FB-8BF4-4E70-8F3B-74E22BC1DE75";
		private static readonly IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

		public static ApplicationSettings Settings
		{
			get
			{
				if (current == null)
				{
					if (!settings.Contains(KEY))
					{
						settings[KEY] = new ApplicationSettings();
					}

					current = (settings[KEY] as ApplicationSettings)
							  ?? new ApplicationSettings();
				}

				return current;
			}
		}

		public static void SaveCurrentSettings()
		{
			settings[KEY] = Settings;
			settings.Save();
		}

		#endregion
	}
}
