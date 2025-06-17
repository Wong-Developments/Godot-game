using Game.Core;
using Game.Overworld.Enemies.States;
using Godot;
using System;

namespace Game.Overworld.Enemies.Monster;

public partial class MonsterAnimation : AnimatedSprite2D
{
	[Export] public FreeRoam state;

	[ExportCategory("Animation Vars")]
	[Export] ECharacterAnimation ECharacterAnimation = ECharacterAnimation.idle_down;

	public override void _Ready()
	{
		Logger.Info("Loading player animation component...");
		state.Animation += PlayAnimation;
	}

	public void PlayAnimation(string animationType)
	{
		ECharacterAnimation previousAnimation = ECharacterAnimation;

		switch (animationType)
		{
			case "walk":
				if (state.direction == Vector2.Up) ECharacterAnimation = ECharacterAnimation.walk_up;
				else if (state.direction == Vector2.Down) ECharacterAnimation = ECharacterAnimation.walk_down;
				else if (state.direction == Vector2.Left) ECharacterAnimation = ECharacterAnimation.walk_left;
				else if (state.direction == Vector2.Right) ECharacterAnimation = ECharacterAnimation.walk_right;
				break;
			case "idle":
				if (ECharacterAnimation == ECharacterAnimation.walk_up) ECharacterAnimation = ECharacterAnimation.idle_up;
				else if (ECharacterAnimation == ECharacterAnimation.walk_down) ECharacterAnimation = ECharacterAnimation.idle_down;
				else if (ECharacterAnimation == ECharacterAnimation.walk_left) ECharacterAnimation = ECharacterAnimation.idle_left;
				else if (ECharacterAnimation == ECharacterAnimation.walk_right) ECharacterAnimation = ECharacterAnimation.idle_right;
				break;
		}

		if (previousAnimation != ECharacterAnimation)
		{
			Logger.Info($"Playing animation {ECharacterAnimation.ToString()}");
			Play(ECharacterAnimation.ToString());
		}

	}

}
