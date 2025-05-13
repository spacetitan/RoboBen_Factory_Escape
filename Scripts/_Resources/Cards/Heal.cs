using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Heal : CardData
{
	public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
	{
		HealEffect heal = new HealEffect(this.cardValue, this.playSFX);
		heal.Execute(targets);
	}
	
	public override string GetDefaultToolip()
	{
		return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
	}
	
	public override string GetModifiedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
	{
		return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
	}
}
