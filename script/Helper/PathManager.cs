using Godot;
using System.IO;

public partial class PathManager : Node
{
	public static string GetPath(string nameFile)
	{
		var path = ProjectSettings.GlobalizePath(OS.GetUserDataDir());
		return Path.Combine(path, nameFile);
	}
}
