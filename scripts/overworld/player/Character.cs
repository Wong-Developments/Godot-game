using Game.Scripts.Overworld.States;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Scripts.Overworld.Player;

public partial class Character : Entity
{
    public List<PackedScene> AvailableCards { get; private set; } = new()
    {
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/DamageCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/HealCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/SheildCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/BurnCard.tscn"),
        GD.Load<PackedScene>("res://Scenes/Combat/Cards/BuffCard.tscn")
    };

    public override void _Ready()
	{
        var gm = GetNode<GameManager>("/root/GameManager");
        gm.PlayerRef = this; // Set the global player reference

        AddToGroup("player");
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));
	}

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate((float)delta);
		Position.Round();
	}
}
