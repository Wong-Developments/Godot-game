using Game.Scripts.Core;
using Game.Scripts.Overworld.Player;
using Game.Scripts.Overworld.States;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Overworld.Enemies;
public partial class EnemyState : State
{
    protected float Acceleration => ((Enemy)stateMachine.owner).acceleration;
    protected float Deceleration => ((Enemy)stateMachine.owner).deceleration;
    protected float DetectionRange => ((Enemy)stateMachine.owner).detectionRange;

    protected Entity player;

    public override void _Ready()
    {
        //var players = GetTree().GetNodesInGroup("player");
        //if (players.Count > 0)
        //    player = players[0] as Entity;
        //else
        //    Logger.Error("No player found in 'player' group");

        DebugUtils.PrintSceneTree(GetTree().Root); // Print ALL nodes in the game
        FindPlayer();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null || !IsInstanceValid(player))
        {
            FindPlayer(); // Keep checking until player is found
        }
    }

    private void FindPlayer()
    {
        // Attempt 1: Absolute path (most reliable)
        player = GetNodeOrNull<Character>("/root/GameManager/OverworldManager/RoomManager/Player");

        // Attempt 2: Relative path (if RoomManager is a sibling)
        if (player == null)
        {
            var roomManager = GetNodeOrNull<RoomManager>("../../../RoomManager");
            player = roomManager?.GetNodeOrNull<Character>("Player");
        }

        // Debug
        if (player != null)
        {
            GD.Print($"SUCCESS: Found player at {player.GetPath()}");
        }
        else
        {
            GD.PrintErr("Player not found in any path!");
        }
    }

    public override void PhysicsUpdate(float delta)
    {
        // Animation control
        EmitSignal(SignalName.Animation, direction != Vector2.Zero ? "walk" : "idle");

        if (player == null)
            return;

        // Smooth movement using acceleration/deceleration
        if (direction != Vector2.Zero)
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed, Acceleration * delta);
        else
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(Vector2.Zero, Deceleration * delta);

        stateMachine.owner.MoveAndSlide();
    }

}
