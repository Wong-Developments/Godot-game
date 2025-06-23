using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Scripts.Combat;
public partial class CombatManager : Node
{
    private bool playerTurn = true;

    [Export] private Player player;
    [Export] private Enemy enemy;

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

    private int turnCount = 0;

    public override void _Ready()
    {
        endTurnButton.Text = "End Turn";
        endTurnButton.Pressed += OnEndTurnPressed;
        UpdateHPLabels();

        var allCards = new List<PackedScene> {
            damageCardScene,
            healCardScene,
            sheildCardScene,
            burnCardScene,
            buffCardScene
        };

        deckManager.InitDeck(allCards);
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
            {
                deckManager.Discard(card.SourceScene); // track original PackedScene
            }
        }

        handUIManager.ClearHand(); // Discard all cards in hand (UI)

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

        StartEnemyTurn();
    }

    private async void StartEnemyTurn()
    {
        enemy.ProcessEffects(); // Enemy effects tick before they attack

        if (enemy.Health <= 0)
        {
            BattleWon();
            return;
        }

        int damage = enemy.Attack();
        player.TakeDamage(damage);
        UpdateHPLabels();

        if (player.Health <= 0)
            BattleLost();
        else
        {
            //playerTurn = true;
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            StartPlayerTurn();
        }
            
    }

    private void AddCardToHand(PackedScene cardScene)
    {
        var card = cardScene.Instantiate<Card>();
        card.SourceScene = cardScene; // track source
        card.SetTextLabel();

        card.Pressed += () =>
        {
            Logger.Debug($"Card played: {card.CardName}");
            card.Play(player, enemy);
            UpdateHPLabels();
            deckManager.Discard(cardScene);
            handUIManager.RemoveCard(card); 
        };

        handUIManager.AddCard(card);
    }

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        endTurnButton.Disabled = true;
        handUIManager.ClearHand();
        deckManager.Reset();
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        endTurnButton.Disabled = true;
        handUIManager.ClearHand();
        deckManager.Reset();
        // TODO: handle game-over
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health} | Shield: {player.Shield}";
        enemyHPLabel.Text = $"Enemy HP: {enemy.Health}";
    }

    private void UpdateTurnCount()
    {
        turnCount++;
        endTurnButton.Text = $"End Turn \n Turn: {turnCount}";
        Logger.Debug($"Turn {turnCount} started.");
    }
}
