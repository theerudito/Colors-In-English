using System.Diagnostics;
using Godot;

public partial class CurrentScene : Node
{
	private string sceneName = "";

	[Signal]
	public delegate void MySceneEventHandler();

	public string SceneName
	{
		get { return sceneName; }
		set { sceneName = value; }
	}

	public void LoadScene()
	{
		var currentScene = GetNode<SceneManager>("/root/SceneManager");
		
		currentScene.LoadScene("res://scene/JSON-Manager.tscn");
		Debug.Print("CurrentScene: " + sceneName);
	}
}
