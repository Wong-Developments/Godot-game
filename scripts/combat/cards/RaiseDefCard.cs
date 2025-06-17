using Game.Core;
using Godot;
using System;

public partial class RaiseDefCard: Card
{
    public override string CardName => "Raise Def";

    public override void Play(Player player, Enemy enemy)
    {
        player.ModifyDefense(0.2f);
        Logger.Info("Played Raised Defense increased to 1.2 (~17% less)");
    }
}

