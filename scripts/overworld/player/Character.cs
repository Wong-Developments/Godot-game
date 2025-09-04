using Game.Scripts.Data;
using Game.Scripts.Overworld.States;
using Godot;
using System;
using System.Collections.Generic;

namespace Game.Scripts.Overworld.Player;

public partial class Character : Entity
{
    public CardInventory Deck { get; private set; } = new CardInventory();

    public override void _Ready()
	{
        //Starting hand, when game first loads
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Bash"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Croix"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Sheild"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Enflame"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Buff"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Salt Blast"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Double Swing"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Hammer Spin"), 1);
        Deck.AddCard(CardDatabase.AllCards.Find(c => c.Name == "Counter Smash"), 1);

        var gm = GetNode<GameManager>("/root/GameManager");
        gm.PlayerRef = this; // Set the global player reference

        AddToGroup("player");
		stateMachine.ChangeState(stateMachine.GetNode<State>("FreeRoam"));

        // Add this temporary debug to your player's _Ready()
        GD.Print($"Player collision shapes:");
        foreach (Node child in GetChildren())
        {
            if (child is CollisionShape2D || child is CollisionPolygon2D)
            {
                GD.Print($"- {child.Name} (Visible: {((Node2D)child).Visible})");
            }
        }
    }

	public override void _Process(double delta)
	{
		stateMachine.PhysicsUpdate((float)delta);
		Position.Round();
	}
}
