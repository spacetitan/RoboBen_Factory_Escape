using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class StunAttack : CardData
{
	[Export] private int stacks = 2;
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		DamageEffect damageEffect = new DamageEffect(player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
		damageEffect.Execute(targets);
		
		Stun stun = ResourceManager.instance.statuses[Status.StatusID.STUN].CreateInstance() as Stun;
		stun.SetDuration(this.stacks);
		
		StatusEffect statusEffect = new StatusEffect(stun, stun.sfx);
		statusEffect.sender = player;
		statusEffect.Execute(targets);
	}
	
	public override string GetDefaultToolip()
	{
		return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
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
			tooltip += "\nRemove from play.";
		}

		return tooltip;
	}
}
