using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Combat.Cards;
public partial class CounterSmashCard : Card
{
    public override TargetType Type => TargetType.SingleEnemy;
    public override string CardName => "Counter Smash";

    public override void Play(Character source, Character target)
    {
        if (target is Enemy enemy && source is Player player)
        {
            int baseDamage = 10;
            int bonus = player.LastDamageTaken; 
            int finalDamage = baseDamage + bonus;
            enemy.TakeDamage(finalDamage);
            Logger.Info($"Counter Smash hits {enemy.Name} for {finalDamage} (base {baseDamage} + bonus {bonus})");
        }
        else
            Logger.Warning("Counter Smash used on invalid target.");
    }
}