using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ToughGen : CardData
{
	public override void ApplyEffects(List<Character> targets, Player player)
	{
		Tough tough = ResourceManager.instance.statuses[Status.StatusID.TOUGH].CreateInstance() as Tough;
		tough.SetStacks(this.cardValue);
		
		StatusEffect statusEffect = new StatusEffect(tough, tough.sfx);
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
		return this.cardDesc.ReplaceN("{value}", this.cardValue.ToString());
	}
}
