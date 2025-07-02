using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class MuscleGen : CardData
{
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		Muscle muscle = ResourceManager.instance.statuses[Status.StatusID.MUSCLE].CreateInstance() as Muscle;
		muscle.SetStacks(this.cardValue);
		
		StatusEffect statusEffect = new StatusEffect(muscle, muscle.sfx);
		statusEffect.sender = player;
		statusEffect.Execute(targets);
		
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
			tooltip += "\nRemove.";
		}

		return tooltip;
	}
}
