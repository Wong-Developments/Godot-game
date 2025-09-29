using Game.Scripts.Combat.Effects;

namespace Game.Scripts.Combat.EnemyAttacks;

public class EnemyAttack
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Damage { get; set; }
    public System.Func<Character, Character, StatusEffect?> EffectFactory { get; set; }

    public void Execute(Character source, Character target)
    {
        if (Damage > 0)
            target.TakeDamage(Damage);

        if (EffectFactory != null)
        {
            var effect = EffectFactory(source, target);
            if (effect != null)
                target.AddEffect(effect);
        }
    }
}