using Game.Scripts.Overworld.States;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Overworld;
public abstract partial class Entity : CharacterBody2D
{
    [Export] public StateMachine stateMachine;
    [Export] public float speed;
}
