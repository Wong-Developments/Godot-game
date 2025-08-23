using Godot;
using System;

public partial class RoomManager : Node2D
{
    const int ROWS = 5;
    const int COLS = 3;

    private PackedScene[,] roomGrid = new PackedScene[ROWS, COLS]; //grid of rooms
    private bool[,] roomDisabledGrid = new bool[ROWS, COLS]; // NEW
    private RoomInstance currentRoomInstance;

    private int currentRow = 4; //starts at the last row
    private int currentCol = 0; //randomly selects room 0,1,2

    private bool isTransitioning = false;

    /* 
        When Room starts create the grid and start at the located room in grid
     */
    public override void _Ready()
    {
        InitializeGrid();
        LoadRoomAt(currentRow, currentCol, ""); // no spawn direction at start
        
    }

    /*
        Generates Grid and randomly generates a scenes to each cell (Revise for later)
     */
    private void InitializeGrid()
    {
        //Add Packed Scenes here manually (revise to do it dynamically)
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


    /*
        Handles Door Entering logic
        Recalculates new grid cell location and prevents movement outside grid locations
        Then Spawns player at the opposite location they entered from
     */
    private void OnDoorEntered(string direction)
    {
        if (isTransitioning)
        {
            GD.Print("Blocked: Already transitioning!");
            return;
        }

        isTransitioning = true;
        currentRoomInstance.SetDoorTriggersEnabled(false); // block future triggers

        int newRow = currentRow;
        int newCol = currentCol;

        // Coordinate calculation
        switch (direction)
        {
            case "north": newRow -= 1; break;
            case "south": newRow += 1; break;
            case "east": newCol += 1; break;
            case "west": newCol -= 1; break;
        }

        // outside grid
        if (newRow < 0 || newRow >= ROWS || newCol < 0 || newCol >= COLS)
        {
            GD.Print("Blocked: No room in that direction.");
            isTransitioning = false;
            currentRoomInstance.SetDoorTriggersEnabled(true);
            return;
        }

        // 🚨 check disabled AFTER calculating target coords
        if (roomDisabledGrid[newRow, newCol])
        {
            GD.Print("That room is disabled!");
            isTransitioning = false;
            currentRoomInstance.SetDoorTriggersEnabled(true);
            return;
        }

        string opposite = GetOppositeDirection(direction);

        // save previous room before updating
        int prevRow = currentRow;
        int prevCol = currentCol;

        // after validating newRow/newCol…
        DisablePreviousRooms(direction, prevRow, prevCol);

        currentRow = newRow;
        currentCol = newCol;

        LoadRoomAt(currentRow, currentCol, opposite);
    }

    private void DisablePreviousRooms(string direction, int prevRow, int prevCol)
    {
        if (direction == "north")
        {
            for (int c = 0; c < COLS; c++)
                roomDisabledGrid[prevRow, c] = true;
        }
        else if (direction == "east" || direction == "west")
        {
            roomDisabledGrid[prevRow, prevCol] = true;
        }

        // Debug: print the current roomDisabledGrid
        GD.Print("Room Disabled Grid:");
        for (int r = 0; r < ROWS; r++)
        {
            string rowStr = "";
            for (int c = 0; c < COLS; c++)
            {
                rowStr += roomDisabledGrid[r, c] ? "[X]" : "[ ]";
            }
            GD.Print(rowStr);
        }
    }




    /*
        Sets the location the player spawns at after entering from a certain direction
    */
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

    /*
        A map of opposite directions of each door location
     */
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
