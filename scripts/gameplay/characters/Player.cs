using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay;

public partial class Player : CharacterBody2D
{
	[Export] public StateMachine StateMachine;

	[Export] public double MoveSpeed = 0;

    public override void _Ready()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("FreeRoam"));
	}

}
