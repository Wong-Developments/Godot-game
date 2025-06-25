using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Enemies.Monster;

public partial class Monster : Enemy
{
    [ExportCategory("Patrol Settings")]
    [Export] public float patrolMultiplier = 1f;
    [Export] public Vector2 walkDurationRange = new(1.0f, 2.0f);
    [Export] public Vector2 waitTimeRange = new(0.5f, 5.0f);

    public override void _Ready()
	{
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));
	}

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate((float)delta);
		Position.Round();
	}
}
