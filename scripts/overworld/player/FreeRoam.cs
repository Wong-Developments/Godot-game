using Godot;
using System;

public partial class FreeRoam : Node
{
	[Signal] public delegate void AnimationEventHandler(string animationName);
	public Vector2 direction;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
