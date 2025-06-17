using Game.Core;
using Godot;
using System;

public partial class HealCard : Card
{
    public override string CardName => "Heal";

    public override void Play(Player player, Enemy enemy)
    {
        int heal = 10;
        player.Heal(heal);
        Logger.Info($"Played Heal: {heal} HP restored");
    }
}

