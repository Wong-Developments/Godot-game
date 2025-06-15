using Godot;
using System;

namespace Game.Utilities;

public partial class StateMachine : Node
{
	[ExportCategory("State Machine Vars")]
	[Export] public Node customer;
	[Export] public State currentState;
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child is State state)
			{
				state.Owner = customer;
				state.SetProcess(false);
			}
		}
	}

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
}
