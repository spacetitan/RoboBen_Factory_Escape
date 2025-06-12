using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Attack : CardData
{
    public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
    {
        DamageEffect damage = new DamageEffect(modifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
        damage.Execute(targets);
    }
    
    public override string GetModifiedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        int damage = playerModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT);

        if (enemyModifiers != null)
        {
            damage = enemyModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_TAKEN);
        }
        
        string tooltip = this.cardDesc.Replace("{value}", damage.ToString());

        if (this.isExhaust)
        {
            tooltip += "\nExhaust.";
        }

        return tooltip;
    }
}
