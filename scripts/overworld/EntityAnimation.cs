using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Overworld;

public abstract partial class EntityAnimation : AnimatedSprite2D
{
    [Export] public State state;

    [ExportCategory("Animation Vars")]
    [Export] public ECharacterAnimation currentAnimation = ECharacterAnimation.idle_down;

    protected Vector2 lastDirection = new(0, 1);

    public override void _Ready() => state.Animation += PlayAnimation;

    public virtual void PlayAnimation(string animationType)
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
            Play(currentAnimation.ToString());        
    }
}