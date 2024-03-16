using System.Diagnostics;
using Godot;

public partial class CurrentScene : Node
{
	public string sceneName = "";

	[Signal]
	public delegate void MySceneEventHandler();

	public void LoadScene(string name)
	{
		sceneName = name;
	}

	public void ChangeScene()
	{
		var currentScene = GetNode<SceneManager>("/root/SceneManager");
		currentScene.LoadScene(sceneName);
	}
}
