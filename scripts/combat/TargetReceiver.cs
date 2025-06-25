using Game.Scripts.Combat.Cards;
using Godot;
using System;

namespace Game.Scripts.Combat
{
    public partial class TargetReceiver : Area2D
    {
        public Character OwnerCharacter { get; set; }

        // New: Callback set by CombatManager
        public Func<Card, bool> OnCardDroppedCallback;

        public override void _Ready()
        {
            if (OwnerCharacter == null)
                OwnerCharacter = GetParent<Character>();
        }

        public bool OnCardDropped(Card card)
        {
            // Delegate decision to CombatManager via callback
            return OnCardDroppedCallback?.Invoke(card) ?? false;
        }

    }
}
