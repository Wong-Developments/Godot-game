using Godot;
using System;

using Game.Scripts.Overworld.Player;
using Game.Scripts.Core;

namespace Game.Scripts.Overworld;

public partial class GameManager : Node
{
    public Character PlayerRef { get; set; }

    private Node currentScene;

    public override void _Ready()
    {
        //DebugUtils.ShowNavigation(true);
        DebugUtils.ShowCollisions(true);

        currentScene = GetNode("OverworldManager"); // Overworld scene (assigned at start)
    }


    public void SwitchToCombat(Character player)
    {
        PlayerRef = player;

        currentScene?.QueueFree(); // Remove the current (overworld) scene

        // Load combat scene as a child
        var combatScene = GD.Load<PackedScene>("res://scenes/core/combat_manager.tscn");
        currentScene = combatScene.Instantiate(); // set the current scene to combat
        AddChild(currentScene);
    }

    public void SwitchToOverworld()
    {
        currentScene?.QueueFree();

        var overworldScene = GD.Load<PackedScene>("res://scenes/core/overworld_manager.tscn"); // adjust path
        currentScene = overworldScene.Instantiate();
        AddChild(currentScene);
    }
}

