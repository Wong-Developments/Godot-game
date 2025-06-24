using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Utils;

public static class AnimationExtensions
{
    private static readonly Dictionary<ECharacterAnimation, Vector2> Directions = new()
    {
        { ECharacterAnimation.walk_up,    new(0, -1) },
        { ECharacterAnimation.walk_down,  new(0, 1) },
        { ECharacterAnimation.walk_left,  new(-1, 0) },
        { ECharacterAnimation.walk_right, new(1, 0) },

        { ECharacterAnimation.idle_up,    new(0, -1) },
        { ECharacterAnimation.idle_down,  new(0, 1) },
        { ECharacterAnimation.idle_left,  new(-1, 0) },
        { ECharacterAnimation.idle_right, new(1, 0) },

        { ECharacterAnimation.turn_up,    new(0, -1) },
        { ECharacterAnimation.turn_down,  new(0, 1) },
        { ECharacterAnimation.turn_left,  new(-1, 0) },
        { ECharacterAnimation.turn_right, new(1, 0) },
    };

    public static ECharacterAnimation GetAnimation(this Vector2 direction, bool isWalking = true)
    {
        direction = direction.Round(); // Ensure values like (0.00001, -1) snap to (0, -1)

        foreach (var pair in Directions)
        {
            if (pair.Value == direction)
            {
                if (isWalking && pair.Key.IsWalk())
                    return pair.Key;
                if (!isWalking && pair.Key.IsIdle())
                    return pair.Key;
            }
        }

        return isWalking ? ECharacterAnimation.walk_down : ECharacterAnimation.idle_down;
    }

    public static Vector2 Direction(this ECharacterAnimation animation) => Directions.TryGetValue(animation, out var dir) ? dir : Vector2.Zero;
    public static bool IsIdle(this ECharacterAnimation animation) => animation.ToString().StartsWith("idle");
    public static bool IsWalk(this ECharacterAnimation animation) => animation.ToString().StartsWith("walk");
    public static bool IsTurn(this ECharacterAnimation animation) => animation.ToString().StartsWith("turn");
}
