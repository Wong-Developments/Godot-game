using Game.Core;
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

    public override void _Ready()
    {
        Logger.Debug("CombatManager Ready");
        attackButton.Pressed += OnAttackPressed;
        UpdateHPLabels();
    }

    private async void OnAttackPressed()
    {
        if (!playerTurn) return;

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

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        attackButton.Disabled = true;
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        attackButton.Disabled = true;
        // TODO: handle game-over
    }

    private void UpdateHPLabels()
    {
        playerHPLabel.Text = $"Player HP: {player.Health}";
        enemyHPLabel.Text = $"Enemy HP: {enemy.Health}";
    }
}
