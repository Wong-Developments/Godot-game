using Game.Scripts.Overworld.States;
using Game.Scripts.Core;
using Godot;
using System;
using Godot.Collections;

namespace Game.Scripts.Overworld.Enemies.Monster;
public partial class MonsterAnimation : EntityAnimation
{
    public override void _Ready()
	{
		Logger.Info("Loading player animation component...");
		state.Animation += PlayAnimation;
	}

    public override void PlayAnimation(string animationType)
    {
        ECharacterAnimation previousAnimation = currentAnimation;

        if (state.direction != Vector2.Zero)
            lastDirection = state.direction;

        Vector2 dir = animationType == "walk" ? state.direction : lastDirection;

        switch (animationType)
        {
            case "walk":
                if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))                
                    currentAnimation = dir.X > 0 ? ECharacterAnimation.walk_right : ECharacterAnimation.walk_left;                
                else                
                    currentAnimation = dir.Y > 0 ? ECharacterAnimation.walk_down : ECharacterAnimation.walk_up;                
                break;

            case "idle":
                if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))                
                    currentAnimation = dir.X > 0 ? ECharacterAnimation.idle_right : ECharacterAnimation.idle_left;                
                else                
                    currentAnimation = dir.Y > 0 ? ECharacterAnimation.idle_down : ECharacterAnimation.idle_up;                
                break;
        }

        if (previousAnimation != currentAnimation)
        {
            Logger.Info($"Playing animation {currentAnimation}");
            Play(currentAnimation.ToString());
        }
    }
}
