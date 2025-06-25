using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Game.Scripts.Utils;
using Godot;
using System;
using System.Linq;

namespace Game.Scripts.Overworld.Enemies.Monster;

public partial class FreeRoam : EnemyState
{
    private float PatrolMultiplier => ((Monster)stateMachine.owner).patrolMultiplier;
    private Vector2 WalkDurationRange => ((Monster)stateMachine.owner).walkDurationRange;
    private Vector2 WaitTimeRange => ((Monster)stateMachine.owner).waitTimeRange;

    private Vector2 patrolDirection = Vector2.Zero;
    private Vector2? blockedDirection = null;
    private Vector2 lastPosition;
    private float patrolTimer = 0f;
    private float waitTime = 0f;
    private readonly ECharacterAnimation[] walkAnimations = [.. Enum.GetValues<ECharacterAnimation>().Where(anim => anim.IsWalk())];

    public override void PhysicsUpdate(float delta)
    {
        // Animation control
        EmitSignal(SignalName.Animation, direction != Vector2.Zero ? "walk" : "idle");
        
        if (direction == Vector2.Zero)
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(Vector2.Zero, Deceleration * delta);

        if (player == null)
            return;

        // enter chase behavior
        if (DetectionRange >= stateMachine.owner.Position.DistanceTo(player.Position))
        {
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed, Acceleration * delta);
            direction = (player.Position - stateMachine.owner.Position).Normalized();
            patrolTimer = 0f;
            waitTime = 0f;
        }
        else        
            Patrol(delta); // enter patrol behavior

        OnCollision(stateMachine.owner.MoveAndCollide(stateMachine.owner.Velocity * delta));
    }

    private void Patrol(float delta)
    {
        patrolTimer += delta;
        if (waitTime > 0) // waiting
        {
            // If wait time is over, pick a new direction
            if (patrolTimer >= waitTime)
            {
                patrolTimer = 0f;
                waitTime = 0f;
                patrolDirection = GetDirection();
                Logger.Debug($"patrolDirection {patrolDirection.GetAnimation()}");
            }
            else
                direction = Vector2.Zero; // Stay idle during wait period
        }
        else // moving
        {
            // set and move in direction
            direction = patrolDirection;
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed * PatrolMultiplier, Acceleration * delta);

            // set wait time once time is up
            if (patrolTimer >= (float)GD.RandRange(WalkDurationRange.X, WalkDurationRange.Y))
            {
                patrolTimer = 0f;
                waitTime = (float)GD.RandRange(WaitTimeRange.X, WaitTimeRange.Y);
                direction = Vector2.Zero;
            }
        }
    }

    private void OnCollision(KinematicCollision2D collision)
    {
        // Only respond if moving and collision
        if (waitTime != 0f || collision == null || DetectionRange >= stateMachine.owner.Position.DistanceTo(player.Position))
            return;

        // Dot product between movement direction and collision normal
        var hitNormal = collision.GetNormal();
        var hitAngle = direction.Dot(hitNormal);

        if (hitAngle < 0)
        {
            Logger.Debug("Monster collided head-on.");
            blockedDirection = patrolDirection;
            patrolDirection = GetDirection();
            direction = patrolDirection; // set to new direction
            Logger.Debug($"New direction after collision: {patrolDirection.GetAnimation()}");
        }
    }

    private Vector2 GetDirection()
    {
        for (int i = 0; i < 10; i++)
        {
            var candidate = walkAnimations[GD.Randi() % walkAnimations.Length].Direction();
            if (candidate != blockedDirection)
                return candidate;
        }

        if (direction != Vector2.Zero)
            blockedDirection = null;

        return patrolDirection;
    }
}