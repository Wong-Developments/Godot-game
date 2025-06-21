using Game.Scripts.Core;
using Godot;

namespace Game.Overworld.Player.States;

public abstract partial class State : Node
{
    [Export] public StateMachine stateMachine;

    public virtual void EnterState()
    {
        Logger.Info($"Entering {GetType().Name} state...");
    }

    public virtual void ExitState()
    {
        Logger.Info($"Exiting {GetType().Name} state...");
    }

    public virtual void PhysicsUpdate(double delta)
    {

    }
}