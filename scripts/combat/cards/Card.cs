using Game.Scripts.Combat.Effects;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;
public abstract partial class Card : Button
{
    public abstract string CardName { get; }
    public PackedScene SourceScene { get; set; }

    public abstract void Play(Player player, Enemy enemy);

    public void SetTextLabel()
    {
        Text = CardName;
    }
}
