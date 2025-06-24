using Game.Scripts.Core;
using Game.Scripts.Overworld.States;
using Godot;
using System;

namespace Game.Scripts.Overworld.Enemies;

public abstract partial class Enemy : Entity
{
    [ExportCategory("General Settings")]
    [Export] public float acceleration = 5f;
    [Export] public float deceleration = 10f;
    [Export] public float detectionRange = 100f;

    // more defaults for base enemy if needed
}
