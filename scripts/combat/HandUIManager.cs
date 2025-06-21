using Godot;
using System;
using System.Collections.Generic;

public partial class HandUIManager : Node
{
    [Export] private Control handContainer;
    [Export] private int maxHandSize = 6;

    public void AddCard(Card card)
    {
        if (handContainer.GetChildCount() >= maxHandSize)
            return;

        handContainer.AddChild(card);
    }

    public void RemoveCard(Card card)
    {
        handContainer.RemoveChild(card);
        card.QueueFree();
    }

    public void ClearHand()
    {
        foreach (var child in handContainer.GetChildren())
            child.QueueFree();
    }

    public int CardCount => handContainer.GetChildCount();

    public IEnumerable<Node> GetCards()
    {
        return handContainer.GetChildren();
    }

}

