using Game.Overworld.Player.States;
using Godot;
using System;

namespace Game.Overworld.Player;

public partial class Player : CharacterBody2D
{
	[Export] public StateMachine stateMachine;

	[Export] public float speed;

	public override void _Ready()
	{
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));
	}

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate(delta);
		Position.Round();
	}
}
