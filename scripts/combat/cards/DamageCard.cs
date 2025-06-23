using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class DamageCard : Card
{
    public override string CardName => "Strike";

    public override void Play(Player player, Enemy enemy)
    {
        int dmg = 30;
        enemy.TakeDamage(dmg);
        Logger.Info($"Played Strike: {dmg} damage dealt");
    }
}

