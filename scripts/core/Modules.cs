using Godot;

namespace Game.Core;

public static class Module
{
    public static bool IsActionJustPressed() => Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_right");

    public static bool IsActionPressed() => Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") || Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right");

    public static bool IsActionJustReleased() => Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") || Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right");
}