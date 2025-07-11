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
		base.ApplyStatus(target);
	}
	
	public override Stun CreateInstance()
	{
		return base.CreateInstance() as Stun;
	}
}
