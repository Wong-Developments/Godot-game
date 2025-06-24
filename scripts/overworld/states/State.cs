using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Overworld.States;

public abstract partial class State : Node
{
    [Export] public StateMachine stateMachine;
    [Signal] public delegate void AnimationEventHandler(string animationName);
    public Vector2 direction;

    public override void _Ready()
    {
        if (stateMachine is null)
            Logger.Error("Assigned stateMachine is not of type State.");
    }
    public virtual void EnterState()
    {
        Logger.Info($"Entering {GetType().Name} state...");
    }

    public virtual void ExitState()
    {
        Logger.Info($"Exiting {GetType().Name} state...");
    }

    public virtual void PhysicsUpdate(float delta)
    {
        EmitSignal(SignalName.Animation, direction != Vector2.Zero ? "walk" : "idle");
    }
}