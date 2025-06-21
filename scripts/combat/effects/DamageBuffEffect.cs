using Game.Scripts.Core;
using Godot;
using System;


public partial class DamageBuffEffect : StatusEffect
{
    private float multiplier;

    public DamageBuffEffect(int duration, float multiplier)
    {
        Duration = duration;
        this.multiplier = multiplier;
        Name = "Damage Buff";
    }

    protected override void OnApply()
    {
        target.BaseDamage = (int)(target.BaseDamage * multiplier);
        Logger.Info($"{target.Name}'s damage increased by {multiplier}x for {Duration} turns.");
    }

    protected override void OnTurn() { } // No per-turn tick needed for this one

    protected override void OnExpire()
    {
        target.BaseDamage = (int)(target.BaseDamage / multiplier);
        Logger.Info($"{target.Name}'s damage buff expired.");
    }
}
