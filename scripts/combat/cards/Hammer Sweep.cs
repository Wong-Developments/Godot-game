using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class HammerSweep : Card
{
    public override TargetType Type => TargetType.AllEnemies;

    public override string CardName => "Sweeping Blow";

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy)
        {
            int dmg = 30;
            enemy.TakeDamage(dmg);
            Logger.Info($"Played Strike: {dmg} damage dealt");
        }
        else
            Logger.Warning("Sweeping Blow used on invalid target.");
    }

}

