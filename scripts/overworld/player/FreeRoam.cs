using Game.Scripts.Overworld.Player;
using Game.Scripts.Core;
using Godot;
using System;
using Game.Scripts.Overworld.States;

namespace Game.Scripts.Overworld.Player;

public partial class FreeRoam : State
{
	[Signal] public delegate void AnimationEventHandler(string animationName);

	public Vector2 direction;
	public Vector2 velocity;
	public override void _Ready()
	{
        if (stateMachine == null)
            Logger.Debug("stateMachine is null");

        if (stateMachine?.owner == null)
            Logger.Debug("stateMachine owner is null");

        if (stateMachine?.owner?.speed == null)
            Logger.Debug("stateMachine owner speed is null");
    }

	public override void PhysicsUpdate(float delta)
	{
        if (direction != Vector2.Zero)
            EmitSignal(SignalName.Animation, "walk");
        else
            EmitSignal(SignalName.Animation, "idle");

        float speed = stateMachine.owner.speed;

        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        stateMachine.owner.Velocity.Normalized();
        stateMachine.owner.Velocity = direction * speed;
        stateMachine.owner.Velocity.Round();

        stateMachine.owner.MoveAndSlide();
    }
}
