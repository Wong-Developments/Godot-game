using Game.Scripts.Core;
using Game.Scripts.Overworld.Player;
using Godot;

namespace Game.Scripts.Overworld.Enemies.States;

public partial class FreeRoam : State
{
    [Signal] public delegate void AnimationEventHandler(string animationName);
    public Vector2 direction;
    
    private CharacterBody2D player;
    private float acceleration = 5.0f; // Adjust for smoother movement
    private float deceleration = 10.0f; // Adjust for quicker stops

    public override void _Ready()
    {
        var players = GetTree().GetNodesInGroup("player");
        if (players.Count > 0)        
            player = players[0] as CharacterBody2D;
        else        
            GD.PrintErr("No player found in 'player' group");
    }

    public void MoveToPlayer(double delta)
    {
        if (player == null) 
            return;

        // Calculate direction to player (normalized)
        direction = (player.Position - stateMachine.owner.Position).Normalized();        
    }

    public override void PhysicsUpdate(double delta)
    {
        // Animation control
        if (direction != Vector2.Zero)        
            EmitSignal(SignalName.Animation, "walk");        
        else        
            EmitSignal(SignalName.Animation, "idle");        

        MoveToPlayer(delta);
        
        if (player == null) 
            return;

        float speed = stateMachine.owner.speed;
        float deltaf = (float)delta;

        // Smooth movement using acceleration/deceleration
        if (direction != Vector2.Zero)        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * speed, acceleration * deltaf);        
        else        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(Vector2.Zero, deceleration * deltaf);

        stateMachine.owner.MoveAndSlide();
    }
}