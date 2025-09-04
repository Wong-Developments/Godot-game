using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Combat.Cards;
public partial class EnflameCard : Card
{
    [Export] public string CardNameExport = "Enflame";
    [Export] public string DescriptionExport = "desc";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI
    public override TargetType Type => TargetType.SingleEnemy;

    public override string CardName => CardNameExport;

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy)
        {
            var burn = new BurnEffect(duration: 3);
            enemy.AddEffect(burn);
            Logger.Info($"EnflameCard played. Enemy will take burn damage for {burn.Duration} turns.");
        }
        else
            Logger.Warning("EnflameCard used on invalid target.");
    }
}
