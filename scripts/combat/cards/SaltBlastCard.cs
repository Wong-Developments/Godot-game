using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class SaltBlastCard : Card
{
    [Export] public string CardNameExport = "Salt Blast";
    [Export] public string DescriptionExport = "desc";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI

    public override TargetType Type => TargetType.AllEnemies;

    public override string CardName => CardNameExport;

    public override void Play(Character source, Character target)
    {
        int baseDamage = 10;
        int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());
        Random random = new Random();

        foreach (var enemy in CombatManager.Instance.GetAliveEnemies())
        {
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Salt Blast hit {enemy.Name} for {finalDamage} damage");

            if (random.NextDouble() < 0.5)
            {
                var burn = new BurnEffect(duration: 2);
                enemy.AddEffect(burn);
                Logger.Info($"{enemy.Name} is burned for {burn.Duration} turns!");
            }
        }
    }

}