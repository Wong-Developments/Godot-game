using Game.Scripts.Overworld.Player.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Player;

public partial class Character : CharacterBody2D
{
	[Export] public StateMachine stateMachine;

	[Export] public float speed;
	public override void _Ready()
	{
		AddToGroup("player");
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));
	}

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate(delta);
		Position.Round();
	}
}
