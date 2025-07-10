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

    [Export] private int handDrawSize = 3;

    [Export] private PackedScene damageCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/damageCard.tscn");
    [Export] private PackedScene healCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/healCard.tscn");
    [Export] private PackedScene sheildCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/sheildCard.tscn");

    [Export] private PackedScene burnCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/burnCard.tscn");
    [Export] private PackedScene buffCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/buffCard.tscn");

    private int turnCount = 0; // how many turns have passed

    public static CombatManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        foreach (var child in enemyContainer.GetChildren())
        {
            if (child is Enemy enemy)
            {
                enemies.Add(enemy);
                if (enemy.GetNodeOrNull<TargetReceiver>("DropArea") is TargetReceiver receiver)
                {
                    receiver.OnCardDroppedCallback = HandleCardDropped;
                }
            }
        }

        if (player.GetNodeOrNull<TargetReceiver>("DropArea") is TargetReceiver playerReceiver)
        {
            playerReceiver.OnCardDroppedCallback = HandleCardDropped;
        }
        endTurnButton.Text = "End Turn";
        endTurnButton.Pressed += OnEndTurnPressed;
        UpdateHPLabels();

        //var allCards = new List<PackedScene> {
        //    damageCardScene,
        //    healCardScene,
        //    sheildCardScene,
        //    burnCardScene,
        //    buffCardScene
        //};

        //deckManager.InitDeck(allCards);

        var gm = GetNode<GameManager>("/root/GameManager");

        GD.Print($"PlayerRef ready: {gm.PlayerRef}");
        var playerData = gm.PlayerRef;

        GD.Print($"Initializing combat with cards: {playerData.AvailableCards}");

        deckManager.InitDeck(playerData.AvailableCards);

        StartPlayerTurn();

    }

    private void StartPlayerTurn()
    {
        Logger.Debug($"Player's turn begins, drawing {handDrawSize} cards");

        player.ProcessEffects(); // Apply effect (burn, buffs, etc)
        handUIManager.ClearHand(); // Saftey check for any leftover cards

        for (int i = 0; i < handDrawSize; i++)
        {
            var cardScene = deckManager.Draw();
            if (cardScene != null)
                AddCardToHand(cardScene);
        }

        playerTurn = true;
    }


    private async void OnEndTurnPressed()
    {
        if (!playerTurn) 
            return;

        UpdateTurnCount();

        playerTurn = false;
        

        foreach (var node in handUIManager.GetCards())
        {
            if (node is Card card)
                deckManager.Discard(card.SourceScene); // track original PackedScene
        }

        handUIManager.ClearHand(); // Discard all cards in hand (UI)

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartEnemyTurn();
    }

    private async void StartEnemyTurn()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                enemy.ProcessEffects();
                int damage = enemy.Attack();
                player.TakeDamage(damage);
            }
        }

        UpdateHPLabels();
        CheckBattleOutcome();

        if (playerTurn) // Prevent starting turn if player already died
            return;

        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartPlayerTurn();
    }

    private void AddCardToHand(PackedScene cardScene)
    {
        var card = cardScene.Instantiate<Card>();
        card.SourceScene = cardScene; // track source
        card.SourcePlayer = player; // Assign player as source
        card.SetTextLabel();

        //card.Pressed += () =>
        //{
        //    Logger.Debug($"Card played by click: {card.CardName}");
        //    var targetEnemy = enemies.Find(e => e.IsAlive());
        //    if (targetEnemy != null)
        //    {
        //        card.Play(player, targetEnemy);
        //        deckManager.Discard(cardScene);
        //        handUIManager.RemoveCard(card);
        //    }
        //};

        handUIManager.AddCard(card);
    }

    public void OnCardPlayed(Card card)
    {
        deckManager.Discard(card.SourceScene);
        handUIManager.RemoveCard(card); // ✅ Properly clears from UI
        UpdateHPLabels(); // Optional: reflect new damage/heal

        CheckBattleOutcome();
    }

    private bool HandleCardDropped(Card card)
    {
        Character target = null;

        // Check if mouse is over player's drop area
        var playerReceiver = player.GetNodeOrNull<TargetReceiver>("DropArea");
        bool isOnPlayer = IsMouseOverReceiver(playerReceiver);

        // Check if mouse is over an enemy's drop area
        var hitEnemy = enemies.FirstOrDefault(e =>
        {
            var enemyReceiver = e.GetNodeOrNull<TargetReceiver>("DropArea");
            return IsMouseOverReceiver(enemyReceiver);
        });

        // Match the card's target type
        switch (card.Type)
        {
            case TargetType.Self:
                if (isOnPlayer)
                    target = player;
                break;

            case TargetType.SingleEnemy:
                if (hitEnemy != null)
                    target = hitEnemy;
                break;

            case TargetType.AllEnemies:
                target = null;
                break;
        }

        if (card.Type != TargetType.AllEnemies && target == null)
            return false;

        card.Play(card.SourcePlayer, target);
        OnCardPlayed(card);
        return true;
    }


    // Helper Function - utility method for detecting drop collisions
    private bool IsMouseOverReceiver(TargetReceiver receiver)
    {
        if (receiver == null)
            return false;

        Vector2 mousePos = GetViewport().GetMousePosition();

        var hits = receiver.GetWorld2D().DirectSpaceState.IntersectPoint(
            new PhysicsPointQueryParameters2D
            {
                Position = mousePos,
                CollisionMask = 1,
                CollideWithAreas = true,
                CollideWithBodies = false
            }
        );

        return hits.Any(result =>
            result.TryGetValue("collider", out var col) &&
            col.As<GodotObject>() == receiver
        );
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health} | Shield: {player.Shield}";
        enemyHPLabel.Text = string.Join("\n", enemies.ConvertAll(e => $"{e.Name} HP: {e.Health}"));
    }

    private void UpdateTurnCount()
    {
        turnCount++;
        endTurnButton.Text = $"End Turn \n Turn: {turnCount}";
        Logger.Debug($"Turn {turnCount} started.");
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
        GetTree().ChangeSceneToFile("res://scenes/core/game_manager.tscn");
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        playerTurn = false;
        endTurnButton.Disabled = true;
        handUIManager.ClearHand();
        deckManager.Reset();
        GetTree().ChangeSceneToFile("res://scenes/core/game_manager.tscn");
        // TODO: handle game-over
    }
}
