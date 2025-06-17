using Game.Overworld.Player;
using Godot;
using System;


namespace Game.Overworld.Player.States;

public partial class FreeRoam : State
{
	[Signal] public delegate void AnimationEventHandler(string animationName);


	public Vector2 direction;
	public Vector2 velocity;
	public override void _Ready()
	{
		
	}

	public void GetInput(double delta)
	{
		if (direction != Vector2.Zero)
		{
			EmitSignal(SignalName.Animation, "walk");
		}
		else
		{
			EmitSignal(SignalName.Animation, "idle");
		}

		double speed = stateMachine.owner.speed;

		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		stateMachine.owner.Velocity.Normalized();
		stateMachine.owner.Velocity = direction * (float)speed;
		stateMachine.owner.Velocity.Round();

		stateMachine.owner.MoveAndSlide();
	}

	public override void PhysicsUpdate(double delta)
	{
		GetInput(delta);
	}
	


}
