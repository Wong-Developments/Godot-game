using Game.Scripts.Combat.Cards;
using Game.Scripts.Core;
using Godot;
using System;

namespace Game.Scripts.Combat.Effects;
public partial class BurnEffect : StatusEffect
{
    private int burnDamage;

    public BurnEffect(int duration, int burnDamage = 5)
    {
        Duration = duration;
        Name = "Burn";
        this.burnDamage = burnDamage;

    }

    protected override void OnApply()
    {
        Logger.Info($"{target.Name} is burned for {Duration} turns!");
    }

    protected override void OnTick()
    {
        target.TakeDamage(burnDamage);
        Logger.Debug($"{target.Name} takes {burnDamage} burn damage. {Duration - 1} turns left.");
    }

    protected override void OnExpire()
    {
        Logger.Info($"{target.Name}'s burn wore off.");
    }
}
