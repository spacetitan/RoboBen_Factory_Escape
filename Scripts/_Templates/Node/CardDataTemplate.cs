// meta-name: Card data
// meta-description: For creating new cards.
using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : CardData
{
    public override void ApplyEffects(List<Character> targets, Player player)
    {
        
    }
    
    public override string GetDefaultToolip()
    {
        return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
    }
    
    public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
    {
        return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
    }
}