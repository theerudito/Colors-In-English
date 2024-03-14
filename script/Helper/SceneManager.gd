extends Node

var one = load("res://scene/JSON-Manager.tscn")
var two = load("res://scene/main.tscn")
#@export_file("*.tscn") var sceneOne;
#@export_file("*.tscn") var sceneTwo;

var scene: PackedScene

func loadScene(scenePath: String) -> void:
    scene = load(scenePath)
    if scene != null:
        sceneChange()
    else:
        print("Error: Scene is not loaded")

func sceneChange() -> void:
    get_tree().change_scene_to_packed(scene)
		#get_tree().change_scene_to_file(sceneOne)