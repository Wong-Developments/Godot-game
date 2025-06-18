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

		float angle = state.direction.Normalized().Angle();

		switch (animationType)
		{
			case "walk":
				if (Mathf.Abs(state.direction.X) > Mathf.Abs(state.direction.Y))
				{
					if (state.direction.X > 0)
					{
						ECharacterAnimation = ECharacterAnimation.walk_right;
					}
					else
					{
						ECharacterAnimation = ECharacterAnimation.walk_left;
					}
				}
				else
				{
					if (state.direction.Y > 0)
					{
						ECharacterAnimation = ECharacterAnimation.walk_down;
					}
					else
					{
						ECharacterAnimation = ECharacterAnimation.walk_up;
					}
				}
				break;
			case "idle":
				if (Mathf.Abs(state.direction.X) > Mathf.Abs(state.direction.Y))
				{
					if (state.direction.X > 0)
					{
						ECharacterAnimation = ECharacterAnimation.idle_right;
					}
					else
					{
						ECharacterAnimation = ECharacterAnimation.idle_left;
					}
				}
				else
				{
					if (state.direction.Y > 0)
					{
						ECharacterAnimation = ECharacterAnimation.idle_down;
					}
					else
					{
						ECharacterAnimation = ECharacterAnimation.idle_up;
					}
				}
				break;
		}

		if (previousAnimation != ECharacterAnimation)
		{
			Logger.Info($"Playing animation {ECharacterAnimation.ToString()}");
			Play(ECharacterAnimation.ToString());
		}

	}

}
