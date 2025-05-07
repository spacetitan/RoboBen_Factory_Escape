using Godot;
using System;

[GlobalClass]
public partial class EnemyData : CharacterData
{
    [Export] public EnemyAction[] actions { get; private set; }
    [Export] public int attack { get; private set; } = 0;
    [Export] public int defense { get; private set; } = 0;
}
