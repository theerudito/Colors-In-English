using Godot;
using System.IO;
using System.Xml.Serialization;

public partial class ConnectionDB : Node
{
	private static string nameDatabase = "Game.db";
	public static string GetConnection()
	{
		var pathDatabase = "";

		if (OS.GetName() == "Android")
		{
			var path = ProjectSettings.GlobalizePath(OS.GetUserDataDir());
			using var dir = DirAccess.Open(path);
			if (!File.Exists(Path.Combine(path, nameDatabase)))
			{
				File.Create(Path.Combine(path, nameDatabase)).Close();
			}
			else
			{
				GD.Print("File exists");
			}
			pathDatabase = Path.Combine(path, nameDatabase);
			GD.Print("Path: ", pathDatabase + " Android");
		}
		else
		{
			var path = ProjectSettings.GlobalizePath(OS.GetUserDataDir());
			using var dir = DirAccess.Open(path);
			if (!File.Exists(Path.Combine(path, nameDatabase)))
			{
				File.Create(Path.Combine(path, nameDatabase)).Close();
			}
			else
			{
				GD.Print("File exists");
			}

			pathDatabase = Path.Combine(path, nameDatabase);
			GD.Print("Path: ", pathDatabase + " Windows");
		}
		return pathDatabase;
	}


	// public static string GetConnection()
	// {
	// 	var pathDatabase = "";

	// 	if (OS.GetName() == "Android")
	// 	{
	// 		pathDatabase = Path.Combine(OS.GetUserDataDir(), nameDatabase);
	// 		GD.Print("Path: ", pathDatabase + " Android");
	// 	}
	// 	else
	// 	{
	// 		pathDatabase = Path.Combine(OS.GetUserDataDir(), nameDatabase);
	// 		GD.Print("Path: ", pathDatabase + " Windows");
	// 	}
	// 	return ProjectSettings.LocalizePath(pathDatabase);
	// }
}
