using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Overworld.Player.States;

public partial class StateMachine : Node
{
	[ExportCategory("State Machine Vars")]

	[Export] public Character owner;

	[Export] public State currentState;

    public override void _Ready()
    {
        if (currentState is null)
            Logger.Error("Assigned currentState is not of type State.");
    }

    public string GetCurrentState() => currentState?.Name?.ToString();

	public void ChangeState(State newState)
	{
		currentState?.ExitState();
		currentState = newState;
		currentState?.EnterState();

		foreach (Node child in GetChildren())
			if (child is State state)
				state.SetProcess(child == currentState);
	}

	public void PhysicsUpdate(double delta)
	{
		currentState?.PhysicsUpdate(delta);
	}
}
