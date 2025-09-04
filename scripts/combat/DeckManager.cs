using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Game.Scripts.Data;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Scripts.Combat;
public partial class DeckManager : Node
{
    private List<CardData> allCardTypes = new();
    private Queue<CardData> deck = new();
    private List<CardData> discardPile = new();
    private Random rng = new();

    public void InitDeck(List<CardData> availableCards)
    {
        allCardTypes = availableCards;
        BuildAndShuffleDeck();
    }

    private void BuildAndShuffleDeck()
    {
        var cardPool = new List<CardData>(allCardTypes);

        while (cardPool.Count > 0)
        {
            int index = rng.Next(cardPool.Count);
            deck.Enqueue(cardPool[index]);
            cardPool.RemoveAt(index);
        }

        Logger.Debug($"Deck initialized with {deck.Count} cards.");
    }

    public CardData Draw()
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



    public void Discard(CardData card)
    {

        discardPile.Add(card);
        Logger.Debug($"Card discarded. Current discard pile size: {discardPile.Count}");
    }

    private void ReshuffleDiscardPile()
    {
        var shuffled = new List<CardData>(discardPile);
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
        BuildAndShuffleDeck();
    }
}