using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class PoisonAttack : CardData
{
	[Export] private int stacks = 2;
	
	public override void ApplyEffects(List<Character> targets, PlayerData playerStats, ModifierHandler modifiers)
	{
		DamageEffect damageEffect = new DamageEffect(modifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT), this.playSFX);
		damageEffect.Execute(targets);
		
		Poison poison = ResourceManager.instance.statuses["poison"].Duplicate() as Poison;
		poison.SetStacks(this.stacks);

		StatusEffect statusEffect = new StatusEffect(poison, null);
		statusEffect.status = poison;
		statusEffect.Execute(targets);
	}
	
	public override string GetDefaultToolip()
	{
		return this.cardDesc.Replace("{value}", this.cardValue.ToString()).Replace("{stacks}", this.stacks.ToString());
	}
	
	public override string GetModifiedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
	{
		int damage = playerModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_DEALT);

		if (enemyModifiers != null)
		{
			damage = enemyModifiers.GetModifiedValue(this.cardValue, Modifier.Type.DMG_TAKEN);
		}

		return this.cardDesc.Replace("{value}", damage.ToString()).Replace("{stacks}", this.stacks.ToString());
	}
}
