using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class SneakAttack : CardData
{
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		DamageEffect damage;
		if (player.statusHandler.HasStatus(Status.StatusID.INVIS))
		{
			damage = new DamageEffect(player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT) * 2, this.playSFX);
		}
		else
		{
			damage = new DamageEffect(player.modifierHandler.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
		}
		
		damage.Execute(targets);
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
        
		string tooltip = this.cardDesc.Replace("{value}", damage.ToString());

		if (this.isExhaust)
		{
			tooltip += "\nRemove from play.";
		}

		return tooltip;
	}
}
