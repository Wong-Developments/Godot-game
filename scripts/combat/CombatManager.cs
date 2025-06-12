using Game.Core;
using Godot;
using System;

public partial class CombatManager : Node
{
    [Export] private Player _player;
    [Export] private Enemy _enemy;

    [Export] private Label _playerHPLabel;
    [Export] private Label _enemyHPLabel;
    [Export] private Button _attackButton;

    private bool _playerTurn = true;

    public override void _Ready()
    {
        Logger.Debug("CombatManager Ready");
        _attackButton.Pressed += OnAttackPressed;
        UpdateHPLabels();
    }

    private async void OnAttackPressed()
    {
        if (!_playerTurn) return;

        int damage = _player.Attack();
        _enemy.TakeDamage(damage);
        UpdateHPLabels();

        _playerTurn = false;

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

        EnemyTurn();
    }

    private void EnemyTurn()
    {
        if (_enemy.Health <= 0)
        {
            BattleWon();
            return;
        }

        int damage = _enemy.Attack();
        _player.TakeDamage(damage);
        UpdateHPLabels();

        if (_player.Health <= 0)
            BattleLost();
        else
            _playerTurn = true;
    }

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        _attackButton.Disabled = true;
        // TODO: handle victory
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        _attackButton.Disabled = true;
        // TODO: handle game-over
    }

    private void UpdateHPLabels()
    {
        _playerHPLabel.Text = $"Player HP: {_player.Health}";
        _enemyHPLabel.Text = $"Enemy HP: {_enemy.Health}";
    }
}
