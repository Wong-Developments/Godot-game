using Game.Core;
using Godot;

public partial class BuffCard : Card
{
    public override string CardName => "Buff";

    public override void Play(Player player, Enemy enemey)
    {
        var buff = new DamageBuffEffect(duration: 2, multiplier: 1.5f);
        player.AddEffect(buff);
        Logger.Info($"BuffCard played. Player damage increased for {buff.Duration} turns.");
    }
}


