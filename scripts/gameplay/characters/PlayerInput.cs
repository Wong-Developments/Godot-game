using Game.Core;
using Godot;
using System;


namespace Game.Gameplay
{
		public partial class PlayerInput : CharacterInput
	{
		[ExportCategory("Player Input")]

		[Export] public double HoldThreshhold = 0.1f;
		[Export] public double HoldTime = 0.0f;
		public override void _Ready()
		{
			Logger.Info("Loading player input component...");
		}

	}

}
