using Game.Scripts.Combat.Effects;
using Godot;
using System;

namespace Game.Scripts.Combat.Cards;

public abstract partial class Card : Button
{
    public abstract string CardName { get; }
    public abstract TargetType Type { get; }

    public PackedScene SourceScene { get; set; } // For discard tracking
    public Player SourcePlayer { get; set; } // Set when drawn

    private Vector2 originalPosition;
    //private bool isDragging = false;

    public override void _Ready()
    {
        //originalPosition = Position;
        SetTextLabel();
    }

    //public abstract void Play(Player player, Enemy enemy);
    public abstract void Play(Character source, Character target);


    public void SetTextLabel()
    {
        Text = CardName;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                originalPosition = Position;
                ZIndex = 100; // Bring to front while dragging
                GrabFocus();
            }
            else if (!mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
                TryDrop();
        }
        else if (@event is InputEventMouseMotion motion)
            if (HasFocus())
                Position += motion.Relative;
    }

    private void TryDrop()
    {
        var space = GetViewport().GetWorld2D().DirectSpaceState;

        var query = new PhysicsPointQueryParameters2D
        {
            Position = GetGlobalMousePosition(),
            CollisionMask = 1,
            CollideWithAreas = true,
            CollideWithBodies = false
        };

        var results = space.IntersectPoint(query);

        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var colliderObj) &&
                colliderObj is Variant variant &&
                variant.As<GodotObject>() is TargetReceiver receiver)
            {
                if (receiver.OnCardDropped(this))
                {
                    return; // Successfully handled — CombatManager removes from UI
                }
                else
                {
                    RejectPlay();
                    return;
                }
            }
        }

        // Drop failed — return card to hand
        RejectPlay();
        ReleaseFocus();
    }



    public void RejectPlay()
    {
        GD.Print("Invalid target. Returning to hand.");
        Position = originalPosition;
        ZIndex = 0;
        ReleaseFocus(); // ✅ prevents lingering drag
    }


}
