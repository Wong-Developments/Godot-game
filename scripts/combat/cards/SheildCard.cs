using Game.Scripts.Core;
using Godot;
using System;

public partial class SheildCard: Card
{
    public override string CardName => "Sheild";

    public override void Play(Player player, Enemy enemy)
    {
        player.AddShield(10);
        Logger.Info("Played Sheilded for 10 dmg");
    }
}

