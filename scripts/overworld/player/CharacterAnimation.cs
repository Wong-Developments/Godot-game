using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Player;

public partial class CharacterAnimation : EntityAnimation
{
    public override void _Ready()
	{
		Logger.Info("Loading player animation component...");
		state.Animation += PlayAnimation;
	}

    /*public override void PlayAnimation(string animationType)
    {
        ECharacterAnimation previousAnimation = currentAnimation;

        switch (animationType)
        {
            case "walk":
                if (state.direction == Vector2.Up) currentAnimation = ECharacterAnimation.walk_up;
                else if (state.direction == Vector2.Down) currentAnimation = ECharacterAnimation.walk_down;
                else if (state.direction == Vector2.Left) currentAnimation = ECharacterAnimation.walk_left;
                else if (state.direction == Vector2.Right) currentAnimation = ECharacterAnimation.walk_right;
                break;
            case "idle":
                if (currentAnimation == ECharacterAnimation.walk_up) currentAnimation = ECharacterAnimation.idle_up;
                else if (currentAnimation == ECharacterAnimation.walk_down) currentAnimation = ECharacterAnimation.idle_down;
                else if (currentAnimation == ECharacterAnimation.walk_left) currentAnimation = ECharacterAnimation.idle_left;
                else if (currentAnimation == ECharacterAnimation.walk_right) currentAnimation = ECharacterAnimation.idle_right;
                break;
        }

        if (previousAnimation != currentAnimation)
        {
            //Logger.Info($"Playing animation {currentAnimation}");
            Play(currentAnimation.ToString());
        }
    }*/
}
