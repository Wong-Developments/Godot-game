using Game.Core;
using Godot;
using System;
using System.Collections.Generic;

public partial class CardDeck : Node
{
    private List<PackedScene> allCardTypes = new();
    private Queue<PackedScene> deck = new();
    private List<PackedScene> discardPile = new();
    private Random rng = new();

    public void InitDeck(List<PackedScene> availableCards, int copiesPerCard = 3)
    {
        allCardTypes = availableCards;
        BuildAndShuffleDeck(copiesPerCard);
    }

    private void BuildAndShuffleDeck(int copies)
    {
        var cardPool = new List<PackedScene>();

        for (int i = 0; i < copies; i++)
            cardPool.AddRange(allCardTypes);

        while (cardPool.Count > 0)
        {
            int index = rng.Next(cardPool.Count);
            deck.Enqueue(cardPool[index]);
            cardPool.RemoveAt(index);
        }

        Logger.Debug($"Deck initialized with {deck.Count} cards.");
    }

    public PackedScene Draw()
    {
        if (deck.Count == 0)
        {
            GD.Print("Deck is empty. Reshuffling discard pile...");
            ReshuffleDiscardPile();
        }

        return deck.Count > 0 ? deck.Dequeue() : null;
    }

    public void Discard(PackedScene card)
    {
        discardPile.Add(card);
    }

    private void ReshuffleDiscardPile()
    {
        foreach (var card in discardPile)
            deck.Enqueue(card);

        discardPile.Clear();
    }

    public void Reset()
    {
        deck.Clear();
        discardPile.Clear();
        allCardTypes.Clear();
    }
}
