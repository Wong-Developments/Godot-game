using Godot;
using System;

namespace Game.Gameplay
{
	public partial class CharacterMovement : Node
	{
		[Signal] public delegate void AnimationEventHandler();
		public override void _Ready()
		{
		}

		
		public override void _Process(double delta)
		{
		}
	}

}
