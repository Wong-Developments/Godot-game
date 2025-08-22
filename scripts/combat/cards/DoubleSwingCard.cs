using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;
using System.Linq;

namespace Game.Scripts.Combat.Cards;
public partial class DoubleSwingCard : Card
{
    public override TargetType Type => TargetType.SingleEnemy;
    public override string CardName => "Double Swing";

    public override void Play(Character source, Character target)
    {
        var enemies = CombatManager.Instance.GetAliveEnemies();
        if (enemies.Count == 0)
        {
            Logger.Warning("No enemies to hit with Double Swing.");
            return;
        }

        var random = new Random();
        var enemy = enemies[random.Next(enemies.Count)];
        int baseDamage = 8; // Low damage

        for (int i = 0; i < 2; i++)
        {
            int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Double Swing hit {enemy.Name} for {finalDamage} damage (hit {i + 1}/2)");
        }
    }
}
