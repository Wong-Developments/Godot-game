using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class SheildCard: Card
{
    [Export] public string CardNameExport = "Sheild";
    [Export] public string DescriptionExport = "desc";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI

    public override TargetType Type => TargetType.Self;

    public override string CardName => CardNameExport;

    public override void Play(Character source, Character target)
    {
        if (target is Player player)
        {
            player.AddShield(10);
            Logger.Info("Played Shield: 10 shield applied.");
        }
        else
            Logger.Warning("ShieldCard used on invalid target.");
    }
}

