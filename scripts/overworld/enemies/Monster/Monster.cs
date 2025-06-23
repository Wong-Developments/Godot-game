using Game.Scripts.Overworld.Enemies.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Enemies.Monster;

public partial class Monster : Enemy
{
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
