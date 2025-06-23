using Game.Scripts.Overworld.Player;
using Game.Scripts.Core;
using Godot;
using System;


namespace Game.Scripts.Overworld.Player.States;

public partial class FreeRoam : State
{
	[Signal] public delegate void AnimationEventHandler(string animationName);

	public Vector2 direction;
	public Vector2 velocity;
	public override void _Ready()
	{
		
	}

	public override void PhysicsUpdate(double delta)
	{
        if (direction != Vector2.Zero)
            EmitSignal(SignalName.Animation, "walk");
        else
            EmitSignal(SignalName.Animation, "idle");

        if (stateMachine == null)
            Logger.Debug("stateMachine is null");

        if (stateMachine?.owner == null)
            Logger.Debug("stateMachine owner is null");

        if (stateMachine?.owner?.speed == null)
            Logger.Debug("stateMachine owner speed is null");


        double? speed = stateMachine?.owner?.speed;

        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        stateMachine.owner.Velocity.Normalized();
        stateMachine.owner.Velocity = direction * (float)speed;
        stateMachine.owner.Velocity.Round();

        stateMachine.owner.MoveAndSlide();
    }
}
