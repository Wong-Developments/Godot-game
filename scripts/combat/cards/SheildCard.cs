using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class SheildCard: Card
{
    public override string CardName => "Sheild";

    public override void Play(Player player, Enemy enemy)
    {
        player.AddShield(10);
        Logger.Info("Played Sheilded for 10 dmg");
    }
}

