using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class HammerSpinCard : Card
{
    [Export] public string CardNameExport = "Hammer Spin";
    [Export] public string DescriptionExport = "desc";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI

    public override TargetType Type => TargetType.AllEnemies;
    public override string CardName => CardNameExport;

    public override void Play(Character source, Character target)
    {
        var enemies = CombatManager.Instance.GetAliveEnemies();
        if (enemies.Count == 0)
        {
            Logger.Warning("No enemies to hit with Hammer Spin.");
            return;
        }

        var random = new Random();
        int hits = random.Next(1, 6); // 1 to 5 times

        int baseDamage = 6; // Lower per-hit damage

        for (int i = 0; i < hits; i++)
        {
            foreach (var enemy in enemies)
            {
                int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());
                enemy.TakeDamage(finalDamage);
                Logger.Info($"Hammer Spin hit {enemy.Name} for {finalDamage} damage (round {i + 1}/{hits})");
            }
        }
    }
}