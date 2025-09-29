using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Game.Scripts.Data;
using Game.Scripts.Overworld;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts.Combat;

public partial class CombatManager : Node
{
    private bool playerTurn = true;

    [Export] private Player player;
    [Export] private Node enemyContainer; // Node holding up to 3 enemy instances
    private List<Enemy> enemies = new();

    [Export] private Label playerHPLabel;
    [Export] private Label enemyHPLabel;
    [Export] private Button endTurnButton;

    [Export] private HandUIManager handUIManager;
    [Export] private DeckManager deckManager;
    [Export] private TurnManager turnManager;

    private TargetingService targetingService;

    [Export] private int handDrawSize = 3;
    
    public static CombatManager Instance { get; private set; }

    public Player Player => player;

    public List<Enemy> GetAliveEnemies()
    {
        return enemies.Where(e => e.IsAlive()).ToList();
    }

    public override void _Ready()
    {
        Instance = this;

        var gm = GetNode<GameManager>("/root/GameManager");
        string enemyTypeName = gm.PendingEnemyTypeName;


        // Clear any existing enemies in the container (if needed)
        foreach (var child in enemyContainer.GetChildren())
            child.QueueFree();
        enemies.Clear();

        // Instantiate the correct enemy from the database
        var enemyData = EnemyDatabase.AllEnemies[enemyTypeName];
        var enemyScene = enemyData.CombatSprite;
        var enemyInstance = enemyScene.Instantiate<Enemy>();
        enemyInstance.InitializeFromData(enemyData);
        enemyContainer.AddChild(enemyInstance);
        enemies.Add(enemyInstance);

        turnManager.Initialize(player, enemies, handUIManager, deckManager, handDrawSize);

        // delegate/callback for turn updates
        turnManager.OnTurnCountUpdated += (turnNumber) =>
        {
            endTurnButton.Text = $"End Turn \n Turn: {turnNumber}";
        };


        targetingService = new TargetingService(player, enemies, OnCardPlayed);

        if (player.GetNodeOrNull<TargetReceiver>("DropArea") is TargetReceiver playerReceiver)
        {
            playerReceiver.OnCardDroppedCallback = targetingService.HandleCardDropped;
        }

        foreach (var enemy in enemies)
        {
            if (enemy.GetNodeOrNull<TargetReceiver>("DropArea") is TargetReceiver receiver)
            {
                receiver.OnCardDroppedCallback = targetingService.HandleCardDropped;
            }
        }

        endTurnButton.Text = "End Turn";
        endTurnButton.Pressed += () => turnManager.OnEndTurnPressed(UpdateHPLabels, CheckBattleOutcome);

        UpdateHPLabels();

        GD.Print($"PlayerRef ready: {gm.PlayerRef}");
        var playerData = gm.PlayerRef;

        // Build deck list from CardInventory
        var deckList = new List<CardData>();
        foreach (var kvp in playerData.Deck.Cards)
            for (int i = 0; i < kvp.Value; i++)
                deckList.Add(kvp.Key);

        GD.Print($"Initializing combat with cards: {string.Join(", ", deckList.Select(c => c.Name))}");

        deckManager.InitDeck(deckList);

        turnManager.StartPlayerTurn();

    }

    public void OnCardPlayed(Card card)
    {
        // Find the CardData for this card (by name)
        var cardData = CardDatabase.AllCards.Find(c => c.Name == card.CardName);

        //deckManager.Discard(card.SourceScene);
        deckManager.Discard(cardData);
        handUIManager.RemoveCard(card); // ✅ Properly clears from UI

        UpdateHPLabels(); // Optional: reflect new damage/heal
        CheckBattleOutcome();
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health} | Shield: {player.Shield}";
        enemyHPLabel.Text = string.Join("\n", enemies.ConvertAll(e => $"{e.Name} HP: {e.Health}"));
    }


    private void CheckBattleOutcome()
    {
        if (player.Health <= 0)
        {
            BattleLost();
            return;
        }

        if (enemies.All(e => !e.IsAlive()))
        {
            BattleWon();
        }
    }


    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        playerTurn = false;
        endTurnButton.Disabled = true;
        handUIManager.ClearHand();
        deckManager.Reset();
        var gm = GetNode<GameManager>("/root/GameManager");
        gm.SwitchToOverworld();
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        playerTurn = false;
        endTurnButton.Disabled = true;
        handUIManager.ClearHand();
        deckManager.Reset();
        var gm = GetNode<GameManager>("/root/GameManager");
        gm.SwitchToOverworld();
        // TODO: handle game-over
    }
}
