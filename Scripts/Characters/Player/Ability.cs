using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ability : Resource
{
    [Export] public String name;
    [Export] public String desc;
    [Export] public int value { get; private set; }
    [Export] public Character.TargetType targetType { get; private set; }
    [Export] public Texture2D texture { get; private set; }
    [Export] public AudioStream playSFX { get; private set; }
    public bool abilityUsed = false;
    
    public virtual void ApplyEffects(List<Character> targets, PlayerData playerData)
    {
        return;
    }
}
