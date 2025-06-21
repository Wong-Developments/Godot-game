using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;

public partial class DeckManager : Node
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
        Logger.Debug($"Drawing card from deck. Current deck size: {deck.Count}, discard pile size: {discardPile.Count}");
        if (deck.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                Logger.Debug("Deck is empty. Reshuffling discard pile...");
                ReshuffleDiscardPile();
            }
            else
            {
                Logger.Warning("No cards left in deck or discard pile.");
                return null;
            }
        }

        return deck.Dequeue();
    }



    public void Discard(PackedScene card)
    {

        discardPile.Add(card);
        Logger.Debug($"Card discarded. Current discard pile size: {discardPile.Count}");
    }

    private void ReshuffleDiscardPile()
    {
        var shuffled = new List<PackedScene>(discardPile);
        discardPile.Clear();
        Logger.Debug($"Reshuffling {shuffled.Count} cards from discard pile back into deck. discardPile cleared");

        // Fisher-Yates shuffle
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        foreach (var card in shuffled)
            deck.Enqueue(card);

    }

    public void Reset()
    {
        deck.Clear();
        discardPile.Clear();
        //allCardTypes.Clear();
        BuildAndShuffleDeck(3);
    }
}