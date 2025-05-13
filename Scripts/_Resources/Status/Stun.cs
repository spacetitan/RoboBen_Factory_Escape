using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Stun : Status
{
	public override void InitializeStatus(Character target)
	{
		
		base.InitializeStatus(target);
	}

	public override void ApplyStatus(Character target)
	{
		this.SetStacks(this.stacks-1);
		base.ApplyStatus(target);
	}
}
