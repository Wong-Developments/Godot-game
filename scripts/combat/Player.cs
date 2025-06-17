using Game.Core;
using Godot;
using System;

public partial class Player : Node2D
{
    [Export] public int maxHealth = 100;
    [Export] public float baseDefenseMultiplier = 1.0f; // 1.0 = normal defense

    public int Health { get; private set; }
    public float DefenseMultiplier { get; private set; } 

    public override void _Ready()
    {
        Health = maxHealth;
        DefenseMultiplier = baseDefenseMultiplier;
    }

    public static int Attack()
    {
        Logger.Debug("Player attacks!");
        return 10;  // damage dealt
    }

    // Modify defense multiplier (with optional min/max bounds)
    public void ModifyDefense(float amount, float minMultiplier = 0.5f, float maxMultiplier = 2.0f)
    {
        DefenseMultiplier = Mathf.Clamp(DefenseMultiplier + amount, minMultiplier, maxMultiplier);
        Logger.Info($"Player defense changed by {amount:0.00}, now at {DefenseMultiplier:0.00}x");
    }


    public void TakeDamage(int amount)
    {
        int finalDamage = Mathf.Max((int)(amount / DefenseMultiplier), 1);
        Health = Mathf.Max(Health - finalDamage, 0);

        Logger.Info($"Player takes {amount} damage, HP is now {Health}");
        Logger.Debug($"Base damage: {amount} | Defense: {DefenseMultiplier:0.00}x | Final damage: {finalDamage} | HP: {Health}/{maxHealth}");
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, maxHealth);
        Logger.Info($"Player healed for {amount} HP, health is now {Health}");
    }
}

