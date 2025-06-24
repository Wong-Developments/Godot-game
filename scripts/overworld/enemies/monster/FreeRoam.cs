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
    private int patrolIndex = 0;
    private float patrolTimer = 0f;
    private float waitTime = 0f;
    private readonly ECharacterAnimation[] walkAnimations = [.. Enum.GetValues<ECharacterAnimation>().Where(anim => anim.IsWalk())];

    public override void PhysicsUpdate(float delta)
    {
        // Animation control
        EmitSignal(SignalName.Animation, direction != Vector2.Zero ? "walk" : "idle");

        if (player == null)
            return;

        if (stateMachine.owner.Position.DistanceTo(player.Position) >= DetectionRange)
        {
            patrolTimer += delta;
            if (waitTime > 0)
            {
                if (patrolTimer >= waitTime)
                {
                    patrolTimer = 0f;
                    waitTime = 0f;
                    for (int i = 0; i < 10; i++)
                    {
                        var candidate = walkAnimations[GD.Randi() % walkAnimations.Length].Direction();
                        if (candidate != blockedDirection)
                        {
                            patrolDirection = candidate;
                            break;
                        }
                    }
                    direction = patrolDirection;
                    Logger.Debug($"patrolDirection {patrolDirection.GetAnimation()}");
                }
                else
                    direction = Vector2.Zero;
            }
            else
            {
                direction = patrolDirection;
                stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed * PatrolMultiplier, Acceleration * delta);

                // Walk for a short time, then wait again
                if (patrolTimer >= (float)GD.RandRange(WalkDurationRange.X, WalkDurationRange.Y))
                {
                    patrolTimer = 0f;
                    waitTime = (float)GD.RandRange(WaitTimeRange.X, WaitTimeRange.Y);
                    direction = Vector2.Zero;
                }
            }
        }
        else
        {
            direction = (player.Position - stateMachine.owner.Position).Normalized();
            patrolTimer = 0f;
            waitTime = 0f;
        }

        // Smooth movement using acceleration/deceleration
        if (direction != Vector2.Zero)        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(direction * stateMachine.owner.speed, Acceleration * delta);        
        else        
            stateMachine.owner.Velocity = stateMachine.owner.Velocity.Lerp(Vector2.Zero, Deceleration * delta);

        //stateMachine.owner.MoveAndSlide();
        var collision = stateMachine.owner.MoveAndCollide(stateMachine.owner.Velocity * delta);

        if (waitTime == 0f && collision != null)
        {
            // Dot product between movement direction and collision normal
            var hitNormal = collision.GetNormal();
            var hitAngle = direction.Dot(hitNormal);

            // If angle is very close to -1, we're pushing directly into the wall
            if (hitAngle <= -0.9f)
            {
                Logger.Debug("Monster collided head-on.");
                patrolTimer = 0f;
                waitTime = (float)GD.RandRange(WaitTimeRange.X, WaitTimeRange.Y);
                direction = Vector2.Zero;
                blockedDirection = patrolDirection;

                for (int i = 0; i < 10; i++)
                {
                    var candidate = walkAnimations[GD.Randi() % walkAnimations.Length].Direction();
                    if (candidate != blockedDirection)
                    {
                        patrolDirection = candidate;
                        break;
                    }
                }

                direction = patrolDirection;
                blockedDirection = null;

                Logger.Debug($"New patrolDirection after collision: {patrolDirection.GetAnimation()}");
            }
        }

    }
}