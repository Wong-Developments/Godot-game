using Godot;
using System;

namespace Game.Overworld.Enemies.States;

public partial class StateMachine : Node
{
	[ExportCategory("State Machine Vars")]

	[Export] public Enemy owner;

	[Export] public State currentState;

	public string GetCurrentState() => currentState.Name.ToString();

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
		currentState.PhysicsUpdate(delta);
	}
}
