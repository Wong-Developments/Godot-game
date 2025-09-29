using Game.Scripts.Combat.Cards;
using Game.Scripts.Combat.Effects;
using Game.Scripts.Core;
using Game.Scripts.Data;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts.Combat;
public partial class TurnManager : Node
{
    public Action<int> OnTurnCountUpdated;  // Pass the current turn count to whoever listens

    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }

    private Player player;
    private List<Enemy> enemies;
    private HandUIManager handUIManager;
    private DeckManager deckManager;
    private int handDrawSize;

    private TurnState currentTurn;
    private int turnCount = 0;

    // Initialize instead of constructor so this can be attached as a Node
    public void Initialize(Player player, List<Enemy> enemies, HandUIManager handUIManager, DeckManager deckManager, int handDrawSize)
    {
        this.player = player;
        this.enemies = enemies;
        this.handUIManager = handUIManager;
        this.deckManager = deckManager;
        this.handDrawSize = handDrawSize;
    }

    public void StartPlayerTurn()
    {
        Logger.Debug($"Player's turn begins, drawing {handDrawSize} cards");

        player.ProcessEffects(); // Apply effect (burn, buffs, etc)
        handUIManager.ClearHand(); // Saftey check for any leftover cards

        for (int i = 0; i < handDrawSize; i++)
        {
            var cardData = deckManager.Draw();
            if (cardData != null)
            {
                var card = cardData.Scene.Instantiate<Card>();
                card.SourcePlayer = player;
                card.SetTextLabel();
                handUIManager.AddCard(card);
            }
        }

        currentTurn = TurnState.PlayerTurn;
    }

    public async void OnEndTurnPressed(Action updateHPLabels, Action checkBattleOutcome)
    {
        if (currentTurn != TurnState.PlayerTurn)
            return;

        turnCount++;

        OnTurnCountUpdated?.Invoke(turnCount); // Notify listener about new turn count

        foreach (var node in handUIManager.GetCards())
        {
            if (node is Card card)
            {
                var cardData = CardDatabase.AllCards.Find(c => c.Name == card.CardName);
                deckManager.Discard(cardData);
            }
                
        }

        handUIManager.ClearHand();

        // Pause 1s
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartEnemyTurn(updateHPLabels, checkBattleOutcome);
    }

    private async void StartEnemyTurn(Action updateHPLabels, Action checkBattleOutcome)
    {
        currentTurn = TurnState.EnemyTurn;

        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                enemy.ProcessEffects();
                //int damage = enemy.Attack();
                //player.TakeDamage(damage);
                enemy.Attack(); // Enemy.Attack() already applies damage
            }
        }

        updateHPLabels();
        checkBattleOutcome();

        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartPlayerTurn();
    }
}