using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;

namespace Game.Scripts.Combat.Cards;
public partial class CounterSmashCard : Card
{
    [Export] public string CardNameExport = "Counter Smash";
    [Export] public string DescriptionExport = "desc";
    //[Export] public Texture2D IconExport; // May be needed later for overworld UI
    public override TargetType Type => TargetType.SingleEnemy;
    public override string CardName => CardNameExport;

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