using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Overworld;

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
    [Export] private int copiesPerCard = 2; // How many copies of each card to include in the deck

    [Export] private PackedScene damageCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/damageCard.tscn");
    [Export] private PackedScene healCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/healCard.tscn");
    [Export] private PackedScene sheildCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/sheildCard.tscn");
    [Export] private PackedScene burnCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/burnCard.tscn");
    [Export] private PackedScene buffCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/buffCard.tscn");



    public static CombatManager Instance { get; private set; }

    public List<Enemy> GetAliveEnemies()
    {
        return enemies.Where(e => e.IsAlive()).ToList();
    }

    public override void _Ready()
    {
        Instance = this;

        foreach (var child in enemyContainer.GetChildren())
        {
            if (child is Enemy enemy)
            {
                enemies.Add(enemy);
            }
        }

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

        var gm = GetNode<GameManager>("/root/GameManager");

        GD.Print($"PlayerRef ready: {gm.PlayerRef}");
        var playerData = gm.PlayerRef;

        GD.Print($"Initializing combat with cards: {playerData.AvailableCards}");

        deckManager.InitDeck(playerData.AvailableCards, copiesPerCard);

        //StartPlayerTurn();
        turnManager.StartPlayerTurn();

    }

    public void OnCardPlayed(Card card)
    {
        deckManager.Discard(card.SourceScene);
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
