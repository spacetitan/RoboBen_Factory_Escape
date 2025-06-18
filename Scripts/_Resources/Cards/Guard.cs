using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Guard : CardData
{
    public override void ApplyEffects(List<Character> targets, Player player)
    {
        GuardEffect armor = new GuardEffect(this.cardValue, this.playSFX);
        armor.Execute(targets);
    }
    
    public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
    {
        int guard = player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.ARMOR_GEN);
        
        string tooltip = this.cardDesc.Replace("{value}", guard.ToString());

        if (this.isExhaust)
        {
            tooltip += "\nRemove.";
        }

        return tooltip;
    }
}
