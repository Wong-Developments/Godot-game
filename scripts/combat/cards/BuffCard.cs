using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Combat.Cards;
public partial class BuffCard : Card
{
    public override TargetType Type => TargetType.Self;

    public override string CardName => "Buff";

    public override void Play(Character source, Character target)
    {
        if (target is Player player)
        {
            var buff = new DamageBuffEffect(duration: 2, multiplier: 1.5f);
            player.AddEffect(buff);
            Logger.Info($"BuffCard played. Player damage increased for {buff.Duration} turns.");
        }
        else
            Logger.Warning("BuffCard used on invalid target.");
    }
}


