using Game.Core;
using Godot;
using System;

namespace Game.Overworld.Player;

public partial class CharacterAnimation : AnimatedSprite2D
{

	[ExportCategory("Nodes")]
	[Export] public FreeRoam freeRoam;

	[ExportCategory("Animation Vars")]
	[Export] ECharacterAnimation ECharacterAnimation = ECharacterAnimation.idle_down;

	public override void _Ready()
	{
		Logger.Info("Loading player animation component...");
		freeRoam.Animation += PlayAnimation;
	}

	public void PlayAnimation(string animationType)
	{
		ECharacterAnimation previousAnimation = ECharacterAnimation;

		switch (animationType)
		{
			case "walk":
				if (freeRoam.direction == Vector2.Up) ECharacterAnimation = ECharacterAnimation.walk_up;
				else if (freeRoam.direction == Vector2.Down) ECharacterAnimation = ECharacterAnimation.walk_down;
				else if (freeRoam.direction == Vector2.Left) ECharacterAnimation = ECharacterAnimation.walk_left;
				else if (freeRoam.direction == Vector2.Right) ECharacterAnimation = ECharacterAnimation.walk_right;
				break;
			case "turn":
				if (freeRoam.direction == Vector2.Up) ECharacterAnimation = ECharacterAnimation.turn_up;
				else if (freeRoam.direction == Vector2.Down) ECharacterAnimation = ECharacterAnimation.turn_down;
				else if (freeRoam.direction == Vector2.Left) ECharacterAnimation = ECharacterAnimation.turn_left;
				else if (freeRoam.direction == Vector2.Right) ECharacterAnimation = ECharacterAnimation.turn_right;
				break;
			case "idle":
				if (freeRoam.direction == Vector2.Up) ECharacterAnimation = ECharacterAnimation.idle_up;
				else if (freeRoam.direction == Vector2.Down) ECharacterAnimation = ECharacterAnimation.idle_down;
				else if (freeRoam.direction == Vector2.Left) ECharacterAnimation = ECharacterAnimation.idle_left;
				else if (freeRoam.direction == Vector2.Right) ECharacterAnimation = ECharacterAnimation.idle_right;
				break;
		}

		if (previousAnimation != ECharacterAnimation)
		{
			Logger.Info($"Playing animation {ECharacterAnimation.ToString()}");
			Play(ECharacterAnimation.ToString());
		}

	}

}
