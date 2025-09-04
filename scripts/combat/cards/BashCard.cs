using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public partial class BashCard : Card
{

    [Export] public string CardNameExport = "Bash";
    [Export] public string DescriptionExport = "Deal heavy damage to a single enemy.";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI

    public override TargetType Type => TargetType.SingleEnemy;

    public override string CardName => CardNameExport;

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy)
        {
            int baseDamage = 30;
            int finalDamage = (int)(baseDamage * source.GetTotalDamageMultiplier());
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Played Bash: {finalDamage} damage dealt (Base: {baseDamage}, Multiplier: {source.GetTotalDamageMultiplier():F1}x)");
        }
        else
            Logger.Warning("Bash used on invalid target.");
    }

}

