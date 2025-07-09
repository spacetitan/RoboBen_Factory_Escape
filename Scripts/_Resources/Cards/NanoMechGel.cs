using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class NanoMechGel : CardData
{
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		Regen regen = ResourceManager.instance.statuses[Status.StatusID.REGEN].CreateInstance() as Regen;
		regen.SetStacks(this.cardValue);
		
		StatusEffect statusEffect = new StatusEffect(regen, regen.sfx);
		statusEffect.sender = player;
		statusEffect.Execute(targets);

		player.statusHandler.RemoveStatus(Status.StatusID.POISON);

		AudioManager.instance.sfxPlayer.Play(this.playSFX);
	}
	
	public override string GetDefaultToolip()
	{
		return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
	}
	
	public override string GetModifiedTooltip(Player player, ModifierHandler enemyModifiers)
	{
		string tooltip = this.cardDesc.Replace("{value}", this.cardValue.ToString());
		
		if (this.isExhaust)
		{
			tooltip += "\nRemove from play.";
		}

		return tooltip;
	}
}
