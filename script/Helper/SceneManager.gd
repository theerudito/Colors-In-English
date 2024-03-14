extends Node

var one = load("res://scene/JSON-Manager.tscn")
var two = load("res://scene/main.tscn")
#@export_file("*.tscn") var sceneOne;
#@export_file("*.tscn") var sceneTwo;

func sceneNext(sceneName):
	var loadedScene = load(sceneName)
	if loadedScene != null:
		get_tree().change_scene(loadedScene)
	else:
		print("Error: Scene is not loaded")
		# get_tree().change_scene_to_file(sceneTwo)

func sceneBack():
	get_tree().change_scene_to_packed(two)