
using Godot;

public static class LocalStorage
{
	private const string FileName = "user://settings.cfg";

	public static void SaveData(string key, string value)
	{
		var config = new ConfigFile();
		config.Load(FileName);
		config.SetValue("settings", key, value);
		config.Save(FileName);
	}

	public static string LoadData(string key)
	{
		var config = new ConfigFile();
		config.Load(FileName);
		return config.GetValue("settings", key, "").ToString();
	}

	public static int LoadDataInt(string key)
	{
		var config = new ConfigFile();
		config.Load(FileName);
		return (int)config.GetValue("settings", key, 0);
	}

}
