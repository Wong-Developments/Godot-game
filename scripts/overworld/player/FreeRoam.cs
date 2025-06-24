using Game.Scripts.Overworld.Player;
using Game.Scripts.Core;
using Godot;
using System;
using Game.Scripts.Overworld.States;

namespace Game.Scripts.Overworld.Player;

public partial class FreeRoam : State
{
	public override void PhysicsUpdate(float delta)
	{
        EmitSignal(SignalName.Animation, direction != Vector2.Zero ? "walk" : "idle");

        float speed = stateMachine.owner.speed;

        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        stateMachine.owner.Velocity.Normalized();
        stateMachine.owner.Velocity = direction * speed;
        stateMachine.owner.Velocity.Round();

        stateMachine.owner.MoveAndSlide();
    }
}
