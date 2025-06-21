using Game.Scripts.Core;
using Godot;

public partial class BurnCard : Card
{
    public override string CardName => "Burn";

    public override void Play(Player player, Enemy enemy)
    {
        var burn = new BurnEffect(duration: 3); // 3 turns of burn
        enemy.AddEffect(burn);
        Logger.Info($"BurnCard played. Enemy will take burn damage for {burn.Duration} turns.");
    }
}
