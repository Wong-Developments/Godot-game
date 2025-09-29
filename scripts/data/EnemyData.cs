using Godot;
using System.Collections.Generic;
using Game.Scripts.Combat.EnemyAttacks;

namespace Game.Scripts.Data;

public class EnemyData
{
    public string Name { get; set; }
    public int MaxHP { get; set; }
    public List<EnemyAttack> Attacks { get; set; }
    public PackedScene CombatSprite { get; set; }
    public PackedScene OverworldSprite { get; set; }
}