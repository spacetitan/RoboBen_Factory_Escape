using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class AllAttack : CardData
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
