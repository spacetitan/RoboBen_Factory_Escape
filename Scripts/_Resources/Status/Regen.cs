using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Regen : Status
{
	public override void InitializeStatus(Character target)
	{
		
		base.InitializeStatus(target);
	}

	public override void ApplyStatus(Character target)
	{
		HealEffect healEffect = new HealEffect(this.stacks, this.sfx);
		healEffect.Execute(target as Character);
		base.ApplyStatus(target);
	}
	
	public override Regen CreateInstance()
	{
		return base.CreateInstance() as Regen;
	}
}
