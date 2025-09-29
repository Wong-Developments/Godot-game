using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Game.Scripts.Data;
using Game.Scripts.Combat.EnemyAttacks;
using Godot;
using System.Collections.Generic;

namespace Game.Scripts.Combat;
public partial class Enemy : Character
{
    public List<EnemyAttack> Attacks { get; set; } = new();

    public void InitializeFromData(EnemyData data)
    {
        maxHealth = data.MaxHP;
        Health = maxHealth;
        Attacks = data.Attacks;
        Name = data.Name;
    }

    // Pick a random attack and use it on the player
    public override int Attack()
    {
        if (Attacks == null || Attacks.Count == 0)
            return 0;

        var attack = Attacks[(int)(GD.Randi() % Attacks.Count)];
        Logger.Info($"{Name} uses {attack.Name}!");
        attack.Execute(this, CombatManager.Instance.Player);
        return attack.Damage;
    }
}

