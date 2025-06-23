using Game.Scripts.Combat.Cards;
using Game.Scripts.Overworld.Player;
using Godot;
using System;

namespace Game.Scripts.Combat.Effects;
public abstract partial class StatusEffect : Node
{
    public int Duration { get; protected set; }
    public new string Name { get; protected set; }

    protected Character target; // could be Player or Enemy

    public void ApplyTo(Character target)
    {
        this.target = target;
        OnApply();
    }

    public void Tick()
    {
        OnTurn();
        Duration--;
        if (Duration <= 0)
            OnExpire();
    }

    protected abstract void OnApply();
    protected abstract void OnTurn(); // was "OnTick"
    protected abstract void OnExpire();
}
