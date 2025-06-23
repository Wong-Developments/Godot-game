using Game.Scripts.Overworld.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Player;

public partial class Character : Entity
{
	public override void _Ready()
	{
		AddToGroup("player");
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));
	}

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate((float)delta);
		Position.Round();
	}
}
