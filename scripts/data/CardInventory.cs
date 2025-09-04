using System.Collections.Generic;

namespace Game.Scripts.Data;
public class CardInventory
{
    public Dictionary<CardData, int> Cards { get; } = new();

    public void AddCard(CardData card, int count = 1)
    {
        if (Cards.ContainsKey(card))
            Cards[card] += count;
        else
            Cards[card] = count;
    }

    public void RemoveCard(CardData card, int count = 1)
    {
        if (Cards.ContainsKey(card))
        {
            Cards[card] -= count;
            if (Cards[card] <= 0)
                Cards.Remove(card);
        }
    }
}