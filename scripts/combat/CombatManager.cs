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
    private const int CardsPerTurn = 3;

    [Export] private PackedScene damageCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/damageCard.tscn");
    [Export] private PackedScene healCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/healCard.tscn");
    [Export] private PackedScene sheildCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/sheildCard.tscn");

    [Export] private PackedScene burnCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/burnCard.tscn");
    [Export] private PackedScene buffCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/buffCard.tscn");

    public override void _Ready()
    {
        attackButton.Pressed += OnAttackPressed;
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
        Logger.Debug($"Player's turn begins, drawing {CardsPerTurn} cards");
        player.ProcessEffects(); // Apply effect (burn, buffs, etc)
        ClearHand();

        for (int i = 0; i < CardsPerTurn; i++)
        {
            var cardScene = cardDeck.Draw(); // pull from draw pile
            if (cardScene != null)
                DrawCard(cardScene); // instantiates and adds to hand
        }

        playerTurn = true;
    }


    private async void OnAttackPressed()
    {
        if (!playerTurn) 
            return;

        int damage = player.Attack();
        enemy.TakeDamage(damage);
        UpdateHPLabels();

        playerTurn = false;

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

        EnemyTurn();
    }

    private void EnemyTurn()
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
            playerTurn = true;
    }

    private void DrawCard(PackedScene cardScene)
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
            CheckTurnEnd();
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

    private void CheckTurnEnd()
    {
        if (cardHand.GetChildCount() == 0)
        {
            Logger.Debug("Player has used all cards");

            bool skip = false;
            if (skip)
            if (skip)
            {
                playerTurn = false;
                EnemyTurn();
            }
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
