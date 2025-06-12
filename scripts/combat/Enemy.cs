using Game.Core;
using Godot;
using System;

public partial class Enemy : Node2D
{
    [Export] public int maxHealth = 100;
    public int Health { get; private set; }

    public override void _Ready()
    {
        Health = maxHealth;
    }

    public int Attack()
    {
        Logger.Debug("Enemy attacks!");
        return 10;  // damage dealt
    }

    public void TakeDamage(int amount)
    {
        Health = Mathf.Max(Health - amount, 0);
        Logger.Info($"Enemy takes {amount} damage, HP is now {Health}");
    }
}

