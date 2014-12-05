using System.IO;
using System.IO.IsolatedStorage;
using NooliteSmartHome.Gateway.Configuration;

namespace NooliteSmartHome.Helpers
{
	public static class ApplicationDataLoader
	{
		private const string NOOLITE_SETTINGS_FILENAME = "noolite_settings.bin";

		private static Pr1132Configuration config;

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
	}
}
