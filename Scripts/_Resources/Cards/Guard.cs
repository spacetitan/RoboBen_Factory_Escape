using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Guard : CardData
{
    public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
    {
        GuardEffect armor = new GuardEffect(this.cardValue, this.playSFX);
        armor.Execute(targets);
    }
    
    public override string GetModifiedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        int guard = playerModifiers.GetModifiedValue(this.cardValue, Modifier.Type.ARMOR_GEN);
        
        string tooltip = this.cardDesc.Replace("{value}", guard.ToString());

        if (this.isExhaust)
        {
            tooltip += "\nExhaust.";
        }

        return tooltip;
    }
}
