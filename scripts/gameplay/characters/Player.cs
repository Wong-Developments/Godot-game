using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay;

public partial class Player : CharacterBody2D
{
	[Export] public StateMachine StateMachine;

	public override void _Ready()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("FreeRoam"));
	}

}
