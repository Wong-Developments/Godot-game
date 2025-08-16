using Godot;
using System;
using System.Threading.Tasks;

public partial class RoomInstance : Node2D
{
    [Signal]
    public delegate void DoorEnteredEventHandler(string direction);
    private bool doorsEnabled = false;
    public override void _Ready()
    {
        ConnectDoor("NorthDoor", "north");
        ConnectDoor("SouthDoor", "south");
        ConnectDoor("EastDoor", "east");
        ConnectDoor("WestDoor", "west");
    }

    /*
        Connects each door to the trigger nodes on the scene
     */
    private void ConnectDoor(string nodeName, string direction)
    {
        var door = GetNodeOrNull<Node2D>(nodeName);
        if (door == null) return;

        var trigger = door.GetNodeOrNull<Area2D>("Trigger");
        if (trigger == null) return;

        trigger.BodyEntered += (Node2D body) =>
        {
            if (!doorsEnabled) return;
            if (body.IsInGroup("Player"))
            {
                GD.Print($"[RoomInstance] Door {direction} entered");
                EmitSignal(SignalName.DoorEntered, direction);
            }
        };
    }
    
    public void SetDoorTriggersEnabled(bool enabled)
    {
        doorsEnabled = enabled;
    }


    public async Task EnableDoorsAfterDelay(float seconds)
    {
        doorsEnabled = false;
        await ToSignal(GetTree().CreateTimer(seconds), "timeout");
        doorsEnabled = true;
    }
}
