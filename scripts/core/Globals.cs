using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Scripts.Core;

public partial class Globals : Node
{
	public static Globals Instance { get; private set; }
	
	[ExportCategory("Gameplay")]

	[Export] public int GRID_SIZE = 16;
	
	public List<PackedScene> AllCards { get; private set; } = new()
    {
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/SheildCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/BuffCard.tscn"),

        GD.Load<PackedScene>("res://Scenes/Combat/Cards/bashCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/croixCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/enflameCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/saltBlastCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/doubleSwingCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/hammerSpinCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/counterSmashCard.tscn"),
    };

	public override void _Ready()
	{
		Instance = this;
		Logger.Info("Loading Globals");
	}
}