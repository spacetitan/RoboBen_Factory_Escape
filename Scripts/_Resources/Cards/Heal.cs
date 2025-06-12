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
		string tooltip = this.cardDesc.Replace("{value}", this.cardValue.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nExhaust.";
		}

		return tooltip;
	}
}
