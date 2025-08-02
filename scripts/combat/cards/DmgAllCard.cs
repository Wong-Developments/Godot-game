using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class DmgAllCard : Card
{
    public override TargetType Type => TargetType.AllEnemies;

    public override string CardName => "AoE Dmg";

    public override void Play(Character source, Character target)
    {
        //if (target is Enemy enemy)
        //{
        //    int baseDamage = 20;
        //    int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());

        //    foreach (var aliveEnemy in CombatManager.Instance.GetAliveEnemies())
        //    {
        //        aliveEnemy.TakeDamage(finalDamage);
        //        Logger.Info($"Hit {aliveEnemy.Name} for {finalDamage} damage");
        //    }
        //}
        //else
        //    Logger.Warning("Strike used on invalid target.");

        int baseDamage = 20;
        int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());

        foreach (var enemy in CombatManager.Instance.GetAliveEnemies())
        {
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Hit {enemy.Name} for {finalDamage} damage");
        }
    }

}