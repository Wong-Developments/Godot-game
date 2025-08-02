using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Godot;
using System;
using System.Collections.Generic;
using Game.Scripts.Core;

namespace Game.Scripts.Combat;
public abstract partial class Character : Node2D
{
    [Export] public int maxHealth = 100;

    public int Health { get; protected set; }
    public int Shield { get; protected set; } = 0;
    public int BaseDamage { get; set; } = 10;


    public List<StatusEffect> ActiveEffects { get; private set; } = new();

    public List<float> DamageMultipliers { get; } = new List<float>();

    public override void _Ready()
    {
        Health = maxHealth;
    }
    public bool IsAlive() => Health > 0;


    public virtual int Attack()
    {
        Logger.Debug($"{Name} attacks for {BaseDamage} damage!");
        return BaseDamage;
    }


    public virtual void TakeDamage(int amount)
    {
        int damageLeft = amount;

        if (Shield > 0)
        {
            int absorbed = Mathf.Min(Shield, damageLeft);
            Shield -= absorbed;
            damageLeft -= absorbed;

            Logger.Info($"{Name}'s shield absorbed {absorbed} damage. Shield remaining: {Shield}");
        }

        if (damageLeft > 0)
        {
            Health = Mathf.Max(Health - damageLeft, 0);
            Logger.Info($"{Name} took {damageLeft} damage. HP is now {Health}");
        }

        //Logger.Debug($"Raw: {amount} | After Shield: {damageLeft} | Shield: {Shield} | HP: {Health}/{maxHealth}");
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, maxHealth);
        Logger.Info($"{Name} healed for {amount}. HP is now {Health}");
    }

    public void AddShield(int amount)
    {
        Shield += amount;
        Logger.Info($"{Name} gains {amount} shield. Total shield: {Shield}");
    }

    public void ResetShield()
    {
        Shield = 0;
        Logger.Info($"{Name}'s shield reset.");
    }

    public void AddEffect(StatusEffect effect)
    {
        effect.ApplyTo(this);
        ActiveEffects.Add(effect);
    }

    public void ProcessEffects()
    {
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffects[i].Tick();
            if (ActiveEffects[i].Duration <= 0)
                ActiveEffects.RemoveAt(i);
        }
    }

    public float GetTotalDamageMultiplier()
    {
        if (DamageMultipliers.Count == 0) return 1.0f;
        float total = 1.0f;
        foreach (var multiplier in DamageMultipliers)
        {
            total *= multiplier;
        }
        return total;
    }

}

