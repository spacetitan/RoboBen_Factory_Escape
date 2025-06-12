using Godot;
using System;
using System.Collections.Generic;

public partial class HealSpray : Ability
{
    public override void ApplyEffects(List<Character> targets, PlayerData playerStats)
    {
        HealEffect heal = new HealEffect(this.value, this.playSFX);
        heal.Execute(targets);
        base.ApplyEffects(targets, playerStats);
    }
}
