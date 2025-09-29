using System.Collections.Generic;
using Godot;
using Game.Scripts.Combat.EnemyAttacks;
using Game.Scripts.Combat.Effects;

namespace Game.Scripts.Data;

public static class EnemyDatabase
{
    public static Dictionary<string, EnemyData> AllEnemies { get; } = new();

    static EnemyDatabase()
    {
        AllEnemies["Monster"] = new EnemyData
        {
            Name = "Monster",
            MaxHP = 75,
            Attacks = new List<EnemyAttack>
            {
                new EnemyAttack
                {
                    Name = "Slash",
                    Description = "A basic attack dealing 15 damage.",
                    Damage = 15,
                    EffectFactory = null
                },
                new EnemyAttack
                {
                    Name = "Inferno",
                    Description = "Deals 10 damage and applies burn of 5 damage for 2 turns.",
                    Damage = 10,
                    EffectFactory = (source, target) => new BurnEffect(duration: 2, burnDamage: 5)
                }
            },
            CombatSprite = GD.Load<PackedScene>("res://Scenes/Combat/Enemy.tscn"),
            OverworldSprite = GD.Load<PackedScene>("res://Scenes/Characters/Monster.tscn")
        };
    }
}
