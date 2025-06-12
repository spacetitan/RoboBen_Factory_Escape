using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class RandomCard : CardData
{
	public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
	{
		if (targets[0] is Player)
		{
			Player player = targets[0] as Player;
			player.hand.AddCard(RunManager.instance.GetRandomCardWithExhaust());
		}
	}
	
	public override string GetDefaultToolip()
	{
		string tooltip = this.cardDesc.Replace("{value}", this.cardValue.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nExhaust.";
		}

		return tooltip;
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
