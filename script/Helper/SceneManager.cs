using System.Diagnostics;
using Godot;


public partial class SceneManager : Node
{
	private PackedScene loadScene;

	public void LoadScene(StringName scenePath)
	{
		loadScene = (PackedScene)ResourceLoader.Load(scenePath);

		if (loadScene != null)
		{
			SceneChange();
		}
		else
		{
			Debug.Print("Error: Scene is not loaded");
		}
	}

	private void SceneChange()
	{
		GetTree().ChangeSceneToPacked(loadScene);
	}
}
