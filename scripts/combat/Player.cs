using Game.Scripts.Core;
using Godot;
using System;

public partial class Player : Node2D
{
    [Export] public int maxHealth = 100;

    public int Health { get; private set; }
    public int Shield { get; private set; } = 0;

    public override void _Ready()
    {
        Health = maxHealth;
    }

    public static int Attack()
    {
        Logger.Debug("Player attacks!");
        return 10;  // damage dealt
    }


    public void TakeDamage(int amount)
    {
        int damageLeft = amount;

        if (Shield > 0)
        {
            int absorbed = Mathf.Min(Shield, damageLeft);
            Shield -= absorbed;
            damageLeft -= absorbed;

            Logger.Info($"Shield absorbed {absorbed} damage. Shield remaining: {Shield}");
        }

        if (damageLeft > 0)
        {
            Health = Mathf.Max(Health - damageLeft, 0);
            Logger.Info($"Player took {damageLeft} damage. HP is now {Health}");
        }

        Logger.Debug($"Raw: {amount} | After Shield: {damageLeft} | Shield: {Shield} | HP: {Health}/{maxHealth}");
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, maxHealth);
        Logger.Info($"Player healed for {amount} HP, health is now {Health}");
    }

    public void AddShield(int amount)
    {
        Shield += amount;
        Logger.Info($"Player gains {amount} shield. Total shield: {Shield}");
    }

    public void ResetShield()
    {
        Shield = 0;
        Logger.Info("Shield reset.");
    }
}

