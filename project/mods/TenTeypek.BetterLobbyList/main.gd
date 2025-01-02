extends Node

enum LOBBY_DISTANCE{CLOSE, NORMAL, FAR, WORLDWIDE}
const distance_filter_button = preload("res://mods/TenTeypek.BetterLobbyList/Scenes/distance_filter.tscn")
var lobby_search_distance = LOBBY_DISTANCE.WORLDWIDE
var distance_filter: Button

func _ready():
	print("BetterLobbyList (main.gd) loaded")
	get_tree().connect("node_added", self, "_on_node_added")

func _on_node_added(node: Node):
	if node.name == "filters" and node is VBoxContainer:
		distance_filter = distance_filter_button.instance()
		node.add_child(distance_filter)
		distance_filter.connect("pressed", self, "_on_distance_filter_pressed")
		Network.distance = lobby_search_distance
		_update_label()

func _on_distance_filter_pressed():
	lobby_search_distance += 1
	if lobby_search_distance >= LOBBY_DISTANCE.size():
		lobby_search_distance = 0
	Network.distance = lobby_search_distance
	_update_label()

func _update_label():
	match lobby_search_distance:
		LOBBY_DISTANCE.CLOSE: distance_filter.text = "Close"
		LOBBY_DISTANCE.NORMAL: distance_filter.text = "Normal"
		LOBBY_DISTANCE.FAR: distance_filter.text = "Far"
		LOBBY_DISTANCE.WORLDWIDE: distance_filter.text = "Worldwide"
