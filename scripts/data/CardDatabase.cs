using System.Collections.Generic;
using Game.Scripts.Combat.Cards;
using Godot;

namespace Game.Scripts.Data;
public static class CardDatabase
{
    private static readonly string[] CardScenePaths = new[]
    {
        "res://Scenes/Combat/Cards/BashCard.tscn",
        "res://Scenes/Combat/Cards/CroixCard.tscn",
        "res://Scenes/Combat/Cards/SheildCard.tscn",
        "res://Scenes/Combat/Cards/EnflameCard.tscn",
        "res://Scenes/Combat/Cards/BuffCard.tscn",
        "res://Scenes/Combat/Cards/SaltBlastCard.tscn",
        "res://Scenes/Combat/Cards/DoubleSwingCard.tscn",
        "res://Scenes/Combat/Cards/HammerSpinCard.tscn",
        "res://Scenes/Combat/Cards/CounterSmashCard.tscn"
    };

    public static List<CardData> AllCards { get; } = new();

    static CardDatabase()
    {
        foreach (var path in CardScenePaths)
        {
            var scene = GD.Load<PackedScene>(path);
            if (scene == null)
            {
                GD.PrintErr($"CardDatabase: Could not load scene at {path}");
                continue;
            }
            var cardInstance = scene.Instantiate() as Card;
            if (cardInstance == null)
            {
                GD.PrintErr($"CardDatabase: Scene at {path} is not a Card");
                continue;
            }
            //var icon = (cardInstance.GetType().GetProperty("IconExport") != null)
            //    ? (Texture2D)(cardInstance as dynamic).IconExport
            //    : null;

            var data = new CardData
            {
                Name = (cardInstance as dynamic).CardNameExport,
                Description = (cardInstance as dynamic).DescriptionExport,
                //Icon = icon,
                Icon = null, // Placeholder until icons are added
                Scene = scene
            };
            AllCards.Add(data);
            cardInstance.QueueFree(); // Clean up
        }
    }
}