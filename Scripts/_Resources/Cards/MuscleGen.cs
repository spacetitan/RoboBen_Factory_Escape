using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class MuscleGen : CardData
{
	public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
	{
		Muscle muscle = ResourceManager.instance.statuses[Status.StatusID.MUSCLE].CreateInstance() as Muscle;
		muscle.SetDuration(this.cardValue);

		BattleModel model = UIManager.instance.models[UIManager.UIState.BATTLE] as BattleModel;
		StatusEffect statusEffect = new StatusEffect(muscle, muscle.sfx);
		statusEffect.sender = model.player;
		statusEffect.Execute(targets);
		
		AudioManager.instance.sfxPlayer.Play(this.playSFX);
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
