using Game.Scripts.Combat.Cards;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts.Combat;
public class TargetingService
{
    private Player player;
    private List<Enemy> enemies;
    private Action<Card> onCardPlayed;

    public TargetingService(Player player, List<Enemy> enemies, Action<Card> onCardPlayed)
    {
        this.player = player;
        this.enemies = enemies;
        this.onCardPlayed = onCardPlayed;
    }

    public bool HandleCardDropped(Card card)
    {
        bool isValidTarget = false;
        Character target = null;

        var playerReceiver = player.GetNodeOrNull<TargetReceiver>("DropArea");
        bool isOnPlayer = IsMouseOverReceiver(playerReceiver);

        var hitEnemy = enemies.FirstOrDefault(e =>
            e.IsAlive() &&
            IsMouseOverReceiver(e.GetNodeOrNull<TargetReceiver>("DropArea")));

        switch (card.Type)
        {
            case TargetType.Self:
                isValidTarget = isOnPlayer;
                target = player;
                break;

            case TargetType.SingleEnemy:
                isValidTarget = hitEnemy != null;
                target = hitEnemy;
                break;

            case TargetType.AllEnemies:
                isValidTarget = hitEnemy != null;
                target = null; // Explicitly null for AOE
                break;
        }

        if (!isValidTarget)
        {
            card.RejectPlay();
            return false;
        }

        if (card.Type != TargetType.AllEnemies && target == null)
            return false;

        card.Play(card.SourcePlayer, target);
        onCardPlayed(card);
        return true;
    }

    private bool IsMouseOverReceiver(TargetReceiver receiver)
    {
        if (receiver == null)
            return false;

        Vector2 mousePos = CombatManager.Instance.GetViewport().GetMousePosition();

        var hits = receiver.GetWorld2D().DirectSpaceState.IntersectPoint(
            new PhysicsPointQueryParameters2D
            {
                Position = mousePos,
                CollisionMask = 1,
                CollideWithAreas = true,
                CollideWithBodies = false
            }
        );

        return hits.Any(result =>
            result.TryGetValue("collider", out var col) &&
            col.As<GodotObject>() == receiver
        );
    }
}

