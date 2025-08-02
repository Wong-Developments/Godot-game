using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class DamageCard : Card
{
    public override TargetType Type => TargetType.SingleEnemy;

    public override string CardName => "Strike";

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy)
        {
            int baseDamage = 30;
            int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Played Strike: {finalDamage} damage dealt (Base: {baseDamage}, Multiplier: {source.GetTotalDamageMultiplier():F1}x)");
        }
        else
            Logger.Warning("Strike used on invalid target.");
    }

}

