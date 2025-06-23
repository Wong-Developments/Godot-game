using Game.Scripts.Core;
using Game.Scripts.Overworld.Enemies;
using Game.Scripts.Overworld.Player;
using Godot;
using System;

namespace Game.Scripts.Overworld.States;

public partial class StateMachine : Node
{
	[ExportCategory("State Machine Vars")]

	[Export] public Entity owner;

    [Export] public State currentState;
    public override void _Ready()
    {
        if (owner is null)
            Logger.Error("Assigned owner is not of type State.");

        if (currentState is null)
            Logger.Error("Assigned currentState is not of type State.");
    }

	public void ChangeState(State newState)
	{
		currentState?.ExitState();
		currentState = newState;
		currentState?.EnterState();

		foreach (Node child in GetChildren())
			if (child is State state)
				state?.SetProcess(child == currentState);
	}

	public void PhysicsUpdate(float delta) => currentState?.PhysicsUpdate(delta);
    public string GetCurrentState() => currentState?.Name?.ToString();
}
