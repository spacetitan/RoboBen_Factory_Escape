using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class PoisonAttack : CardData
{
	[Export] private int stacks = 2;
	
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		DamageEffect damageEffect = new DamageEffect(player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
		damageEffect.Execute(targets);
		
		Poison poison = ResourceManager.instance.statuses[Status.StatusID.POISON].CreateInstance() as Poison;
		poison.SetStacks(this.stacks);
		
		StatusEffect statusEffect = new StatusEffect(poison, poison.sfx);
		statusEffect.sender = player;
		statusEffect.Execute(targets);
	}
	
	public override string GetDefaultToolip()
	{
		return this.cardDesc.Replace("{value}", this.cardValue.ToString()).Replace("{stacks}", this.stacks.ToString());
	}
	
	public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
	{
		int damage = player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT);

		if (enemyModifiers != null)
		{
			damage = enemyModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_TAKEN);
		}
		
		string tooltip = this.cardDesc.Replace("{value}", damage.ToString()).Replace("{stacks}", this.stacks.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nRemove.";
		}

		return tooltip;
	}
}
