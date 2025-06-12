using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Ability : Resource
{
    [Export] public String name = "";
    [Export] public String desc = "";
    [Export] public int value { get; private set; } = 0;
    [Export] public int cooldown { get; private set; } = 0;
    [Export] public int cooldownTimer  = 0;
    [Export] public Character.TargetType targetType { get; private set; } = Character.TargetType.NONE;
    [Export] public Texture2D texture { get; private set; } = null;
    [Export] public AudioStream playSFX { get; private set; } = null;
    public bool abilityUsed { get; private set; } = false;

    public void Reset()
    {
        this.abilityUsed = false;
        this.cooldownTimer = 0;
    }

    public virtual void ApplyEffects(List<Character> targets, PlayerData playerData)
    {
        this.abilityUsed = true;
        return;
    }
}
