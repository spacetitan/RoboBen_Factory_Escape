using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Attack : CardData
{
    public override void ApplyEffects(List<Character> targets, Player player)
    {
        DamageEffect damage = new DamageEffect(player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
        damage.Execute(targets);
    }
    
    public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
    {
        int damage = player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT);

        if (enemyModifiers != null)
        {
            damage = enemyModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_TAKEN);
        }
        
        string tooltip = this.cardDesc.Replace("{value}", damage.ToString());

        if (this.isExhaust)
        {
            tooltip += "\nRemove from play.";
        }

        return tooltip;
    }
}
