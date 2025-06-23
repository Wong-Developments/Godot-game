using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Godot;

namespace Game.Scripts.Overworld.Enemies.Monster;

public partial class FreeRoam : State
{
    [Signal] public delegate void AnimationEventHandler(string animationName);
    public Vector2 direction;
    
    private CharacterBody2D player;
    private readonly float acceleration = 5.0f; // Adjust for smoother movement
    private readonly float deceleration = 10.0f; // Adjust for quicker stops

    public override void _Ready()
    {
        if (stateMachine == null)
            Logger.Error("stateMachine is null");

        if (stateMachine?.owner == null)
            Logger.Error("stateMachine owner is null");

        if (stateMachine?.owner?.speed == null)
            Logger.Error("stateMachine owner speed is null");

        var players = GetTree().GetNodesInGroup("player");
        if (players.Count > 0)        
            player = players[0] as CharacterBody2D;
        else        
            Logger.Error("No player found in 'player' group");
    }

    public override void PhysicsUpdate(float delta)
    {
        // Animation control
        if (direction != Vector2.Zero)        
            EmitSignal(SignalName.Animation, "walk");        
        else        
            EmitSignal(SignalName.Animation, "idle");

        if (player == null)
            return;

        // move player
        direction = (player.Position - stateMachine.owner.Position).Normalized();

        // Smooth movement using acceleration/deceleration
        if (direction != Vector2.Zero)        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed, acceleration * delta);        
        else        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(Vector2.Zero, deceleration * delta);

        stateMachine.owner.MoveAndSlide();
    }
}