using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Poison : Status
{
	public override void InitializeStatus(Character target)
	{
		
		base.InitializeStatus(target);
	}

	public override void ApplyStatus(Character target)
	{
		DamageEffect damageEffect = new DamageEffect(this.stacks, this.sfx, true);
		damageEffect.Execute(target as Character);
		
		base.ApplyStatus(target);
	}
}
