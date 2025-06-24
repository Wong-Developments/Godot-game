using Godot;
using Godot.Collections;

namespace Game.Scripts.Core;

public enum LogLevel
{
    DEBUG,
    INFO,
    WARNING,
    ERROR
}

public enum ECharacterAnimation
{
    idle_down,
    idle_up,
    idle_left,
    idle_right,
    turn_down,
    turn_up,
    turn_left,
    turn_right,
    walk_down,
    walk_up,
    walk_left,
    walk_right,
}
