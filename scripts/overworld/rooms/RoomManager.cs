using Godot;
using System;

public partial class RoomManager : Node2D
{
    const int ROWS = 3;
    const int COLS = 5;

    private PackedScene[,] roomGrid = new PackedScene[ROWS, COLS];
    private RoomInstance currentRoomInstance;

    private int currentRow = 0;
    private int currentCol = 0;

    private bool isTransitioning = false;

    public override void _Ready()
    {
        InitializeGrid();
        LoadRoomAt(currentRow, currentCol, ""); // no spawn direction at start
    }

    private void InitializeGrid()
    {
        var frostWildsOne = GD.Load<PackedScene>("res://scenes/overworld/frostWildsOne.tscn");
        var frostWildsRuins = GD.Load<PackedScene>("res://scenes/overworld/frostWildsRuins.tscn");

        GD.Print("Room Grid Layout:");

        for (int row = 0; row < ROWS; row++)
        {
            string rowStr = "";
            for (int col = 0; col < COLS; col++)
            {
                var chosen = GD.Randf() > 0.5f ? frostWildsOne : frostWildsRuins;
                roomGrid[row, col] = chosen;
                rowStr += chosen.ResourcePath.Contains("frostWildsOne") ? "[O]" : "[R]";
            }
            GD.Print(rowStr);
        }
    }


    private async void LoadRoomAt(int row, int col, string fromDirection)
    {
        if (currentRoomInstance != null)
        {
            currentRoomInstance.DoorEntered -= OnDoorEntered;
            currentRoomInstance.QueueFree();
        }

        var roomScene = roomGrid[row, col];
        currentRoomInstance = roomScene.Instantiate<RoomInstance>();
        currentRoomInstance.Name = $"Room_{row}_{col}";

        AddChild(currentRoomInstance);
        currentRoomInstance.DoorEntered += OnDoorEntered;

        GD.Print($"→ You are now in Room [{row}, {col}]");

        if (!string.IsNullOrEmpty(fromDirection))
            CallDeferred(nameof(SetPlayerSpawn), fromDirection);

        // 🔒 Wait 0.5 seconds before enabling doors
        await currentRoomInstance.EnableDoorsAfterDelay(0.5f);
    }



    private void OnDoorEntered(string direction)
    {
        if (isTransitioning)
        {
            GD.Print("Blocked: Already transitioning!");
            return;
        }

        isTransitioning = true;
        currentRoomInstance.SetDoorTriggersEnabled(false); // ✅ block future triggers

        int newRow = currentRow;
        int newCol = currentCol;

        switch (direction)
        {
            case "north": newRow -= 1; break;
            case "south": newRow += 1; break;
            case "east": newCol += 1; break;
            case "west": newCol -= 1; break;
        }

        if (newRow < 0 || newRow >= ROWS || newCol < 0 || newCol >= COLS)
        {
            GD.Print("Blocked: No room in that direction.");
            isTransitioning = false;
            currentRoomInstance.SetDoorTriggersEnabled(true); // re-enable triggers
            return;
        }

        string opposite = GetOppositeDirection(direction);

        // ✅ Only update current room coordinates after validation
        currentRow = newRow;
        currentCol = newCol;

        LoadRoomAt(currentRow, currentCol, opposite);
    }



    private async void SetPlayerSpawn(string cameFromDirection)
    {
        // Delay to let scene finish adding
        await ToSignal(GetTree(), "process_frame");

        Node2D player = GetNode<Node2D>("Player");
        string spawnNodeName = $"Spawns/SpawnFrom{cameFromDirection.Capitalize()}";
        Node2D spawnPoint = currentRoomInstance.GetNodeOrNull<Node2D>(spawnNodeName);

        if (spawnPoint != null)
        {
            GD.Print($"Spawning player at: {spawnNodeName} → {spawnPoint.GlobalPosition}");
            player.GlobalPosition = spawnPoint.GlobalPosition;
        }
        else
        {
            GD.PrintErr($"Missing spawn point: {spawnNodeName}");
        }

        isTransitioning = false;
    }


    private string GetOppositeDirection(string dir)
    {
        return dir switch
        {
            "north" => "south",
            "south" => "north",
            "east" => "west",
            "west" => "east",
            _ => dir
        };
    }
}
