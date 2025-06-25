using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Combat.Cards;
public partial class BurnCard : Card
{
    public override TargetType Type => TargetType.SingleEnemy;

    public override string CardName => "Burn";

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy)
        {
            var burn = new BurnEffect(duration: 3);
            enemy.AddEffect(burn);
            Logger.Info($"BurnCard played. Enemy will take burn damage for {burn.Duration} turns.");
        }
        else
            Logger.Warning("BurnCard used on invalid target.");
    }
}
