using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class CroixCard : Card
{
    public override TargetType Type => TargetType.Self;

    public override string CardName => "Croix";

    public override void Play(Character source, Character target)
    {
        if (target is Player player)
        {
            int heal = 10;
            player.Heal(heal);
            Logger.Info($"Played Croix: {heal} HP restored");
        }
        else
            Logger.Warning("CroixCard used on invalid target.");
    }
}

