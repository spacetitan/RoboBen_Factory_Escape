using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class RandomCard : CardData
{
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		player.hand.AddCard(RunManager.instance.GetRandomCardWithExhaust());
	}
	
	public override string GetDefaultToolip()
	{
		string tooltip = this.cardDesc.Replace("{value}", this.cardValue.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nRemove.";
		}

		return tooltip;
	}
	
	public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
	{
		string tooltip = this.cardDesc.Replace("{value}", this.cardValue.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nRemove.";
		}

		return tooltip;
	}
}
