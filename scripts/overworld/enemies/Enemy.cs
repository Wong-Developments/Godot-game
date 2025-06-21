using Game.Overworld.Enemies.States;
using Godot;
using System;

namespace Game.Overworld.Enemies;

public abstract partial class Enemy : CharacterBody2D
{
	[Export] public float speed;
	[Export] public StateMachine stateMachine;
}
