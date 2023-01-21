namespace DeZ.Settings
{
	static class Settings
	{
		private static ISettings settings = new FileSettings("settings.st");

		public static string Login
		{
			get => settings.GetString("Login");
			set =>settings.SetString("Login", value);
		}
	}
}
