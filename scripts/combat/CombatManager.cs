using Game.Scripts.Core;
using Godot;
using System;
using System.Collections.Generic;


public partial class CombatManager : Node
{
    [Export] private Player player;
    [Export] private Enemy enemy;

    [Export] private Label playerHPLabel;
    [Export] private Label enemyHPLabel;
    [Export] private Button attackButton;

    private bool playerTurn = true;

    [Export] private Control cardHand;

    [Export] private CardDeck cardDeck;

    [Export] private int handDrawSize = 3;
    [Export] private int maxHandSize = 6; // for draw effects

    [Export] private PackedScene damageCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/damageCard.tscn");
    [Export] private PackedScene healCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/healCard.tscn");
    [Export] private PackedScene sheildCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/sheildCard.tscn");

    [Export] private PackedScene burnCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/burnCard.tscn");
    [Export] private PackedScene buffCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/buffCard.tscn");

    public override void _Ready()
    {
        attackButton.Pressed += OnEndTurnPressed;
        UpdateHPLabels();

        var allCards = new List<PackedScene> {
            damageCardScene,
            healCardScene,
            sheildCardScene,
            burnCardScene,
            buffCardScene
        };

        cardDeck.InitDeck(allCards);
        StartPlayerTurn();

    }

    private void StartPlayerTurn()
    {
        Logger.Debug($"Player's turn begins, drawing {handDrawSize} cards");

        player.ProcessEffects(); // Apply effect (burn, buffs, etc)
        ClearHand(); // Saftey check for any leftover cards

        for (int i = 0; i < handDrawSize; i++) // Draw handsize
        {
            if (cardHand.GetChildCount() < maxHandSize) // check if hand is not full
            {
                var cardScene = cardDeck.Draw(); // pull from draw pile
                if (cardScene != null)
                    AddCardToHand(cardScene); // instantiates and adds to hand 
            }
        }

        playerTurn = true;
    }


    private async void OnEndTurnPressed()
    {
        if (!playerTurn) 
            return;

        playerTurn = false;

        ClearHand(); // Discard all cards in hand

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
        card.SetTextLabel();

        card.Pressed += () =>
        {
            Logger.Debug($"Card played: {card.CardName}");
            card.Play(player, enemy);
            UpdateHPLabels();
            cardDeck.Discard(cardScene);
            DiscardCardVisual(card); 
        };

        cardHand.AddChild(card);
    }


    private void DiscardCardVisual(Card card)
    {
        cardHand.RemoveChild(card);
        card.QueueFree();
    }

    private void ClearHand()
    {
        foreach (var child in cardHand.GetChildren())
        {
            child.QueueFree();
        }
    }

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        attackButton.Disabled = true;
        ClearHand();
        cardDeck.Reset();
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        attackButton.Disabled = true;
        ClearHand();
        cardDeck.Reset();
        // TODO: handle game-over
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health} | Shield: {player.Shield}";
        enemyHPLabel.Text = $"Enemy HP: {enemy.Health}";
    }
}
