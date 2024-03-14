using Godot;


public partial class SceneManager : Node
{
	public void LoadScene(string scenePath)
	{
		PackedScene sceneToLoad = (PackedScene)ResourceLoader.Load($"scene/{scenePath}.tscn");
		var newSceneInstance = sceneToLoad.Instantiate();
		GetTree().Root.AddChild(newSceneInstance);
	}
}
