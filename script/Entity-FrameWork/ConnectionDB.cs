using Godot;
using System.Diagnostics;
using System.IO;
public partial class ConnectionDB : Node
{
	private static string nameDatabase = "Naruto-SQLITE.db";
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
				Debug.Print("File exists");
			}
			pathDatabase = Path.Combine(path, nameDatabase);
			Debug.Print("Path: ", pathDatabase + " Android");
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
				Debug.Print("File exists");
			}

			pathDatabase = Path.Combine(path, nameDatabase);
			Debug.Print("Path: ", pathDatabase + " Windows");
		}
		return pathDatabase;
	}
}
