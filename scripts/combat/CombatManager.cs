using Game.Scripts.Core;
using Godot;
using System;

public partial class CombatManager : Node
{
    [Export] private Player player;
    [Export] private Enemy enemy;

    [Export] private Label playerHPLabel;
    [Export] private Label enemyHPLabel;
    [Export] private Button attackButton;

    private bool playerTurn = true;

    // UI container for the hand
    [Export] private Control cardHand; 

    [Export] private PackedScene damageCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/damageCard.tscn");
    [Export] private PackedScene healCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/healCard.tscn");
    [Export] private PackedScene sheildCardScene = GD.Load<PackedScene>("res://scenes/combat/cards/sheildCard.tscn");

    public override void _Ready()
    {
        Logger.Debug("CombatManager Ready");
        attackButton.Pressed += OnAttackPressed;
        UpdateHPLabels();
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Logger.Debug("Player turn begins: drawing cards");
        ClearHand();
        DrawCard(damageCardScene);
        DrawCard(healCardScene);
        DrawCard(sheildCardScene);
        playerTurn = true;
    }

    private async void OnAttackPressed()
    {
        if (!playerTurn) 
            return;

        enemy.TakeDamage(Player.Attack());
        UpdateHPLabels();

        playerTurn = false;

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

        EnemyTurn();
    }

    private void EnemyTurn()
    {
        if (enemy.Health <= 0)
        {
            BattleWon();
            return;
        }

        player.TakeDamage(Enemy.Attack());
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
            DiscardCard(card);
            CheckTurnEnd();
        };

        cardHand.AddChild(card);
    }

    private void DiscardCard(Card card)
    {
        cardHand.RemoveChild(card);
        card.QueueFree();
    }

    private void ClearHand()
    {
        foreach (var child in cardHand.GetChildren())        
            child.QueueFree();        
    }

    private void CheckTurnEnd()
    {
        if (cardHand.GetChildCount() == 0)
        {
            Logger.Debug("Player has used all cards");
            playerTurn = false;
            EnemyTurn();
        }
    }

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        attackButton.Disabled = true;
        ClearHand();
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        attackButton.Disabled = true;
        ClearHand();
        // TODO: handle game-over
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health} | Shield: {player.Shield}";
        enemyHPLabel.Text = $"Enemy HP: {enemy.Health}";
    }
}
