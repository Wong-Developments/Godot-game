using Game.Scripts.Core;
using Godot;
using System;

public partial class BurnEffect : StatusEffect
{
    public BurnEffect(int duration)
    {
        Duration = duration;
        Name = "Burn";
    }

    protected override void OnApply()
    {
        Logger.Info($"{target.Name} is burned for {Duration} turns!");
    }

    protected override void OnTick()
    {
        int burnDamage = 5;
        target.TakeDamage(burnDamage);
        Logger.Debug($"{target.Name} takes {burnDamage} burn damage. {Duration - 1} turns left.");
    }

    protected override void OnExpire()
    {
        Logger.Info($"{target.Name}'s burn wore off.");
    }
}
