using Godot;
using System;

public abstract partial class Card : Button
{
    public abstract string CardName { get; }
    public abstract void Play(Player player, Enemy enemy);

    public void SetTextLabel()
    {
        Text = CardName;
    }
}
