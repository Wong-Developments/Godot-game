// CombatManager.cs
using Game.Core;
using Godot;
using System;
using System.Drawing;

public partial class CombatManager : Node
{
    // Export to Inspector
    [Export] public int PlayerMaxHealth = 100;
    [Export] public int EnemyMaxHealth = 100;
    [Export] public int AttackDamage = 10;

    //
    private int _playerHealth;
    private int _enemyHealth;
    private bool _playerTurn = true;

    // ui node refs
    [Export] private Label _playerHPLabel;
    [Export] private Label _enemyHPLabel;
    [Export] private Button _attackButton;

    public override void _Ready()
    {
        Logger.Debug("CombatManager Ready");

        _playerHealth = PlayerMaxHealth;
        _enemyHealth = EnemyMaxHealth;
        Logger.Debug("Initialised health");
        
        UpdateHPLabels();

        // wire up button
        _attackButton.Pressed += OnAttackPressed;
    }

    private async void OnAttackPressed()
    {
        if (!_playerTurn) return;

        _enemyHealth -= AttackDamage;
        Logger.Info($"Player attacks for {AttackDamage} dmg");
        UpdateHPLabels();

        _playerTurn = false;

        // 1s pause before the enemy’s turn
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        if (_enemyHealth <= 0)
        {
            BattleWon();
            return;
        }

        _playerHealth -= AttackDamage;
        Logger.Info($"Enemy attacks for {AttackDamage} dmg");
        UpdateHPLabels();

        if (_playerHealth <= 0)
            BattleLost();
        else
            _playerTurn = true;
    }

    private void UpdateHPLabels()
    {
        _playerHPLabel.Text = $"Player HP: {_playerHealth}";
        _enemyHPLabel.Text = $"Enemy HP: {_enemyHealth}";
    }

    private void BattleWon()
    {
        Logger.Info("Enemy defeated!");
        _attackButton.Disabled = true;
        // TODO: emit signal or call back to overworld to return / grant XP
    }

    private void BattleLost()
    {
        Logger.Info("Player defeated!");
        _attackButton.Disabled = true;
        // TODO: handle game-over / respawn
    }
}

